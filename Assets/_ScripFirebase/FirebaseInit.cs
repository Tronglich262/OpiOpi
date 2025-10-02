using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase đã khởi tạo thành công!");
            }
            else
            {
                Debug.LogError("Không thể khởi tạo Firebase: " + dependencyStatus);
            }
        });
    }
}
