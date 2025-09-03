using System;

namespace SalesTax.Core
{
    public class TaxCalculator : ITaxCalculator
    {
        private const decimal BASIC_SALES_TAX_RATE = 0.10M;
        private const decimal IMPORT_DUTY_RATE = 0.05M;
        private const decimal ROUNDING_STEP = 0.05M;

        public decimal CalculateTax(decimal price, int qty, bool isImported, bool isExempt)
        {
            decimal taxRate = 0;
            if (!isExempt)
            {
                taxRate += BASIC_SALES_TAX_RATE;
            }
            if (isImported)
            {
                taxRate += IMPORT_DUTY_RATE;
            }

            if (taxRate == 0)
            {
                return 0;
            }

            decimal taxAmount = qty * price * taxRate;
            return RoundTax(taxAmount);
        }

        private decimal RoundTax(decimal taxAmount)
        {
            return Math.Ceiling(taxAmount / ROUNDING_STEP) * ROUNDING_STEP;
        }
    }
}