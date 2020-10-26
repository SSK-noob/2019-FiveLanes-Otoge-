using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameController : MonoBehaviour
{
    //Musiclist Musiclist = new Musiclist();
    ReadJson readJson = null;
    NotesController notesController;
    CanvasController canvasController;
    EffecsController effecsController;

    public GameObject[] notes;
    public GameObject[] longnotes;
    public GameObject[] Poor;
    public GameObject[] JudgeString;
    public GameObject effect;
    private VideoPlayer videoPlayer;
    public GameObject[] Videos;

    public GameObject[] NotesParent;

    private GameObject note;
    private GameObject[] LaneNotes;

    private GameObject Music;//楽曲をまとめたオブジェクト
    private Transform NowMusic;//選択された楽曲のオブジェクト
    private AudioSource audioSource;//曲の再生などを行う
    public string MusicName;//選曲画面から選択された楽曲の名前を取得（予定）
    public GameObject[] MusicList;//楽曲リスト

    private AudioSource HandClap;

    public float StartMusicTime = 0;//曲が始まる時間
    public int NotesCount = 0;//ノーツ番号(インスタンス生成用)
    public int notescount = 0;//ロングノーツ生成時の始点座標用
    public int LongNotesCount = 0;//ロングノーツ番号(インスタンス生成用)
    public int longnotescount = 0;//ロングノーツ生成時の終点座標用
    public int Count = 0;//これこそ判定に使いたい

    public bool Playing = false;
    private float[] NotesXposition = new float[5] { -2.98f, -1.4f, 0.175f, 1.75f, 3.34f };//ノーツのｘ座標

    public float offset = 0f;
    public string[] FilePath;//譜面ファイル
    public float[] Length;//ロングノーツの長さ
    public float[] timing;//各ノーツのタイミング
    public float[] longtiming;//ロングノーツ終点のタイミング
    public bool Instantiated = false;

    public int ComboCount = 0;//コンボ
    public int Score = 0;//スコア
    public int GreatScore;//great時のスコア
    public int GoodScore;//good時のスコア
    public int TotalNotesNumber;//総ノーツ数
    public float notespeed;
    public GameObject speed;
    private Text speed_str;
    private float Ypos;

    List<float> timing1 = new List<float>();
    List<float> timing2 = new List<float>();
    List<float> timing3 = new List<float>();
    List<float> timing4 = new List<float>();
    List<float> timing5 = new List<float>();

    List<float> longendtiming1 = new List<float>();
    List<float> longendtiming2 = new List<float>();
    List<float> longendtiming3 = new List<float>();
    List<float> longendtiming4 = new List<float>();
    List<float> longendtiming5 = new List<float>();

    private float[][] AllTiming;
    private float[][] AllEndLongTiming;
    private List<GameObject> LaneNotes1 = new List<GameObject>();
    private List<GameObject> LaneNotes2 = new List<GameObject>();
    private List<GameObject> LaneNotes3 = new List<GameObject>();
    private List<GameObject> LaneNotes4 = new List<GameObject>();
    private List<GameObject> LaneNotes5 = new List<GameObject>();

    private float uptime;
    private float GreatTime = 0.032f;
    private float GoodTime = 0.064f;
    private float EalryErrorTime = 0.5f;
    private int num;

    protected static int MaxCombo;
    protected static int GreatCount;
    protected static int GoodCount;
    public int ErrorCount;
    protected static int errorCount;
    protected static int score;

    private float t;
    void Awake()
    {
        Application.targetFrameRate = 60; //60FPSに設定
    }
    void Start()
    {
        num = MusicSelect.GetMusicNum();
        readJson = gameObject.AddComponent<ReadJson>();
        readJson.ReadAndMakeNotesInfomation(FilePath[num]);
        notesController = GameObject.Find("Notes").GetComponent<NotesController>();
        canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        effecsController = GameObject.Find("Effecs").GetComponent< EffecsController>();
        HandClap = gameObject.GetComponent<AudioSource>();
        speed_str = speed.GetComponent<Text>();
        speed_str.text = notespeed.ToString();
        Length = new float[1024];

        MaxCombo = 0;
        GreatCount = 0;
        GoodCount = 0;
        errorCount = 0;
        ErrorCount = 0;
        score = 0;

        //longtiming = readJson._LongnoteTiming;
        TotalNotesNumber = readJson._TotalNotesNumber;
        GreatScore = (int)1e6 / TotalNotesNumber;
        GoodScore = (int)GreatScore / 2;

        //CheckLane();

        MakeNotes();//曲が始まる前にノーツを全て生成しておく

        Invoke("PlayMusic", 5.8f);//5.8秒後に曲再生
        Invoke("StartFumen", 5.8f);

        notescount = 0;
        longnotescount = 0;

        timing = readJson._Timing.ToArray();
        longtiming = readJson._LongnoteTiming.ToArray();

        MakeTimingArray();//ノーツのタイミングを格納した配列を作成
        MakeLongTimingArray();//ロングノーツの終点のタイミングを格納した配列を作成

        //実際に判定で使う配列
        AllTiming = new float[5][];
        AllTiming[0] = new float[timing1.Count];
        AllTiming[1] = new float[timing2.Count];
        AllTiming[2] = new float[timing3.Count];
        AllTiming[3] = new float[timing4.Count];
        AllTiming[4] = new float[timing5.Count];

        AllEndLongTiming = new float[5][];
        AllEndLongTiming[0] = new float[longendtiming1.Count];
        AllEndLongTiming[1] = new float[longendtiming2.Count];
        AllEndLongTiming[2] = new float[longendtiming3.Count];
        AllEndLongTiming[3] = new float[longendtiming4.Count];
        AllEndLongTiming[4] = new float[longendtiming5.Count];

        //ジャグ配列を作成
        for (int i = 1; i <= 5; i++)
        {
            MakeTimingJugArray(i);
            MakeEndLongTimingJugArray(i);
        }

        //各レーンのノーツを格納した配列(GameObject型)を作成
        MakeNotesArray();
        Debug.Log(readJson._TotalNotesNumber);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FadeManager.Instance.LoadScene("MusicSelect", 1.5f);
        }
        //プレイ中なら
        if (Playing)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                Judge(0, Time.timeSinceLevelLoad - StartMusicTime);
                //HandClap.Play();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Judge(1, Time.timeSinceLevelLoad - StartMusicTime);
                //HandClap.Play();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Judge(2, Time.timeSinceLevelLoad - StartMusicTime);
                //HandClap.Play();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Judge(3, Time.timeSinceLevelLoad - StartMusicTime);
                //HandClap.Play();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Judge(4, Time.timeSinceLevelLoad - StartMusicTime);
                //HandClap.Play();
            }
        }
        //ハイスピード調整
        ChangeHispeed();

        if (Playing == true && videoPlayer.isPlaying == false)
        {
            FadeManager.Instance.LoadScene("Result", 1f);
        }
        if(ComboCount > MaxCombo)
        {
            MaxCombo = ComboCount;
        }

        //リザルト画面にミスカウントを渡すために必要
        errorCount = ErrorCount;
        t = Time.deltaTime;
    }

