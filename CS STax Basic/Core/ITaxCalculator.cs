namespace SalesTax.Core
{
    public interface ITaxCalculator
    {
        decimal CalculateTax(decimal price, int qty, bool isImported, bool isExempt);
    }
}