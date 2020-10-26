using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboDisplay : MonoBehaviour
{
    private Text combo;
    public void CountCombo(int num)
    {
        this.combo = this.GetComponent<Text>();
        this.combo.text = num.ToString();
    }
}
