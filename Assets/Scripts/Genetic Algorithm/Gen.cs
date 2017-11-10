using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen {
    float _time;
    LanderAction _action;

    public Gen(LanderAction action, float time) {
        _action = action;
        _time = time;
    }

    public float getTime() {
        return _time;
    }

    public LanderAction getAction() {
        return _action;
    }
}
