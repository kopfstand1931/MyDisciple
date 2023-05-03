using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class EvasionAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SpriteRenderer floorMeshRenderer;
    private BufferSensorComponent evasion_BufferSensor;

    private Rigidbody2D agent_Rigid2D;  

    private float max_RecogRange = 8f;
    private float RecogRangeSquared;

    public override void Initialize()
    {
        evasion_BufferSensor = GetComponent<BufferSensorComponent>();
        RecogRangeSquared = Mathf.Pow(max_RecogRange, 2f);
        agent_Rigid2D = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        // Destory any bullets remain on environment
        var bullets = new List<GameObject>();
        for (int i = 3; i < transform.parent.childCount; i++)
        {
            var tempChild = transform.parent.GetChild(i);
            if (tempChild != null && tempChild.tag == "Projectile")
            {
                bullets.Add(tempChild.gameObject);
            }
        }
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null) { Destroy(bullet); }
        }
        bullets.Clear();


        // relocate agent and goal randomly
        transform.localPosition = new Vector3(Random.Range(-8f, 3f), Random.Range(-8f, 3f), 0);
        targetTransform.localPosition = new Vector3(Random.Range(0, 5f), Random.Range(0, 5f), 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
        // Static length observation
        // Observe direction vector from Agent to Target
        Vector3 dirToTarget = (targetTransform.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(dirToTarget.x);
        sensor.AddObservation(dirToTarget.y);


        // Variable length observation with using Buffer Sensor
        var bullets = new List<GameObject>();
        for (int i = 3; i < transform.parent.childCount; i++)
        {
            var tempChild = transform.parent.GetChild(i);
            if (tempChild != null && tempChild.tag == "Projectile")
            {
                bullets.Add(tempChild.gameObject);
            }
        }
        foreach (GameObject bullet in bullets.Take(5))
        {
            if (bullet != null) 
            {
                // Check if the bullet is in reception range. We use sqrMagnitude (for performance), so Compare 'distance' to 'square of max_RecogRange'
                Vector3 dirToBullet = (bullet.transform.localPosition - transform.localPosition);
                float disToBullet = dirToBullet.sqrMagnitude;
                if (disToBullet <= RecogRangeSquared)
                {

                    // Normalize direction from agent to bullet, because BufferSensor only act with normalized value.
                    // Since we have determined the maximum recog range, dividing the direction vector by the maximum recog range will 'preserve the distance' information as well,
                    // unlike simply using .normalize, which lose information about distance.
                    dirToBullet = dirToBullet / max_RecogRange;

                    // Get velocity vector. cause use .normalized, We can 'only' get movement 'direction' of bullet.
                    var velOfBullet = bullet.GetComponent<Rigidbody2D>().velocity.normalized;

                    // and We can get velocity by .magnitude, but to normalize, we should know about maxium of magnitude of velocity. So do not use now.
                    // Debug.Log(bullet.GetComponent<Rigidbody2D>().velocity.Magnitude);

                    // Input Obervation to BufferSensor. Agent observe normalized direction To Bullet(with distance), and velocity of bullet.
                    float[] tempObserv = { dirToBullet.x, dirToBullet.y,
                                       velOfBullet.x, velOfBullet.y};
                    evasion_BufferSensor.AppendObservation(tempObserv);
                }
            }
        }
        bullets.Clear();

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float moveSpeed = 5f;

        // transform.localPosition += new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed;
        agent_Rigid2D.velocity = new Vector3(moveX, moveY, 0) * moveSpeed;

        AddReward(0.0001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Win : Touch the Shooter
        if (collision.TryGetComponent<Goal>(out Goal goal)) 
        { 
            AddReward(+1f);
            floorMeshRenderer.color = new Color(0.3f, 0.7f, 0.3f, 0.5f);
            EndEpisode();
        }

        /*
        // Lose : Touch the Wall
        if (collision.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-1f);
            floorMeshRenderer.color = new Color(0.7f, 0.3f, 0.3f, 0.5f);
            EndEpisode();
        }
        */

        // Lose : Shot by Bullet
        if (collision.TryGetComponent<Bullet>(out Bullet bullet))
        {
            AddReward(-1f);
            floorMeshRenderer.color = new Color(0.7f, 0.3f, 0.3f, 0.5f);
            EndEpisode();
        }
    }

}
