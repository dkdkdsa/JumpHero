using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;




public class LunchUI : WindowUI
{


    private TextField dateField;
    private Label lunchLabel;

    public LunchUI(VisualElement root) : base(root)
    {
        dateField = root.Q<TextField>("DateField");
        lunchLabel = root.Q<Label>("LunchLabel");

        root.Q<Button>("LoadBTN").RegisterCallback<ClickEvent>(OnLoadButtonHandle);
        root.Q<Button>("CloseBTN").RegisterCallback<ClickEvent>(OnCloseButtonHandle);
    }


    private void OnLoadButtonHandle(ClickEvent evt)
    {

        string dateStr = dateField.value;

        Regex regex = new Regex(@"20[0-9]{2}[0-1][0-9][0-3][0-9]");

        if (!regex.IsMatch(dateStr))
        {

            UIController.Instance.messageSystem.AddMessage("날짜 확인", 1);
            return;

        }

        NetworkManager.instance.GetReq("lunch", $"?date={dateStr}", (type, json) =>
        {


            if(type == MessageType.SUCCESS)
            {

                LunchVO vo = JsonUtility.FromJson<LunchVO>(json);

                string meunStr = vo.menus.Aggregate("", (sum ,x) => sum + x + '\n');


                lunchLabel.text = meunStr;

            }
            else
            {

                Debug.LogError(json);

            }

        });



    }

    private void OnCloseButtonHandle(ClickEvent evt)
    {

        Close();

    }


}
