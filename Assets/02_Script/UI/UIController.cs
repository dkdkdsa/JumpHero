using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Windows
{

    Lunch = 1,
    Login,
    Inven
}

public class UIController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private VisualTreeAsset lunchUIAssets;
    [SerializeField] private VisualTreeAsset loginUIAssets;
    [SerializeField] private VisualTreeAsset invenUIAssets;
    [SerializeField] private VisualTreeAsset itemAssets;

    private Dictionary<Windows, WindowUI> windowDic = new Dictionary<Windows, WindowUI>();
    private UIDocument document;
    private VisualElement content;
    private LunchUI lunchUI;
    private LoginUI loginUI;
    private Button loginBTN;
    private InvenUI invenUI;
    private UserInfoPanel userinfoPanel;

    public List<ItemSO> itemLS;
    public MessageSystem messageSystem { get; private set; }

    public static UIController Instance;

    private void Awake()
    {
        
        if(Instance != null) return;
        Instance = this;

        document = GetComponent<UIDocument>();
        messageSystem = GetComponent<MessageSystem>();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            int idx = Random.Range(0, itemLS.Count);
            var inven = windowDic[Windows.Inven] as InvenUI;

            Debug.Log(idx);

            inven.AddItem(itemLS[idx], 3);

        }

    }

    private void OnEnable()
    {
        
        Button lunchBTN = document.rootVisualElement.Q<Button>("LunchBTN");
        Button invenBTN = document.rootVisualElement.Q<Button>("InvenBTN");
        loginBTN = document.rootVisualElement.Q<Button>("LogInBTN");
        lunchBTN.RegisterCallback<ClickEvent>(OnOpenLunchHandle);
        loginBTN.RegisterCallback<ClickEvent>(OnOpenLoginHandle);
        invenBTN.RegisterCallback<ClickEvent>(OnOpenInvenHandle);

        content = document.rootVisualElement.Q("Content");


        var messageCon = document.rootVisualElement.Q("MessageContainer");
        messageSystem.SetContainer(messageCon);

        var pop = document.rootVisualElement.Q("UserPopOver");
        userinfoPanel = new UserInfoPanel(document.rootVisualElement.Q("UserInfoPanel"), pop,LogOut); 



        var lunchRoot = lunchUIAssets.Instantiate().Q<VisualElement>("LunchContainer");
        content.Add(lunchRoot);
        lunchUI = new LunchUI(lunchRoot);
        windowDic.Clear();
        lunchUI.Close();
        windowDic.Add(Windows.Lunch, lunchUI);

        var loginRoot = loginUIAssets.Instantiate().Q<VisualElement>("LoginWindow");
        content.Add(loginRoot);
        loginUI = new LoginUI(loginRoot);
        loginUI.Close();
        windowDic.Add(Windows.Login, loginUI);

        var invenRoot = invenUIAssets.Instantiate().Q("InvenBody");
        content.Add(invenRoot);
        invenUI = new InvenUI(invenRoot, itemAssets);
        invenUI.Close();
        windowDic.Add(Windows.Inven, invenUI);


    }

    private void OnOpenLunchHandle(ClickEvent evt)
    {


        foreach(var item in windowDic.Values)
        {

            item.Close();

        }

        windowDic[Windows.Lunch].Open();

    }

    private void OnOpenInvenHandle(ClickEvent evt)
    {


        foreach (var item in windowDic.Values)
        {

            item.Close();

        }

        windowDic[Windows.Inven].Open();

    }

    private void OnOpenLoginHandle(ClickEvent evt)
    {


        foreach (var item in windowDic.Values)
        {

            item.Close();

        }

        windowDic[Windows.Login].Open();

    }

    public void SetLogin(UserVO user)
    {

        loginBTN.style.display = DisplayStyle.None;

        userinfoPanel.Show(true);
        userinfoPanel.userName = user;

    }

    private void LogOut(ClickEvent evt)
    {

        loginBTN.style.display = DisplayStyle.Flex;
        userinfoPanel.Show(false);
        GameManager.instance.DestroyToken();

    }

}
