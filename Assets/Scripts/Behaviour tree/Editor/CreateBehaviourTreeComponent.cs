using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateBehaviourTreeComponent {
    [MenuItem("Assets/Create/Behaviour tree")]
    public static void CreateAsset()
    {
        CustomAssetUtility.CreateAsset<BehaviourTreeComponent>();
    }
}
