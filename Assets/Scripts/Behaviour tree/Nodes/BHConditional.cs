using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHConditional : BHNode {
    public BHConditional(): base() {
        _type = "BHConditional";
    }

    protected override bool canHaveChilds(){ return false; }

    public override BHNodeState start()
    {
        BHNodeState result = _nodeFunction();

        if (result == BHNodeState.EXECUTING)
            return BHNodeState.ERROR;

        return result;
    }
}
