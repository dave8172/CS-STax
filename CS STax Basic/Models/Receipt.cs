//Receipt model has the total receipt data of a basket

using System.Collections.Generic;

namespace SalesTax.Models
{
    public class Receipt
    {
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
        public decimal TotalSalesTax { get; set; }
        public decimal TotalPrice { get; set; }
    }
}