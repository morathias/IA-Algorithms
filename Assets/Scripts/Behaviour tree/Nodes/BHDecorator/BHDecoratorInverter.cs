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
            return BHNodeState.ERROR;

        if (result == BHNodeState.ERROR)
            return BHNodeState.OK;

        if (result == BHNodeState.EXECUTING)
            return BHNodeState.EXECUTING;

        return BHNodeState.OK;
    }
}
