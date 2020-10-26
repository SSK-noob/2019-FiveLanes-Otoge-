using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private Text score;
    private TypefaceAnimator typefaceAnimator;
    void Start()
    {
        score = this.GetComponent<Text>();
        typefaceAnimator = this.GetComponent<TypefaceAnimator>();
    }
    public void DisplayScore(int Score)
    {
        score.text = Score.ToString();
        typefaceAnimator.enabled = true;
        Invoke("SetActive", 0.2f);
    }

    void SetActive()
    {
        typefaceAnimator.enabled = false;
    }
}
