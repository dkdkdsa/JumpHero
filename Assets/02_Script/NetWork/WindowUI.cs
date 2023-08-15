using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WindowUI
{

    protected VisualElement root;
    protected bool isStart;

    public WindowUI(VisualElement root) 
    {
        
        this.root = root;

    }

    public virtual void Open()
    {

        root.RemoveFromClassList("fade");

    }

    public virtual void Close() 
    {

        isStart = true;
        root.AddToClassList("fade");

    }


}
