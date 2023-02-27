using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("GameplayScene");
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
