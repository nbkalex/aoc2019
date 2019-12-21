using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace day20
{
  class TraceVal
  {
    public int steps;
    Point Position;
    public List<Tuple<string, int, int>> portals = new List<Tuple<string, int, int>>();
  }

  class Program
  {
    static readonly Point Nord = new Point(0, -1);
    static readonly Point Sud = new Point(0, 1);
    static readonly Point Est = new Point(1, 0);
    static readonly Point Vest = new Point(-1, 0);

    static readonly List<Point> directions = new List<Point>() { Nord, Sud, Est, Vest };

    static Dictionary<Point, char> map = new Dictionary<Point, char>();

    static bool IsOuterPortal(Point p, int width, int height)
    {
      return p.X < 2 || p.X > width - 3 ||
             p.Y < 2 || p.Y > height - 3;
    }

    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        Dictionary<string, Point> outerPortals = new Dictionary<string, Point>();
        Dictionary<string, Point> innerPortals = new Dictionary<string, Point>();

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

            Dictionary<string, Point> portals = IsOuterPortal(p.Key, lines[0].Length, lines.Length)
                                                ? outerPortals : innerPortals;
            if (!path.IsEmpty)
            {

              if (gateName == "AA")
                path = p.Key;

              portals.Add(gateName, path);
            }
          }
        }

        Dictionary<int, Dictionary<Point, TraceVal>> levels = new Dictionary<int, Dictionary<Point, TraceVal>>();

        // lee
        levels.Add(0, new Dictionary<Point, TraceVal>());

        Queue<Tuple<Point, int>> queue = new Queue<Tuple<Point, int>>();
        queue.Enqueue(new Tuple<Point, int>(outerPortals["AA"], 0));

        levels[0].Add(outerPortals["AA"], new TraceVal() { steps = 0 });

        bool found = false;
        while (queue.Any() && !found)
        {
          Tuple<Point, int> p = queue.Dequeue();

          foreach (var dir in directions)
          {
            Point neighbour = new Point(p.Item1.X + dir.X, p.Item1.Y + dir.Y);

            if (neighbour == outerPortals["AA"])
              continue;

            if (!map.ContainsKey(neighbour) || map[neighbour] == '#' || levels[p.Item2].ContainsKey(neighbour) || map[neighbour] == ' '
              || (char.IsLetter(map[p.Item1]) && char.IsLetter(map[neighbour])))
              continue;

            int level = p.Item2;
            var minVal = GetMinVal(levels[level], neighbour);
            var minValTrace = new List<Tuple<string, int, int>>(minVal.portals);

            if (char.IsLetter(map[neighbour]) && char.IsDigit(map[p.Item1]))
              continue;

            Tuple<string, int, int>? portal = null;

            if (char.IsLetter(map[neighbour]))
            {

              bool isOuterPortal = IsOuterPortal(neighbour, lines[0].Length, lines.Length);

              Dictionary<string, Point> portals = isOuterPortal ? outerPortals : innerPortals;
              string portalName = portals.First(portal => portal.Value == p.Item1).Key;

              if (portalName == "ZZ")
              {
                if (level == 0)
                {
                  found = true;
                  Console.SetCursorPosition(0, 100);
                  for(int i = 0; i < minVal.portals.Count; i ++)
                  {
                    int stepsval = i == 0 ? minVal.portals[i].Item2 -1 : minVal.portals[i].Item2 - minVal.portals[i-1].Item2;
                    Console.WriteLine("level " + minVal.portals[i].Item3 + " - " +  minVal.portals[i].Item1 + " - " + stepsval);
                  }

                  Console.WriteLine();
                  Console.WriteLine(minVal.steps);

                  break;
                }

                continue;
              }

              level = isOuterPortal ? level - 1 : level + 1;

              minValTrace.Add(new Tuple<string, int, int>(portalName, minVal.steps, level));

              Dictionary<string, Point> otherPortals = !isOuterPortal ? outerPortals : innerPortals;
              neighbour = otherPortals[portalName];

              if (!levels.ContainsKey(level))
                levels.Add(level, new Dictionary<Point, TraceVal>());
            }

            if (level == 0 || level == 1)
              PrintMap(levels[level], level);

            if (!levels[level].ContainsKey(neighbour))
            {
              queue.Enqueue(new Tuple<Point, int>(neighbour, level));
              TraceVal val = new TraceVal() { steps = minVal.steps + 1, portals = minValTrace };

              levels[level].Add(neighbour, val);
              //PrintMap(levels[level], level);
            }
          }
        }

        //Console.WriteLine(steps[portals["ZZ"]] - 1);
      }

      static void PrintMap(Dictionary<Point, TraceVal> paths, int level)
      {
        //foreach (var p in map)
        //{
        //  Console.SetCursorPosition(p.Key.X, p.Key.Y);
        //  Console.Write(p.Value);
        //}


        //foreach (var p in paths)
        //{
        //  Console.SetCursorPosition(p.Key.X, p.Key.Y);
        //  Console.Write('@');
        //}

        //Console.SetCursorPosition(50, 2);
        //Console.Write("current level: " + level + "   ");
      }

      static TraceVal GetMinVal(Dictionary<Point, TraceVal> steps, Point p)
      {
        int min = int.MaxValue;
        TraceVal found = null;
        foreach (var dir in directions)
        {
          Point neighbour = new Point(p.X + dir.X, p.Y + dir.Y);

          if (!steps.ContainsKey(neighbour))
            continue;

          if (steps[neighbour].steps < min)
          {
            min = steps[neighbour].steps;
            found = steps[neighbour];
          }
        }

        return found;
      }
    }
  }
}

