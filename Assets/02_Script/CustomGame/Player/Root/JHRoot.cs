using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHRoot
{

    protected JHController controller;
    protected Transform transform;
    protected GameObject gameObject;
    protected PlayerValueSystem playerValueSystem;

    public JHRoot(JHController controller) 
    { 
        
        this.controller = controller;
        playerValueSystem = controller.GetComponent<PlayerValueSystem>();
        gameObject = controller.gameObject;
        transform = controller.transform;

    }

    public virtual void Init() { }
    public virtual void EnterState() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void ExitState() { }

}
