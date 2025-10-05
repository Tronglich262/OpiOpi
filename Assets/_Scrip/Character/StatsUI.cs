using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public Text hpText;
    public Text attackText;
    public Text cuteText;
    public Text speedText;
    public Image avatarImage;

    private CharacterStats stats;

    public void Setup(CharacterStats characterStats)
    {
        stats = characterStats;
        if (stats.data.characterSprite != null)
            avatarImage.sprite = stats.data.characterSprite;

        stats.onStatsChanged += UpdateUI;
        UpdateUI();
    }

    void UpdateUI()
    {
        hpText.text = $"HP: {stats.currentHP}/{stats.maxHP}";
        attackText.text = $"ATK: {stats.attack}";
        cuteText.text = $"Cute: {stats.cuteness}";
        speedText.text = $"SPD: {stats.speed}";
    }
}
