using UnityEngine;

public class AIBomSaveSystem : MonoBehaviour
{
    private AIBom aiBom;

    [Header("ID duy nhất cho mỗi AIBom")]
    public string aiID = "AI1"; // Gán khác nhau cho từng AI

    void Awake()
    {
        aiBom = GetComponent<AIBom>();
    }

    public void Save()
    {
        AIBomData data = new AIBomData();
        data.posX = transform.position.x;
        data.posY = transform.position.y;
        data.isHoldingBom = aiBom.IsHoldingBom();
        data.isThrowing = aiBom.IsThrowing();

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(aiID + "_data", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(aiID + "_data"))
        {
            string json = PlayerPrefs.GetString(aiID + "_data");
            AIBomData data = JsonUtility.FromJson<AIBomData>(json);

            transform.position = new Vector2(data.posX, data.posY);
            aiBom.SetHoldingBom(data.isHoldingBom);
            aiBom.SetThrowing(data.isThrowing);
        }
    }
}

[System.Serializable]
public class AIBomData
{
    public float posX;
    public float posY;
    public bool isHoldingBom;
    public bool isThrowing;
}
