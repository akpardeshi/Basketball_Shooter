using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] Text g_ScoreText = null;

    // Start is called before the first frame update
    void Start()
    {
        g_ScoreText.text = "SCORE : " + PlayerPrefs.GetInt("PlayerScore").ToString();
    }

    public void m_BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void m_RestartGame()
    {
        SceneManager.LoadScene("Level_1");
    }
}
