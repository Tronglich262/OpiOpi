using UnityEngine;

public class CharacterStatPanel : MonoBehaviour
{
    public StatRowUI hpRow;
    public StatRowUI attackRow;
    public StatRowUI cuteRow;
    public StatRowUI speedRow;

    // gọi hàm này sau khi đã spawn 4 stat row
    public void AssignRows()
    {
        var rows = GetComponentsInChildren<StatRowUI>(true);

        if (rows.Length >= 4)
        {
            hpRow = rows[0];
            attackRow = rows[1];
            cuteRow = rows[2];
            speedRow = rows[3];

            Debug.Log("✅ Đã gán 4 StatRowUI cho panel.");
        }
        else
        {
            Debug.LogError($"❌ CharacterStatPanel: Tìm thấy {rows.Length} StatRowUI, cần 4!");
        }
    }

    public void SaveStats()
    {
        if (hpRow) PlayerPrefs.SetInt("Stat_HP", hpRow.GetCurrentPoints() * 100);
        if (attackRow) PlayerPrefs.SetInt("Stat_ATK", attackRow.GetCurrentPoints() * 10);
        if (cuteRow) PlayerPrefs.SetInt("Stat_CUTE", cuteRow.GetCurrentPoints() * 5);
        if (speedRow) PlayerPrefs.SetInt("Stat_SPD", speedRow.GetCurrentPoints() * 1);
    }
}
