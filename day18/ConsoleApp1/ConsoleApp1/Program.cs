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
      Tuple<char, int>[,] map;

      int width = 0;
      int height = 0;
      Point start = new Point();

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        string[] lines = input.Split("\r\n");
        height = lines.Length;
        width = lines[0].Length;

        map = new Tuple<char, int>[height, width];


        for (int i = 0; i < height; i++)
          for (int j = 0; j < width; j++)
          {
            if (lines[i][j] == '@')
              start = new Point(j, i);

            if (IsKey(lines[i][j]))
              totalKeys.Add(lines[i][j]);

            map[i, j] = new Tuple<char, int>(lines[i][j], int.MaxValue);
          }
      }

      PrintMap(map, width, height);

      Go(map, width, height, new List<Tuple<char, Tuple<Point, int>>>(), start, 0);
      Console.WriteLine(min);
    }

    static int min = int.MaxValue;

    static void Go(Tuple<char, int>[,] map, int width, int height, List<Tuple<char, Tuple<Point, int>>> keys, Point start, int total)
    {
      if (total >= min)
        return;

      Dictionary<char, int> result = new Dictionary<char, int>();

      var foundkeys = Lee(map, width, height, start, keys);

      if (keys.Count == totalKeys.Count - 1)
      {
        total += foundkeys.ElementAt(0).Item2.Item2;
        if (min > total)
          min = total;
        //Console.WriteLine(total);
        return;
      }

      foreach (var key in foundkeys)
      {
        List<Tuple<char, Tuple<Point, int>>> nextkeys = new List<Tuple<char, Tuple<Point, int>>>(keys);
        nextkeys.Add(new Tuple<char, Tuple<Point, int>>(key.Item1, new Tuple<Point, int>(key.Item2.Item1, key.Item2.Item2)));

        Go(map, width, height, nextkeys, key.Item2.Item1, total + key.Item2.Item2);
      }
    }

    static void PrintMap(Tuple<char, int>[,] map, int width, int height)
    {
      for (int i = 0; i < height; i++)
      {
        for (int j = 0; j < width; j++)
          Console.Write(map[i, j].Item1);
        Console.WriteLine();
      }
    }

    // returns keys
    static List<Tuple<char, Tuple<Point, int>>> Lee(Tuple<char, int>[,] map, int width, int height, Point start, List<Tuple<char, Tuple<Point, int>>> keys)
    {
      // lee
      map = (Tuple<char, int>[,])map.Clone();
      map[start.Y, start.X] = new Tuple<char, int>(map[start.Y, start.X].Item1, 0);

      List<Tuple<char, Tuple<Point, int>>> keysFound = new List<Tuple<char, Tuple<Point, int>>>();

      int discovered = 1;
      while (discovered != 0)
      {
        discovered = 0;

        int radius = 1;

        while (radius < Math.Max(width, height))
        {
          List<Point> edges = new List<Point>(0);
          for (int i = 0; i <= radius * 2 + 1; i++)
          {
            edges.Add(new Point(start.X - radius, start.Y - radius + i)); // left
            edges.Add(new Point(start.X + radius, start.Y - radius + i)); // right
            edges.Add(new Point(start.X - radius + i, start.Y - radius)); // top
            edges.Add(new Point(start.X - radius + i, start.Y + radius)); // bot
          }

          foreach (var edge in edges)
          {
            if (edge.X < 0 || edge.Y < 0 || edge.X >= width || edge.Y >= height || map[edge.Y, edge.X].Item1 == '#' || IsDoorLocked(map[edge.Y, edge.X].Item1, keys.Select(k => k.Item1).ToList()))
              continue;

            int minedge = GetMinVal(map, edge);
            if (minedge != int.MaxValue)
            {
              if (map[edge.Y, edge.X].Item2 != minedge + 1)
              {
                if (IsKey(map[edge.Y, edge.X].Item1) && !keysFound.Any( k => k.Item1 == map[edge.Y, edge.X].Item1) && !keys.Any(k => k.Item1 == map[edge.Y, edge.X].Item1))
                  keysFound.Add(new Tuple<char, Tuple<Point, int>>(map[edge.Y, edge.X].Item1, new Tuple<Point, int>(new Point(edge.X, edge.Y), minedge + 1)));

                map[edge.Y, edge.X] = new Tuple<char, int>(map[edge.Y, edge.X].Item1, minedge + 1);
                discovered++;
              }
            }
          }

          radius++;
        }
      }

      return keysFound;
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
