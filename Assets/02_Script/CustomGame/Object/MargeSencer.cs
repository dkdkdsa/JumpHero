using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MargeSencer : SencerRoot
{

    [SerializeField] private SencerRoot s1, s2;

    public override bool isSencing { get => s1.isSencing && s2.isSencing; protected set {  base.isSencing = s1.isSencing && s2.isSencing; } }

}
