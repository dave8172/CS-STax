//ReceiptItem has the tax calculation of each item, which is then added to Receipt, to calculate total

namespace SalesTax.Models
{
    public class ReceiptItem
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal PriceWithTax { get; set; }
    }
}