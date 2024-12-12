using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Features.Purchasing;
using Framework.Log;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MyPayController : MonoBehaviour
{
    public Transform btnBoard;

    public TMP_InputField inputField;
    public TMP_InputField serverInfoField;
    public Button changeServerBtn;
    public RectTransform rectTransform;
    
    private List<Button> allBtns = new List<Button>();

    private const string TAG = "PayController";

    
    
    public List<BtnInfo> _allBtnInfos = new List<BtnInfo>()
    {
    };
    
    #region 切换商品

    private IPurchasing _purchasingClient;


    private int _productIndex = 0;
    private string _curProductString = "";
    
    private void SwitchProductPer()
    {
        _productIndex--;
        OnSwitchProduct(nameof(SwitchProductPer));
    }
    
    private void SwitchProductNext()
    {
        _productIndex++;
        OnSwitchProduct(nameof(SwitchProductNext));
    }

    private void OnSwitchProduct(string tag)
    {
        var allProducts = BillingConfig.AllProducts;
        _productIndex = (_productIndex + BillingConfig.AllProducts.Count) % allProducts.Count;
        _curProductString = allProducts[_productIndex];
        inputField.text = _curProductString;
        DebugInfo($"{tag} => {_productIndex} : {_curProductString}");
    }

    #endregion
    
    
    private void QueryProductDetail()
    {
        TestProductDetail(_curProductString);
    }
    private  void BuyProduct()
    {
        DoPayProduct(_curProductString);
    }

    private void TestProductDetail(string productId)
    {
        DebugInfo($"TestProductDetail {productId}");
        _purchasingClient.DoQueryProductDetails(productId);
    }
    
    private void DoPayProduct(string productId)
    {
        DebugInfo($"DoPayProduct {productId}");
        _purchasingClient.DoLaunchPurchaseFlow(productId, $"my_product_{productId}-{DateTime.Now}");
    }

    private void DebugInfo(string info)
    {
        Debug.Log($"[Unity] [Billing]: {info}");
    }
   
    private void DoClientInit()
    {
        Debug.Log($"[Unity]: google pay DoClientInit");
        _purchasingClient.InitializeClient(BillingConfig.AllProducts, result =>
        {
            LoggerEx.Debug(TAG, $"[OnPurchasingResult]:{result}");
            if (result.eventType == MyPurchasingEventType.Purchasing)
            {
                ServerClient.Instance.SendPurchasingInfo(result.receipt);
            }
        });
    }
    
    private void DoDumpProducts()
    {
        Debug.Log($"[Unity]: google pay DoClientInit");
        _purchasingClient.DoDumpProducts();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this);

        var serverIp = ServerClient.Instance.ServerIp;
        var serverIpPort = ServerClient.Instance.ServerPort;
        serverInfoField.text = $"{serverIp}:{serverIpPort}";
        
        LoggerEx.RegisterLogger(new ServerLogger());

        for (int i = 0; i < btnBoard.childCount; i++)
        {
            var obj = btnBoard.GetChild(i);
            var btn = obj.GetComponent<Button>();
            allBtns.Add(btn);
        }


        changeServerBtn.onClick.AddListener(OnServerChangeBtnClicked);

        _allBtnInfos.Add(new BtnInfo() { Title = "Pre Product", OnClick = SwitchProductPer });
        _allBtnInfos.Add(new BtnInfo() { Title = "Next Product", OnClick = SwitchProductNext });


        _allBtnInfos.Add(new BtnInfo() { Title = "Init Client", OnClick = DoClientInit });
        _allBtnInfos.Add(new BtnInfo() { Title = "Dump Purchase", OnClick = DoDumpProducts });
        
        _allBtnInfos.Add(new BtnInfo() { Title = "QueryProductDetail", OnClick = QueryProductDetail });
        _allBtnInfos.Add(new BtnInfo() { Title = "BuyProduct", OnClick = BuyProduct });

        for (int i = 0; i < _allBtnInfos.Count; i++)
        {
            var btnInfo = _allBtnInfos[i];
            allBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{i}:{btnInfo.Title}";
            allBtns[i].onClick.AddListener(btnInfo.OnClick);
        }

        OnSwitchProduct("Start");

        _purchasingClient = PurchasingUtils.GetPurchasingClient();
     
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    private void OnServerChangeBtnClicked()
    {
        var serverInfo = serverInfoField.text;
        if(string.IsNullOrEmpty(serverInfo))
            return;
        var list = serverInfo.Split(":");
        if(list.Length < 2)
            return;
        ServerClient.Instance.SetNewServerInfo(list[0], list[1]);
    }


    public class BtnInfo
    {
        public string Title;
        public UnityAction OnClick;
    }

}
