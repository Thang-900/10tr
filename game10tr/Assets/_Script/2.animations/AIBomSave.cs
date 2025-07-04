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
        Vector3 pos = transform.position;
        PlayerPrefs.SetFloat(aiID + "_posX", pos.x);
        PlayerPrefs.SetFloat(aiID + "_posY", pos.y);
        PlayerPrefs.SetInt(aiID + "_isHoldingBom", aiBom.IsHoldingBom() ? 1 : 0);
        PlayerPrefs.SetInt(aiID + "_isThrowing", aiBom.IsThrowing() ? 1 : 0);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(aiID + "_posX"))
        {
            float x = PlayerPrefs.GetFloat(aiID + "_posX");
            float y = PlayerPrefs.GetFloat(aiID + "_posY");
            transform.position = new Vector2(x, y);

            aiBom.SetHoldingBom(PlayerPrefs.GetInt(aiID + "_isHoldingBom") == 1);
            aiBom.SetThrowing(PlayerPrefs.GetInt(aiID + "_isThrowing") == 1);
        }
    }
}
