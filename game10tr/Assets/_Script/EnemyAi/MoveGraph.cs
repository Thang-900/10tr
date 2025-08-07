using Pathfinding;
using UnityEngine;

public class EnemyMoveGraph : MonoBehaviour
{

    private Seeker seeker;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        if (seeker == null)
        {
            Debug.LogError("Cần gắn component Seeker vào nhân vật!");
        }
        seeker.graphMask = GraphMask.FromGraphName("WallOnlyGraph");
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    
}
