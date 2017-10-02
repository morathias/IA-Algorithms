using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHSelector : BHNode {
    int i = 0;

    public BHSelector() : base()
    {
        _type = "BHSelector";
    }

    public override BHNodeState start()
    {
        BHNodeState result;

        while (i < _childs.Count)
        {
            result = _childs[i].start();

            if (result == BHNodeState.ERROR)
            {
                Debug.Log("child " + i + " failed");
                i++;
            }

            else if (result == BHNodeState.EXECUTING)
            {
                Debug.Log("child " + i + " executing");
                return result;
            }

            else break;
        }

        Debug.Log("child " + i + " finished");
        i = 0;
        return BHNodeState.OK;
    }
}
