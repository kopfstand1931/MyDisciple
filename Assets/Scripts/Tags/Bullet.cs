using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject m_vfxPrefab;

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

    public void Awake()
    {
        if (m_vfxPrefab != null)
        {
            m_vfxPrefab = Instantiate(m_vfxPrefab, transform.position, transform.rotation);

            // Schedule the destruction of the prefab after 0.5 seconds
            Destroy(m_vfxPrefab, 0.5f);
        }
    }
}
