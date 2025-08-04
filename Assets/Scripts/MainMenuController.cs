using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string raceSceneName = "Race";

    public void OnStartButton()
    {
        SceneManager.LoadScene(raceSceneName);
       
    }

    public void Quit()
    {
        Application.Quit();
    }
}
