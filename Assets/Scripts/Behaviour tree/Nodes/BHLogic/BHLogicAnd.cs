using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHLogicAnd : BHLogic {
    public BHLogicAnd() : base()
    {
        _type = "BHLogicAnd";
    }

    public override BHNodeState start()
    {
        if (_childs[0].start() == BHNodeState.OK &&
           _childs[1].start() == BHNodeState.OK)
            return BHNodeState.OK;

        return BHNodeState.ERROR;
    }
}
