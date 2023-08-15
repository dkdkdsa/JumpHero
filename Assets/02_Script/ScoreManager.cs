using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;


    public void Release()
    {

        Destroy(gameObject);
        instance = null;

    }

}
