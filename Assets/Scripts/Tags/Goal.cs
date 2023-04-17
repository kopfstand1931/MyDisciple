using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Transform targetTransform;
    public GameObject bullet;
    public float speed = 10f; 

    

    void Start()
    {
        InvokeRepeating("ShootBullet", 1f, 1f); // 1초뒤 1초주기로 ShootBullet 함수 반복 호출
    }

    void ShootBullet()
    {
        targetTransform = this.transform.parent.GetChild(1);
        var temp_Distance = Vector3.Distance(transform.position, targetTransform.position);
        if (temp_Distance < 2f)
        {

        }
        else
        {
            Vector3 dirToTarget = (targetTransform.localPosition - this.transform.localPosition).normalized;
            GameObject newBullet =
                Instantiate(bullet, this.transform.position,
                Quaternion.identity, this.transform.parent) as GameObject;
            newBullet.GetComponent<Rigidbody2D>().velocity = dirToTarget * speed;
        }
        
    }
}
