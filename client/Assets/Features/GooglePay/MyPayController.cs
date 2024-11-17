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
    
    private List<Button> allBtns = new List<Button>();

    
    public static List<BtnInfo> _allBtnInfos = new List<BtnInfo>()
    {
        new BtnInfo() { Title = "Init Client", OnClick = DoClientInit },
        new BtnInfo() { Title = "FetchProducts", OnClick = DoFetchProducts },
        
        
        new BtnInfo() { Title = "Detail:purchased", OnClick = DoTestPurchased },
        new BtnInfo() { Title = "Detail:canceled", OnClick = DoTestCanceled },
        new BtnInfo() { Title = "Detail:item_unavailable", OnClick = DoTestItemUnavailable },
        new BtnInfo() { Title = "Detail:refunded", OnClick = DoTestRefunded },
        
        new BtnInfo() { Title = "Pay:purchased", OnClick = DoPayPurchased },
        new BtnInfo() { Title = "Pay:canceled", OnClick = DoPayCanceled },
        new BtnInfo() { Title = "Pay:item_unavailable", OnClick = DoPayItemUnavailable },
        new BtnInfo() { Title = "Pay:refunded", OnClick = DoPayRefunded },
    };

    private static void DoTestPurchased()
    {
        TestProductDetail("android.test.purchased");
    }

    private static void DoTestCanceled()
    {
        TestProductDetail("android.test.purchased");
    }
    private static void DoTestItemUnavailable()
    {
        TestProductDetail("android.test.item_unavailable");
    }
    private static void DoTestRefunded()
    {
        TestProductDetail("android.test.refunded");
    }

    private static void DoPayPurchased()
    {
        TestProductDetail("android.test.purchased");
    }

    private static void DoPayCanceled()
    {
        TestProductDetail("android.test.purchased");
    }
    private static void DoPayItemUnavailable()
    {
        TestProductDetail("android.test.item_unavailable");
    }
    private static void DoPayRefunded()
    {
        TestProductDetail("android.test.refunded");
    }
    
    private static void TestProductDetail(string productId)
    {
        DebugInfo($"TestProductDetail {productId}");
        GooglePlayBilling._instance.QueryProductDetails(productId);
    }
    
    private static void DoPayProduct(string productId)
    {
        DebugInfo($"DoPayProduct {productId}");
        GooglePlayBilling._instance.LaunchPurchaseFlow(productId);
    }

    private static void DebugInfo(string info)
    {
        Debug.Log($"[Unity] [Billing]: {info}");
    }
   
    private static void DoFetchProducts()
    {
        Debug.Log($"[Unity]: google pay DoFetchProducts");
        GooglePlayBilling.Instance.FetchProducts();
    }

    private static void DoClientInit()
    {
        Debug.Log($"[Unity]: google pay DoClientInit");
        GooglePlayBilling.Instance.Initialize();
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

        for (int i = 0; i < _allBtnInfos.Count; i++)
        {
            var btnInfo = _allBtnInfos[i];
            allBtns[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{i}:{btnInfo.Title}";
            allBtns[i].onClick.AddListener(btnInfo.OnClick);
        }
    }

    
    public class BtnInfo
    {
        public string Title;
        public UnityAction OnClick;
    }

}
