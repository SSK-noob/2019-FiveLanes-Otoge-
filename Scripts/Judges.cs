using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judges : MonoBehaviour
{
    void Start()
    {
        Delay();
    }
    void Update()
    {
    }

    void Delay()
    {
        Invoke("Destroy",0.5f);
    }

    public void Destroy()
    {
        Debug.Log("デストローイ!");
        Destroy(this.gameObject);
    }
}
