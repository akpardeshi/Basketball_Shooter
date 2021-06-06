using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSelectScript : MonoBehaviour
{
    public GameObject [] g_Buttons;
    public List<Button> g_ButtonList = new List<Button>();

    void Awake()
    {
        g_Buttons = GameObject.FindGameObjectsWithTag("Button");
    }

    void m_ExtractButtons()
    {
        for (int i = 0; i < g_Buttons.Length; i++)
        {
            g_ButtonList.Add( g_Buttons[i].GetComponent<Button>());
        }
    }

    public void m_SelectBall()
    {
        int l_IntBallIndex = int.Parse( UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name );
        PlayerPrefs.SetInt( "BallColorIndex" , l_IntBallIndex );
        PlayerPrefs.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ExtractButtons();
    }
}
