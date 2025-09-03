/*This defines the Item model, and exemptitem or non-exempt item extends from abstract item class
It has taxCalculator object(extends from interface) so that tax of each item can be directly calculated
without knowing how tax calculator works.
*/

using System.Dynamic;
using SalesTax.Core;

namespace SalesTax.Models
{
    public abstract class ItemBase
    {
        public string? Name { get; set; } //? operator denotes a nullable reference type
        public decimal Price { get; set; }
        public bool IsImported { get; set; }
        public int Quantity { get; set; }

        protected readonly ITaxCalculator _taxCalculator;

        protected ItemBase(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        public abstract decimal CalculateTax();
    }

    public class ExemptItem : ItemBase
    {
        public ExemptItem(ITaxCalculator taxCalculator) : base(taxCalculator) { }

        public override decimal CalculateTax()
        {
            //true for exempt Tax
            return _taxCalculator.CalculateTax(Price, Quantity, IsImported, true);
        }
    }

    public class NonExemptItem : ItemBase
    {
        public NonExemptItem(ITaxCalculator taxCalculator) : base(taxCalculator) { }

        public override decimal CalculateTax()
        {
            //false for Non-exmept item
            return _taxCalculator.CalculateTax(Price, Quantity, IsImported, false);
        }
    }
}
