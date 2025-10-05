using UnityEngine;
using System.Collections;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characters;
    public Transform spawnPoint;

    void Start()
    {
        StartCoroutine(SpawnAfterFrame());
    }

    IEnumerator SpawnAfterFrame()
    {
        yield return null; // đợi 1 frame cho Canvas/UI load

        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject player = Instantiate(characters[selectedCharacter], spawnPoint.position, spawnPoint.rotation);

        CharacterData data = player.GetComponent<CharacterData>();
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats != null && data != null)
        {
            stats.LoadFromData(data);

            StatsUI ui = FindObjectOfType<StatsUI>();
            if (ui != null)
                ui.Setup(stats);
            else
                Debug.LogWarning("⚠️ Không tìm thấy StatsUI trong scene!");
        }
        else
        {
            Debug.LogError("❌ CharacterStats hoặc CharacterData bị thiếu trên prefab!");
        }
    }
}
