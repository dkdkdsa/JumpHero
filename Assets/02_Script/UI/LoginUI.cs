using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginUI : WindowUI
{

    public const string TokenKey = "token";
    private TextField emailField;
    private TextField passwordField;

    public LoginUI(VisualElement root) : base(root)
    {

        emailField = root.Q<TextField>("EmailInput");
        passwordField = root.Q<TextField>("PasswordInput");

        root.Q<Button>("OK").RegisterCallback<ClickEvent>(OnLoginHd);
        root.Q<Button>("NO").RegisterCallback<ClickEvent>(OnCancelHd);

    }

    private void OnCancelHd(ClickEvent evt)
    {

        Close();

    }

    private void OnLoginHd(ClickEvent evt)
    {

        LoginDTO loginDTO = new LoginDTO{ email = emailField.value, password = passwordField.value};

        NetworkManager.instance.PostReq("user/login", loginDTO, (type, json) =>
        {

            if(type == MessageType.SUCCESS)
            {

                var dto = JsonUtility.FromJson<TokenRepDTO>(json);
                PlayerPrefs.SetString(TokenKey, dto.token);

                Close();
                UIController.Instance.SetLogin(dto.user);

            }
            else
            {

                UIController.Instance.messageSystem.AddMessage(json, 3);

            }

        });

    }

}
