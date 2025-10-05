using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float holdTime = 0.3f; // giữ bao lâu thì hiện panel
    private float holdTimer;
    private bool isHolding;

    private SkillData skill;
    private SkillUIManager uiManager;

    // gọi từ SkillUIManager
    public void Setup(SkillData data, SkillUIManager manager)
    {
        skill = data;
        uiManager = manager;
    }

    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                uiManager.ShowSkillDescription(skill);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        holdTimer = 0f;
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        uiManager.ClosePanel();
    }
}
