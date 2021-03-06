﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHSequencer : BHNode {
    int i = 0;

    public BHSequencer() : base()
    {
        _type = "BHSequencer";
    }

    public override BHNodeState start()
    {
        BHNodeState result;

        while (i < _childs.Count)
        {
            result = _childs[i].start();

            if (result == BHNodeState.OK)
                i++;

            else if (result == BHNodeState.EXECUTING)
                return result;

            else break;
        }

        i = 0;
        return BHNodeState.ERROR;
    }
}
