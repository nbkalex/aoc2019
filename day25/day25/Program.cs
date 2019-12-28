using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Numerics;

namespace day5
{
  class Program
  {
    const string COMMAND_REQUEST = "Command?";
    const string DOORS_HEADER = "Doors here lead:";
    const string ITEMS_HEADER = "Items here:\n";

    static void Main(string[] args)
    {
      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        const int size = 100;

        char[,] map = new char[size, size];

        long offset = 0;

        bool readDoors = false;
        bool readItems = false;

        List<string> doors = new List<string>();
        List<string> items = new List<string>();
        List<string> inventory = new List<string>();

        string input = sr.ReadToEnd();

        List<long> values = input.Split(',').Select(i => long.Parse(i)).ToList();
        for (int i = 0; i < 10000; i++)
          values.Add(0);

        int index = 0;

        List<string> readLines = new List<string>();

        string currentCommand = string.Empty;
        int currentCommandIndex = 0;

        string currentLine = "";
        while (index < values.Count)
        {
          string op = new string(values[index].ToString().Reverse().ToArray());

          char param1Mod = '0';
          char param2Mod = '0';
          char param3Mod = '0';

          if (op == "99")
            break;

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

          if (numarParametrii == 0)
            break;

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
              if (currentCommand.Length == currentCommandIndex)
              { 
                var key = Console.ReadKey();
                switch(key.Key)
                {
                  case ConsoleKey.UpArrow:
                    currentCommand= "north";
                    break;
                    case ConsoleKey.DownArrow:
                    currentCommand = "south";
                    break;
                  case ConsoleKey.RightArrow:
                    currentCommand = "east";
                    break;

                  case ConsoleKey.LeftArrow:
                    currentCommand = "west";
                    break;

                  case ConsoleKey.D1:
                    currentCommand = "take " + items[0];
                    inventory.Insert(0,items[0]);
                    break;

                  case ConsoleKey.D2:
                    currentCommand = "take " + items[1];
                    inventory.Insert(0,items[1]);
                    break;
                  case ConsoleKey.D3:
                    currentCommand = "take " + items[2];
                    inventory.Insert(0,items[2]);
                    break;
                  case ConsoleKey.D4:
                    currentCommand = "take " + items[3];
                    inventory.Insert(0,items[3]);
                    break;
                  case ConsoleKey.D5:
                    currentCommand = "take " + items[4];
                    inventory.Insert(0,items[4]);
                    break;

                  case ConsoleKey.F1:
                    currentCommand = "drop " + inventory[0];
                    inventory.Remove(inventory[0]);
                    break;

                  case ConsoleKey.F2:
                    currentCommand = "drop " + inventory[1];
                    inventory.Remove(inventory[1]);
                    break;
                  case ConsoleKey.F3:
                    currentCommand = "drop " + inventory[2];
                    inventory.Remove(inventory[2]);
                    break;
                  case ConsoleKey.F4:
                    currentCommand = "drop " + inventory[3];
                    inventory.Remove(inventory[3]);
                    break;
                  case ConsoleKey.F5:
                    currentCommand = "drop " + inventory[4];
                    inventory.Remove(inventory[4]);
                    break;

                  case ConsoleKey.F6:
                    currentCommand = "drop " + inventory[5];
                    inventory.Remove(inventory[5]);
                    break;

                  case ConsoleKey.I:
                    currentCommand = "inv";
                    break;

                  case ConsoleKey.M:
                    currentCommand = Console.ReadLine();
                    break;
                }

                currentCommand += '\n';
                currentCommandIndex = 0;
              }

              values[param1Index] = currentCommand[currentCommandIndex];
              currentCommandIndex++;

              break;

            case '4':

              currentLine += (char)values[param1Index];
              if ((char)values[param1Index] == '\n')
              {
                readLines.Add(currentLine);

                if (currentLine == DOORS_HEADER)
                {
                  readDoors = true;
                  doors.Clear();
                }
                else if (readDoors)
                {
                  if (currentLine == "\n")
                    readDoors = false;
                  else
                    doors.Add(currentLine.Substring(2));
                }
                else if(currentLine == ITEMS_HEADER)
                {
                  items.Clear();
                  readItems = true;
                }
                else if(readItems)
                {
                  if (currentLine == "\n")
                    readItems = false;
                  else
                    items.Add(currentLine.Substring(2, currentLine.Length - 3));
                }

                Console.Write(currentLine);
                currentLine = string.Empty;
              }

              break;

            case '5':
              if (values[param1Index] != 0)
              {
                index = (int)values[param2Index];
                continue;
              }
              break;

            case '6':
              if (values[param1Index] == 0)
              {
                index = (int)values[param2Index];
                continue;
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
        }
      }
    }

    static char GetDirVal(Point currendDir, Point nextDir)
    {
      int horizontal = currendDir.X * nextDir.Y;
      if (horizontal != 0)
        return horizontal < 0 ? 'L' : 'R';

      int vertical = currendDir.Y * nextDir.X;
      if (vertical != 0)
        return vertical > 0 ? 'L' : 'R';





      return ' ';
    }

    static Point GetNextNeighbour(char[,] map, Point currentPos, Point currentDir)
    {
      foreach (var dir in Directions)
      {
        if (Math.Abs(dir.X) == Math.Abs(currentDir.X) || Math.Abs(dir.Y) == Math.Abs(currentDir.Y))
          continue;

        if (currentPos.X + dir.X >= 0 && currentPos.Y + dir.Y >= 0 && map[currentPos.X + dir.X, currentPos.Y + dir.Y] == '#')
          return new Point(currentPos.X + dir.X, currentPos.Y + dir.Y);
      }

      throw new Exception("stop");
    }

    static readonly List<Point> Directions = new List<Point>()
    {
      new Point(-1,0),
      new Point(0,-1),
      new Point(1,0),
      new Point(0,1)
    };

    static bool IsIntersection(char[,] map, int i, int j)
    {
      if (map[i, j] != '#')
        return false;

      int count = 0;
      if (i > 0 && map[i - 1, j] == '#') count++;// left
      if (map[i + 1, j] == '#') count++;// right
      if (j > 0 && map[i, j - 1] == '#') count++; //TOP
      if (map[i, j + 1] == '#') count++; //BOT

      return count > 2;
    }
  }
}

