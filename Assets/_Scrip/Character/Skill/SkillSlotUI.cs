using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    public Image iconImage;
    private SkillData skillData;
    private SkillPopupUI popup;

    public void Setup(SkillData skill, SkillPopupUI popupUI)
    {
        skillData = skill;
        popup = popupUI;
        iconImage.sprite = skill.icon;
    }

    // Gọi từ OnClick() trong Button
    public void OnClickSkill()
    {
        if (skillData != null && popup != null)
        {
            popup.Show(skillData);
        }
    }
}
