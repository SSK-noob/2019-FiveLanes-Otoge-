using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNotes : MonoBehaviour
{
    GameController gameController;
    CanvasController canvasController;
    NotesController notesController;

    private float len;
    private int StartNum;//始点ノーツの番号
    private int EndNum;//終点ノーツの番号
    private float longtime;//ロングノーツの時間
    private float[] StartTime;//始点の時間
    private float[] EndTime;//終点の時間

    private int count_this;
    private bool move;
    public int LaneNum;
    private KeyCode LaneKey;

    private float great_time = 0.16f;
    private float good_time = 0.32f;

    private float time;//始点
    private float time2;//終点
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Master").GetComponent<GameController>();
        canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        notesController = GameObject.Find("Notes").GetComponent<NotesController>();

        count_this = gameController.notescount;//このノーツの番号を取得
        len = gameController.Length[gameController.longnotescount];//ロングノーツの長さ

        move = false;
        //ノーツの大きさを変化
        this.transform.localScale = new Vector3(1.5f,len,1);

        gameController.notescount += 1;
        gameController.longnotescount += 1;
    }

    // Update is called once per frame
    void Update()
    {
        //条件を満たすとノーツが流れ出す
        if (gameController.Playing && move == false && Mathf.Abs((Time.timeSinceLevelLoad - gameController.StartMusicTime) - gameController.timing[count_this]) <= 9.66 / gameController.notespeed)
        {
            move = true;//ノーツが流れ出す
        }

        if(move)
        {
            Vector3 pos = transform.position;
            pos.y = (gameController.timing[count_this] - (Time.timeSinceLevelLoad - gameController.StartMusicTime)) * gameController.notespeed - 3.5f + len / 2f;
            this.transform.position = pos;
            
        }
        //もし判定ラインを越えたら消す
        if (move && this.transform.position.y < -5f - len)
        {
            gameController.notescount += 1;
            notesController.NotesActive(this.gameObject);
            gameController.ComboCount = 0;
            canvasController.Display(false, false, true, gameController.ComboCount, gameController.Score);
        }
    }
}
