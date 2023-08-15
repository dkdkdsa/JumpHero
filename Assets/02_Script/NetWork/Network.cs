using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageDTO
{

    public MessageType type;
    public string message;
    public string color;

}

public class LoginDTO : Payload
{

    public string email;
    public string password;

    public string GetJsonString()
    {

        return JsonUtility.ToJson(this);

    }

    public string GetQureyString()
    {
        return $"?email{email}&?password={password}";
    }
}

public class LunchVO
{

    public string date;
    public List<string> menus;

}

[System.Serializable]
public class UserVO
{

    public int id;
    public string email;
    public int exp;
    public string name;

}

public class TokenRepDTO
{

    public string token;
    public UserVO user;

}

public class UserInfoPanel
{

    private VisualElement root;
    private Button infoBtn;
    private UserVO user;

    public UserPopOver userPop { get; private set; }

    public UserVO userName { get => user; 
        set
        { 
            user = value;
            infoBtn.text = user.name;
            userPop.userName = user.name;
            userPop.email = user.email;
            userPop.Exp = user.exp;
        
        } 
    }

    public UserInfoPanel(VisualElement root, VisualElement pop,EventCallback<ClickEvent> logout)
    {
        this.root = root;
        infoBtn = root.Q<Button>("InfoBTN");
        root.Q<Button>("LogoutBTN").RegisterCallback<ClickEvent>(logout);
        userPop = new UserPopOver(pop);


        infoBtn.RegisterCallback<MouseEnterEvent>((evt) =>
        {

            Rect rect = infoBtn.worldBound;
            Vector2 pos = rect.position;
            pos.y += rect.height + 10;
            userPop.PopOver(pos);


        });

        infoBtn.RegisterCallback<MouseLeaveEvent>((evt) =>
        {

            userPop.Hide();


        });

    }



    public void Show(bool v)
    {

        if (v)
        {

            root.RemoveFromClassList("widthzero");

        }
        else
        {

            root.AddToClassList("widthzero");

        }

    }
}

public class UserPopOver
{

    private VisualElement root;
    private Label nameLabel;
    private Label emailLabel;
    private Label expLabel;
    private int exp;

    public string userName { get { return nameLabel.text; } set { nameLabel.text = value; } }
    public string email { get { return emailLabel.text; } set { emailLabel.text = value; } }
    public int Exp { get { return exp; } set {exp = value; expLabel.text = exp.ToString(); } }

    public UserPopOver(VisualElement root)
    {

        this.root = root;
        nameLabel = root.Q<Label>("NameLabel");
        emailLabel = root.Q<Label>("EmailLabel");
        expLabel = root.Q<Label>("ExpLabel");

    }

    public void PopOver(Vector2 pos)
    {

        root.style.top = new Length(pos.y, LengthUnit.Pixel);
        root.style.left = new Length(pos.x, LengthUnit.Pixel);

        root.transform.scale = Vector3.one;

    }

    public void Hide()
    {

        root.transform.scale = new Vector3(1, 0, 1);

    }

}

[System.Serializable]
public class ItemVO
{

    public int itemCode;
    public int count;
    public int slotNumber;

}

public class InventoryVO : Payload
{

    public int count;
    public List<ItemVO> list;

    public string GetJsonString()
    {

        return JsonUtility.ToJson(this);

    }

    public string GetQureyString()
    {
        return "";
        //return $"?json{GetJsonString()}";

    }
    

}

[System.Serializable]
public class A
{

    public int user_id;
    public int score;
    public string memo;

}

[System.Serializable]
public class RankingVO : Payload
{

    public int user_id;
    public int score;
    public string memo;

    public string GetJsonString()
    {

        return JsonUtility.ToJson(this);

    }

    public string GetQureyString()
    {
        return "";
    }
}

public interface Payload
{

    public string GetJsonString();
    public string GetQureyString();

}


