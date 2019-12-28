using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day6
{
  class Program
  {
    class satelite
    {
      public string name;
      public satelite parent;

      public int orbits()
      {        
        if(parent != null)
         return parent.orbits() + 1;
        return 0;
      }
    }

    static void Main(string[] args)
    {
      Dictionary<string, satelite> tree = new Dictionary<string, satelite>();

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string all = sr.ReadToEnd();
        string[] vals = all.Split("\r\n");
        foreach(var val in vals)
        {
          var data = val.Split(')');
          satelite current = null;
          satelite currentSatelite = null;
          if (tree.ContainsKey(data[0]))
          {
            current = tree[data[0]];
          }
          else
          { 
            current = new satelite() { name = data[0]};
            tree.Add(current.name, current);
          }

          if (tree.ContainsKey(data[1]))
            currentSatelite = tree[data[1]];
          else
          {
            currentSatelite = new satelite() { name = data[1] };
            tree.Add(currentSatelite.name, currentSatelite);
          }

          currentSatelite.parent = current;
        }

        int count = 0;
        foreach(var satelite in tree)
        {
          count += satelite.Value.orbits();
        }

        satelite san = tree["SAN"];
        satelite you = tree["YOU"];

        satelite cur = you;
        List<satelite> wayYouRoot = new List<satelite>();
        List<satelite> waySanRoot = new List<satelite>();
        while (cur != null)
        {
          wayYouRoot.Add(cur);
          cur = cur.parent;
        }

        cur = san;
        while (cur != null)
        {
          waySanRoot.Add(cur);
          cur = cur.parent;
        }

        var res = waySanRoot.Count + wayYouRoot.Count - (2 * wayYouRoot.Intersect(waySanRoot).Count()) - 2;

        Console.WriteLine(count);
      }
    }
  }
}
