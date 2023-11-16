using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;
using TMPro;
using Unity.Barracuda;
using UnityEngine.UI;

public class MartialAgentPlayer : Agent
{
    /*
        <���� ������Ʈ ����:>
        �ִ� ü�� = 13
        ���ǵ� = 5f
        ź�� �ν� ���� = 5f
        ���� ������ = 2
        ���Ÿ� ������ = 1
        ���� = 0
        ====================
        <������ �ݿ��Ǵ� ����:>
        (1*0.75 + 2*0.2 + 3*0.05) * 12 / 3 = 5.2 �̹Ƿ�, ������ �����ϴ� ������ 5�� �մ��ϴ�.

        Off�� ������ ������: ����: x/5, ���Ÿ�: x/10
        Dff�� ���� ������: x/5
        Spd�� ���ǵ�, ź�� �ν� ���� ������: x/5
     */
    // Stats
    [SerializeField] private float statOff = 1f;
    [SerializeField] private float statDff = 1f;
    [SerializeField] private float statSpd = 1f;

    [SerializeField] private float agent_maxHP = 13f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float max_RecogRange = 5f; // the range of the recognition
    [SerializeField] public float meleeDamage = 2f;
    [SerializeField] public float rangeDamage = 1f;
    [SerializeField] private float defense = 0f;


    // Initial Dependency Settings
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SpriteRenderer floorMeshRenderer;

    private BufferSensorComponent evasion_BufferSensor;

    private SoundEffectPlayer m_soundEffectPlayer;

    private Rigidbody2D agent_Rigid2D;
    private SpriteRenderer agent_Renderer;

    [SerializeField] private Image agentHPUIIMG;
    private float agent_currentHP;

    [SerializeField] private TextMeshProUGUI playerNameTag;

    [SerializeField] private GameObject bulletPrefab;

    private AnimationController m_animator;

    private bool isInvincible = false;

    private struct BulletData // bullet data for buffer sensor
    {
        public float dx, dy, vx, vy, sqrDis;
        public BulletData(float f1, float f2, float f3, float f4, float f5)
        {
            dx = f1;
            dy = f2;
            vx = f3;
            vy = f4;
            sqrDis = f5;
        }
    }

    // Recognition Range
    private float RecogRangeSquared;
    private List<BulletData> bulletObjects = new List<BulletData>(); // An array to store the bullet game objects

    public float timeBetweenDecisionsAtInference;
    float m_TimeSinceDecision;

    VectorSensorComponent m_GoalSensor;

    private bool isHit = false;
    public bool IsHit
    {
        get { return isHit; }
        set { isHit = value; }
    }

    public enum MartialGoal
    {
        Evasion,
        Approach,
        Melee,
    }

    MartialGoal m_CurrentGoal;

    public MartialGoal CurrentGoal
    {
        get { return m_CurrentGoal; }
        set
        {
            switch (value)
            {
                case MartialGoal.Evasion:
                    agent_Renderer.color = new Color(1, 0, 1, 0.3f);
                    break;
                case MartialGoal.Approach:
                    agent_Renderer.color = new Color(0, 0, 1, 0.3f);
                    break;
                case MartialGoal.Melee:
                    agent_Renderer.color = new Color(0, 1, 0, 0.3f);
                    break;
            }
            m_CurrentGoal = value;
        }
    }

    // AnimationController�� ������ �� Observing
    private int currentAnimationState = 0;
    private int nextAnimationState = 0;

    // Lose Message Invoker
    public delegate void LoseMessageRaisedEventHandler();
    public static event LoseMessageRaisedEventHandler OnLoseMessageRaised;

    public override void Initialize()
    {
        evasion_BufferSensor = GetComponent<BufferSensorComponent>();
        RecogRangeSquared = Mathf.Pow(max_RecogRange, 2f);
        agent_Rigid2D = GetComponent<Rigidbody2D>();
        agent_Renderer = GetComponentInChildren<SpriteRenderer>();
        m_GoalSensor = GetComponent<VectorSensorComponent>();
        m_animator = GetComponentInChildren<AnimationController>();
        m_soundEffectPlayer = GetComponent<SoundEffectPlayer>();

        // Update Stats
        /*
        Off�� ������ ������: ����: x/5, ���Ÿ�: x/10
        Dff�� ���� ������: x/5
        Spd�� ���ǵ�, ź�� �ν� ���� ������: x/5
         */
        // Player�� ���⼭ �����͸� �ε��Ѵ�.
        DataController.Instance.LoadGameData();
        statOff = DataController.Instance.gameData.statOFF;
        statDff = DataController.Instance.gameData.statDFF;
        statSpd = DataController.Instance.gameData.statSPD;

        playerNameTag.text = DataController.Instance.gameData.name;

        meleeDamage += statOff / 5f;
        rangeDamage += statOff / 10f;
        defense += statDff / 5f;
        moveSpeed += statSpd / 5f;
        max_RecogRange += statSpd / 5f;

        /*
        // �ӽ�:�� ȯ�� �� ��� ������Ʈ�� ü�� �ʱ�ȭ.
        MartialAgent _target;
        if (_target = targetTransform.GetComponent<MartialAgent>())
            _target.InitializeHP();
        */
        InitializeHP();  // �� ü�� �ʱ�ȭ.
    }

