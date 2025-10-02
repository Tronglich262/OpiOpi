using UnityEngine;

public class CameraCursorToggle : MonoBehaviour
{
    private bool cursorLocked = true;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // giữ shift thì mở chuột
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // khóa chuột giữa màn hình
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // trả chuột tự do
        Cursor.visible = true;
    }
}
