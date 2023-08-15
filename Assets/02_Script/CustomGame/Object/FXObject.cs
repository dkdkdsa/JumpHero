using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXObject : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        
        animator = GetComponent<Animator>();

    }

    public void Play()
    {

        animator.SetTrigger("Play");

    }

    public void End()
    {

        FAED.Push(gameObject);

    }

}
