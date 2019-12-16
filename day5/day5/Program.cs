using System;
using System.IO;
using System.Linq;

namespace day5
{
  class Program
  {
    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        int intputVal = 5;

        string input = sr.ReadToEnd();

        int[] values = input.Split(',').Select(i => int.Parse(i)).ToArray();

        int index = 0;

        while (index < values.Length)
        {
          string op = new string(values[index].ToString().Reverse().ToArray());

          char param1Mod = '0';
          char param2Mod = '0';

          if (op.Length > 2)
            param1Mod = op[2];

          if (op.Length > 3)
            param2Mod = op[3];

          int numarParametrii = 0;

          switch (op[0])
          {
            case '1':
            case '2':
              numarParametrii = 4;
              break;
            case '3':
            case '4':
              numarParametrii = 2;
              break;
            case '5':
            case '6':
              numarParametrii = 3;
              break;
            case '7':
            case '8':
              numarParametrii = 4;
              break;
          }

          if (numarParametrii == 0)
            break;

          int param1Index = param1Mod == '0' ? values[index + 1] : index + 1;

          int param2Index = 0;

          if (numarParametrii > 2)
            param2Index = param2Mod == '0' ? values[index + 2] : index + 2;

          switch (op[0])
          {
            case '1':
              values[values[index + 3]] = values[param1Index] + values[param2Index];
              break;

            case '2':
              values[values[index + 3]] = values[param1Index] * values[param2Index];
              break;

            case '3':
              values[values[index + 1]] = intputVal;
              break;

            case '4':
              Console.WriteLine(values[param1Index]);
              break;

            case '5':
              if (values[param1Index] != 0)
              {
                index = values[param2Index];
                continue;
              }
              break;

            case '6':
              if (values[param1Index] == 0)
              {
                index = values[param2Index];
                continue;
              }
              break;
            case '7':
              values[values[index + 3]] = values[param1Index] < values[param2Index] ? 1 : 0;
              break;
            case '8':
              values[values[index + 3]] = values[param1Index] == values[param2Index] ? 1 : 0;
              break;
          }

          index += numarParametrii;
        }
      }
    }
  }
}
