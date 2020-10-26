using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDisplay : MonoBehaviour
{
    public void Active(bool active)
    {
        if (active)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
