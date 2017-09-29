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
        if (_childs[0].start() == BHNodeState.OK)
            return BHNodeState.ERROR;

        if (_childs[0].start() == BHNodeState.ERROR)
            return BHNodeState.OK;

        if (_childs[0].start() == BHNodeState.EXECUTING)
            return BHNodeState.EXECUTING;

        return BHNodeState.OK;
    }
}
