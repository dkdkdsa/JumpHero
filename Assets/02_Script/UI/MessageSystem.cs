using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MessageSystem : MonoBehaviour
{

    [SerializeField] private VisualTreeAsset messageTemp;

    private List<MessageTEMP> messageTempList = new List<MessageTEMP>();
    private VisualElement container;

    private void Update()
    {

        for (int i = 0; i < messageTempList.Count; i++)
        {
            MessageTEMP item = messageTempList[i];
            item.UpdateMessage();

            if (item.IsComp)
            {

                item.Root.RemoveFromHierarchy();
                messageTempList.RemoveAt(i);
                --i;

            }

        }

    }

    public void AddMessage(string text, float timer)
    {

        var msgElem = messageTemp.Instantiate().Q("MessageBox");
        var msg = new MessageTEMP(msgElem, timer);

        msg.Text = text;

        container.Add(msgElem);
        messageTempList.Add(msg);

    }

    public void SetContainer(VisualElement con)
    {

        container = con;

    }

}
