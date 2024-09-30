using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void OnLeaveGameButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnOptionsGameButtonClicked()
    {
        SceneManager.LoadScene("Options");
    }

    public void OnCreditGameButtonClicked()
    {
        SceneManager.LoadScene("Credits");
    }
    public void OnExitGameButtonClicked()
    {
        Application.Quit();

        Debug.Log("ExitGame");
    }
}