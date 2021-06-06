using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    Animator g_MainMenuAnimator;
    Animator g_BallSelectorAnimator;

    void Awake()
    {
        g_MainMenuAnimator = GameObject.Find("MainMenuGroup").GetComponent<Animator>();
        g_BallSelectorAnimator = GameObject.Find("BallsHolderParent").GetComponent<Animator>();
    }

    public void m_PlayGame()
    {
        SceneManager.LoadScene("Level_1");        
    }

    public void m_SelectBall()
    {
        g_MainMenuAnimator.Play("MainMenuFadeOut");
        g_BallSelectorAnimator.Play("BallHoderParent_FadeIn");
    }

    public void m_BackToMenu()
    {
        g_MainMenuAnimator.Play("MainMenuFadeIn");
        g_BallSelectorAnimator.Play("BallHoderParent_FadeOut");
    }
}
