using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Slot
{

    private VisualElement root;
    public int slotNum;

    public VisualElement Child
    {

        get => root.childCount == 0 ? null : root.Children().First();

    }

    public VisualElement elem => root;

    public Slot(VisualElement root, int slotNum)
    {

        this.root = root;
        this.slotNum = slotNum;

    }

}

public class Item
{

    private VisualElement root;
    private VisualElement sprite;
    private Label ctLabel;
    private int count;

    public ItemSO data;
    public int slotNum;

    public int Count { get => count; set { count = value; ctLabel.text = count.ToString(); } }

    public Item(VisualElement root, ItemSO data, int slotNum, int count)
    {

        this.root = root;
        this.slotNum = slotNum;
        this.data = data;

        sprite = root.Q<VisualElement>("Sprite");
        ctLabel = root.Q<Label>("CountLabel");

        sprite.style.backgroundImage = new StyleBackground(data.sprite); //이걸로 배경 교체

    }

}

public class Human
{

    public int id;
    public string name;

}

public class InvenUI : WindowUI
{

    private List<Slot> slotLs = new List<Slot>();
    private Dictionary<int, Item> itemCon = new Dictionary<int, Item>();
    private VisualTreeAsset itemTemp;
    

    public InvenUI(VisualElement root, VisualTreeAsset itemTemp) : base(root)
    {

        slotLs = root.Query(className:"slot").ToList().Select((elem, idx) => new Slot(elem, idx)).ToList();

        this.itemTemp = itemTemp;

        root.Q<Button>("ExitBtn").RegisterCallback<ClickEvent>((evt) => { Close(); });

    }

    private Slot FindSlot(Vector2 mousePosition)
    {

        return slotLs.Find(x => x.elem.worldBound.Contains(mousePosition));

    }

    private Slot FindEmpSlot()
    {

        return slotLs.Find(x => x.Child == null);

    }

    private Slot FindSlotN(int n)
    {

        return slotLs[n];

    }

    public void AddItem(ItemSO itemSO, int v, int sn = -1)
    {

        if (itemCon.TryGetValue(itemSO.itemCode, out var findIT))
        {

            findIT.Count += v;
            return;

        }

        VisualElement itemEl = itemTemp.Instantiate().Q("Item");
        Slot empSlot;

        if(sn < 0)
        {

            empSlot = FindEmpSlot();

            if(empSlot == null)
            {

                UIController.Instance.messageSystem.AddMessage("카ㅣㄴ부봊", 3);
                return;

            }

        }
        else
        {

            empSlot = FindSlotN(sn);

        }

        empSlot.elem.Add(itemEl);

        var item = new Item(itemEl, itemSO, empSlot.slotNum, v);
        itemCon.Add(itemSO.itemCode, item);

        itemEl.AddManipulator(new Dragger((evt, target, bfSlot) =>
        {

            var slot = FindSlot(evt.mousePosition);

            if (slot == null)
            {

                target.RemoveFromHierarchy();
                bfSlot.Add(target);

            }
            else if (slot.Child != null)
            {
                VisualElement existItem = slot.Child;
                existItem.RemoveFromHierarchy();
                slot.elem.Add(target);

                foreach (var kvP in itemCon)
                {
                    if (kvP.Value.slotNum == slot.slotNum)
                    {
                        kvP.Value.slotNum = FindSlotByElement(bfSlot).slotNum;
                        break;
                    }
                }

                item.slotNum = slot.slotNum;
                bfSlot.Add(existItem);
            }
            else
            {

                target.RemoveFromHierarchy();
                item.slotNum = slot.slotNum;
                slot.elem.Add(target);
            }

        }));

    }

    private Slot FindSlotByElement(VisualElement beforeSlot)
    {

        return slotLs.Find(s => s.elem == beforeSlot);

    }

    public void SaveToDB()
    {

        List<ItemVO> voLs = itemCon.Select(kbp => kbp.Value).ToList().Select(item => new ItemVO { itemCode = item.data.itemCode, count = item.Count, slotNumber = item.slotNum }).ToList();

        InventoryVO invenVo = new InventoryVO { list = voLs, count = slotLs.Count};

        if (NetworkManager.instance == null) return;

        NetworkManager.instance.PostReq("inven", invenVo, (type, meg) =>
        {


            if(type == MessageType.ERROR)
            {

                UIController.Instance.messageSystem.AddMessage(meg, 3);
                return;

            }

        });

    }

    private void LoadToDB()
    {

        itemCon.Clear();
        root.Query(className: "item").ToList().ForEach(item => { item.RemoveFromHierarchy(); });

        NetworkManager.instance.GetReq("inven", "", (type, meg) =>
        {

            if (type == MessageType.ERROR)
            {

                UIController.Instance.messageSystem.AddMessage(meg, 3);
                return;

            }

            if (type == MessageType.SUCCESS)
            {

                var vo = JsonUtility.FromJson<InventoryVO>(meg);


                vo.list.ForEach(item =>
                {

                    var data = UIController.Instance.itemLS.Find(x => x.itemCode == item.itemCode);
                    AddItem(data, item.count, item.slotNumber);

                });

            }


        });

    }

    public override void Close()
    {
        if (!root.ClassListContains("fade") && isStart)
        {

            SaveToDB();

        }
        base.Close();

    }

    public override void Open()
    {

        base.Open();
        LoadToDB();

    }

}
