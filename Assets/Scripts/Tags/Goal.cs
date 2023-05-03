using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Transform targetTransform;
    public GameObject bullet;
    public float bulletSpeed = 10f;

    public float moveSpeed = 2.5f;
    public float moveRange = 9.5f;

    private Vector3 moveDirection;



    void Start()
    {
        InvokeRepeating("ShootBullet", 0f, Random.Range(1.5f,3f)); // 0초뒤 1.5~3초주기로 ShootBullet 함수 반복 호출
        
        InvokeRepeating("MakeRandomDirection", 0f, 1f); // Set a random initial move direction
    }

    private void Update()
    {
        // Move the object in the current move direction
        transform.localPosition += moveDirection * moveSpeed * Time.deltaTime;

        // Clamp the position within the move range
        Vector3 clampedPosition = transform.localPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -moveRange, moveRange);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -moveRange, moveRange);
        transform.localPosition = clampedPosition;

        // Check if the object has reached the edge of the move range
        if (Mathf.Abs(transform.localPosition.x) >= moveRange || Mathf.Abs(transform.localPosition.y) >= moveRange)
        {
            // Reverse the move direction
            moveDirection = -moveDirection;
        }
    }

    void ShootBullet()
    {
        targetTransform = this.transform.parent.GetChild(1);
        var temp_Distance = Vector3.Distance(transform.position, targetTransform.position);
        if (temp_Distance < 3f)
        {

        }
        else
        {
            Vector3 dirToTarget = (targetTransform.localPosition - this.transform.localPosition).normalized;
            GameObject newBullet =
                Instantiate(bullet, this.transform.position,
                Quaternion.identity, this.transform.parent) as GameObject;
            newBullet.GetComponent<Rigidbody2D>().velocity = dirToTarget * bulletSpeed;
        }
        
    }

    void MakeRandomDirection()
    {
        moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
    }
}
