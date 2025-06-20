using UnityEngine;
using System; // << thêm dòng này để dùng Action

public class DisappearAfterAnim : MonoBehaviour
{
    public Action onAnimationEnd; // Callback dùng để báo về appearPrefabs

    // Hàm này được gọi từ Animation Event
    public void OnAnimationEnd()
    {
        onAnimationEnd?.Invoke(); // Gọi callback nếu có
        Destroy(gameObject); // Hủy đối tượng sau khi animation kết thúc
    }
}
