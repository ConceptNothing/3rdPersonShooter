using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private Text pointsText;
    [SerializeField]
    private Text gameOverScreenText;
    public void Setup(int score)
    {
            gameObject.SetActive(true);
            pointsText.text = score.ToString() + " Points";
    }
}
