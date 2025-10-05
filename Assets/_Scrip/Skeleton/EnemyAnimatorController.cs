using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    private Animator animator;

    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int AttackHash = Animator.StringToHash("Attack");
    private readonly int AttackKindHash = Animator.StringToHash("AttackIndex");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Cập nhật animation di chuyển
    public void UpdateMovement(Vector3 move, bool isGrounded)
    {
        float speed = new Vector2(move.x, move.z).magnitude;
        animator.SetFloat(SpeedHash, speed);
        animator.SetBool(IsGroundedHash, isGrounded);
    }

    // Gọi tấn công
    public void PlayAttack(int index)
    {
        animator.SetInteger(AttackKindHash, index);
        animator.SetTrigger(AttackHash);
    }

    // Lấy thời lượng clip hiện tại
    public float GetCurrentClipLength()
    {
        if (animator == null) return 0f;
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
            return clipInfo[0].clip.length;
        return 0f;
    }
}
