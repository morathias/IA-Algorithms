using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHLogicOr : BHLogic {
    public BHLogicOr() : base()
    {
        _type = "BHLogicOr";
    }

    public override BHNodeState start()
    {
        if (_childs[0].start() == BHNodeState.OK)
            return BHNodeState.OK;

        if (_childs[1].start() == BHNodeState.OK)
            return BHNodeState.OK;

        return BHNodeState.ERROR;
    }
}
