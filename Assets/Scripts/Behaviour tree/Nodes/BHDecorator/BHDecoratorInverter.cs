using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHDecoratorInverter : BHDecorator {
    public BHDecoratorInverter() : base()
    {
        _type = "BHDecoratorInverter";
    }

    public override BHNodeState start()
    {
        BHNodeState result = _childs[0].start();

        if (result == BHNodeState.OK)
        {
            Debug.Log("child finished, returning error");
            return BHNodeState.ERROR;
        }

        if (result == BHNodeState.ERROR)
        {
            Debug.Log("child failed, returning finished");
            return BHNodeState.OK;
        }

        if (result == BHNodeState.EXECUTING)
        {
            Debug.Log("child is executing, returning the same");
            return BHNodeState.EXECUTING;
        }

        return BHNodeState.OK;
    }
}
