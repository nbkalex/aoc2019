using System;
using System.Collections.Generic;
using System.IO;

namespace day16
{
  class Program
  {
    static List<int> basepattern = new List<int>() { 0, 1, 0, -1 };

    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        string baseInput = (string)input.Clone();

        for (int faze = 0; faze < 100; faze++)
        {
          int index = 0;
          string current = string.Empty;
          for (int i = 0; i < input.Length; i++)
          {
            int number = Math.Abs((GetValue(input, index) % 10));
            current += number.ToString()[0];
            index++;
          }

          input = current;
        }


        Console.WriteLine(input);

        Console.WriteLine(baseInput.Substring((int)(baseInput.Length/4 *3)));

        Console.WriteLine(input.Substring(0, 8));
      }

      static int GetValue(string input, int index)
      {
        var currentPattern = GetParrent(index);
        int result = 0;


        for (int i = 0; i < input.Length; i++)
          result += currentPattern[(i + 1) % currentPattern.Count] * int.Parse(input[i].ToString());

        return result;
      }

      static List<int> GetParrent(int index)
      {
        List<int> pattern = new List<int>();
        foreach (int val in basepattern)
          for (int i = 0; i <= index; i++)
            pattern.Add(val);

        return pattern;
      }
    }
  }
}
