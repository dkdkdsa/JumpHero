using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisonSencer : SencerRoot
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        isSencing = true;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        isSencing = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        isSencing = false;

    }

}
