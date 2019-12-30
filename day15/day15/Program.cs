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
    static readonly Point Nord = new Point(0, -1);
    static readonly Point Sud = new Point(0, 1);
    static readonly Point Est = new Point(1, 0);
    static readonly Point Vest = new Point(-1, 0);

    static readonly List<Point> directions = new List<Point>() { Nord, Sud, Est, Vest };

    static void PrintPoint(Point p, char c)
    {
      Console.SetCursorPosition(p.X, p.Y);
      Console.Write(c);
    }

    static int GetMinVal(Dictionary<Point, int> minutes, Point p)
    {
      int min = int.MaxValue;

      foreach (var dir in directions)
      {
        Point neightbour = new Point(p.X + dir.X, p.Y + dir.Y);
        if (minutes.ContainsKey(neightbour))
          if (min > minutes[neightbour])
            min = minutes[neightbour];
      }

      if(min == int.MaxValue)
      {

      }

      return min;
    }

    static void Main(string[] args)
    {
      var map = new Dictionary<Point, char>();

      //using (StreamReader sr = new StreamReader("map.txt"))
      //{
      //  string[] lines = sr.ReadToEnd().Split("\r\n");

      //  foreach (var line in lines)
      //  {
      //    string[] values = line.Split(',');
      //    map.Add(new Point(int.Parse(values[0]), int.Parse(values[1])), values[2] == "" ? ' ' : values[2][0]);
      //  }
      //}


      const int size = 50;
      Point currentPosition = new Point(size / 2, size / 2);
      Point currentDirection = Nord;

      //for (int i = 0; i < size; i++)
      //{
      //  PrintPoint(new Point(0, i), '-');
      //  PrintPoint(new Point(size - 1, i), '|');
      //  PrintPoint(new Point(i, 0), '|');
      //  PrintPoint(new Point(i, size - 1), '|');
      //}

      //foreach (var point in map)
      //  PrintPoint(point.Key, point.Value);

      //Point start = map.First(p => p.Value == 'o').Key;
      //Dictionary<Point, int> minutes = new Dictionary<Point, int>();
      //foreach (var p in map)
      //  minutes.Add(p.Key, int.MaxValue);

      //minutes[start] = 0;


      //// lee
      //while (minutes.Any(m => m.Value == int.MaxValue && map[m.Key] == ' '))
      //{
      //  int radius = 1;

      //  while (radius < size)
      //  {
      //    List<Point> edges = new List<Point>(0);
      //    for (int i = 0; i <= radius * 2 +1; i++)
      //    {
      //      edges.Add(new Point(start.X - radius, start.Y - radius + i)); // left
      //      edges.Add(new Point(start.X + radius, start.Y - radius + i)); // right
      //      edges.Add(new Point(start.X - radius + i, start.Y - radius)); // top
      //      edges.Add(new Point(start.X - radius + i, start.Y + radius)); // bot
      //    }

      //    foreach (var edge in edges)
      //    {
      //      if (map.ContainsKey(edge) && map[edge] == ' ')
      //      {
      //        int minedge = GetMinVal(minutes, edge);
      //        if (minedge != int.MaxValue)
      //          minutes[edge] = minedge+1;
      //      }
      //    }

      //    radius++;
      //  }

      //  foreach (var point in map)
      //  {
      //    if (minutes[point.Key] != int.MaxValue)
      //      PrintPoint(point.Key, 'O');
      //    else
      //      PrintPoint(point.Key, point.Value);
      //  }
      //}


      //return;

      // Part 1

      Console.SetCursorPosition(currentPosition.X, currentPosition.Y);

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();

        List<long> values = input.Split(',').Select(i => long.Parse(i)).ToList();
        for (int i = 0; i < 10000; i++)
          values.Add(0);

        int index = 0;

        int offset = 0;
        int count = 0;

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


              //Console.SetCursorPosition(0, 60);
              //foreach (var point in map)
              //  Console.WriteLine($"{point.Key.X},{ point.Key.Y},{point.Value}");


              var key = Console.ReadKey();
              if (map.ContainsKey(currentPosition))
                PrintPoint(currentPosition, map[currentPosition]);

              switch (key.Key)
              {
                case ConsoleKey.RightArrow: currentDirection = Est; values[param1Index] = 4; break;
                case ConsoleKey.UpArrow: currentDirection = Nord; values[param1Index] = 1; break;
                case ConsoleKey.DownArrow: currentDirection = Sud; values[param1Index] = 2; break;
                case ConsoleKey.LeftArrow: currentDirection = Vest; values[param1Index] = 3; break;
              }

              Console.SetCursorPosition(currentPosition.X, currentPosition.Y);

              break;

            case '4':
              int value = (int)values[param1Index];
              Point nextPos = new Point(currentPosition.X + currentDirection.X, currentPosition.Y + currentDirection.Y);
              char c = ' '; //empty
              if (value == 0)
                c = '#'; // wall
              else
              {
                currentPosition.X = nextPos.X;
                currentPosition.Y = nextPos.Y;

                count++;
              }

              if (value == 2)
                c = 'o'; // oxigen

              if (!map.ContainsKey(nextPos))
              {
                map.Add(nextPos, c);
                PrintPoint(nextPos, c);
              }

              Console.SetCursorPosition(currentPosition.X, currentPosition.Y);
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
      }
    }
  }
}
