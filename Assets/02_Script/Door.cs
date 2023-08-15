using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    public string uan;

    private void Start()
    {

        ///
        



    }

    public float time;
    public bool isStop;
    public bool ends;

    private void Update()
    {

        if (isStop) return;
        time += Time.deltaTime;

    }

    public int end()
    {

        isStop = true;
        return (int)time;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (ends) return;
        ends = true;
        int time = end();

        var a = new RankingVO { score = time, memo = GameManager.instance.usname, };

        NetworkManager.instance.PostReq("game", a, (type, json) =>
        {


        });

        SceneManager.LoadScene("Intro");

    }

}