    public void InitializeHP()
    {
        // restore all HP
        agent_currentHP = agent_maxHP;
        IsHit = false;
    }

    // Update �Լ�
    public void Update()
    {  // Death Check
        if (agent_currentHP <= 0)
            AgentDie();


        // Hit Check
        if (IsHit)
        {
            // HitByMelee
            HitByMelee();
        }

        // Direction Check
        Vector3 dirToTarget = (targetTransform.localPosition - transform.localPosition).normalized;
        if (dirToTarget.x > 0)
            m_animator.IsRight = true;
        else
            m_animator.IsRight = false;

        // ���°� ��ȭ�� ��쿡�� Animation �ݿ�
        if (nextAnimationState != currentAnimationState)
        {
            // Only react when the status changes
            m_animator.SetCurrentState(nextAnimationState);
        }
        currentAnimationState = nextAnimationState;

        // HP UI ����
        UpdateHPUI();

    }

    public override void OnEpisodeBegin()
    {
        /*
        // �ӽ�:�� ȯ�� �� ��� ������Ʈ�� ü�� �ʱ�ȭ.
        MartialAgent _target;
        if (_target = targetTransform.GetComponent<MartialAgent>())
            _target.InitializeHP();
        */
        InitializeHP();  // �� ü�� �ʱ�ȭ.

        // Animation �ʱ�ȭ
        currentAnimationState = 0;
        nextAnimationState = 0;

        // Destory any bullets remain on environment
        DestroyAllProjectiles();

        // check Goal
        UpdateBulletObjects();
        CheckGoal();

        // relocate agent and goal randomly
        transform.localPosition = new Vector3(UnityEngine.Random.Range(-8f, 3f), UnityEngine.Random.Range(-8f, 3f), 0);
        targetTransform.localPosition = new Vector3(UnityEngine.Random.Range(0, 5f), UnityEngine.Random.Range(0, 5f), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Check Goal
        UpdateBulletObjects();
        CheckGoal();

        Array values = Enum.GetValues(typeof(MartialGoal));
        if (m_GoalSensor is object)
        {
            int goalNum = (int)CurrentGoal;
            m_GoalSensor.GetSensor().AddOneHotObservation(goalNum, values.Length);
        }
        // Static length observation
        // Observe direction vector from Agent to Target
        Vector3 dirToTarget = (targetTransform.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(dirToTarget.x);    // 1th observ vector - direction to target.x
        sensor.AddObservation(dirToTarget.y);    // 2nd observ vector - direction to target.y
        sensor.AddObservation(agent_currentHP * 1.0f / agent_maxHP);  // 3rd observ vector - agent's current HP


        // Variable length observation with using Buffer Sensor
        if (bulletObjects.Count > 0)
        {
            foreach (BulletData bullet in bulletObjects)
            {
                // Input Obervation to BufferSensor. Agent observe normalized direction To Bullet(with distance), and velocity of bullet.
                float[] tempObserv = { bullet.dx, bullet.dy,
                                   bullet.vx, bullet.vy};
                evasion_BufferSensor.AppendObservation(tempObserv);
            }
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (m_CurrentGoal == MartialGoal.Melee)  // ���� ���� ��Ȳ�� �ÿ�
        {   // ���� ������ �õ��ߴٸ� ����
            bool isTryToAttack = actions.DiscreteActions[2] == 1;
            if (isTryToAttack)
            {
                if (m_soundEffectPlayer is not null)
                {
                    m_soundEffectPlayer.PlaySfx1();
                }
                nextAnimationState = 3;
                MartialAgentNPC _target;
                if (_target = targetTransform.GetComponent<MartialAgentNPC>())  // �ӽ�:���� �������̰� Ÿ���� ������Ʈ�� ���
                {
                    _target.IsHit = true;
                    AddReward(+0.3f);
                }
                else  // ���� �н����� ��� 
                {
                    AgentWin();
                }

            }
        }

        // �� �̿��� ��Ȳ��
        // �̵�
        // ���� �� ���� ����

        if (m_CurrentGoal == MartialGoal.Approach)
        {
            float _disToTarget = (targetTransform.localPosition - transform.localPosition).sqrMagnitude;  // ��ǥ���� �Ÿ� ���
            if (_disToTarget > 12.25f)  // �Ÿ��� 3.5 �̻��� �� �������
            {
                bool isTryToShot = actions.DiscreteActions[3] == 1;
                if (isTryToShot)
                {
                    if (m_soundEffectPlayer is not null)
                    {
                        m_soundEffectPlayer.PlaySfx2();
                    }
                    nextAnimationState = 4;
                    StartCoroutine("MakeTemporalInvincible");

                    float bulletSpeed = 10f;
                    Vector3 dirToTarget = (targetTransform.localPosition - this.transform.localPosition).normalized;
                    GameObject newBullet =
                        Instantiate(bulletPrefab, this.transform.position,
                        Quaternion.identity, this.transform.parent) as GameObject;
                    newBullet.GetComponent<Rigidbody2D>().velocity = dirToTarget * bulletSpeed;
                    float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
                    newBullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }
            }
        }


        int moveX = actions.DiscreteActions[0];  // 0 = Don't Move; 1 = Left; 2 = Right
        int moveY = actions.DiscreteActions[1];  // 0 = Don't Move; 1 = Back; 2 = Forward;

        Vector3 addForce = Vector3.zero;

        switch (moveX)
        {
            case 0:
                addForce.x = 0f;
                break;
            case 1:
                addForce.x -= 1f;
                break;
            case 2:
                addForce.x += 1f;
                break;
        }

        switch (moveY)
        {
            case 0:
                addForce.y = 0f;
                break;
            case 1:
                addForce.y -= 1f;
                break;
            case 2:
                addForce.y += 1f;
                break;
        }

        agent_Rigid2D.velocity = addForce * moveSpeed;

        AddReward(-1f / MaxStep);  // accelerate decision speed
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        // ���� �̵�

        switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case -1:
                discreteActions[0] = 1;
                break;
            case 0:
                discreteActions[0] = 0;
                break;
            case +1:
                discreteActions[0] = 2;
                break;
        }

        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case -1:
                discreteActions[1] = 1;
                break;
            case 0:
                discreteActions[1] = 0;
                break;
            case +1:
                discreteActions[1] = 2;
                break;
        }

        discreteActions[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;  // Space�� ���� ���� �Է�
        discreteActions[3] = Input.GetKey(KeyCode.V) ? 1 : 0;  // VŰ�� ���� ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Win : Touch the Shooter
        /*
        if (collision.TryGetComponent<Goal>(out Goal goal)) 
        { 
            SetReward(+1f);
            StartCoroutine(ResultColoring(new Color(0.3f, 0.7f, 0.3f, 0.5f)));
            EndEpisode();
        }
        

        // Lose : Touch the Wall
        if (collision.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-1f);
            floorMeshRenderer.color = new Color(0.7f, 0.3f, 0.3f, 0.5f);
            EndEpisode();
        }
        */


        // Lose : Shot by Bullet
        if (collision.TryGetComponent<Bullet>(out Bullet bullet) && (!isInvincible))
        {
            // HitByBullet
            nextAnimationState = 2;
            Destroy(bullet.gameObject);
            StartCoroutine(OnHitColoring(Color.red));
            float targetRangeDamage = targetTransform.GetComponent<MartialAgentNPC>().rangeDamage;
            agent_currentHP -= Mathf.Max(targetRangeDamage - defense, 0.5f);  // �ӽ�:TargetTransform üũ�ؼ� Ÿ���� ������ �����;� ��.
        }
    }

    private void UpdateBulletObjects()
    {
        // Clear the bulletColliders array before filling it with new values
        bulletObjects.Clear();

        // Get all the objects with the "Projectile" tag among all children of this.transform.parent
        Transform[] projectiles = this.transform.parent.GetComponentsInChildren<Transform>().Where(obj => obj.CompareTag("Projectile")).ToArray();
        var projectList = new List<BulletData>();

        // Sort only <RecogRange Projectiles
        foreach (Transform projectile in projectiles)
        {
            Vector3 dirToBullet = (projectile.localPosition - transform.localPosition);
            float disToBullet = dirToBullet.sqrMagnitude;
            if (disToBullet <= RecogRangeSquared)
            {
                // Normalize direction from agent to bullet, because BufferSensor only act with normalized value.
                // Since we have determined the maximum recog range, dividing the direction vector by the maximum recog range will 'preserve the distance' information as well,
                // unlike simply using .normalize, which lose information about distance.
                dirToBullet = dirToBullet / max_RecogRange;

                // Get velocity vector. cause use .normalized, We can 'only' get movement 'direction' of bullet.
                var velOfBullet = projectile.GetComponent<Rigidbody2D>().velocity.normalized;

                BulletData tempBullet = new BulletData(dirToBullet.x, dirToBullet.y,
                                       velOfBullet.x, velOfBullet.y, disToBullet);
                projectList.Add(tempBullet);
            }
        }
        projectList.Sort((a, b) => a.sqrDis.CompareTo(b.sqrDis));

        // input data to bulletObjectsList only 5
        foreach (BulletData bulletData in projectList.Take(5))
        {
            bulletObjects.Add(bulletData);
        }
    }

    void DestroyAllProjectiles()
    {
        // Get all the objects with the "Projectile" tag among all children of this.transform.parent
        Transform[] projectiles = this.transform.parent.GetComponentsInChildren<Transform>().Where(obj => obj.CompareTag("Projectile")).ToArray();

        // Destroy all the projectiles
        foreach (Transform projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }
    }

    private IEnumerator ResultColoring(Color r_color)
    {
        floorMeshRenderer.color = r_color;
        yield return new WaitForSeconds(0.05f);
        // Restore Color of Floor
        floorMeshRenderer.color = Color.white;
    }
    private IEnumerator OnHitColoring(Color r_color)
    {
        Color _color = agent_Renderer.color;
        agent_Renderer.color = r_color;
        yield return new WaitForSeconds(0.05f);
        // Restore Color of Floor
        agent_Renderer.color = _color;
    }

    private IEnumerator MakeTemporalInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1.0f);
        isInvincible = false;
    }

