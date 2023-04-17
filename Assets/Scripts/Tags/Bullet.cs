using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Wall>(out Wall wall))
        {
            Destroy(gameObject);
        }
        if (collision.TryGetComponent<EvasionAgent>(out EvasionAgent evasionAgent))
        {
            Destroy(gameObject);
        }
    }
}
