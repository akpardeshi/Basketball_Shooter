using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ball : MonoBehaviour
{
    // Ball color selection
    [SerializeField] Sprite [] g_BallColors = null;
    public int g_IntBallColorId = 0;
    
    SpriteRenderer g_SpriteRenderer;

    // Shooting 
    [SerializeField] float g_FloatPower = 0.0f;
    [SerializeField] float g_FloatLife = 0.0f;
    [SerializeField] float g_FloatDeadSence = 0.0f;

    [SerializeField] int g_IntDotsCount = 0;

    Vector2 g_StartingPosition;
    bool g_BoolHasBoolShot;
    bool g_BoolIsAiming;
    bool g_BoolHasTouchedGround;

    GameObject g_GoDots;
    public List<GameObject> g_GoPathList = new List<GameObject>();

    Rigidbody2D g_Rigidbody;
    Collider2D g_Collider;

    float g_FloatReliazeZone;

    Color g_BallColor;
    Renderer g_Renderer;

    AudioManager g_AudioManager;
    float g_FloatGroundTouch;

    bool g_BoolHasTouchedRam;
    BallsManager g_BallsManager;

    int g_IntPlayerScore;

    void Awake()
    {
        g_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        g_Rigidbody = this.GetComponent<Rigidbody2D>();
        g_Collider = this.GetComponent<Collider2D>();

        g_Renderer = this.GetComponent<Renderer>();
        g_BallColor = this.GetComponent<Renderer>().material.GetColor("_Color");
        g_AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        g_GoDots = GameObject.Find("Dots");    

        g_BallsManager = GameObject.Find("BallsManager") .GetComponent<BallsManager>()   ;

        PlayerPrefs.SetInt( "PlayerScore", 0 );
        g_IntPlayerScore = PlayerPrefs.GetInt("PlayerScore" );
    }

    // Start is called before the first frame update
    void Start()
    {
        g_Rigidbody.isKinematic = true;
        g_Collider.enabled = false;
        g_StartingPosition = this.transform.position;
        g_BoolHasTouchedRam = false;

        g_FloatReliazeZone = 70.0f;

        g_FloatGroundTouch = 0.0f;
        
        m_AssignColor();

        m_ResetProperties();
    }

    public void m_GetPathList()
    {
        g_GoPathList = g_GoDots.transform.Cast<Transform>().ToList().ConvertAll ( t => t.gameObject);
    }

    void m_DisableRenderer()
    {
        for (int i = 0; i < g_IntDotsCount; i++)
        {
            g_GoPathList[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_Aim();

        if ( g_BoolHasTouchedGround )
        {
            g_FloatLife -= Time.deltaTime;

            g_Renderer.material.SetColor("_Color", new Color( g_BallColor.r, g_BallColor.g, g_BallColor.b, g_FloatLife ));


            if ( g_FloatLife <= 0.0f )
            {
                m_ResetProperties();            
                g_BallsManager.GetComponent<BallsManager>().m_ActivateBall();
                this.gameObject.SetActive(false);
            }
        }
    }

    void m_ResetProperties()
    {
        g_FloatLife = 1.0f;        
        this.g_Rigidbody.isKinematic = true;
        this.g_Collider.enabled = false;
        g_BoolHasTouchedGround = false;
        g_BoolHasBoolShot = false;
        g_BoolIsAiming = false;
        g_FloatGroundTouch = 0.0f;
        g_Renderer.material.SetColor("_Color", new Color( g_BallColor.r, g_BallColor.g, g_BallColor.b, g_FloatLife ));
    }

    public void m_AssignColor()
    {
        g_SpriteRenderer.sprite = g_BallColors[ PlayerPrefs.GetInt("BallColorIndex") ];
    }

    void m_Aim()
    {
        if ( g_BoolHasBoolShot )
            return;

        if ( Input.GetAxis ("Fire1") == 1)
        {
            if ( !g_BoolIsAiming )
            {
                g_BoolIsAiming = true;
                g_StartingPosition = Input.mousePosition;
                m_CalculatePath();
                m_ShowPath();                
            }

            else
            {
                m_CalculatePath();
            }
        }

        else if ( g_BoolIsAiming && ! g_BoolHasBoolShot )
        {
            if ( m_BoolIsReleaseZone( Input.mousePosition ) || m_BoolIsDeadZone( Input.mousePosition ) )
            {
                g_BoolIsAiming = false;
                m_HidePath();
                return;
            }

            g_Rigidbody.isKinematic = false;
            g_Collider.enabled = true;
            g_BoolHasBoolShot = true;
            g_BoolIsAiming = false;
            g_Rigidbody.AddForce( m_GetForce( Input.mousePosition ));
            m_HidePath();
            g_BallsManager.m_IncrementDecrementBalls( -1 );
            g_BallsManager.m_DisplayBallCount();
        }
    }

    bool m_BoolIsReleaseZone ( Vector2 l_MousePosition )
    {
        if ( l_MousePosition.x <= g_FloatReliazeZone )
        {
            return true;
        }

        return false;
    }

    bool m_BoolIsDeadZone( Vector2 l_MousePosition )
    {
        if ( Mathf.Abs( g_StartingPosition.x - l_MousePosition.x ) <= g_FloatDeadSence && Mathf.Abs( g_StartingPosition.y - l_MousePosition.y) <= g_FloatDeadSence )
        {
            return true;
        }

        return false;
    }

    void m_CalculatePath ()
    {
        Vector2 l_Velocity = m_GetForce ( Input.mousePosition ) * Time.fixedDeltaTime / g_Rigidbody.mass ;
        float l_FloatTime = 0.0f;
        Vector3 l_Point = Vector3.zero;

        for (int i = 0; i < g_GoPathList.Count ; i++)
        {
            g_GoPathList[i].GetComponent<SpriteRenderer>().enabled = true;
            l_FloatTime = i / 30.0f;
            l_Point = m_GetPathPoint( this.transform.position, l_Velocity, l_FloatTime);
            l_Point.z = 1.0f;
            g_GoPathList[i].transform.position = l_Point;
        }
    }

    Vector3 m_GetForce( Vector3 l_MousePosition )
    {
        return ( ( Vector2 ) g_StartingPosition - ( Vector2 ) l_MousePosition ) * g_FloatPower ;
    }

    Vector2 m_GetPathPoint( Vector2 l_StartingPoint, Vector2 l_StartingVelocity, float l_FloatTime )
    {
        return l_StartingPoint + l_StartingVelocity * l_FloatTime + 0.5f * Physics2D.gravity * l_FloatTime * l_FloatTime;
    }

    void m_ShowPath ()
    {
        for (int i = 0; i < g_GoPathList.Count; i++)
        {
            g_GoPathList[i].gameObject.SetActive(true);
            g_GoPathList[i].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void m_HidePath ()
    {
        for (int i = 0; i < g_GoPathList.Count; i++)
        {
            g_GoPathList[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void OnCollisionEnter2D( Collision2D collision )
    {
        switch ( collision.gameObject.tag )
        {
            case "Ground":
                g_FloatGroundTouch++;
                g_BoolHasTouchedGround = true;

                if ( g_FloatGroundTouch <= 3.0f  )
                {
                    g_AudioManager.m_PlaySound( 2 );
                }

                g_BallsManager.m_GameOver();
                break;

            case "Ram":
            case "Table":
                g_BoolHasTouchedRam = true;
                g_AudioManager.m_PlaySound(3);
                break;
                
            case "Holder":
                g_AudioManager.m_PlaySound(3);
                break;
        }
    }

    void OnTriggerEnter2D ( Collider2D trigger )
    {
        switch ( trigger.gameObject.tag )
        {
            case "Net":
                if ( g_BoolHasTouchedRam )
                {
                    g_BallsManager.m_IncrementDecrementBalls(1);
                    g_BallsManager.m_DisplayBallCount();
                    g_BallsManager.m_DisplayPlayerScore( 1 );
                }

                else 
                {
                    g_BallsManager.m_IncrementDecrementBalls(2);
                    g_BallsManager.m_DisplayBallCount();
                    g_BallsManager.m_DisplayPlayerScore( 2 );
                }

                g_AudioManager.m_PlaySound(1);
                break;
        }
    }
}
 