    private void UpdateHPUI()
    {
        agentHPUIIMG.fillAmount = agent_currentHP / agent_maxHP;
    }

    private void AgentDie()  // Handling Death 
    {
        // �÷��̾�� �������� Lose �޽����� �߻���Ų��.
        nextAnimationState = 5;
        SetReward(-1f);
        StartCoroutine(ResultColoring(new Color(0.7f, 0.3f, 0.3f, 0.5f)));

        OnLoseMessageRaised?.Invoke();
        StopAgent();
        //EndEpisode();
    }

    private void AgentWin()  // Handling Win
    {
        AddReward(+1f * (agent_currentHP / agent_maxHP));  // agent�� ü�� �ܷ��� ����� �����带 �޴´�.
        StartCoroutine(ResultColoring(new Color(0.3f, 0.7f, 0.3f, 0.5f)));
        //EndEpisode();
    }

    private void CheckGoal()
    {
        // check Goal
        if (m_GoalSensor is object)
        {
            if (bulletObjects.Count == 0)  // �߻�� ����ü�� �߰ߵ��� �ʾҴٸ�
            {
                float _disToTarget = (targetTransform.localPosition - transform.localPosition).sqrMagnitude;  // ��ǥ���� �Ÿ� ���
                if (_disToTarget < 2.25f)  // �Ÿ��� 1.5 ���ϸ� ��������
                {
                    CurrentGoal = MartialGoal.Melee;
                    nextAnimationState = 0;
                }
                else  // �׷��� �ʴٸ� ����
                {
                    CurrentGoal = MartialGoal.Approach;
                    nextAnimationState = 1;
                }
            }
            else
            {  // ����ü�� �߰ߵǾ��ٸ� ȸ��
                CurrentGoal = MartialGoal.Evasion;
            }
        }
        else
        {
            CurrentGoal = MartialGoal.Evasion;
        }
    }

    private void HitByMelee()
    {
        // HitByMelee
        IsHit = false;
        float targetMeleeDamage = targetTransform.GetComponent<MartialAgentNPC>().meleeDamage;
        agent_currentHP -= Mathf.Max(targetMeleeDamage - defense, 0.5f);  // �ӽ�:TargetTransform üũ�ؼ� Ÿ���� ������ �����;� ��.
        // �˹�
        Vector3 dirToTarget = (targetTransform.localPosition - transform.localPosition).normalized;
        dirToTarget = -dirToTarget;
        transform.localPosition += dirToTarget * 1.1f;
        nextAnimationState = 2;
    }


    // for check battle ends
    public void StopAgent()
    {
        targetTransform = null;
        moveSpeed = 0f;
        agent_Rigid2D.velocity = Vector3.zero;
        m_soundEffectPlayer = null;
        Destroy(this);
    }

    
}
