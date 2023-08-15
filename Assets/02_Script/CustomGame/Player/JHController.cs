using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JHState
{

    Idle,
    Jump,
    Die

}

public class JHController : MonoBehaviour
{

    private Dictionary<JHState, HashSet<JHRoot>> stateContainer = new Dictionary<JHState, HashSet<JHRoot>>();
    private HashSet<JHRoot> currentContainer = new HashSet<JHRoot>();

    public JHState currentState {  get; private set; } = JHState.Idle;

    private void Awake()
    {

        var playerArrowRotate = new PlayerArrowRotate(this);
        var jumpStateChanger = new JumpStateChanger(this);
        var playerJump = new PlayerJump(this);
        var playerWallSlide = new PlayerWallSlide(this);

        stateContainer.Add(JHState.Idle, new HashSet<JHRoot>
        {

            playerArrowRotate,
            playerJump,
            playerWallSlide

        });

        stateContainer.Add(JHState.Jump, new HashSet<JHRoot>
        {

            jumpStateChanger

        });

        stateContainer.Add(JHState.Die, new HashSet<JHRoot>
        {



        });

        ChageState(currentState);

    }

    private void Update()
    {
        
        for(var item = currentContainer.GetEnumerator(); item.MoveNext();)
        {

            item.Current.Update();

        }

    }

    private void FixedUpdate()
    {

        for (var item = currentContainer.GetEnumerator(); item.MoveNext();)
        {

            item.Current.FixedUpdate();

        }

    }

    public void ChageState(JHState state)
    {

        foreach(var item in currentContainer)
        {

            item.ExitState();

        }

        currentState = state;
        currentContainer = stateContainer[currentState];

        foreach (var item in currentContainer)
        {

            item.EnterState();

        }

    }

}
