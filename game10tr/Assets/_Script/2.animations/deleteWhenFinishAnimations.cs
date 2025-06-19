using UnityEngine;

public class DisappearAfterAnim : MonoBehaviour
{
    // Hàm này sẽ được gọi bởi Animation Event
    public bool endedAnimation = false;
    public void OnAnimationEnd()
    {
        // Ẩn hoặc hủy object
        gameObject.SetActive(false); // hoặc: Destroy(gameObject);
        endedAnimation = true; // Đặt cờ để biết animation đã kết thúc
    }
}