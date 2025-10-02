using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Facebook.Unity;

public class FacebookLoginManager : MonoBehaviour
{
    private FirebaseAuth auth;

    void Awake()
    {
        if (!FB.IsInitialized)
            FB.Init();
        else
            FB.ActivateApp();

        auth = FirebaseAuth.DefaultInstance;
    }

    public void OnFacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, OnFacebookAuthFinished);
    }

    private void OnFacebookAuthFinished(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            var accessToken = AccessToken.CurrentAccessToken.TokenString;
            var credential = FacebookAuthProvider.GetCredential(accessToken);

            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Firebase Facebook Login lỗi: " + task.Exception);
                    return;
                }

                FirebaseUser newUser = task.Result;
                Debug.Log("Đăng nhập Facebook thành công: " + newUser.DisplayName);
            });
        }
        else
        {
            Debug.Log("Đăng nhập Facebook thất bại hoặc bị hủy.");
        }
    }
}
