using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Numerics;

namespace day5
{
  class Program
  {
    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        using (StreamWriter sw = new StreamWriter("result.txt"))
        {

          List<long> valuesBase = input.Split(',').Select(i => long.Parse(i)).ToList();
          for (int i = 0; i < 10000; i++)
            valuesBase.Add(0);

          const int size = 1200;

          byte[,] matrix = new byte[size,size];

          int count1 = 0;

          for (int y = 0; y < size; y++)
          { 
            int first1Index = -1;

            for (int x = 0; x < size; x++)
            {

              List<long> values = new List<long>(valuesBase);
              long offset = 0;
              int index = 0;
              int currentRead = 0;

              while (index < values.Count)
              {
                string op = new string(values[index].ToString().Reverse().ToArray());

                char param1Mod = '0';
                char param2Mod = '0';
                char param3Mod = '0';

                if (op == "99")
                  break;

                if (op.Length > 2)
                  param1Mod = op[2];

                if (op.Length > 3)
                  param2Mod = op[3];

                if (op.Length > 4)
                  param3Mod = op[4];

                int numarParametrii = 0;

                switch (op[0])
                {
                  case '1':
                  case '2':
                    numarParametrii = 4;
                    break;
                  case '3':
                  case '4':
                  case '9':
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

                int param1Index = param1Mod == '0' ? (int)values[index + 1] : param1Mod == '2' ? (int)(offset + values[index + 1]) : index + 1;

                int param2Index = 0;
                int param3Index = 0;

                if (numarParametrii > 2)
                  param2Index = param2Mod == '0' ? (int)values[index + 2] : param2Mod == '2' ? (int)(offset + values[index + 2]) : index + 2;

                if (numarParametrii > 3)
                  param3Index = param3Mod == '0' ? (int)values[index + 3] : param3Mod == '2' ? (int)(offset + values[index + 3]) : index + 3;

                switch (op[0])
                {
                  case '1':
                    values[param3Index] = values[param1Index] + values[param2Index];
                    break;

                  case '2':
                    values[param3Index] = values[param1Index] * values[param2Index];
                    break;

                  case '3':
                    values[param1Index] = currentRead % 2 == 0 ? x : y;
                    currentRead++;
                    break;

                  case '4':

                    //Print(sw, values[param1Index]);
                    if (values[param1Index] == 1)
                    { 
                      matrix[y, x] = 1;
                      count1++;

                      if (first1Index == -1)
                        first1Index = x;
                    }

                    break;

                  case '5':
                    if (values[param1Index] != 0)
                    {
                      index = (int)values[param2Index];
                      continue;
                    }
                    break;

                  case '6':
                    if (values[param1Index] == 0)
                    {
                      index = (int)values[param2Index];
                      continue;
                    }
                    break;
                  case '7':
                    values[param3Index] = values[param1Index] < values[param2Index] ? 1 : 0;
                    break;
                  case '8':
                    values[param3Index] = values[param1Index] == values[param2Index] ? 1 : 0;
                    break;
                  case '9':
                    offset += (long)values[param1Index];
                    break;
                }

                index += numarParametrii;
              }
            }

            if(y > 100 && matrix[y - 99, first1Index + 99] == 1)
            {
              Console.WriteLine(first1Index* 10000 + (y-99));
              break;
            }

            sw.Write("\r\n");
          }

          //Console.SetCursorPosition(0, size + 1);
          //Console.WriteLine(count1);
        }
      }

      static void Print(StreamWriter sw, long val)
      {
        sw.Write(val);
      }
    }
  }
}
