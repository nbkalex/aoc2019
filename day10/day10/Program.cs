using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      string input;
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        input = sr.ReadToEnd();
      }

      string[] map = input.Split("\r\n");
      int height = map.Length;
      int width = map[0].Length;

      Dictionary<Point, HashSet<Point>> asteroidsSight = new Dictionary<Point, HashSet<Point>>();

      for (int i = 0; i < height; i++)
        for (int j = 0; j < width; j++)
          if (map[i][j] == '#')
            asteroidsSight.Add(new Point(j, i), new HashSet<Point>());

      foreach (var asteroidSight in asteroidsSight)
      {
        Point asteroid = asteroidSight.Key;
        HashSet<Point> cantBeSeen = asteroidSight.Value;

        foreach (Point nextAsteroid in asteroidsSight.Keys)
        {
          if (nextAsteroid == asteroid)
            continue;

          foreach (Point toBeSeenAsteroid in asteroidsSight.Keys)
          {
            if (toBeSeenAsteroid == asteroid || toBeSeenAsteroid == nextAsteroid)
              continue;

            if (CantBeSeenFrom(asteroid, nextAsteroid, toBeSeenAsteroid))
              cantBeSeen.Add(toBeSeenAsteroid);
          }
        }
      }

      var min = asteroidsSight.Values.Min(a => a.Count);

      var count = asteroidsSight.Count;

      var found = asteroidsSight.FirstOrDefault(ast => ast.Value.Count == min);

      int remove = count - 200;

      var visible = new HashSet<Point>(asteroidsSight.Keys.Except(found.Value));
      visible.Remove(found.Key);

      // d1: x= found.X; ax + by + c = 0 => a=found.X; b= 0; c = 0;

      var sorted = visible.OrderByDescending( v=> Math.Atan2(v.X- found.Key.X, v.Y - found.Key.Y));
      var sorted2 = visible.OrderByDescending(v => Math.Atan2(v.X, v.Y));

      int maxVisibile = asteroidsSight.Count - 1 - min;

      Console.WriteLine(maxVisibile);
    }

    static bool IsBetween(Point first, Point mid, Point second)
    {
      if(mid.X < first.X && mid.X < second.X)
        return false;

      if (mid.X > first.X && mid.X > second.X)
        return false;

      if (mid.Y < first.Y && mid.Y < second.Y)
        return false;

      if (mid.Y > first.Y && mid.Y > second.Y)
        return false;

      return true;
    }

    static bool CantBeSeenFrom(Point from, Point other, Point toBeSeen)
    {
      if(!IsBetween(from, other, toBeSeen))
        return false;

      return (from.X - toBeSeen.X) * (other.Y - toBeSeen.Y) == (from.Y - toBeSeen.Y) * (other.X - toBeSeen.X);
    }
  }
}
