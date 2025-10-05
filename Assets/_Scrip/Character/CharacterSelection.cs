using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;   // Gán 5 prefab nhân vật vào Inspector
    public Transform spawnPoint;      // Vị trí hiển thị nhân vật
    public Text characterNameText;    // Text UI hiển thị tên nhân vật

    private int currentIndex = 0;
    private GameObject currentCharacter;

    void Start()
    {
        ShowCharacter(currentIndex);
    }

    public void NextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characters.Length) currentIndex = 0;
        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = characters.Length - 1;
        ShowCharacter(currentIndex);
    }

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);

        // lưu stat từ panel
        FindObjectOfType<CharacterStatPanel>().SaveStats();

        SceneManager.LoadScene("Play");
    }


    private void ShowCharacter(int index)
    {
        if (currentCharacter != null) Destroy(currentCharacter);
        Vector3 vector3 = new Vector3(0, 0, 0); // điều chỉnh vị trí nhân vật
        Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);
        currentCharacter = Instantiate(characters[index], vector3, spawnRotation);
       // currentCharacter.transform.SetParent(spawnPoint); // để không bị bay lung tung
        characterNameText.text = characters[index].name;   // hiển thị tên
    }
}
