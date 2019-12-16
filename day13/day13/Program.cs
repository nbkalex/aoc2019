using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Numerics;

namespace day5
{
  class Program
  {
    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        int intputVal = 0;
        long score = 0;
        List<long> outputs = new List<long>();
        Dictionary<Point, long> map = new Dictionary<Point, long>();

        long offset = 0;

        string input = sr.ReadToEnd();

        List<long> values = input.Split(',').Select(i => long.Parse(i)).ToList();
        for (int i = 0; i < 10000; i++)
          values.Add(0);

        int index = 0;

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
              values[param1Index] = intputVal;
              break;

            case '4':
              outputs.Add(values[param1Index]);

              if (outputs.Count % 3 == 0)
              {
                long x = outputs[outputs.Count - 3];
                long y = outputs[outputs.Count - 2];
                long id = outputs[outputs.Count - 1];

                if (x == -1 && y == 0)
                {
                  score = id;
                }
                else
                {
                  Point p = new Point((int)x, (int)y);

                  if (map.ContainsKey(p))
                  { 
                    map[p] = id;
                    Print(map);
                  }
                  else
                    map.Add(p, id);

                  if(id == 3)
                    boad = p;

                  if(id ==4)
                  {
                    if (boad.X < x)
                      intputVal = 1;
                    else if (boad.X > x)
                      intputVal = -1;
                    else
                      intputVal = 0;
                  }
                }
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

        Console.WriteLine(score);
      }
    }

    static void Print(Dictionary<Point, long> map)
    {
      foreach(var pv in map)
      { 
        Point p = pv.Key;
        Console.SetCursorPosition(p.X, p.Y);

        if (map[p] == 0)
          Console.Write(" ");
        else if (map[p] == 3)
          Console.Write("_");
        else if (map[p] == 4)
          Console.Write("o");
        else if (map[p] == 2)
          Console.Write('O');
        else if (map[p] == 1)
          Console.Write("#");
        else
          Console.Write(map[p]);
      }
    }
  }
}
