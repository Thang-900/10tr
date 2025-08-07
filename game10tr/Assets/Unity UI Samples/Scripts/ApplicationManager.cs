using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ApplicationManager : MonoBehaviour {

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (SceneManager.GetActiveScene().name== "_VictoryScene")
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayBGM("Gamplay");
            }
        }
        else
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayBGM("Bonus Theme 2 The Northmen March to War");
            }
        }
        
    }
    public void Quit () 
	{
		
		Application.Quit();
		Debug.Log("Game is exiting...");
    }
    public void LoadMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
