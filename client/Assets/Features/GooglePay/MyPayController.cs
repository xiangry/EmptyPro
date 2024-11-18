using System.Collections;
using System.Collections.Generic;
using Features.GooglePay;
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

    
    public List<BtnInfo> _allBtnInfos = new List<BtnInfo>()
    {
    };

    #region 切换商品

    private List<string> _allProducts = new List<string>()
    {
        "google_product_6",
        "google_product_12",
        "google_product_25",
        "google_product_30",
        "google_product_50",
        "google_product_60",
        "google_product_68",
        "google_product_88",
        "google_product_98",
        "google_product_128",
        "google_product_188",
        "google_product_328",
        "google_product_648",
    };

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
        _productIndex = (_productIndex + _allProducts.Count) % _allProducts.Count;
        _curProductString = _allProducts[_productIndex];
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
        GooglePlayBilling._instance.QueryProductDetails(productId);
    }
    
    private void DoPayProduct(string productId)
    {
        DebugInfo($"DoPayProduct {productId}");
        GooglePlayBilling._instance.LaunchPurchaseFlow(productId);
    }

    private void DebugInfo(string info)
    {
        Debug.Log($"[Unity] [Billing]: {info}");
    }
   
    private void DoClientInit()
    {
        Debug.Log($"[Unity]: google pay DoClientInit");
        GooglePlayBilling.Instance.Initialize();
    }
    
    private void DoDumpProducts()
    {
        Debug.Log($"[Unity]: google pay DoClientInit");
        GooglePlayBilling.Instance.DoDumpProducts();
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

        _curProductString = _allProducts[_productIndex];
        inputField.text = _curProductString;
     
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    
    public class BtnInfo
    {
        public string Title;
        public UnityAction OnClick;
    }

}
