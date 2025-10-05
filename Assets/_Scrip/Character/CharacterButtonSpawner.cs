using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform contentParent;
    public CharacterSelectUI selectUI;

    void Start()
    {
        for (int i = 0; i < selectUI.characterPrefabs.Length; i++)
        {
            GameObject btnObj = Instantiate(buttonPrefab, contentParent);

            // Lấy dữ liệu nhân vật
            CharacterData data = selectUI.characterPrefabs[i].GetComponent<CharacterData>();

            // Gán text
            Text txt = btnObj.GetComponentInChildren<Text>();
            if (txt != null)
                txt.text = data.characterName;

            // Gán ảnh
            Image img = btnObj.transform.Find("CharacterImage")?.GetComponent<Image>();
            if (img != null && data.characterSprite != null)
                img.sprite = data.characterSprite;

            // Gán chỉ số index cho button
            CharacterButton cb = btnObj.GetComponent<CharacterButton>();
            cb.characterIndex = i;
        }
    }
}
