using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/* 選曲画面で選択された楽曲*/
public class Musiclist : MonoBehaviour
{

    private static string folderPath = "Assets/Audio";
    private static string[] files = Directory.GetFiles(folderPath, "*", System.IO.SearchOption.TopDirectoryOnly);//ファイルパスを取得
    private string[] fileNames;

    public string SelectAudioClip(string name)
    {
        //ファイルパスからファイル名（楽曲名）を抽出
        fileNames = new string[10];
        int i = 0;
        foreach (string str in files)
        {
            fileNames[i] = Path.GetFileNameWithoutExtension(str);
            i++;
        }

        for (i = 0; i < 10; i++)
        {
            if(fileNames[i] == name)
            {
                Debug.Log("音楽ファイルが見つかりました");
                Debug.Log("曲名:" + fileNames[i]);
                return fileNames[i];//楽曲名を返す
            } 
        }
        Debug.Log("音楽ファイルが見つかりませんでした");
        return fileNames[i];
    }
}
