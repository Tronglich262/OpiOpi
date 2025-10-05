using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Button[] skillButtons;      // 3 button skill
    public Image[] skillIcons;         // Icon trong 3 button

    [Header("Skill Panel")]
    public GameObject skillPanel;      // Panel hiện mô tả skill
    public Text skillDescriptionText;  // Text hiển thị thông tin skill

    private CharacterSkills currentSkills;

    void Start()
    {
        if (skillPanel != null)
            skillPanel.SetActive(false);

        // ⭐ Tự động load kỹ năng từ nhân vật đã chọn
        if (PlayerDataHolder.SelectedCharacterSkills != null)
        {
            LoadSkills(PlayerDataHolder.SelectedCharacterSkills);
            Debug.Log($"🎯 SkillUIManager: Load kỹ năng của {PlayerDataHolder.SelectedCharacterSkills.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ Không có dữ liệu kỹ năng — có thể chưa chọn nhân vật!");
        }
    }

    public void LoadSkills(CharacterSkills charSkills)
    {
        currentSkills = charSkills;

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < currentSkills.skills.Length && currentSkills.skills[i] != null)
            {
                skillIcons[i].sprite = currentSkills.skills[i].icon;
                skillIcons[i].enabled = true;

                // Gắn script giữ nút
                SkillButtonHold hold = skillButtons[i].GetComponent<SkillButtonHold>();
                if (hold == null)
                    hold = skillButtons[i].gameObject.AddComponent<SkillButtonHold>();

                hold.Setup(currentSkills.skills[i], this);
            }
            else
            {
                skillIcons[i].sprite = null;
                skillIcons[i].enabled = false;
            }
        }

        if (skillPanel != null)
            skillPanel.SetActive(false);
    }


    public void ShowSkillDescription(SkillData skill)
    {
        if (skillPanel != null)
        {
            skillPanel.SetActive(true);
            skillDescriptionText.text = $"{skill.skillName}\n{skill.description}";
        }
    }

    public void ClosePanel()
    {
        if (skillPanel != null)
            skillPanel.SetActive(false);
    }

}
