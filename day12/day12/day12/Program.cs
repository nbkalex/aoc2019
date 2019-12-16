using System;
using System.Collections.Generic;
using System.Drawing;

namespace day12
{
  /*
   <x=-1, y=0, z=2>
   <x=2, y=-10, z=-7>
   <x=4, y=-8, z=8>
   <x=3, y=5, z=-1>
  */

  class Moon
  {
    public long x, y, z, vx, vy, vz;
  }


  class Program
  {
    static void Main(string[] args)
    {
      long[] cicles = new long[4];

      //List<Moon> moons = new List<Moon>()
      //{
      //  new Moon(){x = -1, y = 0, z= 2},
      //  new Moon(){x = 2, y = -10, z= -7},
      //  new Moon(){x = 4, y = -8, z=8},
      //  new Moon(){x = 3, y = 5, z= -1 }
      //};

      long[] indices = new long[3];

      List<Moon> moons = new List<Moon>()
      {
        new Moon(){x = -13, y = 14, z= -7},
        new Moon(){x = -18, y = 9, z= 0},
        new Moon(){x = 0, y = -3, z=-3 },
        new Moon(){x = -15, y = 3, z= -13 }
      };

      Dictionary<Tuple<long, long, long, long, long, long, long, Tuple<long>>, int> xs = new Dictionary<Tuple<long, long, long, long, long, long, long, Tuple<long>>, int>();
      Dictionary<Tuple<long, long, long, long, long, long, long, Tuple<long>>, int> ys = new Dictionary<Tuple<long, long, long, long, long, long, long, Tuple<long>>, int>();
      Dictionary<Tuple<long, long, long, long, long, long, long, Tuple<long>>, int> zs = new Dictionary<Tuple<long, long, long, long, long, long, long, Tuple<long>>, int>();

      int step = 0;
      for (; ; )
      {
        Tuple<long, long, long, long, long, long, long,Tuple<long>> snapshotx = new Tuple<long, long, long, long, long, long, long,Tuple<long>>(moons[0].x, moons[1].x, moons[2].x, moons[3].x, moons[0].vx, moons[1].vx, moons[2].vx, Tuple.Create(moons[3].vx));
        Tuple<long, long, long, long, long, long, long,Tuple<long>> snapshoty = new Tuple<long, long, long, long, long, long, long,Tuple<long>>(moons[0].y, moons[1].y, moons[2].y, moons[3].y, moons[0].vy, moons[1].vy, moons[2].vy, Tuple.Create(moons[3].vy));
        Tuple<long, long, long, long, long, long, long,Tuple<long>> snapshotz = new Tuple<long, long, long, long, long, long, long, Tuple<long>>(moons[0].z, moons[1].z, moons[2].z, moons[3].z, moons[0].vz, moons[1].vz, moons[2].vz, Tuple.Create(moons[3].vz));

        try
        { 
          xs.Add(snapshotx, step);
        }
        catch
        {
          if(indices[0] == 0)
            indices[0] = step;
        }

        try
        {
          ys.Add(snapshoty, step);
        }
        catch
        {
          if (indices[1] == 0)
            indices[1] = step;
        }

        try
        {
          zs.Add(snapshotz, step);
        }
        catch
        {
          if (indices[2] == 0)
            indices[2] = step;
        }

        bool cont = false;
        foreach(var s in indices)
          if(s == 0)
            cont = true;

        if(!cont)
          break;

        step++;
        foreach (var m in moons)
        {
          foreach (var m2 in moons)
          {
            if (m2 == m)
              continue;

            if (m.x < m2.x)
              m.vx++;
            else if (m.x > m2.x) m.vx--;

            if (m.y < m2.y)
              m.vy++;
            else if (m.y > m2.y) m.vy--;

            if (m.z < m2.z)
              m.vz++;
            else if (m.z > m2.z) m.vz--;
          }

          //Console.ReadKey();
        }

        foreach (var m in moons)
        {
          m.x += m.vx;
          m.y += m.vy;
          m.z += m.vz;
        }
      }

      //long total = 0;

      //foreach (var m in moons)
      //  total += (Math.Abs(m.x) + Math.Abs(m.y) + Math.Abs(m.z)) * (Math.Abs(m.vx) + Math.Abs(m.vy) + Math.Abs(m.vz));

      long cmmmmc = indices[0];

      foreach(var i in indices)
        cmmmmc = LCM(cmmmmc, i);

      Console.WriteLine(cmmmmc);
    }

    private static long GCD(long a, long b)
    {
      a = Math.Abs(a);
      b = Math.Abs(b);

      // Pull out remainders.
      for (; ; )
      {
        long remainder = a % b;
        if (remainder == 0) return b;
        a = b;
        b = remainder;
      };
    }

    private static long LCM(long a, long b)
    {
      return a * b / GCD(a, b);
    }
  }
}
