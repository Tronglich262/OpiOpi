using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public int characterIndex; // index trong mảng characterPrefabs
    public Button button;

    void Start()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Tìm object CharacterSelectUI trong scene
        CharacterSelectUI ui = FindObjectOfType<CharacterSelectUI>();
        if (ui != null)
        {
            ui.SelectCharacter(characterIndex);
        }
    }
}
