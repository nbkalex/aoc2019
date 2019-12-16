using System;
using System.Collections.Generic;

namespace day4
{
  class Program
  {
    static void Main(string[] args)
    {
      int first = 264793, last = 803935, count = 0;


      for(int i = first; i<= last; i++)
      {
        string s = i.ToString();

        bool ordered = true;

        List<Tuple<char,int>> repeatDigit = new List<Tuple<char, int>>();

        int currentSequenceSize = 0;
        char currentChar = ' ';
        for(int charindex = 0; charindex <= s.Length; charindex++)
        {
          if(charindex == s.Length || currentChar != s[charindex])
          {
            repeatDigit.Add(new Tuple<char, int>(currentChar, currentSequenceSize));
            currentSequenceSize = 0;

            if(charindex == s.Length)
              break;
          }

          currentSequenceSize++;
          currentChar = s[charindex];

          if(charindex < s.Length-1 && s[charindex] > s[charindex+1])
            ordered = false;
        }

        bool found_adiacent = false;
        foreach(var kvp in repeatDigit)
        {
          if(kvp.Item2 == 2)
            found_adiacent = true;
        }

        if(!found_adiacent || !ordered)
          continue;

        count++;
      }

      Console.WriteLine(count);
    }
  }
}
