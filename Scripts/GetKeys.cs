using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetKeys
{
    public static KeyCode GetKeyCodeByLane(int lane)
    {
        switch (lane)
        {
            case 1:
                return KeyCode.D;
            case 2:
                return KeyCode.F;
            case 3:
                return KeyCode.Space;
            case 4:
                return KeyCode.J;
            case 5:
                return KeyCode.K;
            default:
                return KeyCode.None;
        }
    }
}
