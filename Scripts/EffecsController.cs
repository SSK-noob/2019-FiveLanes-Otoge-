using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffecsController : MonoBehaviour
{
    public GameObject[] effecs;
    public int lane;
    private int num;

    void Update()
    {
    }

    public void EffecsSetActive(int lane)
    {
        effecs[lane].SetActive(true);
        num = lane;
        Invoke("EffecsSetActive2", 0.6f);
    }
    void EffecsSetActive2()
    {
        effecs[0].SetActive(false);
        effecs[1].SetActive(false);
        effecs[2].SetActive(false);
        effecs[3].SetActive(false);
        effecs[4].SetActive(false);
    }
}
