using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ReadJson : MonoBehaviour
{
    //Json全体のデータを格納するオブジェクト
    JObject notes = new JObject();
    public string _FilePath;

    //ノーツの各情報
    public List<float> _Timing = new List<float>();//ノーツの到達時間
    public List<int> _Lane = new List<int>();//レーン(block)
    public List<int> _Notetype = new List<int>();//Tap or Long(type)
    public List<int> _Num = new List<int>();//ノーツの場所(num)
    public List<int> _LongnoteNum = new List<int>();//ロングノーツ終端の位置(num)
    public List<float> _LongnoteTiming = new List<float>();
    public List<int> _LongLane = new List<int>();
    public int _TotalNotesNumber;

    private float BPM;
    private int LPB;
    private bool onRandom = MusicSelect.GetOnRandom();
    private int[] Lane = new int[5] {0,1,2,3,4};
    private int[] RandomLane = new int[5];
    void ShuffleLane()
    {
        if(onRandom)
        {
            int[] randomLane = Lane.OrderBy(i => Guid.NewGuid()).ToArray();
            RandomLane = randomLane;
        }
        foreach(int i in RandomLane){
            //Debug.Log("ランダムソート後"+i);
        }
    }
    public void ReadAndMakeNotesInfomation(string _FilePath)
    {
        if(onRandom)
        {
            ShuffleLane();
        }        
        //Debug.Log(RandomLane[0]);
        //Jsonファイルを読み込む
        using(StreamReader reader = File.OpenText(_FilePath))
        {
            //notesオブジェクトにjsonファイルの中身を代入?
            notes = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
        }
        BPM = (float)notes["BPM"];

        //配列,変数に各ノーツの情報を記録
        JArray Notes = (JArray)notes["notes"];//ノーマルノーツ
        LPB = (int)(JValue)Notes[0]["LPB"];

        int i = 0;
        int j = 0;

        foreach (JObject fumenobj in Notes)
        {
            _Num.Add((int)(JValue)fumenobj["num"]);//場所(何拍目か)
            //正規譜面なら
            if(onRandom == false)
            {            
                _Lane.Add((int)(JValue)fumenobj["block"]);//レーン
            }
            //ランダム譜面なら
            else
            {
                int lane = ReturnRandomLane((int)(JValue)fumenobj["block"]);
                _Lane.Add(lane);
            }
            _Notetype.Add((int)(JValue)fumenobj["type"]);//ノーツ種類
            _Timing.Add((60 / (BPM * 4)) * _Num[i]);

            if(_Timing[i] == 0)
            {
                _Timing[i] += 0.001f;
            }

            if(_Notetype[i] == 2)
            {
                JArray longnotes = (JArray)fumenobj["notes"];//ロングノーツの終端情報を取得
                _LongnoteNum.Add((int)(JValue)longnotes[0]["num"]);
                //_LongLane.Add((int)(JValue)longnotes[0]["block"]);
                //ランダムなら
                if(onRandom)
                {
                    _LongLane.Add(ReturnRandomLane((int)(JValue)longnotes[0]["block"]));
                }else
                {
                    _LongLane.Add((int)(JValue)longnotes[0]["block"]);
                }
                _LongnoteTiming.Add((60 / BPM) * _LongnoteNum[j] / 4);//ロングノーツ終端の時間
                j++;
            }
            i++;
        }
        _TotalNotesNumber = i;
    }

    int ReturnRandomLane(int lane)
    {
        switch(lane)
        {
            case 0: return RandomLane[0];
            case 1: return RandomLane[1];
            case 2: return RandomLane[2];
            case 3: return RandomLane[3];
            case 4: return RandomLane[4];
            default: return 0;
        }
    }

    void checkLane()
    {
        foreach(int i in _Lane)
        {
            Debug.Log(i);
        }
    }
}