//------------------------------ゲーム開始までの準備-----------------------------------------------------------
    //選択された楽曲のAudioClipを返す
/*
    private AudioClip PlayMusicAudio(string name)
    {
        foreach (AudioClip audio in MusicList)
        {
            if (audio.name == name)
            {
                return audio;
            }
        }
        return MusicList[0];//見つからなかった場合は先頭のAudioClipを返す
    }
*/
    void PlayMusic()
    {
        audioSource = MusicList[num].GetComponent<AudioSource>();
        audioSource.Play();
        StartMusicTime = Time.timeSinceLevelLoad;
        videoPlayer = Videos[num].GetComponent<VideoPlayer>();
        videoPlayer.Play();
        Debug.Log("曲スタート!" + StartMusicTime);
    }

    //ノーツのy座標はワールド座標
    //判定ラインのy座標 + 各ノーツのy座標
    //ノーツのインスタンスを生成
    void SpawnNotes(int lane)
    {
        note = (GameObject)Instantiate(notes[lane],new Vector3(NotesXposition[lane],5.5f,-1),Quaternion.identity);
        //対応するレーンオブジェクトの子オブジェクトとする（親オブジェクトの指定）
        note.transform.parent = NotesParent[lane].transform;
    }

    //ロングノーツのインスタンスを生成
    void SpawnLongNotes(int lane)
    {
        note = (GameObject)Instantiate(longnotes[lane], new Vector3(NotesXposition[lane],5.5f + Length[LongNotesCount] / 2f,0)
                , Quaternion.identity);
        note.transform.parent = NotesParent[lane].transform;
    }

    void StartFumen()
    {
        Playing = true;
    }

    //ノーツ生成
    void MakeNotes()
    {
        for(int i = 0; i < TotalNotesNumber; i++)
        {
            if (readJson._Notetype[NotesCount] == 1)
            {
                //ノーツのインスタンスを生成
                SpawnNotes(readJson._Lane[NotesCount]);
                NotesCount++;
            }
            else if (readJson._Notetype[NotesCount] == 2)
            {
                //ロングノーツの長さ = (終点のタイミング - 始点のタイミング) * ノーツスピード(判定ラインまで流れる時間)
                Length[LongNotesCount] = readJson._LongnoteNum[LongNotesCount] - readJson._Num[NotesCount] + 9.66f/notespeed;
                //ロングノーツのインスタンスを生成
                SpawnLongNotes(readJson._Lane[NotesCount]);
                NotesCount++;
                LongNotesCount++;
            }
        }
    }

    //レーンごとに判定タイミングのリストを作る
    void MakeTimingArray()
    {
        for(int i = 0; i < TotalNotesNumber; i++)
        {
            switch (readJson._Lane[i])
            {
                case 0:
                    timing1.Add(readJson._Timing[i]);
                    break;
                case 1:
                    timing2.Add(readJson._Timing[i]);
                    break;
                case 2:
                    timing3.Add(readJson._Timing[i]);
                    break;
                case 3:
                    timing4.Add(readJson._Timing[i]);
                    break;
                case 4:
                    timing5.Add(readJson._Timing[i]);
                    break;
                default:
                    break;
            }
        }
    }
    //ロングノーツ用
    void MakeLongTimingArray()
    {
        for(int i = 0; i < readJson._LongnoteTiming.ToArray().Length; i++)
        {
            switch (readJson._LongLane[i])
            {
                case 0:
                    longendtiming1.Add(readJson._LongnoteTiming[i]);
                    break;
                case 1:
                    longendtiming2.Add(readJson._LongnoteTiming[i]);
                    break;
                case 2:
                    longendtiming3.Add(readJson._LongnoteTiming[i]);
                    break;
                case 3:
                    longendtiming4.Add(readJson._LongnoteTiming[i]);
                    break;
                case 4:
                    longendtiming5.Add(readJson._LongnoteTiming[i]);
                    break;
                default:
                    break;
            }
        }
    }

    //リストをもとにジャグ配列化
    void MakeTimingJugArray(int lane)
    {
        int i = 0;
        switch (lane)
        {
            case 1:
                foreach(float t in timing1)
                {
                    AllTiming[0][i] = t;
                    i++;
                }
                break;
            case 2:
                foreach (float t in timing2)
                {
                    AllTiming[1][i] = t;
                    i++;
                }
                break;
            case 3:
                foreach (float t in timing3)
                {
                    AllTiming[2][i] = t;
                    i++;
                }
                break;
            case 4:
                foreach (float t in timing4)
                {
                    AllTiming[3][i] = t;
                    i++;
                }
                break;
            case 5:
                foreach (float t in timing5)
                {
                    AllTiming[4][i] = t;
                    i++;
                }
                break;
            default:
                break;
        }
    }
    //ロングノーツ終点のジャグ配列
    void MakeEndLongTimingJugArray(int lane)
    {
        int i = 0;
        switch (lane)
        {
            case 1:
                foreach(float t in longendtiming1)
                {
                    AllEndLongTiming[0][i] = t;
                    i++;
                }
                break;
            case 2:
                foreach (float t in longendtiming2)
                {
                    AllEndLongTiming[1][i] = t;
                    i++;
                }
                break;
            case 3:
                foreach (float t in longendtiming3)
                {
                    AllEndLongTiming[2][i] = t;
                    i++;
                }
                break;
            case 4:
                foreach (float t in longendtiming4)
                {
                    AllEndLongTiming[3][i] = t;
                    i++;
                }
                break;
            case 5:
                foreach (float t in longendtiming5)
                {
                    AllEndLongTiming[4][i] = t;
                    i++;
                }
                break;
            default:
                break;
        }
    }
    void MakeNotesArray()
    {
        //各レーンのノーツを格納した配列を作成
        for (int i = 0; i < 5; i++)
        {
            switch (i)
            {
                case 0:
                    ReturnLaneNotes(i, LaneNotes1);
                    break;
                case 1:
                    ReturnLaneNotes(i, LaneNotes2);
                    break;
                case 2:
                    ReturnLaneNotes(i, LaneNotes3);
                    break;
                case 3:
                    ReturnLaneNotes(i, LaneNotes4);
                    break;
                case 4:
                    ReturnLaneNotes(i, LaneNotes5);
                    break;
            }
        }
    }

    List<GameObject> ReturnLaneNotes(int lane,List<GameObject> list)
    {
        foreach(Transform obj in NotesParent[lane].transform)
        {
            list.Add(obj.gameObject);
        }
        return list;
    }

    void CheckLane(){
        foreach(int i in readJson._Lane){
            Debug.Log(i);
        }
    }

