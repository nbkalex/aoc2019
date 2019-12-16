//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace day5
//{
//  class Program
//  {

//    static bool next_permutation<T>(IList<T> a) where T : IComparable
//    {
//      if (a.Count < 2) return false;
//      var k = a.Count - 2;

//      while (k >= 0 && a[k].CompareTo(a[k + 1]) >= 0) k--;
//      if (k < 0) return false;

//      var l = a.Count - 1;
//      while (l > k && a[l].CompareTo(a[k]) <= 0) l--;

//      var tmp = a[k];
//      a[k] = a[l];
//      a[l] = tmp;

//      var i = k + 1;
//      var j = a.Count - 1;
//      while (i < j)
//      {
//        tmp = a[i];
//        a[i] = a[j];
//        a[j] = tmp;
//        i++;
//        j--;
//      }

//      return true;
//    }

//    static void Main(string[] args)
//    {
//      using (StreamReader sr = new StreamReader("TextFile1.txt"))
//      {
//        string input = sr.ReadToEnd();

//        var values = input.Split(',').Select(i => long.Parse(i)).ToList();

//        long maxOutput = int.MinValue;

//        int[] settingsSequence = { 9, 8, 7, 6, 5 };

//        do
//        {
//          long currentOutput = 0;

//          Dictionary<int, List<long>> settingProgram = new Dictionary<int, List<long>>();
//          foreach (var setting in settingsSequence)
//            settingProgram.Add(setting, values.ToList());

//          int settingsIndex = 0;
//          try
//          {
//            while (true)
//            {
//              currentOutput = Run(settingProgram[settingsSequence[settingsIndex]], new long[] { settingsSequence[settingsIndex], currentOutput });
//              settingsIndex++;
//              settingsIndex %= settingsSequence.Count();

//              Console.WriteLine(currentOutput);

//              if (currentOutput > maxOutput)
//                maxOutput = currentOutput;              
//            }
//          }
//          catch
//          {
//          }

//        } while (next_permutation(settingsSequence));

//        Console.WriteLine(maxOutput);
//      }
//    }

//    static long Run(List<long> values, long[] inputs)
//    {
//      int index = 0;
//      int currentInputIndex = 0;

//      while (index < values.Count)
//      {
//        string op = new string(values[index].ToString().Reverse().ToArray());

//        char param1Mod = '0';
//        char param2Mod = '0';

//        if (op.Length > 2)
//          param1Mod = op[2];

//        if (op.Length > 3)
//          param2Mod = op[3];

//        int numarParametrii = 0;

//        switch (op[0])
//        {
//          case '1':
//          case '2':
//            numarParametrii = 4;
//            break;
//          case '3':
//          case '4':
//            numarParametrii = 2;
//            break;
//          case '5':
//          case '6':
//            numarParametrii = 3;
//            break;
//          case '7':
//          case '8':
//            numarParametrii = 4;
//            break;
//        }

//        if (numarParametrii == 0)
//          break;

//        long param1Index = param1Mod == '0' ? (int)values[index + 1] : index + 1;

//        long param2Index = 0;

//        if (numarParametrii > 2)
//          param2Index = param2Mod == '0' ? values[index + 2] : index + 2;

//        if(param1Index > int.MaxValue || param2Index > int.MaxValue)
//          throw new Exception();

//        switch (op[0])
//        {
//          case '1':
//            values[(int)values[index + 3]] = values[(int)param1Index] + values[(int)param2Index];
//            break;

//          case '2':
//            values[(int)values[index + 3]] = values[(int)param1Index] * values[(int)param2Index];
//            break;

//          case '3':
//            values[(int)values[index + 1]] = inputs[currentInputIndex];
//            currentInputIndex++;
//            break;

//          case '4':
//            //Console.WriteLine(values[param1Index]);
//            return values[(int)param1Index];

//          case '5':
//            if (values[(int)param1Index] != 0)
//            {
//              if(values[(int)param2Index] > int.MaxValue)
//                throw new Exception();

//              index = (int)values[(int)param2Index];
//              continue;
//            }
//            break;

//          case '6':
//            if (values[(int)param1Index] == 0)
//            {
//              index = (int)values[(int)param2Index];
//              continue;
//            }
//            break;
//          case '7':
//            if(values[index + 3] > int.MaxValue)
//              throw new Exception();

//            values[(int)values[index + 3]] = values[(int)param1Index] < values[(int)param2Index] ? 1 : 0;
//            break;
//          case '8':
//            if (values[index + 3] > int.MaxValue)
//              throw new Exception();
//            values[(int)values[index + 3]] = values[(int)param1Index] == values[(int)param2Index] ? 1 : 0;
//            break;
//        }

//        index += numarParametrii;
//      }

