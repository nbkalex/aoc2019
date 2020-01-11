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

      //PrintMap(map);

      Dictionary<char, int> result = new Dictionary<char, int>();

      var foundkeys = Lee(map, start);

      Dictionary<char, Dictionary<char, ScannedPoint>> stepsBetweenKeys = new Dictionary<char, Dictionary<char, ScannedPoint>>();
      stepsBetweenKeys.Add('?', foundkeys);

      Dictionary<char, Dictionary<string, int>> minRoads = new Dictionary<char, Dictionary<string, int>>();
      foreach (var foundKey in foundkeys)
      {
        stepsBetweenKeys.Add(foundKey.Key, Lee(map, foundKey.Value.Point));
        minRoads.Add(foundKey.Key, new Dictionary<string, int>());
      }

      Dictionary<string, int> roadSteps = new Dictionary<string, int>();

      Queue<Tuple<string, int>> nextKeys = new Queue<Tuple<string, int>>();
      nextKeys.Enqueue(new Tuple<string, int>("?", 0));

      int min = int.MaxValue;

      while (nextKeys.Any())
      {
        var current = nextKeys.Dequeue();

        foreach (var key in stepsBetweenKeys.Keys.Except(current.Item1))
        {
          var nextKey = stepsBetweenKeys[current.Item1.Last()][key];
          if (nextKey.BlockingDoors.Except(current.Item1).Any() || current.Item2 + nextKey.steps > min)
            continue;


          string newRoad = current.Item1 + key;
          string hashRoad = string.Concat(newRoad.OrderBy(c => c)) + key;
          int len = current.Item2 + stepsBetweenKeys[current.Item1.Last()][key].steps;

          if (newRoad.Length == stepsBetweenKeys.Count)
          {
            if (min > len)
            {
              min = len;
              Console.WriteLine(min);
            }
          }

          if (minRoads[key].ContainsKey(hashRoad))
          {
            if (minRoads[key][hashRoad] > len)
              minRoads[key][hashRoad] = current.Item2 + nextKey.steps; 
            else
              continue;
          }
          else
            minRoads[key].Add(hashRoad, len);

          var next = new Tuple<string, int>(current.Item1 + key, current.Item2 + nextKey.steps);
          nextKeys.Enqueue(next);
        }
      }

      Console.WriteLine(min);
    }


    static void PrintMap(Dictionary<Point, char> map)
    {
      foreach (var p in map)
      {
        Console.SetCursorPosition(p.Key.Y, p.Key.X);
        Console.Write(p.Value);
      }
    }

    class ScannedPoint
    {
      public int steps = 0;
      public Point Point = new Point();
      public HashSet<char> BlockingDoors = new HashSet<char>();
    }

    // returns keys
    static Dictionary<char, ScannedPoint> Lee(Dictionary<Point, char> map, Point start)
    {
      Dictionary<char, ScannedPoint> foundKeys = new Dictionary<char, ScannedPoint>();

      // lee      
      Queue<Point> queue = new Queue<Point>();
      queue.Enqueue(start);

      Dictionary<Point, ScannedPoint> paths = new Dictionary<Point, ScannedPoint>();
      paths.Add(start, new ScannedPoint() { steps = 0, Point = start });

      while (queue.Any())
      {
        Point p = queue.Dequeue();

        foreach (var dir in directions)
        {
          Point neighbour = new Point(p.X + dir.X, p.Y + dir.Y);

          if (paths.ContainsKey(neighbour))
            continue;

          if (!map.ContainsKey(neighbour) || map[neighbour] == '#')
            continue;

          var minPoint = GetMinStepsPoint(paths, neighbour);
          int minSteps = minPoint.steps + 1;
          HashSet<char> doors = new HashSet<char>(minPoint.BlockingDoors);

          if (IsDoor(map[neighbour]))
            doors.Add(char.ToLower(map[neighbour]));

          if (paths.ContainsKey(neighbour))
            paths[neighbour].steps = minSteps;
          else
            paths.Add(neighbour, new ScannedPoint() { steps = minSteps, Point = neighbour, BlockingDoors = doors });

          queue.Enqueue(neighbour);

          if (IsKey(map[neighbour]))
            foundKeys.Add(map[neighbour], paths[neighbour]);
        }
      }

      return foundKeys;
    }

    static ScannedPoint GetMinStepsPoint(Dictionary<Point, ScannedPoint> map, Point current)
    {
      int min = int.MaxValue;
      ScannedPoint minPoint = null;

      foreach (var dir in directions)
      {
        Point neightbour = new Point(current.X + dir.X, current.Y + dir.Y);
        if (map.ContainsKey(neightbour) && map[neightbour].steps < min)
        {
          min = map[neightbour].steps;
          minPoint = map[neightbour];
        }
      }

      return minPoint;
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

    static bool IsDoor(char c)
    {
      return c >= 'A' && c <= 'Z';
    }

    static bool IsKey(char c)
    {
      return c >= 'a' && c <= 'z';
    }
  }
}
