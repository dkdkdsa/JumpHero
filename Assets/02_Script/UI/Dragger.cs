using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragger : MouseManipulator
{
    private bool isDrg;
    private Vector2 startPos;
    private VisualElement beforeSlot;

    private Action<MouseUpEvent, VisualElement, VisualElement> dropCB;

    public Dragger(Action<MouseUpEvent, VisualElement, VisualElement> dropCB)
    {

        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        this.dropCB = dropCB;

    }

    protected override void RegisterCallbacksOnTarget()
    {

        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);


    }

    protected override void UnregisterCallbacksFromTarget()
    {

        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);

    }

    protected void OnMouseDown(MouseDownEvent e)
    {

        if(CanStartManipulation(e))
        {

            var x = target.layout.x;
            var y = target.layout.y;
            var con = target.parent.parent;
            beforeSlot = target.parent;

            target.RemoveFromHierarchy();
            con.Add(target);


            isDrg = true;
            target.CaptureMouse();
            startPos = e.localMousePosition;

            Vector2 offset = e.mousePosition - con.worldBound.position - startPos;

            target.style.position = Position.Absolute;
            target.style.left = offset.x;
            target.style.top = offset.y;

        }

    }

    protected void OnMouseMove(MouseMoveEvent e)
    {

        if (!isDrg || !CanStartManipulation(e) || !target.HasMouseCapture()) return;

        Vector2 dif = e.localMousePosition - startPos;

        var x = target.layout.x;
        var y = target.layout.y;

        target.style.left = x + dif.x;
        target.style.top = y + dif.y;

    }

    protected void OnMouseUp(MouseUpEvent e)
    {

        if(!isDrg || !target.HasMouseCapture())
        {

            return;

        }

        isDrg = false;
        target.ReleaseMouse();

        target.style.position = Position.Relative;
        target.style.left = 0;
        target.style.top = 0;

        dropCB?.Invoke(e, target, beforeSlot);

    }

}
