using System;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      StreamReader sr = new StreamReader("TextFile1.txt");
      string all = sr.ReadToEnd();
      var valsinit = all.Split(',').Select(v => int.Parse(v)).ToList();

      int x = 95;
      int y = 7;

      while (true)
      {
        var vals = valsinit.ToList();
               

        vals[1] = x;
        vals[2] = y;

        try
        {
          for (int i = 0; i < vals.Count(); i += 4)
          {
            if (vals[i] == 1)
              vals[vals[i + 3]] = vals[vals[i + 1]] + vals[vals[i + 2]];
            else if (vals[i] == 2)
              vals[vals[i + 3]] = vals[vals[i + 1]] * vals[vals[i + 2]];
            else break;
          }

          if(vals[0] == 19690720)
          {
            Console.WriteLine($"x = {x}, y = {y}");
            return;
          }
        }
        catch
        {
          Console.WriteLine(vals[0]);
        }
        x++; y++;

        Console.WriteLine();
        Console.Write(vals[0]);
      }
    }
  }
}
