/*Problem 1: Sales Tax

Basic sales tax is applicable at a rate of 10% on all goods, except books, food,
and medical products that are exempt. Import duty is an additional sales tax
applicable on all imported goods at a rate of 5%, with no exemptions.

When I purchase items I receive a receipt which lists the name of all the items
and their price (including tax), finishing with the total cost of the items,
and the total amounts of sales taxes paid.  The rounding rules for sales tax are
that for a tax rate of n%, a shelf price of p contains (np/100 rounded up to
the nearest 0.05) amount of sales tax.

Write an application that prints out the receipt details for these shopping baskets...
INPUT:
Input 1:
1 book at 12.49
1 music CD at 14.99
1 chocolate bar at 0.85

Input 2:
1 imported box of chocolates at 10.00
1 imported bottle of perfume at 47.50

Input 3:
1 imported bottle of perfume at 27.99
1 bottle of perfume at 18.99
1 packet of headache pills at 9.75
1 box of imported chocolates at 11.25

Output 1
Output 1:
1 book: 12.49
1 music CD: 16.49
1 chocolate bar: 0.85
Sales Taxes: 1.50
Total: 29.83

Output 2:
1 imported box of chocolates: 10.50
1 imported bottle of perfume: 54.65
Sales Taxes: 7.65
Total: 65.15

Output 3:
1 imported bottle of perfume: 32.19
1 bottle of perfume: 20.89
1 packet of headache pills: 9.75
1 imported box of chocolates: 11.85
Sales Taxes: 6.70
Total: 74.68
-----------------------------------------------------------------------------------------

SOLUTION:
Flow:
1. Get UserInput in pre-defined format, parse it and get list of items
2. Pass that list of items to ReceiptGenerator, which 
    a. creates each line receiptItem with item price + tax
    b. pass that list of receiptItems to receipt, which calculates total tax and price and outputs 
    in specified format

Models (modules):
1. ItemBase - blueprint for item typ with currently exempt and non-exempt types
2. ReceiptItem - each line item of receipt with item name and price+tax
3. Receipt - list of ReceiptItems for printing and calculating total tax and price

Core/ Services (logic):
1. InputHandler - gets user input, parses into individual items, using regex for matching against format.
It has list of keywords to determine the type of item, whether exempt or non-exempt
2. TaxCalculator - tax calculation logic based on sales tax and import tax specified
3. ReceiptGenerator - gets list of items, creates each line receiptItem with the help of TaxCalculator,
populates the receipt with list of receiptItems and prints it

(TaxCalculator extends from an interface just for learning,
each item type has TaxCalculator object to calculate tax easily on each item type).

*/

using SalesTax.Core;
using SalesTax.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesTax.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var receiptGenerator = new ReceiptGenerator();

            Console.WriteLine("Welcome to the Sales Tax Calculator!");
            Console.WriteLine("====================================");

            try
            {
                //1. Get user input, parse into indivudal items
                var items = UserInputHandler.GetItemsFromUserInput();
                if (items.Any())
                {
                    //2. If items are parsed correctly, pass those list of items to calculate tax and print receipt
                    receiptGenerator.GenerateReceipt(items);
                    Console.WriteLine("Thank you for using the Sales Tax Calculator!");
                }
                else throw new FormatException($"No items were entered.: {items.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
