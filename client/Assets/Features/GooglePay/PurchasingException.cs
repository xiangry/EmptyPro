using System;

namespace Features.Purchasing
{
    public class PurchasingException : Exception
    {
        public PurchasingException(string info):base($"Purchasing Exception:{info}")
        {
            
        }
        
    }
}