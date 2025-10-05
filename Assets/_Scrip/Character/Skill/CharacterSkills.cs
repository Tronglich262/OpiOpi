using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSkills", menuName = "Game/Character Skills")]
public class CharacterSkills : ScriptableObject
{
    public SkillData[] skills = new SkillData[3]; // đúng 3 skill
}
