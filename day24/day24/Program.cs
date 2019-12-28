using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace day24
{
  class Program
  {
    static readonly List<Point> directions = new List<Point>()
    {
      new Point(0,1),
      new Point(0,-1),
      new Point(1,0),
      new Point(-1,0),
    };

    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        string[] lines = input.Split("\r\n");

        char[,] map = new char[5, 5];
        for (int i = 0; i < 5; i++)
          for (int j = 0; j < 5; j++)
            map[i, j] = lines[i][j];

        //Console.WriteLine(Part1(map));
        Console.WriteLine(Part2(map));
      }
    }

    static long Part1(char[,] map)
    {
      HashSet<long> set = new HashSet<long>();

      for (; ; )
      {
        Print(map, 0);

        char[,] next = (char[,])map.Clone();

        long count = 0;
        for (int i = 0; i < 5; i++)
          for (int j = 0; j < 5; j++)
            if (map[i, j] == '#')
              count += (long)Math.Pow(2, i * 5 + j);

        if (!set.Add(count))
          return count;

        for (int i = 0; i < 5; i++)
          for (int j = 0; j < 5; j++)
          {
            char location = map[i, j];

            Dictionary<char, int> neighbours = new Dictionary<char, int>();
            neighbours.Add('.', 0);
            neighbours.Add('#', 0);
            foreach (var dir in directions)
            {
              Point neighbour = new Point(j + dir.X, i + dir.Y);
              if (neighbour.Y < 0 || neighbour.Y == 5 || neighbour.X < 0 || neighbour.X == 5)
                continue;

              neighbours[map[neighbour.Y, neighbour.X]]++;
            }

            if (location == '#')
              next[i, j] = neighbours['#'] == 1 ? '#' : '.';
            if (location == '.')
              next[i, j] = neighbours['#'] == 1 || neighbours['#'] == 2 ? '#' : '.';
          }

        map = next;
      }
    }

    static void Print(char[,] map, int lvl)
    {
      Console.WriteLine(lvl);

      for (int i = 0; i < 5; i++)
      {
        for (int j = 0; j < 5; j++)
          Console.Write(map[i, j]);

        Console.WriteLine();
      }


      Console.WriteLine();
      Console.WriteLine();
    }

    static char[,] NewMap()
    {
      char[,] result = new char[5, 5];

      for (int i = 0; i < 5; i++)
        for (int j = 0; j < 5; j++)
          result[i, j] = '.';

      return result;
    }

    static List<Tuple<Point, int>> GetNeighbours(Point p, int lvl)
    {
      List<Tuple<Point, int>> result = new List<Tuple<Point, int>>();
      foreach (var dir in directions)
      {
        Point neighbour = new Point(p.X + dir.X, p.Y + dir.Y);

        if (neighbour.Y == -1 || neighbour.Y == 5 || neighbour.X == -1 || neighbour.X == 5) //outer
          result.Add(new Tuple<Point, int>(new Point(2 + dir.X, 2 + dir.Y), lvl + 1));
        else if (neighbour.X == 2 && neighbour.Y == 2)                                     // inner
        {
          for (int i = 0; i < 5; i++)
          {
            int iEdge = dir.X == 1 ? 0 : dir.X == -1 ? 4 : i;
            int jEdge = dir.Y == 1 ? 0 : dir.Y == -1 ? 4 : i;

            result.Add(new Tuple<Point, int>(new Point(iEdge, jEdge), lvl - 1));
          }
        }
        else
          result.Add(new Tuple<Point, int>(neighbour, lvl));
      }


      return result;
    }

    static bool IsEmpty(char[,] map)
    {
      return map.Cast<char>().All(c => c == '.' || c == '?');
    }

    static int Part2(char[,] startMap)
    {
      int count = 0;

      Dictionary<int, char[,]> maps = new Dictionary<int, char[,]>();
      maps.Add(0, startMap);

      const int minutes = 200;

      startMap[2,2] = '?';

      for (int i = 1; i <= minutes; i++)
      {
        maps.Add(i, NewMap());
        maps.Add(i * -1, NewMap());
      }

      Print(startMap, 0);

      for (int min = 0; min < minutes; min++)
      {
        Dictionary<int, char[,]> nextMaps = new Dictionary<int, char[,]>();

        foreach (var kvp in maps)
        {
          char[,] map = kvp.Value;
          int lvl = kvp.Key;

          char[,] nextMap = (char[,])map.Clone();
          nextMaps.Add(lvl, nextMap);

          if (IsEmpty(nextMap))
          {
            int innerLvl = lvl - 1;
            int outerLvl = lvl + 1;
            foreach (var dir in directions)
            {
              if (maps.ContainsKey(innerLvl))
              {
                int edgeCount = 0;
                for (int i = 0; i < 5; i++)
                {
                  int iEdge = dir.X == -1 ? 0 : dir.X == 1 ? 4 : i;
                  int jEdge = dir.Y == -1 ? 0 : dir.Y == 1 ? 4 : i;

                  if (maps[innerLvl][iEdge, jEdge] == '#')
                    edgeCount++;
                }

                if (edgeCount == 1 || edgeCount == 2)
                  nextMap[2 + dir.X, 2 + dir.Y] = '#';
              }

              if (maps.ContainsKey(outerLvl))
              {
                if (maps[outerLvl][2 + dir.X, 2 + dir.Y] == '#')
                {
                  for (int i = 0; i <= 4; i++)
                  {
                    int iEdge = dir.X == -1 ? 0 : dir.X == 1 ? 4 : i;
                    int jEdge = dir.Y == -1 ? 0 : dir.Y == 1 ? 4 : i;

                    nextMap[iEdge, jEdge] = '#';
                  }
                }
              }
            }

            nextMap[2,2] = '?';
            continue;
          }

          for (int i = 0; i < 5; i++)
          {
            for (int j = 0; j < 5; j++)
            {
              if(i == 2 && j == 2)
              {
                nextMap[i,j] = '?';
                continue;
              }

              char location = map[i, j];
              Dictionary<char, int> neighboursVals = new Dictionary<char, int>();
              neighboursVals.Add('.', 0);
              neighboursVals.Add('#', 0);

              var neighbours = GetNeighbours(new Point(i, j), lvl);
              foreach (var neighbour in neighbours)
              {
                if(maps.ContainsKey(neighbour.Item2))
                  neighboursVals[maps[neighbour.Item2][neighbour.Item1.X, neighbour.Item1.Y]]++;
              }

              if (location == '#')
                nextMap[i, j] = neighboursVals['#'] == 1 ? '#' : '.';
              if (location == '.')
                nextMap[i, j] = neighboursVals['#'] == 1 || neighboursVals['#'] == 2 ? '#' : '.';
            }
          }
        }

        maps = nextMaps;

        //foreach (var map in maps)
        //{
        //  if(!IsEmpty(map.Value))
        //    Print(map.Value, map.Key);
        //}
      }

      foreach(var map in maps)
        foreach(var c in map.Value.Cast<char>())
          if(c == '#')
            count++;

      return count;
    }
  }
}


