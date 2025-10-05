using Unity.Cinemachine;
using UnityEngine;

public class CameraCursorToggle : MonoBehaviour
{
    private CinemachineCamera freeLook;

    void Awake()
    {
        freeLook = GetComponent<CinemachineCamera>();
        if (freeLook == null)
        {
            Debug.LogError("❌ Không tìm thấy CinemachineFreeLook trên Camera này!");
        }
    }

    void Start()
    {
        // thử gán target ngay từ đầu
        AssignPlayerTarget();
        // nếu chưa có thì check lại sau 1 giây
        InvokeRepeating(nameof(AssignPlayerTarget), 0.5f, 1f);
    }

    public void AssignPlayerTarget()
    {
        if (freeLook == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            freeLook.Follow = player.transform;
            freeLook.LookAt = player.transform;
            Debug.Log("🎯 FreeLook Camera đã gán target vào Player!");
            CancelInvoke(nameof(AssignPlayerTarget)); // ngừng loop sau khi gán xong
        }
        else
        {
            Debug.Log("⏳ Đang chờ Player spawn...");
        }
    }
}
