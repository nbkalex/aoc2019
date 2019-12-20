using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace day20
{
  class Program
  {
    static readonly Point Nord = new Point(0, -1);
    static readonly Point Sud = new Point(0, 1);
    static readonly Point Est = new Point(1, 0);
    static readonly Point Vest = new Point(-1, 0);

    static readonly List<Point> directions = new List<Point>() { Nord, Sud, Est, Vest };

    static Dictionary<Point, char> map = new Dictionary<Point, char>();

    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        Dictionary<string, Point> portals = new Dictionary<string, Point>();

        string input = sr.ReadToEnd();

        string[] lines = input.Split("\r\n");

        for (int i = 0; i < lines.Length; i++)
        {
          for (int j = 0; j < lines[0].Length; j++)
          {
            char c = lines[i][j];
            map.Add(new Point(j, i), c);
          }
        }

        foreach (var p in map)
        {
          char c = p.Value;
          if (char.IsLetter(c))
          {
            Point path = Point.Empty;

            string gateName = c.ToString();
            foreach (var dir in directions)
            {
              Point neightbour = new Point(p.Key.X + dir.X, p.Key.Y + dir.Y);
              if (!map.ContainsKey(neightbour))
                continue;

              if (map[neightbour] == '.')
                path = neightbour;

              if (char.IsLetter(map[neightbour]))
              {
                gateName += map[neightbour];
                if (dir.X < 0 || dir.Y < 0)
                  gateName = new string(gateName.Reverse().ToArray());
              }
            }

            if (!path.IsEmpty)
            {
              if (portals.ContainsKey(gateName))
                gateName = new string(gateName.Reverse().ToArray());

              if (gateName == "AA")
                path = p.Key;

              portals.Add(gateName, path);
            }
          }
        }

        // lee
        Dictionary<Point, int> steps = new Dictionary<Point, int>();

        Queue<Point> queue = new Queue<Point>();
        queue.Enqueue(portals["AA"]);

        steps.Add(portals["AA"], 0);

        while (queue.Any())
        {
          Point p = queue.Dequeue();

          foreach (var dir in directions)
          {
            Point neighbour = new Point(p.X + dir.X, p.Y + dir.Y);

            if (!map.ContainsKey(neighbour) || map[neighbour] == '#' || steps.ContainsKey(neighbour) || map[neighbour] == ' '
              || (char.IsLetter(map[p]) && char.IsLetter(map[neighbour])))
              continue;

            int minVal = GetMinVal(steps, neighbour);
            if (char.IsLetter(map[neighbour]) && portals.First(portal => portal.Value == p).Key != "AA")
              neighbour = portals[new string(portals.First(portal => portal.Value == p).Key.Reverse().ToArray())];

            if (!steps.ContainsKey(neighbour))
            {
              queue.Enqueue(neighbour);
              steps.Add(neighbour, minVal + 1);
            }
          }
        }

        Console.WriteLine(steps[portals["ZZ"]]-1);
      }

      static int GetMinVal(Dictionary<Point, int> steps, Point p)
      {
        int min = int.MaxValue;
        foreach (var dir in directions)
        {
          Point neighbour = new Point(p.X + dir.X, p.Y + dir.Y);

          if (!steps.ContainsKey(neighbour))
            continue;

          if (steps[neighbour] < min)
            min = steps[neighbour];
        }

        return min;
      }
    }
  }
}

