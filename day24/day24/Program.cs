using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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

        HashSet<long> set = new HashSet<long>();

        for (; ; )
        {
          char[,] next = (char[,])map.Clone();

          long count = 0;
          for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
              if (map[i, j] == '#')
                count += (long)Math.Pow(2, i * 5 + j);

          if (!set.Add(count))
          {
            Console.WriteLine(count);
            return;
          }

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

          for (int i = 0; i < 5; i++)
          {
            for (int j = 0; j < 5; j++)
            {
              Console.Write(map[i, j]);
            }

            Console.WriteLine();
          }

          Console.WriteLine();
          Console.WriteLine();
        }
      }
    }
  }
}


