using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private float scale_x;
    private Animator animator;

    // ������ ������ ������ ����
    // State
    // 0 = idle
    // 1 = run
    // 2 = takeHit
    // 3 = attack1
    // 4 = attack2
    // 5 = death
    private int previousState;
    private int currentState;

    // �¿���� ����
    private bool isRight;
    public bool IsRight
    {
        set { isRight = value; }
        get { return isRight; }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        scale_x = transform.localScale.x;
        previousState = 0; currentState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �¿���� �ݿ�
        transform.localScale = IsRight ? new Vector3(scale_x, transform.localScale.y, transform.localScale.z) : new Vector3(-scale_x, transform.localScale.y, transform.localScale.z);

        // ���°� ��ȭ�� ��쿡�� �ִϸ��̼� ����� ��ȭ
        if (currentState != previousState)
        {
            // Only react when the status changes
            ReactToState(currentState);
        }

        previousState = currentState;

    }

    public void SetCurrentState(int value)
    {
        currentState = value;
    }

    private void ReactToState(int value)
    {
        animator.SetInteger("state", value);
    }

    public void EndAnimation()
    {
        animator.SetTrigger("EndAnimation");
    }

}
