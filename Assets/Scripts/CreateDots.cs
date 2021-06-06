using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDots : MonoBehaviour
{
    [SerializeField] GameObject g_GoDotPrefab = null;
    [SerializeField] int g_IntDotsCount = 0;

    List<GameObject> g_GoDotPool = new List<GameObject>();

    // // Start is called before the first frame update
    void Awake()
    {
        m_CreateDots();
    }

    public void m_CreateDots()
    {
        GameObject l_GoDot = null;

        for (int i = 0; i < g_IntDotsCount; i++)
        {
            l_GoDot = Instantiate( g_GoDotPrefab, this.transform.position, Quaternion.identity);
            l_GoDot.gameObject.SetActive(false);
            l_GoDot.transform.SetParent(this.transform);
            g_GoDotPool.Add( l_GoDot );
        }
    }
}
