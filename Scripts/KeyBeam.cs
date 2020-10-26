using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBeam : MonoBehaviour
{
    public GameObject[] keybeams;
    private string[] keys = new string[5] { "D", "F", "Space", "J", "K" };
    private float[] X = new float[5] { -3f, -1.4f, 0.2f, 1.8f,3.3f };

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            keybeams[0].SetActive(true);
        }
        else
        {
            keybeams[0].SetActive(false);
        }
        if (Input.GetKey(KeyCode.F))
        {
            keybeams[1].SetActive(true);
        }
        else
        {
            keybeams[1].SetActive(false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            keybeams[2].SetActive(true);
        }
        else
        {
            keybeams[2].SetActive(false);
        }
        if (Input.GetKey(KeyCode.J))
        {
            keybeams[3].SetActive(true);
        }
        else
        {
            keybeams[3].SetActive(false);
        }
        if (Input.GetKey(KeyCode.K))
        {
            keybeams[4].SetActive(true);
        }
        else
        {
            keybeams[4].SetActive(false);
        }
    }
}
