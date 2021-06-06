using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource g_AudioSource;
    [SerializeField] AudioClip g_RamHit_1;
    [SerializeField] AudioClip g_RamHit_2;
    [SerializeField] AudioClip g_Bounce_1;
    [SerializeField] AudioClip g_Bounce_2;
    [SerializeField] AudioClip g_Net_Hit;

    float g_FloatMaxVolume;
    float g_FloatMinVolume;

    void Awake()
    {
        g_AudioSource = this.GetComponent< AudioSource >();
    }

    // Start is called before the first frame update
    void Start()
    {
        g_FloatMinVolume = 0.4f;
        g_FloatMaxVolume = 1.0f;
    }

    public void m_PlaySound( int l_IntSoundId )
    {
        switch ( l_IntSoundId )
        {
            case 1 :
                g_AudioSource.clip = g_Net_Hit;                
                break;

            case 2 :
                if ( Random.Range(0, 2) > 1 )
                {
                    g_AudioSource.clip = g_Bounce_1;
                }

                else 
                {
                    g_AudioSource.clip = g_Bounce_2;
                }

                break;

            case 3 :
                if ( Random.Range ( 0, 2 ) > 1 )
                {
                    g_AudioSource.clip = g_RamHit_1;
                }

                else
                {
                    g_AudioSource.clip = g_RamHit_2;
                }
                break;
        }

        g_AudioSource.volume = Random.Range( g_FloatMinVolume, g_FloatMaxVolume);
        g_AudioSource.Play();
    }
}
