using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private Text pointsText;
    [SerializeField]
    private Text gameOverScreenText;
    public void Setup(int score)
    {
        Cursor.lockState = CursorLockMode.Confined;
            gameObject.SetActive(true);
            pointsText.text = score.ToString() + " Points";
    }
    public void ExitToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void PlayAgainButton()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
