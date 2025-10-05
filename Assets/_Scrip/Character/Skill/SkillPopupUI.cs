using UnityEngine;
using UnityEngine.UI;

public class SkillPopupUI : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public GameObject rootPanel;

    private void Awake()
    {
        Hide();
    }

    public void Show(SkillData skill)
    {
        rootPanel.SetActive(true);
        nameText.text = skill.skillName;
        descriptionText.text = skill.description;
    }

    public void Hide()
    {
        rootPanel.SetActive(false);
    }
}
