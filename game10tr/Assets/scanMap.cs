using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class AstarScanner : MonoBehaviour
{
    private bool hasScanned = false;

    private void Update()
    {
        Invoke("scanWall", 0.3f);
    }

    void scanWall()
    {
        if (!hasScanned)
        {
            AstarPath.active.Scan();
            hasScanned = true;
        }
    }
}