using System;
using System.Collections.Generic;
using System.Linq;

public class LambdaExample
{
    public static void Main(string[] args)
    {
        List<string> words = new List<string> { "apple", "banana", "cat", "dog", "elephant" };

        Func<string, string, int> compareStrings;

        compareStrings = (x, y) => string.Compare(x,y);

        words.Sort((x, y) => compareStrings(x, y));
        Console.WriteLine(string.Join(", ", words));  // Ãâ·Â: apple, banana, cat, dog, elephant
    }
}
