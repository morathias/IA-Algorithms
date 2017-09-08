using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHConditional : BHNode {
    public BHConditional(): base() {
        _type = "BHConditional";
    }

    protected override bool canHaveChilds(){ return false; }
}
