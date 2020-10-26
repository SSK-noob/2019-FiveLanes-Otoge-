using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    CanvasController canvasController;
    GameController gameController;
    NotesController notesController;

    public int LaneNum;
    private KeyCode LaneKey;

    private int count_this;
    public bool judged = false;
    void Start()
    {
        gameController = GameObject.Find("Master").GetComponent<GameController>();
        canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        notesController = GameObject.Find("Notes").GetComponent<NotesController>();

        count_this = gameController.notescount;

        gameController.notescount += 1;
        LaneKey = GetKeys.GetKeyCodeByLane(LaneNum);//レーンに割り当てられたキーを取得
    }

    void Update()
    {
        if (gameController.Playing && Mathf.Abs((Time.timeSinceLevelLoad - gameController.StartMusicTime) - gameController.timing[count_this])<=9.66 / gameController.notespeed)
        {
            Vector3 pos = transform.position;
            pos.y = (gameController.timing[count_this] - (Time.timeSinceLevelLoad - gameController.StartMusicTime)) * gameController.notespeed - 3.25f;
            this.transform.position = pos;
            
        }
        if (judged == false && this.transform.position.y < -5f)
        {
            judged = true;
            JudgeError();
        }
    }

    void JudgeError()
    {
        gameController.ErrorCount += 1;
        notesController.NotesActive(this.gameObject);
        gameController.ComboCount = 0;
        canvasController.Display(false, false, true, gameController.ComboCount, gameController.Score);
    }
}