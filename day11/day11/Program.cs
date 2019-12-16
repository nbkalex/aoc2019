using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Numerics;

namespace day5
{
  class Program
  {
    static readonly List<Tuple<int, int>> directions = new List<Tuple<int,int>>(){ {-1,0 }, {0,-1 }, {1,0 }, {0,1 } };
    static readonly char black = '\0';
    static readonly char white = '#';

    static void GetNextPos(Point currentPosition, int currentDirectionIndex)
    {
      int x = currentPosition.X;
      int y = currentPosition.Y;
      switch(directions[currentDirectionIndex %4])
      {
        case '<': 
          x--;
          break;
        case '>':
          x++;
          break;
        case '^':
          y--;
          break;
        case 'v':
          y++;
          break;
      }

      return new Point(x,y);
    }

    static int GetInput(char c)
    {
      return c == black ? 0 : 1;
    }

    static void Main(string[] args)
    {
      const int size = 100;
      char[,] map = new char[size, size];
      int currentDirectionIndex = 4* 100 + 1;

      Point currentPosition = new Point(size/2, size/2);
      var painted = new HashSet<Point>();

      int currentColor = GetInput(map[currentPosition.X, currentPosition.Y]);
      map[currentPosition.X, currentPosition.Y] ='#';

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();

        List<long> values = input.Split(',').Select(i => long.Parse(i)).ToList();
        for(int i = 0; i < 10000; i++)
          values.Add(0);

        int index = 0;

        int offset = 0;

        List<int> cicle = new List<int>();

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
              values[param1Index] = GetInput(map[currentPosition.X, currentPosition.Y]);
              break;

            case '4':
              cicle.Add((int)values[param1Index]);
              if (cicle.Count == 1)
              { 
                map[currentPosition.X, currentPosition.Y] = values[param1Index] ==0 ? black : white;
                painted.Add(currentPosition);
              }

              if (cicle.Count == 2)
              {
                int dir = values[param1Index] == 0 ? -1 : 1;
                currentDirectionIndex +=dir;
                currentPosition = GetNextPos(currentPosition, currentDirectionIndex);
                cicle.Clear();
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
              offset += (int)values[param1Index];
              break;
          }

          index += numarParametrii;
        }

        for(int i = 0; i < size; i++)
        {
          for (int j = 0; j < size; j++)
            Console.Write(map[j,i]);

          Console.WriteLine();
        }

        Console.WriteLine(painted.Count());
      }
    }
  }
}
