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
    [SerializeField]
    private Text highestScoreText;
    public void Setup(int score,bool isWin)
    {
        Cursor.lockState = CursorLockMode.Confined;
            gameObject.SetActive(true);
        if (isWin)
        {
            gameOverScreenText.text = "New best score!";
            gameOverScreenText.color= Color.green;
            highestScoreText.text ="Best Score: "+ score.ToString();
        }
        highestScoreText.text= "Best Score: "+PlayerPrefs.GetInt("HighScore", 0).ToString();
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
