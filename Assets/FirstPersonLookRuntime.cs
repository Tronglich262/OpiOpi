using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FirstPersonLookRuntime_Forward : MonoBehaviour
{
    [Header("Cài đặt góc nhìn")]
    public float sensitivity = 0.15f;
    public float headOffset = 1.6f;
    public float forwardOffset = 0.2f;
    public float playerTurnSmooth = 10f;

    [Header("Tự tìm Player")]
    public bool autoFindPlayer = true;
    public string playerTag = "Player";

    private Transform playerBody;
    private float pitch = 0f;
    private float yaw = 0f;
    private bool playerFound = false;
    private int touchId = -1;

    [Header("Chuyển góc nhìn")]
    public Button camera;
    bool isFPP = true;

    void Start()
    {
        if (autoFindPlayer)
            StartCoroutine(WaitForPlayerSpawn());

        if (camera != null)
            camera.onClick.AddListener(ToggleView);
    }

    public void ToggleView()
    {
        if (isFPP) { TPP(); isFPP = false; }
        else { FPP(); isFPP = true; }
    }
    public void TPP() => forwardOffset = -2.71f;
    public void FPP() => forwardOffset = 1.1f;

    void LateUpdate()
    {
        if (!playerFound || playerBody == null) return;

        // ----- Input -----
#if UNITY_EDITOR || UNITY_STANDALONE
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * 20f;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * 20f;
        yaw += mouseX;
        pitch -= mouseY;
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif

        pitch = Mathf.Clamp(pitch, -80f, 80f);

        // ----- Xoay Camera -----
        Quaternion camRot = Quaternion.Euler(pitch, yaw, 0f);
        transform.rotation = camRot;

        // ----- Xoay Player mượt theo camera -----
        float targetYaw = Mathf.LerpAngle(playerBody.eulerAngles.y, yaw, Time.deltaTime * playerTurnSmooth);
        playerBody.rotation = Quaternion.Euler(0f, targetYaw, 0f);

        // ----- Vị trí Camera -----
        Vector3 headPos = playerBody.position + Vector3.up * headOffset;
        Vector3 offset = camRot * Vector3.forward * forwardOffset;
        transform.position = headPos + offset;
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            touchId = -1;
            return;
        }

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began && touchId == -1)
                touchId = t.fingerId;
            else if (t.fingerId == touchId && t.phase == TouchPhase.Moved)
            {
                yaw += t.deltaPosition.x * sensitivity;
                pitch -= t.deltaPosition.y * sensitivity;
            }
            else if (t.fingerId == touchId &&
                    (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
                touchId = -1;
        }
    }

    IEnumerator WaitForPlayerSpawn()
    {
        GameObject playerObj = null;
        while (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag(playerTag);
            yield return new WaitForSeconds(0.2f);
        }

        playerBody = playerObj.transform;
        playerFound = true;
        yaw = playerBody.eulerAngles.y;
        pitch = 0f;

        Debug.Log($"🎯 Camera đã gán Player runtime: {playerBody.name}");
    }

    public void SetPlayer(Transform newPlayer)
    {
        playerBody = newPlayer;
        playerFound = true;
        yaw = newPlayer.eulerAngles.y;
    }
}
