using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallsManager : MonoBehaviour
{
    [Range(0, 10)] [SerializeField] int g_IntBallsCount = 0;

    [SerializeField] GameObject g_BallPrefab = null;

    Ball [] g_BallsPool;

    public int g_IntBallsLives;
    [SerializeField] Text g_BallCount = null;
    [SerializeField] Text g_PlayerScore = null;
    int g_IntMaxBallLives;

    [SerializeField] float g_FloatMinX = 0.0f;
    [SerializeField] float g_FloatMaxX = 0.0f;
    [SerializeField] float g_FloatMinY = 0.0f;
    [SerializeField] float g_FloatMaxY = 0.0f;

    int g_IntPlayerScore;

    void Awake()
    {
        PlayerPrefs.SetInt("PlayerScore", 0);
    }

    // Start is called before the first frame update
    void Start()
    {  
        g_BallsPool = new Ball[ g_IntBallsCount ];        

        m_CreateBall();
        g_IntMaxBallLives = 10;
        g_IntBallsLives = g_IntMaxBallLives;

        g_IntPlayerScore = PlayerPrefs.GetInt("PlayerScore");

        m_DisplayBallCount();
        m_DisplayPlayerScore( 0 );

        m_ActivateBall();
    }

    void m_CreateBall()
    {
        GameObject l_TempBall = null;

        for (int i = 0; i < g_IntBallsCount; i++)
        {
            l_TempBall = Instantiate( g_BallPrefab, this.transform.position, Quaternion.identity );
            l_TempBall.transform.SetParent(this.transform);
            l_TempBall.SetActive(false);
            g_BallsPool[i] = l_TempBall.GetComponent<Ball>();
            g_BallsPool[i].m_GetPathList();
        }
    }

    public void m_DisplayBallCount()
    {
        g_BallCount.text = "Balls : " + g_IntBallsLives.ToString();
    }

    public void m_DisplayPlayerScore( int l_IntPlayerScore )
    {
        int l_IntCurrentPlayerScore = PlayerPrefs.GetInt("PlayerScore");
        l_IntCurrentPlayerScore += l_IntPlayerScore;
        g_PlayerScore.text = "Score : " + l_IntCurrentPlayerScore.ToString();
        m_SaveScore( l_IntCurrentPlayerScore );
    }

    void m_SaveScore(  int l_SaveValue )
    {
        PlayerPrefs.SetInt( "PlayerScore", l_SaveValue );
        PlayerPrefs.Save();
    }

    public void m_IncrementDecrementBalls( int l_IntCounter )
    {
        g_IntBallsLives += l_IntCounter;

        if ( g_IntBallsLives > g_IntMaxBallLives )
        {
            g_IntBallsLives = g_IntMaxBallLives;
        }
    }

    public void m_ActivateBall()
    {
        Vector3 l_BallPosition = Vector3.zero;

        for (int i = 0; i < g_IntBallsCount; i++)
        {
            if ( !g_BallsPool[i].gameObject.activeSelf )
            {
                g_BallsPool[i].gameObject.SetActive(true);
                l_BallPosition = new Vector3( Random.Range( g_FloatMinX, g_FloatMaxX), Random.Range( g_FloatMinY, g_FloatMaxY), 0.0f);
                g_BallsPool[i].transform.position = l_BallPosition;
                return;
            }
        }
    }

    public void m_GameOver()
    {
        if ( g_IntBallsLives <= 0 )
        {
            StartCoroutine(m_LoadGameOver());
        }
    }

    IEnumerator m_LoadGameOver()
    {
        yield return new WaitForSeconds( 1.0f );
        SceneManager.LoadScene("GameOver");
    }
}
