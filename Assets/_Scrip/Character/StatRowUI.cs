using UnityEngine;
using UnityEngine.UI;

public class StatRowUI : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public Transform barContainer;  // chứa slot_0..slot_4
    public Button plusButton;
    public Text costText;

    private int currentPoints = 0;
    private int maxPoints = 5;

    private int costPerPoint = 500;

    public void Setup(Sprite iconSprite, int startPoints)
    {
        icon.sprite = iconSprite;
        currentPoints = Mathf.Clamp(startPoints, 0, maxPoints);

        plusButton.onClick.RemoveAllListeners();
        plusButton.onClick.AddListener(OnClickPlus);

        UpdateUI();
    }

    public void OnClickPlus()
    {
        if (currentPoints < maxPoints)
        {
            currentPoints++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // Update bar slots
        for (int i = 0; i < barContainer.childCount; i++)
        {
            var slot = barContainer.GetChild(i).GetComponent<Image>();
            slot.color = (i < currentPoints) ? Color.green : Color.white;
        }

        // Update cost
        if (currentPoints < maxPoints)
            costText.text = costPerPoint.ToString();
        else
            costText.text = "MAX";
    }
    public int GetCurrentPoints()
    {
        return currentPoints;
    }

}
