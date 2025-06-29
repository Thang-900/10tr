using UnityEngine;
using Pathfinding;

public class SelectableUnit : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color defaultColor = Color.white;
    private Color selectedColor = Color.green;

    private AIPath aiPath;
    private Animator animator;

    public bool isSelected = false;
    private Vector3 originalScale;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Deselect();
        originalScale = transform.localScale;
    }

    void Update()
    { 
    }

    public void Select()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;
        spriteRenderer.color = defaultColor;
    }

    public void MoveTo(Vector3 position)
    {
        aiPath.destination = position;
    }
}
