using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMusicSelect : MonoBehaviour
{
    private AudioSource audio;
    private AudioSource Music;
    private GameObject obj;
    void Awake()
    {
        Application.targetFrameRate = 60; //60FPSに設定
        audio = this.GetComponent<AudioSource>();
        obj = GameObject.Find("TitleMusic");
        Music = obj.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Music.Stop();
            audio.Play();
            ChangeScene();
        }
    }
    void ChangeScene()
    {
        FadeManager.Instance.LoadScene("MusicSelect", 2f);
    }
}
