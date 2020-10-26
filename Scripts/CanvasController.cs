using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CanvasController : MonoBehaviour
{
    public GameObject[] JudgeString;
    public GameObject canvas;
    public GameObject combo;
    public GameObject score;

    public GameObject[] JudgeObject;
    private ErrorDisplay ed;
    private GreatDisplay gd;
    private OkDisplay od;

    private ComboDisplay cd;
    private ScoreDisplay sd;

    
    void Start()
    {
        gd = JudgeObject[0].GetComponent<GreatDisplay>();
        od = JudgeObject[1].GetComponent<OkDisplay>();
        ed = JudgeObject[2].GetComponent<ErrorDisplay>();
        cd = combo.GetComponent<ComboDisplay>();
        sd = score.GetComponent<ScoreDisplay>();
    }

    public void Display(bool g ,bool o ,bool e,int c_num,int s_num)
    {
        cd.CountCombo(c_num);
        if(g == true || o == true)
        {
            sd.DisplayScore(s_num);
        }
        if(g == true && o == false && e == false)
        {
            gd.Active(true);
            od.Active(false);
            ed.Active(false);
        }
        else if(g == false && o == true && e == false)
        {
            gd.Active(false);
            od.Active(true);
            ed.Active(false);
        }
        else if(g == false && o == false && e == true)
        {
            ed.Active(false);
            od.Active(false);
            ed.Active(true);
        }
    }

    public void DisplayPoor()
    {
        GameObject Poor = Instantiate(JudgeString[0], new Vector3(0, 0, 0), Quaternion.identity);
        Poor.transform.SetParent(canvas.transform, false);
    }
    public void DisplayGreat()
    {
        GameObject Great = Instantiate(JudgeString[2], new Vector3(0, 0, 0), Quaternion.identity);
        Great.transform.SetParent(canvas.transform, false);
    }
    public void DisplayOk()
    {
        GameObject Ok = Instantiate(JudgeString[1], new Vector3(0, 0, 0), Quaternion.identity);
        Ok.transform.SetParent(canvas.transform, false);
    }
}