//      throw new Exception();
//    }
//  }
//}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day5
{

  class ProgramState
  {
    public List<long> program;
    public int instructionPointer = 0;
    public long output = 0;
    public bool alive = true;
  }

  class Program
  {
    static bool next_permutation<T>(IList<T> a) where T : IComparable
    {
      if (a.Count < 2) return false;
      var k = a.Count - 2;

      while (k >= 0 && a[k].CompareTo(a[k + 1]) >= 0) k--;
      if (k < 0) return false;

      var l = a.Count - 1;
      while (l > k && a[l].CompareTo(a[k]) <= 0) l--;

      var tmp = a[k];
      a[k] = a[l];
      a[l] = tmp;

      var i = k + 1;
      var j = a.Count - 1;
      while (i < j)
      {
        tmp = a[i];
        a[i] = a[j];
        a[j] = tmp;
        i++;
        j--;
      }

      return true;
    }

    static int firstReadInstructionIndex = -1;

    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();

        var values = input.Split(',').Select(i => long.Parse(i)).ToList();

        long maxOutput = int.MinValue;

        int[] settingsSequence = { 5,6,7,8,9 };

        do
        {
          Dictionary<int, ProgramState> settingProgram = new Dictionary<int, ProgramState>();
          foreach (var setting in settingsSequence)
            settingProgram.Add(setting, new ProgramState() { program = values.ToList() });

          int settingsIndex = 0;
          try
          {

            while (true)
          {

            var currentSettingsProgram = settingProgram[settingsSequence[settingsIndex]];
            var currentSettings = settingsSequence[settingsIndex];
            int prevIndex = settingsIndex != 0 ? settingsIndex - 1 : settingsSequence.Count() - 1;

            settingsIndex++;
            settingsIndex %= settingsSequence.Count();


            var prevSettingsProgram = settingProgram[settingsSequence[prevIndex]];

            
              Run(currentSettingsProgram, new long[] { currentSettings, prevSettingsProgram.output });


              Console.WriteLine(currentSettingsProgram.output);

              if (currentSettingsProgram.output > maxOutput)
                maxOutput = currentSettingsProgram.output;
          }

          }
          catch
          {
          }

        } while (next_permutation(settingsSequence));

        Console.WriteLine(maxOutput);
      }
    }

    static void Run(ProgramState programState, long[] inputs)
    {
      List<long> values = programState.program;
      while (true)
      {
        string op = new string(values[programState.instructionPointer].ToString().Reverse().ToArray());

        char param1Mod = '0';
        char param2Mod = '0';

        if (op.Length > 2)
          param1Mod = op[2];

        if (op.Length > 3)
          param2Mod = op[3];

        int numarParametrii = 0;

        switch (op[0])
        {
          case '1':
          case '2':
            numarParametrii = 4;
            break;
          case '3':
          case '4':
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
          default:
            throw new Exception();
        }

        if (numarParametrii == 0)
          break;

        long param1Index = param1Mod == '0' ? (int)values[programState.instructionPointer + 1] : programState.instructionPointer + 1;

        long param2Index = 0;

        if (numarParametrii > 2)
          param2Index = param2Mod == '0' ? values[programState.instructionPointer + 2] : programState.instructionPointer + 2;

        if (param1Index > int.MaxValue || param2Index > int.MaxValue)
          throw new Exception();

        switch (op[0])
        {
          case '1':
            values[(int)values[programState.instructionPointer + 3]] = values[(int)param1Index] + values[(int)param2Index];
            break;

          case '2':
            values[(int)values[programState.instructionPointer + 3]] = values[(int)param1Index] * values[(int)param2Index];
            break;

          case '3':

            if (firstReadInstructionIndex == -1)
              firstReadInstructionIndex = programState.instructionPointer;

            values[(int)values[programState.instructionPointer + 1]] = 
              firstReadInstructionIndex == programState.instructionPointer 
              ? inputs[0] 
              : inputs[1];

            break;

          case '4':
            //Console.WriteLine(values[param1Index]);
            programState.instructionPointer += numarParametrii;
            programState.output = values[(int)param1Index];
            return;

          case '5':
            if (values[(int)param1Index] != 0)
            {
              if (values[(int)param2Index] > int.MaxValue)
                throw new Exception();

              programState.instructionPointer = (int)values[(int)param2Index];
              continue;
            }
            break;

          case '6':
            if (values[(int)param1Index] == 0)
            {
              programState.instructionPointer = (int)values[(int)param2Index];
              continue;
            }
            break;
          case '7':
            if (values[programState.instructionPointer + 3] > int.MaxValue)
              throw new Exception();

            values[(int)values[programState.instructionPointer + 3]] = values[(int)param1Index] < values[(int)param2Index] ? 1 : 0;
            break;
          case '8':
            if (values[programState.instructionPointer + 3] > int.MaxValue)
              throw new Exception();
            values[(int)values[programState.instructionPointer + 3]] = values[(int)param1Index] == values[(int)param2Index] ? 1 : 0;
            break;
          default:
            throw new Exception();
        }

        programState.instructionPointer += numarParametrii;
      }

      throw new Exception();
    }
  }
}

