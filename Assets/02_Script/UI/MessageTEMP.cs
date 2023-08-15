using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageTEMP
{

    private VisualElement root;
    private Label label;
    private float timer = 0;
    private float currentTimer = 0;
    private bool fade;
    private bool isComp;

    public VisualElement Root { get {  return root; } }
    public bool IsComp { get { return isComp; } }
    public string Text { get { return label.text; } set { label.text = value; } }

    public MessageTEMP(VisualElement root, float timer)
    {

        this.root = root;
        label = root.Q<Label>("Message");
        currentTimer = 0;
        fade = false;
        this.timer = timer;

    }

    public void UpdateMessage()
    {

        currentTimer += Time.deltaTime;

        if(currentTimer >= timer && !fade)
        {

            root.AddToClassList("off");
            fade = true;

        }

        if(currentTimer >= timer + 0.6f)
        {

            isComp = true;

        }

    }

}
