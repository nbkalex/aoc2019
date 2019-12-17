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
    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        int intputVal = 2;
        const int size = 100;

        char[,] map = new char[size, size];

        int currentLine = 0;
        int currentColumn = 0;

        Point startPos = new Point();


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
              //intputVal = Console.Read();
              values[param1Index] = intputVal;
              break;

            case '4':
              long val = values[param1Index];

              char c = ' ';
              switch (val)
              {
                case 35: c = '#'; break;
                case 46: c = '.'; break;
                case 10:
                  currentLine++;
                  currentColumn = -1;
                  break;
                case 94:
                  {
                    startPos.X = currentColumn;
                    startPos.Y = currentLine;
                    map[currentColumn, currentLine] = '^';
                    break;
                  }
              }

              if (c != ' ')
                map[currentColumn, currentLine] = c;

              currentColumn++;
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

        int totalAllignment = 0;
        for (int i = 0; i < size; i++)
        {
          for (int j = 0; j < size; j++)
          {
            Console.Write(map[j, i]);
          }
          //Console.Write(" " + i);
          Console.WriteLine();
        }

        for (int i = 0; i < size; i++)
        {
          for (int j = 0; j < size; j++)
          {
            if (IsIntersection(map, i, j))
            {
              map[i, j] = 'O';
              totalAllignment += i * j;
              Console.SetCursorPosition(i, j);
              Console.Write('O');
            }
          }
        }

        Console.SetCursorPosition(0, 100);
        Console.WriteLine(totalAllignment);

        Point currentDir = new Point(0, -1);

        Point currentPos = startPos;
        char dirVal = ' ';

        string result = "";


        int len = 0;
        try
        {

          while (true)
          {
            Point next = GetNextNeighbour(map, currentPos, currentDir);
            Point newDir = new Point(next.X - currentPos.X, next.Y - currentPos.Y);

            dirVal = GetDirVal(currentDir, newDir);

            Console.WriteLine(result);
            currentDir = newDir;
            int count = -1;
            do
            {
              currentPos.X += currentDir.X;
              currentPos.Y += currentDir.Y;

              count++;
            } while (currentPos.X >= 0 && currentPos.Y >= 0 && (map[currentPos.X, currentPos.Y] == '#' || map[currentPos.X, currentPos.Y] == 'O'));

            currentPos.X -= currentDir.X;
            currentPos.Y -= currentDir.Y;

            result += dirVal + count.ToString() + ",";
            len++;
          }
        }
        catch
        {
          Console.WriteLine(result);
        }
      }
    }

    static char GetDirVal(Point currendDir, Point nextDir)
    {
      int horizontal = currendDir.X * nextDir.Y;
      if (horizontal != 0)
        return horizontal < 0 ? 'L' : 'R';

      int vertical = currendDir.Y * nextDir.X;
      if (vertical != 0)
        return vertical > 0 ? 'L' : 'R';

      return ' ';
    }

    static Point GetNextNeighbour(char[,] map, Point currentPos, Point currentDir)
    {
      foreach (var dir in Directions)
      {
        if (Math.Abs(dir.X) == Math.Abs(currentDir.X) || Math.Abs(dir.Y) == Math.Abs(currentDir.Y))
          continue;

        if (currentPos.X + dir.X >= 0 && currentPos.Y + dir.Y >=0 && map[currentPos.X + dir.X, currentPos.Y + dir.Y] == '#')
          return new Point(currentPos.X + dir.X, currentPos.Y + dir.Y);
      }

      throw new Exception("stop");
    }

    static readonly List<Point> Directions = new List<Point>()
    {
      new Point(-1,0),
      new Point(0,-1),
      new Point(1,0),
      new Point(0,1)
    };

    static bool IsIntersection(char[,] map, int i, int j)
    {
      if (map[i, j] != '#')
        return false;

      int count = 0;
      if (i > 0 && map[i - 1, j] == '#') count++;// left
      if (map[i + 1, j] == '#') count++;// right
      if (j > 0 && map[i, j - 1] == '#') count++; //TOP
      if (map[i, j + 1] == '#') count++; //BOT

      return count > 2;
    }
  }
}

