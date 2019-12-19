using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
  class Program
  {
    static HashSet<char> totalKeys = new HashSet<char>();

    static void Main(string[] args)
    {

      Dictionary<Point, char> map = new Dictionary<Point, char>();
      Point start = new Point();

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        string[] lines = input.Split("\r\n");
        int height = lines.Length;
        int width = lines[0].Length;


        for (int i = 0; i < height; i++)
          for (int j = 0; j < width; j++)
          {
            Point p = new Point(i, j);

            if (lines[i][j] == '@')
              start = p;

            map.Add(p, lines[i][j]);

            if (IsKey(lines[i][j]))
              totalKeys.Add(lines[i][j]);
          }
      }

      PrintMap(map);

      //var lee = Lee(map, start, new List<char>());
      Go(map, new List<char>(), start, 0);
      Console.WriteLine(min);
    }

    static int min = int.MaxValue;

    static void Go(Dictionary<Point, char> map, List<char> keys, Point start, int total)
    {
      if (total >= min)
        return;

      Dictionary<char, int> result = new Dictionary<char, int>();

      var foundkeys = Lee(map, start, keys);

      if (keys.Count == totalKeys.Count - 1)
      {
        total += foundkeys[0].Item3;
        if (min > total)
          min = total;
        //Console.WriteLine(total);
        return;
      }

      foreach (var key in foundkeys)
      {
        List<char> nextKeys = new List<char>(keys);
        nextKeys.Add(key.Item1);

        Go(map, nextKeys, key.Item2, total + key.Item3);
      }
    }

    static void PrintMap(Dictionary<Point, char> map)
    {
      foreach (var p in map)
      {
        Console.SetCursorPosition(p.Key.Y, p.Key.X);
        Console.Write(p.Value);
      }
    }

    // returns keys
    static List<Tuple<char, Point, int>> Lee(Dictionary<Point, char> map, Point start, List<char> keys)
    {
      List<Tuple<char, Point, int>> foundKeys = new List<Tuple<char, Point, int>>();

      // lee      
      Queue<Point> queue = new Queue<Point>();
      queue.Enqueue(start);

      Dictionary<Point, int> paths = new Dictionary<Point, int>();
      paths.Add(start, 0);

      while (queue.Any())
      {
        Point p = queue.Dequeue();

        foreach (var dir in directions)
        {
          Point neighbour = new Point(p.X + dir.X, p.Y + dir.Y);

          if(paths.ContainsKey(neighbour))
            continue;
          
          if (!map.ContainsKey(neighbour) || map[neighbour] == '#' || IsDoorLocked(map[neighbour], keys))
            continue;

          if (paths.ContainsKey(neighbour))
            paths[neighbour] = GetMin(paths, neighbour) + 1;
          else
            paths.Add(neighbour, GetMin(paths, neighbour) + 1);

          queue.Enqueue(neighbour);

          if (IsKey(map[neighbour]) && !keys.Contains(map[neighbour]))
            foundKeys.Add(new Tuple<char, Point, int>(map[neighbour], neighbour, paths[neighbour]));
        }
      }

      return foundKeys;
    }

    static int GetMin(Dictionary<Point, int> map, Point current)
    {
      int min = int.MaxValue;
      foreach (var dir in directions)
      {
        Point neightbour = new Point(current.X + dir.X, current.Y + dir.Y);
        if (map.ContainsKey(neightbour) && map[neightbour] < min)
          min = map[neightbour];
      }

      return min;
    }


    static readonly Point Nord = new Point(0, -1);
    static readonly Point Sud = new Point(0, 1);
    static readonly Point Est = new Point(1, 0);
    static readonly Point Vest = new Point(-1, 0);

    static readonly List<Point> directions = new List<Point>() { Nord, Sud, Est, Vest };
    static int GetMinVal(Tuple<char, int>[,] map, Point p)
    {
      int min = int.MaxValue;
      foreach (var dir in directions)
      {
        Point neightbour = new Point(p.X + dir.X, p.Y + dir.Y);
        if (map[neightbour.Y, neightbour.X].Item2 < min)
          min = map[neightbour.Y, neightbour.X].Item2;
      }

      return min;
    }

    static bool IsDoorLocked(char c, List<char> keys)
    {
      return c >= 'A' && c <= 'Z' && !keys.Contains(char.ToLower(c));
    }

    static bool IsKey(char c)
    {
      return c >= 'a' && c <= 'z';
    }
  }
}
