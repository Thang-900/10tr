using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassSelector : MonoBehaviour
{
    public void Select_Cach_Mang()
    {
        ClassSelection.SelectedClassName = "Cach_Mang";
        Debug.Log("Selected Class: " + ClassSelection.SelectedClassName);
    }

    public void SelectMage_Cong_Dong()
    {
        ClassSelection.SelectedClassName = "Cong_Dong";
        Debug.Log("Selected Class: " + ClassSelection.SelectedClassName);
    }
    public void SelectRogue_Lao_Dong()
    {
        ClassSelection.SelectedClassName = "Lao_Dong";
        Debug.Log("Selected Class: " + ClassSelection.SelectedClassName);
    }
    public void SelectArcher_Phi_Chu_Nghia()
    {
        ClassSelection.SelectedClassName = "Phi_Chu_Nghia";
        Debug.Log("Selected Class: " + ClassSelection.SelectedClassName);
    }
}
