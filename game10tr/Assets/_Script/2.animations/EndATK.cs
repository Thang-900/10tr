using Pathfinding;
using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    
    public void EndAttack()
    {
        AILogic aiLogic = GetComponentInParent<AILogic>();
        if (aiLogic != null)
        {
            aiLogic.EndAttack();
        }
        else
        {
            Debug.LogError("Không tìm thấy AIHammer trong cha.");
        }
    }
}
