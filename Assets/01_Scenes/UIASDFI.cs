using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIASDFI : MonoBehaviour
{

    public TMP_Text nameTex;
    public TMP_Text lankTex;

    public void SceneL(string value) => SceneManager.LoadScene(value);

    private void Start()
    {


        NetworkManager.instance.GetReq("game", "", (type, json) =>
        {

            lankTex.text = json;

        });


    }

}
