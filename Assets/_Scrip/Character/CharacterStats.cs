using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    public CharacterData data;

    [Header("Runtime Stats")]
    public int currentHP;
    public int maxHP;
    public int attack;
    public int speed;
    public int cuteness;

    public event Action onStatsChanged;

    void Start()
    {
        LoadFromData(data);
    }

    public void LoadFromData(CharacterData d)
    {
        data = d;

        // chỉ số gốc
        maxHP = d.hp;
        attack = d.attack;
        speed = d.speed;
        cuteness = d.cuteness;

        // cộng thêm từ PlayerPrefs (cộng điểm ngoài menu)
        maxHP += PlayerPrefs.GetInt("Stat_HP", 0);
        attack += PlayerPrefs.GetInt("Stat_ATK", 0);
        cuteness += PlayerPrefs.GetInt("Stat_CUTE", 0);
        speed += PlayerPrefs.GetInt("Stat_SPD", 0);

        currentHP = maxHP;

        onStatsChanged?.Invoke();
    }

    // Buff / Damage
    public void TakeDamage(int dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);
        onStatsChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        onStatsChanged?.Invoke();
    }
}
