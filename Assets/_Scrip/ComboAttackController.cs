using UnityEngine;
using UnityEngine.UI; // cần để dùng Button UI

public class PlayerAttackSequence : MonoBehaviour
{
    private Animator animator;

    [Header("Attack Q Settings")]
    private int attackIndexQ = 0;
    private bool isAttackingQ = false;
    public float attackDurationQ = 0.7f;
    private float attackTimerQ;

    [Header("Attack Mouse Settings")]
    private int attackIndexMouse = 5;
    private bool isAttackingMouse = false;
    public float attackDurationMouse = 0.7f;
    private float attackTimerMouse;

    [Header("UI Buttons")]
    public Button attackQButton;       // Button cho attack Q (trước là phím 1)
    public Button attackMouseButton;   // Button cho attack Mouse (trước là phím 2)

    void Start()
    {
        animator = GetComponent<Animator>();

        // gán sự kiện cho button nếu có
        if (attackQButton != null)
            attackQButton.onClick.AddListener(HandleAttackQ);

        if (attackMouseButton != null)
            attackMouseButton.onClick.AddListener(HandleAttackMouse);
    }

    void Update()
    {
        // ================= Attack Q =================
        if (isAttackingQ)
        {
            attackTimerQ += Time.deltaTime;
            if (attackTimerQ >= attackDurationQ)
                EndAttackQ();
        }

        // ================= Attack Mouse =================
        if (isAttackingMouse)
        {
            attackTimerMouse += Time.deltaTime;
            if (attackTimerMouse >= attackDurationMouse)
                EndAttackMouse();
        }
    }

    // ================= Attack Q =================
    void HandleAttackQ()
    {
        if (isAttackingQ) return;

        attackIndexQ++;
        if (attackIndexQ > 4) attackIndexQ = 1;

        PlayAttackQ(attackIndexQ);
    }

    void PlayAttackQ(int index)
    {
        isAttackingQ = true;
        attackTimerQ = 0f;

        animator.SetInteger("AttackIndex", index);
        animator.SetTrigger("Attack");
    }

    void EndAttackQ()
    {
        isAttackingQ = false;
        attackTimerQ = 0f;
        animator.ResetTrigger("Attack");
    }

    // ================= Attack Mouse =================
    void HandleAttackMouse()
    {
        if (isAttackingMouse) return;

        attackIndexMouse++;
        if (attackIndexMouse > 8) attackIndexMouse = 5;

        PlayAttackMouse(attackIndexMouse);
    }

    void PlayAttackMouse(int index)
    {
        isAttackingMouse = true;
        attackTimerMouse = 0f;

        animator.SetInteger("AttackIndex1", index);
        animator.SetTrigger("Attack1");
    }

    void EndAttackMouse()
    {
        isAttackingMouse = false;
        attackTimerMouse = 0f;
        animator.ResetTrigger("Attack1");
    }
}
