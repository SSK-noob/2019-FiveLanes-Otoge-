using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opning : MonoBehaviour
{
    TypefaceAnimator script;
    // Start is called before the first frame update
    void Start()
    {
        script = this.GetComponent<TypefaceAnimator>();
        Invoke("Enable", 3.5f);
    }

    void Enable()
    {
        script.enabled = true;
    }
}
