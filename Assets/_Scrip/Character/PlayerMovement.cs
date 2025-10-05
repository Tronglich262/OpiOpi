using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementBlendTree : MonoBehaviour
{
    public float speed = 4f;
    public float turnSmoothTime = 0.08f;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float fallMultiplier = 2.0f; // rơi nhanh hơn khi thả nút
    private float verticalVelocity;

    [Header("Jump Assist")]
    public float coyoteTime = 0.15f;      // thời gian nhảy sau khi rời đất
    public float jumpBufferTime = 0.15f;  // thời gian lưu input nhảy

    private float coyoteCounter;
    private float jumpBufferCounter;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    private bool isGrounded;
    private bool wasGrounded;

    [Header("Animator")]
    public float animatorDampTime = 0.08f;

    [Header("Joystick + UI")]
    public SimpleJoystick joystick;
    public Button jumpButton;

    public Transform cameraTransform;
    private CharacterController controller;
    private Animator animator;
    private float turnSmoothVelocity;

    private bool jumpRequest = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (joystick == null)
        {
            GameObject joyObj = GameObject.FindGameObjectWithTag("Joystick");
            if (joyObj != null)
                joystick = joyObj.GetComponent<SimpleJoystick>();
        }

        if (jumpButton == null)
        {
            GameObject jumpObj = GameObject.FindGameObjectWithTag("JumpButton");
            if (jumpObj != null)
            {
                jumpButton = jumpObj.GetComponent<Button>();
                jumpButton.onClick.AddListener(OnJumpButtonPressed);
            }
        }
    }

    void Update()
    {
        // ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("IsGrounded", isGrounded);

        // coyote time
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // jump buffer
        if (Input.GetKeyDown(KeyCode.Space) || jumpRequest)
        {
            jumpBufferCounter = jumpBufferTime;
            jumpRequest = false;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // input movement
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

        // nhảy
        if (coyoteCounter > 0f && jumpBufferCounter > 0f)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        // reset khi vừa tiếp đất
        if (isGrounded && !wasGrounded)
        {
            animator.ResetTrigger("Jump");
        }

        // gravity
        if (verticalVelocity < 0) // đang rơi
            verticalVelocity += gravity * fallMultiplier * Time.deltaTime;
        else
            verticalVelocity += gravity * Time.deltaTime;

        // Variable Jump Height (nhảy thấp nếu nhả nút sớm)
        if (verticalVelocity > 0 && !Input.GetKey(KeyCode.Space))
        {
            verticalVelocity += gravity * (fallMultiplier - 1) * Time.deltaTime;
        }

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

        wasGrounded = isGrounded;
    }

    void OnJumpButtonPressed()
    {
        jumpRequest = true;
    }
}
