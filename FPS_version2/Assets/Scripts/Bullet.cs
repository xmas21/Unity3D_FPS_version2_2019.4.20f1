using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("子彈傷害值")]
    public float damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
