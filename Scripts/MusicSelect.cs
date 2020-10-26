using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicSelect : MonoBehaviour
{
    AudioSource voice;
    AudioSource select;
    AudioSource decide;
    public GameObject[] se;
    public GameObject[] Musics;
    public Button[] buttons;
    public GameObject Option;
    protected static int num;

    protected static bool OnRandom;

    private Text OptionName;

    private int c = 0;
    private int d = 0;
    private bool can;
    private float[] Musictime = new float[5]{70f,76.9f,76.9f,70f,70f};
    private bool decided;

    void Awake()
    {
        Application.targetFrameRate = 60; //60FPSに設定
    }
    // Start is called before the first frame update
    void Start()
    {
        voice = GetComponent<AudioSource>();
        select = se[0].GetComponent<AudioSource>();
        decide = se[1].GetComponent<AudioSource>();

        num = 0;
        OptionName = Option.GetComponent<Text>();
        decided = false;

        can = false;
        OnRandom = false;
        
        Invoke("PlayVoice", 1f);
        Invoke("PlayInitMusic", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(decided == false){
            if (Input.GetKeyDown(KeyCode.F) && num != buttons.Length - 1 && can)
            {
                num += 1;
                MoveLeft();
                select.Play();
                buttons[num].Select();
                buttons[num].gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                buttons[num - 1].gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            if (Input.GetKeyDown(KeyCode.D) && num != 0 && can)
            {
                num -= 1;
                MoveRight();
                select.Play();
                buttons[num].Select();
                buttons[num].gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1);
                buttons[num + 1].gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Musics[num].GetComponent<AudioSource>().Stop();
                decide.Play();
                FadeManager.Instance.LoadScene("GameScene", 1.5f);
            }

            if(Input.GetKeyDown(KeyCode.R) && c == 0 && d == 0){
                OptionName.text = "RANDOM";
                OnRandom = true;
                c = 1;
            }        
            if(Input.GetKeyDown(KeyCode.R) && c == 0 && d == 1){
                OptionName.text = "NORMAL";
                OnRandom = false;
                c = 1;
            }        
            if(Input.GetKeyDown(KeyCode.R) && c == 1 && d == 0){
                OptionName.text = "RANDOM";
                OnRandom = true;
                c = 0;
                d = 1;
            }        
            if(Input.GetKeyDown(KeyCode.R) && c == 1 && d == 1){
                OptionName.text = "NORMAL";
                OnRandom = false;
                c = 0;
                d = 0;
            }
        }
        
    }
    void PlayVoice()
    {
        voice.Play();
        can = true;
    }

    void PlayInitMusic()
    {
        if(num == 0){
            Musics[0].GetComponent<AudioSource>().time = 70f;
            Musics[0].GetComponent<AudioSource>().Play(); 
        }
    }
    void MoveLeft()
    {
        foreach(Button obj in buttons)
        {
            Vector3 pos = obj.transform.position;
            pos.x -= 320;
            obj.transform.position = pos;
        }
        Musics[num - 1].GetComponent<AudioSource>().Stop();
        Musics[num].GetComponent<AudioSource>().time = Musictime[num];
        Musics[num].GetComponent<AudioSource>().Play();
    }
    void MoveRight()
    {
        foreach (Button obj in buttons)
        {
            Vector3 pos = obj.transform.position;
            pos.x += 320;
            obj.transform.position = pos;
        }
        Musics[num + 1].GetComponent<AudioSource>().Stop();
        Musics[num].GetComponent<AudioSource>().time = Musictime[num];
        Musics[num].GetComponent<AudioSource>().Play();
    }

    public static int GetMusicNum()
    {
        return num;
    }
    public static bool GetOnRandom(){
        return OnRandom;
    }
}
