using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private float scale_x;
    private Animator animator;

    // 간단한 옵저버 디자인 패턴
    // State
    // 0 = idle
    // 1 = run
    // 2 = takeHit
    // 3 = attack1
    // 4 = attack2
    // 5 = death
    private int previousState;
    private int currentState;

    // 좌우반전 판정
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
        // 좌우반전 반영
        transform.localScale = IsRight ? new Vector3(scale_x, transform.localScale.y, transform.localScale.z) : new Vector3(-scale_x, transform.localScale.y, transform.localScale.z);

        // 상태가 변화한 경우에만 애니메이션 재생에 변화
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
