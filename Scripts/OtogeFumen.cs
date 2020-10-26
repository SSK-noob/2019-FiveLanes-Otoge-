using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[SerializeField]
public class OtogeFumen //他のスクリプトから参照されるのでMono...は消す
{
    public string name;
    public int maxBlock;
    public int BPM;
    public int offset;
    public List<Note> notes;
    public class Note
    {
        public int LPB;
        public int num;
        public int block;
        public int type;
        public List<Note> notes;
    }
}
