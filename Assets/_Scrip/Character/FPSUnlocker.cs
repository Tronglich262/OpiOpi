using UnityEngine;

public class FPSUnlocker : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;        // Tắt VSync
        Application.targetFrameRate = 120;      // Hoặc 120 nếu máy hỗ trợ
    }
}
