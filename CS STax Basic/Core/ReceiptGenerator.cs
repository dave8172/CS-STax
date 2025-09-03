/*This gets list of items from input, calculates tax and creates receipt object with name, price with tax
and calculates total tax and price. It then outputs in the requird format. 
*/

using SalesTax.Models;
using System.Collections.Generic;

namespace SalesTax.Core
{
    public class ReceiptGenerator
    {
        public void GenerateReceipt(List<ItemBase> items)
        {
            var receipt = new Receipt();

            foreach (var item in items)
            {
                var tax = item.CalculateTax();
                var priceWithTax = (item.Price * item.Quantity) + tax;

                receipt.Items.Add(new ReceiptItem
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    PriceWithTax = priceWithTax
                });

                receipt.TotalSalesTax += tax;
                receipt.TotalPrice += priceWithTax;
            }

            PrintReceipt(receipt);
        }

        public static void PrintReceipt(Receipt receipt)
        {
            Console.WriteLine("\n Format: Quantity product_name: Price with tax included" +
                                "\n with total Tax and total amount in the end" +
                                "\n ------------ Your Receipt ------------ \n");

            foreach (var item in receipt.Items)
            {
                Console.WriteLine($"{item.Quantity} {item.Name}: {item.PriceWithTax:0.00}");
            }
            Console.WriteLine($"Sales Taxes: {receipt.TotalSalesTax:0.00}");
            Console.WriteLine($"Total: {receipt.TotalPrice:0.00}");
            Console.WriteLine("--------------------\n");
        }
    }
}
