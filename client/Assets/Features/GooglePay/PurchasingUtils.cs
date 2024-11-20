#if USE_IN_APP_PURCHASING
using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Log;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UIElements;

namespace Features.Purchasing
{
    public static class PurchasingUtils
    {
        public static IPurchasing GetPurchasingClient()
        {
#if USE_IN_APP_PURCHASING
            return PurchasingManager.Instance;
#endif
            return null;
        }
    }
}
#endif