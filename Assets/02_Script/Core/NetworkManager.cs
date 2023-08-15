using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum MessageType
{

    ERROR = 1,
    SUCCESS = 2,
    EMPTY = 3

}

public class NetworkManager
{
    private string host;
    private int port;
    public UserVO user;

    public static NetworkManager instance;
    public NetworkManager(string host, int port)
    {

        this.host = host;
        this.port = port;

    }

    private void SetReqToken(UnityWebRequest req)
    {

        if(!string.IsNullOrEmpty(GameManager.instance.token)) 
        {

            req.SetRequestHeader("Authorization", $"Bearer{GameManager.instance.token}");

        }

    }

    public void GetReq(string uri, string qurey, Action<MessageType, string> cb)
    {

        GameManager.instance.StartCoroutine(GetCo(uri, qurey, cb));

    }
    public void PostReq(string uri, Payload payload, Action<MessageType, string> cb)
    {

        GameManager.instance.StartCoroutine(PostCo(uri, payload, cb));

    }


    public void DoAuth()
    {


        GetReq("user", "", (type, json) =>
        {

            if(type == MessageType.SUCCESS) 
            {

                var user = JsonUtility.FromJson<UserVO>(json);
                Debug.Log(json);
                GameManager.instance.usname = user.name;
                

            }


        });

    }
    private IEnumerator GetCo(string uri, string qurey, Action<MessageType, string> cb)
    {

        string url = $"{host}:{port}/{uri}{qurey}";
        UnityWebRequest req = UnityWebRequest.Get(url);

        SetReqToken(req);

        yield return req.SendWebRequest();

        if(req.result != UnityWebRequest.Result.Success)
        {

            Debug.LogError($"{url} : {req.responseCode} :: ¾ÆÁÖ ²ûÂïÇÑ ¿¡·¯¹ß»ý");
            yield break;

        }

        MessageDTO messageDto = JsonUtility.FromJson<MessageDTO>(req.downloadHandler.text);

        cb?.Invoke(messageDto.type, messageDto.message);
        Debug.Log(req.downloadHandler.text);

        req.Dispose();

    }
    private IEnumerator PostCo(string uri, Payload payload, Action<MessageType, string> cb)
    {

        string url = $"{host}:{port}/{uri}";
        UnityWebRequest req = UnityWebRequest.Post(url, payload.GetJsonString(), "application/json");
        //req.SetRequestHeader("Content-Type", "application/json");
        SetReqToken(req);

        yield return req.SendWebRequest();

        if(req.result != UnityWebRequest.Result.Success)
        {

            Debug.LogError($"{url} : {req.responseCode} :: ¾ÆÁÖ ²ûÂïÇÑ ¿¡·¯¹ß»ý");
            yield break;

        }
        MessageDTO messageDto = JsonUtility.FromJson<MessageDTO>(req.downloadHandler.text);

        cb?.Invoke(messageDto.type, messageDto.message);

        req.Dispose();

    }

}