//------------------------------ゲーム中の処理-----------------------------------------------------------
    //判定用
    List<GameObject> ReturnNotesArray(int lane)
    {
        switch (lane)
        {
            case 0:
                return LaneNotes1;
            case 1:
                return LaneNotes2;
            case 2:
                return LaneNotes3;
            case 3:
                return LaneNotes4;
            case 4:
                return LaneNotes5;
            default:
                return null;
        }
    }

    void Judge(int lane, float time)//timeはキーが押された時間
    {
        GameObject note = null;
        List<GameObject> LaneNotes = new List<GameObject>();
        //対応したレーンのオブジェクトが格納されているGameobject型の配列を取得
        LaneNotes = ReturnNotesArray(lane);
        float min = 1f;
        int count = 0;
        int count2 = 0;
        int i = 0;

        //もっとも判定時間に近いノーツを取得
        foreach (float Jtime in AllTiming[lane])
        {
            if (Mathf.Abs(time - Jtime) <= min)//判定時間との差を計算
            {
                min = Mathf.Abs(time - Jtime);
                note = LaneNotes[i].gameObject;
                if(note.transform.localScale.y > 0.2f)//ロングノーツかどうかの判定
                {
                    count2 += 1;
                }
                count = i;
            }
            i++;
        }
        //判定する(ノーマルノーツ)
        if(note.transform.localScale.y <= 0.2f && note.activeSelf){
            if (note != null && Mathf.Abs(AllTiming[lane][count] - time) <= GreatTime)//+0.025f
            {
                note.SetActive(false);
                ComboCount += 1;
                Score += GreatScore;
                score = Score;
                GreatCount += 1;
                HandClap.Play();
                effecsController.EffecsSetActive(lane);
                canvasController.Display(true, false, false, ComboCount,Score);
            }
            else if (note != null && Mathf.Abs(AllTiming[lane][count] - time) <= GoodTime)
            {
                note.SetActive(false);
                ComboCount += 1;
                Score += GoodScore;
                score = Score;
                GoodCount += 1;
                HandClap.Play();
                effecsController.EffecsSetActive(lane);
                canvasController.Display(false, true, false, ComboCount, Score);
            }
        //判定するロングノーツ
        }
        else
        {
            HandClap.Play();
            float longtime = AllEndLongTiming[lane][count2] - AllTiming[lane][count];//キーが押され続けるべき時間
            float waittime = 0;
            KeyCode key = GetKeyLane(lane);
            //キーが離された時間を取得
            
            Debug.Log("time" + time);
            Debug.Log("uptime" + uptime);
            Debug.Log("ロングノーツの長さ" + longtime);
            Debug.Log("キーが押され続けていた時間" + (uptime - time));

            
            if (Mathf.Abs(longtime - (uptime - time)) <= GreatTime)
            {
                //note.SetActive(false);
                ComboCount += 1;
                Score += GreatScore;
                score = Score;
                GreatCount += 1;
                HandClap.Play();
                canvasController.Display(true, false, false, ComboCount,Score);
            }
            else if (Mathf.Abs(longtime - (uptime - time)) <= GoodTime)
            {
                //note.SetActive(false);
                ComboCount += 1;
                Score += GoodScore;
                score = Score;
                GoodCount += 1;
                HandClap.Play();
                canvasController.Display(false, true, false,ComboCount,Score);
            }
            else
            {
                errorCount = ErrorCount;
            }
        }
    }
