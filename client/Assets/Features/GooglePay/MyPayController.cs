using System;
using System.Collections;
using System.Collections.Generic;
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
    public RectTransform rectTransform;
    
    private List<Button> allBtns = new List<Button>();

    private const string TAG = "PayController";

    public string ServerIP = "127.0.0.1:14100";

    
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
        _purchasingClient.DoLaunchPurchaseFlow(productId, $"Pay_{DateTime.Now}");
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
            LoggerEx.Debug(TAG, $"On Purchasing Info:{result}");
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

        for (int i = 0; i < btnBoard.childCount; i++)
        {
            var obj = btnBoard.GetChild(i);
            var btn = obj.GetComponent<Button>();
            allBtns.Add(btn);
        }



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

    
    public class BtnInfo
    {
        public string Title;
        public UnityAction OnClick;
    }

}
