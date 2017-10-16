using System.Collections;
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

        Debug.Log("starting sequencer");

        while (i < _childs.Count)
        {
            result = _childs[i].start();

            if (result == BHNodeState.OK)
            {
                Debug.Log("child " + i + " finished");
                i++;
            }

            else if (result == BHNodeState.EXECUTING)
            {
                Debug.Log("child " + i + " executing");
                return result;
            }

            else break;
        }

        Debug.Log("child " + i + " failed");
        i = 0;
        return BHNodeState.ERROR;
    }
}
