using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveDir;

    void Update()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Lật nhân vật
        if (moveDir.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDir.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        if (moveDir.sqrMagnitude > 1)
            moveDir = moveDir.normalized;

        transform.Translate(moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}
