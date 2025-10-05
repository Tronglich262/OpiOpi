using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform characterPreviewPoint;
    public Text nameText;

    [Header("Stat UI")]
    public StatRowUI statRowPrefab;
    public Transform statContainer;

    [Header("Stat Icons")]
    public Sprite atkIcon;
    public Sprite hpIcon;
    public Sprite cuteIcon;
    public Sprite spdIcon;

    [Header("Prefabs")]
    public GameObject[] characterPrefabs;
    private GameObject currentCharacter;
    private int currentIndex = 0;

    [Header("Skills")]
    public SkillUIManager skillUIManager;
    public CharacterSkills[] characterSkills;

    [Header("Play Button")]
    public Button playButton;

    void Start()
    {
        SelectCharacter(0);

        if (playButton != null)
            playButton.onClick.AddListener(OnPlayPressed);

    }

    private void ClearStats()
    {
        foreach (Transform child in statContainer)
            Destroy(child.gameObject);
    }

    public void SelectCharacter(int index)
    {
        if (currentCharacter != null)
            Destroy(currentCharacter);

        currentIndex = index;

        // Load skill UI theo nhân vật
        if (skillUIManager != null && index < characterSkills.Length)
        {
            skillUIManager.LoadSkills(characterSkills[index]);
        }

        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        Vector3 position = new Vector3(0, -1.21f, 0);
        currentCharacter = Instantiate(characterPrefabs[index], position, rotation);
        currentCharacter.transform.localScale = Vector3.one * 2f;

        CharacterData data = characterPrefabs[index].GetComponent<CharacterData>();
        if (data != null)
        {
            nameText.text = data.characterName;

            ClearStats();
            CreateStatRow("ATK", data.attack, atkIcon);
            CreateStatRow("HP", data.hp, hpIcon);
            CreateStatRow("CUTE", data.cuteness, cuteIcon);
            CreateStatRow("SPD", data.speed, spdIcon);
        }
    }

    private void CreateStatRow(string name, int value, Sprite icon)
    {
        var row = Instantiate(statRowPrefab, statContainer);
        int scaledValue = Mathf.Clamp(value / 10, 0, 5);
        row.Setup(icon, scaledValue);
    }

    private void OnPlayPressed()
    {
        // Lưu chỉ số nhân vật đã chọn
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);

        // ⭐ Lưu bộ kỹ năng của nhân vật đã chọn
        if (characterSkills != null && currentIndex < characterSkills.Length)
        {
            PlayerDataHolder.SelectedCharacterSkills = characterSkills[currentIndex];
            PlayerDataHolder.SelectedCharacterIndex = currentIndex;
            Debug.Log($"✅ Đã lưu skill của nhân vật: {characterSkills[currentIndex].name}");
        }

        // Load scene game chính
        SceneManager.LoadScene("Play");
    }
}
