using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Billing
{
    [CreateAssetMenu(fileName = "ProductsData", menuName = "Billing/Products", order = 1)]
    public class ProductScriptableObject : ScriptableObject
    {
        public List<string> ConsumableProducts = new List<string>();
        public List<string> NonConsumableProducts = new List<string>();
        public List<string> SubscriptionProducts = new List<string>();
    }
}