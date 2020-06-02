using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksUtilities
{
    public class Utilities
    {
        public static double GetPriceBasedOnQuantity(int quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
            {
                return price;
            }
            else if (quantity < 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }

        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
