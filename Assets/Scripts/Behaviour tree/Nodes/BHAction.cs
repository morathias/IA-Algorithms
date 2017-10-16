using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHAction : BHNode {

    public BHAction(): base() {
        _type = "action";
    }

    protected override bool canHaveChilds() { return false; }
}
