using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;   // ⭐ BẮT BUỘC phải có
using System.Threading.Tasks;
using System.Collections;

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("UI")]
    public InputField emailInput;
    public InputField passwordInput;
    public Text logText;
    public GameObject dangky;   // Panel Đăng ký
    public GameObject banglogin;
    public GameObject dangnhapbtn; // Panel Đăng nhập
    public GameObject dangkybtn; // Panel Đăng nhập


    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
       logText.text = "";
    }

    // Đăng ký
    public void Register()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Register error: " + task.Exception);
                logText.text = " Đăng ký lỗi: " + task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            user = task.Result.User;        
            Debug.Log("User created: " + user.UserId);
            StartCoroutine(deleytext());

            // Ẩn panel đăng ký, hiện panel đăng nhập
            dangkybtn.SetActive(false);
            dangnhapbtn.SetActive(true);
        });
    }
    IEnumerator deleytext()
    {
        logText.text = " Đăng ký thành công: " + user.Email;
        yield return new WaitForSeconds(2f);
        logText.text = "";

    }

    // Đăng nhập
    public void Login()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Login error: " + task.Exception);
                logText.text = " Đăng nhập lỗi: " + task.Exception.Flatten().InnerExceptions[0].Message;
                return;
            }

            user = task.Result.User;
            Debug.Log("Login success, UserID: " + user.UserId);

            logText.text = " Đăng nhập thành công: " + user.Email;

            // Load scene sau khi đăng nhập thành công
            SceneManager.LoadScene("Menu");
        });
    }
    public void dangky1()
    {
        dangky.SetActive(true);
        dangnhapbtn.SetActive(false);
        dangkybtn.SetActive(true);
        banglogin.SetActive(false);
        
    }
    public void dangnhap1()
    {

        dangky.SetActive(true);
        dangkybtn.SetActive(false);

        dangnhapbtn.SetActive(true);
        banglogin.SetActive(false);

    }
    public void back() 
    {
        dangky.SetActive(false);
        banglogin.SetActive(true);
    
    }
}
