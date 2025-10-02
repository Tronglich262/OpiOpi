using System;
using UnityEngine;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;

public class GoogleSignInManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;

        configuration = new GoogleSignInConfiguration
        {
            WebClientId = "YOUR_WEB_CLIENT_ID.apps.googleusercontent.com",
            RequestIdToken = true
        };

        GoogleSignIn.Configuration = configuration;
    }

    public void OnGoogleSignIn()
    {
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthFinished);
    }

    private void OnGoogleAuthFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Google Sign-In lỗi: " + task.Exception);
            return;
        }

        if (task.IsCanceled)
        {
            Debug.Log("Google Sign-In bị hủy.");
            return;
        }

        GoogleSignInUser googleUser = task.Result;
        Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
        {
            if (authTask.IsCanceled || authTask.IsFaulted)
            {
                Debug.LogError("Firebase Auth lỗi: " + authTask.Exception);
                return;
            }

            FirebaseUser newUser = authTask.Result;
            Debug.Log("Đăng nhập Google thành công: " + newUser.DisplayName);
        });
    }
}
