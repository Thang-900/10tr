using UnityEngine;

public enum CharacterType
{
    Bom,
    Gunner,
    Harmer,
    Scythe
}

[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Character/Stats")]
public class CharacterStats : ScriptableObject
{
    public CharacterType characterType;
    public float health = 100f;
    public float damage = 10f;
    public float moveSpeed = 3f;
    public float attackSpeed = 1f;
}
