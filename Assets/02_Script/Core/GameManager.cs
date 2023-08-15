using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private string host;
    [SerializeField] private int port;

    public static GameManager instance;
    public string token { get; private set; }
    public string usname { get; set; }

    private void Awake()
    {
        

        if(instance != null)
        {

            return;

        }

        DontDestroyOnLoad(this);
        instance = this;

        NetworkManager.instance = new NetworkManager(host, port);

        token = PlayerPrefs.GetString(LoginUI.TokenKey, string.Empty);

        if (!string.IsNullOrEmpty(token))
        {

            NetworkManager.instance.DoAuth();

        }



    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.W))
        {

            NetworkManager.instance.GetReq("lunch", "?date=20230703", (type, message) =>
            {

                if(type == MessageType.SUCCESS)
                {

                    LunchVO lunch = JsonUtility.FromJson<LunchVO>(message);

                    foreach(var item in lunch.menus)
                    {

                        Debug.Log(item);

                    }

                }
                else
                {

                    Debug.LogError($"¿¡·¯ : {message}");

                }

            });

        }

    }

    public void DestroyToken()
    {

        PlayerPrefs.DeleteKey(LoginUI.TokenKey);
        token = string.Empty;

    }
}
