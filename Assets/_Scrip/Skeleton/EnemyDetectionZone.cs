using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{
    private EnemyAI enemyAI;

    void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>(); // lấy script EnemyAI cha
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.SetTarget(other.transform);
            enemyAI.PlayerInZone(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.PlayerInZone(false);
        }
    }
}
