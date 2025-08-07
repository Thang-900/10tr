using UnityEngine;

public class AnimationAttackEventHandler : MonoBehaviour
{
    private AIHammer aiHammer;
    //private GunnerAppearentDisplay gunnerAppearentDisplay;
    void Start()
    {
        //gunnerAppearentDisplay = GetComponentInParent<GunnerAppearentDisplay>();
        //aiHammer = GetComponentInParent<AIHammer>();
        //if (aiHammer == null)
        //{
        //    Debug.LogError("Không tìm thấy AIHammer trong cha.");
        //}
    }

    // GỌI HÀM NÀY TỪ ANIMATION EVENT
    public void EndAttack()
    {
        //gunnerAppearentDisplay.OnBulletFired();
        //if (aiHammer == null) return;

        //// Nếu là Gunner
        //if (aiHammer.characterStats.characterType == CharacterType.Gunner)
        //{
        //    int currentBullet = aiHammer.bulletOnEvenUp.currentBulletCount +
        //                        aiHammer.bulletOnEvenDown.currentBulletCount +
        //                        aiHammer.bulletOnEvenSide.currentBulletCount;

        //    if (currentBullet < aiHammer.GetMaxBulletCount())
        //    {
        //        aiHammer.EndAttack(); // Bắn tiếp nếu còn slot đạn
        //    }
        //    else
        //    {
        //        Debug.Log("Đủ đạn rồi → không bắn nữa");
        //    }
        //}
        //else
        //{
        //    // Nếu không phải gunner → kiểm tra còn địch không
        //    if (aiHammer.HasEnemyInRange())
        //    {
        //        aiHammer.EndAttack(); // Đánh tiếp nếu còn địch
        //    }
        //    else
        //    {
        //        Debug.Log("Không còn kẻ địch trong vùng → không đánh nữa");
        //    }
        //}
    }
}
