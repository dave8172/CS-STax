/* This is the first functionality that is called. It takes user input, checks for its format(against regex),
parses if correct format, and passes that list of items to ReceiptGenerator.

Regex explanation:
/
@"^(\d+)\s+(.+?)\s+at\s+([\d+\.?\d*)$
@" matches the characters @" literally (case sensitive)
^ asserts position at start of a line

1st Capturing Group (\d+)
\d matches a digit (equivalent to [0-9])
+ matches the previous token between one and unlimited times, as many times as possible, giving back as needed (greedy)
\s matches any whitespace character (equivalent to [\r\n\t\f\v ])
+ matches the previous token between one and unlimited times, as many times as possible, giving back as needed (greedy)

2nd Capturing Group (.+?)
. matches any character (except for line terminators)
+? matches the previous token between one and unlimited times, as few times as possible, expanding as needed (lazy)
\s matches any whitespace character (equivalent to [\r\n\t\f\v ])
+ matches the previous token between one and unlimited times, as many times as possible, giving back as needed (greedy)

at matches the characters at literally (case sensitive)
\s matches any whitespace character (equivalent to [\r\n\t\f\v ])
+ matches the previous token between one and unlimited times, as many times as possible, giving back as needed (greedy)

3rd Capturing Group (\d+\.?\d*)
\d matches a digit (equivalent to [0-9])
+ matches the previous token between one and unlimited times, as many times as possible, giving back as needed (greedy)
\. matches the character . with index 4610 (2E16 or 568) literally (case sensitive)
? matches the previous token between zero and one times, as many times as possible, giving back as needed (greedy)
\d matches a digit (equivalent to [0-9])
* matches the previous token between zero and unlimited times, as many times as possible, giving back as needed (greedy)
$ asserts position at the end of a line

Some error-handling is implemented in regex itself, like quantity must be >0 (between 1-9),
price should be > 0 (digits before decimal should be between 1-9).
*/

//InputHandler and Parser

using SalesTax.Core;
using SalesTax.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text.RegularExpressions;


namespace SalesTax.Core
{
    public class UserInputHandler
    {
        //books, food and medical products are exempt. So create a list of keywords, which might
        //relate to books, food and medical products, to compare with input.
        public static List<string> keywords = new List<string> {"book", "food", "chocolate", "pills", "medical"};
        protected static readonly Regex InputPattern = new Regex(
                @"^(\d+)\s+(.+?)\s+at\s+(\d+\.?\d*)$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static List<ItemBase> GetItemsFromUserInput()
        {
            var taxCalculator = new TaxCalculator();

            Console.WriteLine("Enter shopping items (format: quantity product_name at price). If item is imported, mentionn imported keyword before product_name");
            Console.WriteLine("  Examples:");
            Console.WriteLine("  1 book at 12.49");
            Console.WriteLine("  2 imported bottles of perfume at 27.99");
            Console.WriteLine("Enter 'done' when finished:");
            Console.WriteLine();

            var items = new List<ItemBase>();
            string? inputLine; //? operator denotes a nullable reference type

            while ((inputLine = Console.ReadLine()?.Trim().ToLower()) != "done")
            {
                try
                {
                    //continue if user enters null or whitespace as input, break only if 'done' is entered
                    if (string.IsNullOrWhiteSpace(inputLine)) continue;
                    var item = ParseInputLine(inputLine, taxCalculator);
                    items.Add(item);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing input: {ex.Message}. Please try again.");
                }
            }
            return items;
        }

        private static ItemBase ParseInputLine(string input, ITaxCalculator taxCalculator)
        {
            var match = InputPattern.Match(input.Trim());
            if (!match.Success)
                throw new FormatException($"Invalid input format: {input}");

            /*Some error-handling is implemented in regex itself, like following the order of input format. 
            Items with price 0 is allowed as it can be free (and tax-free too!). */
            
            int quantity = int.Parse(match.Groups[1].Value);
            if (quantity <= 0)
                throw new FormatException($"Quantity must be positive: {input}");
            
            string itemName = match.Groups[2].Value;
            decimal price = decimal.Parse(match.Groups[3].Value);

            bool isExempt = keywords.Any(s => itemName.Contains(s, StringComparison.OrdinalIgnoreCase));
            bool isImported = itemName.Contains("imported");
            
            ItemBase newItem = isExempt ? new ExemptItem(taxCalculator) : new NonExemptItem(taxCalculator);

            newItem.Name = itemName;
            newItem.Price = price;
            newItem.Quantity = quantity;
            newItem.IsImported = isImported;

            return newItem;
        }
    }
}