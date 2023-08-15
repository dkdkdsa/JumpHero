using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LOG : MonoBehaviour
{
    public const string TokenKey = "token";
    private TextField emailField;
    private TextField passwordField;
    private UIDocument document;
    private void OnEnable()
    {

        document = GetComponent<UIDocument>();

        var root = document.rootVisualElement;

        emailField = root.Q<TextField>("EmailInput");
        passwordField = root.Q<TextField>("PasswordInput");

        root.Q<Button>("OK").RegisterCallback<ClickEvent>(OnLoginHd);
        root.Q<Button>("NO").RegisterCallback<ClickEvent>(OnCancelHd);

    }

    private void OnCancelHd(ClickEvent evt)
    {

        SceneManager.LoadScene("Intro");

    }

    private void OnLoginHd(ClickEvent evt)
    {
        LoginDTO loginDTO = new LoginDTO { email = emailField.value, password = passwordField.value };

        NetworkManager.instance.PostReq("user/login", loginDTO, (type, json) =>
        {

            SceneManager.LoadScene("Intro");

            if (type == MessageType.SUCCESS)
            {

                var dto = JsonUtility.FromJson<TokenRepDTO>(json);
                PlayerPrefs.SetString(TokenKey, dto.token);
                GameManager.instance.usname = dto.user.name;

            }

        });
    }
}
