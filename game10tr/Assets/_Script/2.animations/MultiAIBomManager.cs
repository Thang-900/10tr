using UnityEngine;

public class MultiAIBomManager : MonoBehaviour
{
    private AIBomSaveSystem[] allAI;

    void Awake()
    {
        allAI = FindObjectsOfType<AIBomSaveSystem>();
    }

    public void SaveAll()
    {
        foreach (var ai in allAI)
            ai.Save();

        PlayerPrefs.Save();
        Debug.Log("Đã lưu tất cả AIBom.");
    }

    public void LoadAll()
    {
        foreach (var ai in allAI)
            ai.Load();
    }
}
