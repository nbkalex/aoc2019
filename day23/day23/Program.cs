using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Numerics;

namespace day5
{

  class Intcode
  {
    public static int producedCount = 0;

    public int index = 0;
    long offset = 0;
    List<long> values;
    public bool isIdle = false;
    int id;

    public Queue<long> input = new Queue<long>();
    int receivedCooords = 0;

    Dictionary<int, Intcode> Programs;

    List<long> currentPackage = new List<long>();

    public static Tuple<long, long> lastNATval;

    public Intcode(List<long> avalues, int aid, Dictionary<int, Intcode> aPrograms)
    {
      id = aid;
      values = avalues;
      input.Enqueue(id);
      Programs = aPrograms;
    }
    public bool RunOnce()
    {
      string op = new string(values[index].ToString().Reverse().ToArray());

      char param1Mod = '0';
      char param2Mod = '0';
      char param3Mod = '0';

      if (op == "99")
        return false;

      if (op.Length > 2)
        param1Mod = op[2];

      if (op.Length > 3)
        param2Mod = op[3];

      if (op.Length > 4)
        param3Mod = op[4];

      int numarParametrii = 0;

      switch (op[0])
      {
        case '1':
        case '2':
          numarParametrii = 4;
          break;
        case '3':
        case '4':
        case '9':
          numarParametrii = 2;
          break;
        case '5':
        case '6':
          numarParametrii = 3;
          break;
        case '7':
        case '8':
          numarParametrii = 4;
          break;
      }

      int param1Index = param1Mod == '0' ? (int)values[index + 1] : param1Mod == '2' ? (int)(offset + values[index + 1]) : index + 1;

      int param2Index = 0;
      int param3Index = 0;

      if (numarParametrii > 2)
        param2Index = param2Mod == '0' ? (int)values[index + 2] : param2Mod == '2' ? (int)(offset + values[index + 2]) : index + 2;

      if (numarParametrii > 3)
        param3Index = param3Mod == '0' ? (int)values[index + 3] : param3Mod == '2' ? (int)(offset + values[index + 3]) : index + 3;

      switch (op[0])
      {
        case '1':
          values[param3Index] = values[param1Index] + values[param2Index];
          break;

        case '2':
          values[param3Index] = values[param1Index] * values[param2Index];
          break;

        case '3':
          values[param1Index] = input.Count > 0 ? input.Dequeue() : -1;
          isIdle = values[param1Index] == -1;
          break;

        case '4':
          currentPackage.Add(values[param1Index]);
          isIdle = false;

          if (currentPackage.Count == 3)
          {
            producedCount++;

            if ((int)currentPackage[0] == 255)
            {
              lastNATval = new Tuple<long, long>(currentPackage[1], currentPackage[2]);
            }

            if (Programs.ContainsKey((int)currentPackage[0]))
            {
              Programs[(int)currentPackage[0]].input.Enqueue(currentPackage[1]);
              Programs[(int)currentPackage[0]].input.Enqueue(currentPackage[2]);
            }

            currentPackage.Clear();
          }
          break;

        case '5':
          if (values[param1Index] != 0)
          {
            index = (int)values[param2Index];
            return true;
          }
          break;

        case '6':
          if (values[param1Index] == 0)
          {
            index = (int)values[param2Index];
            return true;
          }
          break;
        case '7':
          values[param3Index] = values[param1Index] < values[param2Index] ? 1 : 0;
          break;
        case '8':
          values[param3Index] = values[param1Index] == values[param2Index] ? 1 : 0;
          break;
        case '9':
          offset += (long)values[param1Index];
          break;
      }

      index += numarParametrii;

      return true;
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();

        List<long> values = input.Split(',').Select(i => long.Parse(i)).ToList();
        for (int i = 0; i < 10000; i++)
          values.Add(0);

        Dictionary<int, Intcode> programs = new Dictionary<int, Intcode>();
        for (int i = 0; i < 50; i++)
          programs.Add(i, new Intcode(new List<long>(values), i, programs));
        HashSet<Tuple<long, long>> packetsTo0 = new HashSet<Tuple<long, long>>();

        int isIdleCount = 0;

        for (; ; )
        {
          foreach (var program in programs)
            program.Value.RunOnce();

          if (programs.All(p => p.Value.isIdle))
          {
            isIdleCount++;

            if (isIdleCount == 10000)
            {
              isIdleCount = 0;

              if (Intcode.lastNATval == null)
                continue;

              programs[0].input.Enqueue(Intcode.lastNATval.Item1);
              programs[0].input.Enqueue(Intcode.lastNATval.Item2);

              if (!packetsTo0.Add(Intcode.lastNATval))
              {
                Console.WriteLine(Intcode.lastNATval.Item2);
                return;
              }
            }
          }
        }
      }
    }
  }
}
