using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public GameObject[] JudgeCount;
    public GameObject Music;
    private AudioSource audio;

    private Text Score;
    private Text GreatCount;
    private Text GoodCount;
    private Text ErrorCount;
    private Text MaxCombo;
    private Text Rank;
    // Start is called before the first frame update
    void Start()
    {
        Score = JudgeCount[0].GetComponent<Text>();
        GreatCount = JudgeCount[1].GetComponent<Text>();
        GoodCount = JudgeCount[2].GetComponent<Text>();
        ErrorCount = JudgeCount[3].GetComponent<Text>();
        MaxCombo = JudgeCount[4].GetComponent<Text>();
        Rank = JudgeCount[5].GetComponent<Text>();

        Score.text = GameController.GetScore().ToString();
        GreatCount.text = GameController.GetGreatCount().ToString();
        GoodCount.text = GameController.GetGoodCount().ToString();
        ErrorCount.text = GameController.GetErrorCount().ToString();
        MaxCombo.text = GameController.GetMaxCombo().ToString();

        JudgeRank();

        audio = Music.GetComponent<AudioSource>();
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audio.Stop();
            FadeManager.Instance.LoadScene("MusicSelect", 1f);
        }
    }
    void JudgeRank()
    {
        int score;
        score = GameController.GetScore();
        if(score >= 900000)
        {
            Rank.text = "AAA";
        }
        else if (score >= 850000)
        {
            Rank.text = "AA";
        }
        else if(score >= 800000)
        {
            Rank.text = "A";
        }
        else if(score >= 700000)
        {
            Rank.text = "B";
        }
        else if(score >= 600000)
        {
            Rank.text = "C";
        }
        else if(score >= 500000)
        {
            Rank.text = "D";
        }
        else
        {
            Rank.text = "E";
        }
    }
}
