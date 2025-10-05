using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(EnemyAnimatorController))]
public class EnemyAI : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 2f;

    [Header("Chase")]
    public float chaseSpeed = 3.5f;

    [Header("Attack")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    private int currentPoint = 0;
    private float lastAttackTime;
    private bool isAttacking = false;
    private bool isWaitingPatrol = false;
    private float patrolWaitTimer = 0f;

    private Transform player;
    private CharacterController controller;
    private EnemyAnimatorController animController;
    private Animator animator;

    private Vector3 velocity;
    private float gravity = -9.81f;
    private bool isGrounded;
    private bool playerInZone = false; // ⭐ trạng thái có player trong vùng hay không

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animController = GetComponent<EnemyAnimatorController>();
        animator = GetComponent<Animator>();

        if (animator != null)
            animator.applyRootMotion = false;
    }

    void Update()
    {
        // Nếu đang attack → đứng yên
        if (isAttacking)
        {
            velocity.y = -2f;
            controller.Move(velocity * Time.deltaTime);
            animController.UpdateMovement(Vector3.zero, true);
            return;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector3 moveXZ = Vector3.zero;

        if (playerInZone && player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);

            // ⭐ Nếu trong vùng và đủ gần thì tấn công
            if (dist <= attackRange)
            {
                moveXZ = Vector3.zero;
                TryAttack();
            }
            else
            {
                // ⭐ Nếu Player trong vùng nhưng chưa đủ gần thì đuổi theo
                Vector3 dir = (player.position - transform.position).normalized;
                moveXZ = dir * chaseSpeed;
            }
        }
        else
        {
            // ⭐ Nếu player ra khỏi vùng → quay lại patrol
            moveXZ = Patrol();
        }

        // Move logic
        if (moveXZ.sqrMagnitude > 0.01f)
        {
            controller.Move(moveXZ * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(new Vector3(moveXZ.x, 0, moveXZ.z));
            animController.UpdateMovement(moveXZ, true);
        }
        else
        {
            animController.UpdateMovement(Vector3.zero, true);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    // ========== PATROL ==========
    Vector3 Patrol()
    {
        if (patrolPoints.Length == 0) return Vector3.zero;

        if (isWaitingPatrol)
        {
            patrolWaitTimer += Time.deltaTime;
            if (patrolWaitTimer >= waitTimeAtPoint)
            {
                isWaitingPatrol = false;
                patrolWaitTimer = 0;
            }
            return Vector3.zero;
        }

        Transform target = patrolPoints[currentPoint];
        Vector3 dir = (target.position - transform.position);
        dir.y = 0;
        Vector3 move = dir.normalized * patrolSpeed;

        if (Vector3.Distance(transform.position, target.position) < 0.6f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            isWaitingPatrol = true;
        }

        return move;
    }

    // ========== ATTACK ==========
    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            lastAttackTime = Time.time;
            int index = Random.Range(0, 4);
            StartCoroutine(PerformAttack(index));
        }
    }

    IEnumerator PerformAttack(int index)
    {
        isAttacking = true;
        animController.PlayAttack(index);
        velocity.y = -2f;

        float animTime = animController.GetCurrentClipLength();
        if (animTime <= 0f) animTime = 1f;

        yield return new WaitForSeconds(animTime + 0.2f);

        isAttacking = false;
    }

    // ========== ZONE LOGIC ==========
    public void PlayerInZone(bool inZone)
    {
        playerInZone = inZone;
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }
}
