using UnityEngine;
using UnityEngine.UI; // để dùng Button

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementBlendTree : MonoBehaviour
{
    public float speed = 4f;
    public float turnSmoothTime = 0.08f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float fallMultiplier = 2.0f; // rơi nhanh hơn
    private float verticalVelocity;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    private bool isGrounded;
    private bool wasGrounded;

    [Header("Animator")]
    public float animatorDampTime = 0.08f;

    [Header("Joystick + UI")]
    public SimpleJoystick joystick;   // joystick tự tạo
    public Button jumpButton;         // nút Jump UI

    public Transform cameraTransform;
    private CharacterController controller;
    private Animator animator;
    private float turnSmoothVelocity;

    private bool jumpRequest = false; // flag để nhận lệnh nhảy từ UI

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        // gán sự kiện cho nút Jump
        if (jumpButton != null)
            jumpButton.onClick.AddListener(OnJumpButtonPressed);
    }

    void Update()
    {
        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("IsGrounded", isGrounded);

        // input movement từ joystick
        float h = joystick != null ? joystick.Horizontal : Input.GetAxis("Horizontal");
        float v = joystick != null ? joystick.Vertical : Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0f, v);
        float inputMag = Mathf.Clamp01(input.magnitude);

        // camera-relative moveDir
        Vector3 moveDir = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            moveDir = (cameraTransform.right * h + camForward * v).normalized;
        }
        else moveDir = (transform.right * h + transform.forward * v).normalized;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // jump
        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        // nhảy bằng Space hoặc nút UI
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || jumpRequest))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
            Debug.Log("Chạy jump");
            jumpRequest = false; // reset flag
        }

        // reset khi vừa tiếp đất
        if (isGrounded && !wasGrounded)
        {
            animator.ResetTrigger("Jump");
        }

        // gravity + fallMultiplier
        if (verticalVelocity < 0) // đang rơi
            verticalVelocity += gravity * fallMultiplier * Time.deltaTime;
        else
            verticalVelocity += gravity * Time.deltaTime;

        // apply movement
        Vector3 velocity = moveDir * speed * inputMag;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        // animator params
        Vector3 localMove = transform.InverseTransformDirection(moveDir * inputMag);
        animator.SetFloat("MoveX", localMove.x, animatorDampTime, Time.deltaTime);
        animator.SetFloat("MoveZ", localMove.z, animatorDampTime, Time.deltaTime);
        animator.SetFloat("Run", inputMag, animatorDampTime, Time.deltaTime);

        animator.SetFloat("VerticalVelocity", verticalVelocity);

        // cập nhật trạng thái trước đó
        wasGrounded = isGrounded;
    }

    void OnJumpButtonPressed()
    {
        jumpRequest = true; // khi ấn nút UI → set flag để Update xử lý
    }
}