KeyCode GetKeyLane(int lane){
    switch(lane){
        case 1: return KeyCode.D;
        case 2: return KeyCode.F;
        case 3: return KeyCode.Space;
        case 4: return KeyCode.J;
        case 5: return KeyCode.K;
        default: return KeyCode.D;
    }
}
//ハイスピード設定
void ChangeHispeed()
{
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
        notespeed = 2;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2))
    {
        notespeed = 4;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha3))
    {
        notespeed = 6;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha4))
    {
        notespeed = 8;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha5))
    {
        notespeed = 10;
        speed_str.text = notespeed.ToString();
    }    
    else if (Input.GetKeyDown(KeyCode.Alpha6))
    {
        notespeed = 12;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha7))
    {
        notespeed = 14;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha8))
    {
        notespeed = 16;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha9))
    {
        notespeed = 18;
        speed_str.text = notespeed.ToString();
    }
    else if (Input.GetKeyDown(KeyCode.Alpha0))
    {
        notespeed = 20;
        speed_str.text = notespeed.ToString();
    }
}

//-------------------------------------リザルト用---------------------------------
    public static int GetScore()
    {
        return score;
    }
    public static int GetGreatCount()
    {
        return GreatCount;
    }
    public static int GetGoodCount()
    {
        return GoodCount;
    }
    public static int GetErrorCount()
    {
        return errorCount;
    }
    public static int GetMaxCombo()
    {
        return MaxCombo;
    }
}
