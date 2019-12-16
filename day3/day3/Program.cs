using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace day3
{
  class Program
  {
    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        var values = sr.ReadToEnd().Split("\r\n").Select(v => v.Split(','));
        List<Dictionary<Point, int>> wires = new List<Dictionary<Point, int>>();

        foreach (string[] wire in values)
        {
          var wireObj = new Dictionary<Point, int>();
          wires.Add(wireObj);

          int currenti = 0;
          int currentj = 0;

          int count = 1;

          foreach (string val in wire)
          {
            int nexti = currenti;
            int nextj = currentj;

            int dist = int.Parse(val.Substring(1));
            int stephor = 0;
            int stepvert = 0;

            if (val[0] == 'R')
              stephor = 1;

            if (val[0] == 'U')
              stepvert = -1;

            if (val[0] == 'D')
              stepvert = 1;

            if (val[0] == 'L')
              stephor = -1;

            int steps = 0;
            while(steps < dist)
            {
              steps++;
              currenti += stepvert;
              currentj += stephor;

              Point p = new Point(currenti, currentj);
              if (wireObj.ContainsKey(p))
                wireObj[p] = count ;
              else
                wireObj.Add(p, count);

              count++;
            }
          }
        }

        
        HashSet<Point> interserctions = new HashSet<Point>(wires[0].Keys.Intersect(wires[1].Keys));        

        Console.WriteLine(interserctions.Min(i => Math.Abs(i.X) + Math.Abs(i.Y)));
        Console.WriteLine(interserctions.Min(i => wires[0][i] + wires[1][i]));
      }
    }
  }
}
