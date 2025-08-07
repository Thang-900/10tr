using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyAtEnd : MonoBehaviour
{
    public void destroyEnding()
    {
        // Gọi hàm này để xoá đối tượng sau khi hoạt ảnh kết thúc
        Destroy(gameObject);
    }
}
