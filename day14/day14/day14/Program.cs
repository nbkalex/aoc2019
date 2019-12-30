using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
  class Reaction
  {
    public long ResultQuantity;
    public Dictionary<string, long> Agents = new Dictionary<string, long>();
  }

  class Program
  {
    static void Main(string[] args)
    {
      Dictionary<string, Reaction> reactions = new Dictionary<string, Reaction>();

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        string[] lines = input.Split("\r\n");

        foreach (var line in lines)
        {
          string[] agenstResult = line.Split(" => ");
          string result = agenstResult[1].Split(" ")[1];
          int resultQuantity = int.Parse(agenstResult[1].Split(" ")[0]);

          Reaction reaction = new Reaction();
          reaction.ResultQuantity = resultQuantity;

          string[] agents = agenstResult[0].Split(", ");
          foreach (var agent in agents)
            reaction.Agents.Add(agent.Split(' ')[1], int.Parse(agent.Split(' ')[0]));

          reactions.Add(result, reaction);
        }

        long fuelCount = 99999999;
        int digits = 8;

        while (digits > 0)
        {
          if (GetTotalOre(reactions, fuelCount - (long)Math.Pow(10, digits - 1)) > 1000000000000)
            fuelCount -= (long)Math.Pow(10, digits - 1);
          else
            digits--;
        }

        Console.WriteLine(fuelCount-1);
      }
    }

    static long GetTotalOre(Dictionary<string, Reaction> reactions, long fuelCount)
    {
      long total = 0;

      Dictionary<string, long> countReactions = new Dictionary<string, long>();
      Dictionary<string, long> leftOvers = new Dictionary<string, long>();

      foreach (var crK in reactions.Keys)
      {
        countReactions.Add(crK, 0);
        leftOvers.Add(crK, 0);
      }

      Queue<Tuple<string, long>> todo = new Queue<Tuple<string, long>>();
      todo.Enqueue(new Tuple<string, long>("FUEL", fuelCount));

      while (todo.Count > 0)
      {
        var reactionTodo = todo.Dequeue();
        var reaction = reactions[reactionTodo.Item1];
        long result = reaction.ResultQuantity;

        long qTodo = reactionTodo.Item2 - leftOvers[reactionTodo.Item1];

        long reactionCount = qTodo < 0 ? 0 : qTodo / reaction.ResultQuantity;

        if (qTodo % reaction.ResultQuantity != 0 && qTodo > 0)
          reactionCount++;

        countReactions[reactionTodo.Item1] += reactionCount * result;

        leftOvers[reactionTodo.Item1] = reactionCount * result - qTodo;

        foreach (var agent in reaction.Agents)
        {
          if (agent.Key != "ORE")
            todo.Enqueue(new Tuple<string, long>(agent.Key, agent.Value * reactionCount));
        }
      }

      foreach (var reaction in countReactions)
      {
        var r = reactions[reaction.Key].Agents.ElementAt(0);
        if (r.Key == "ORE")
          total += r.Value * reaction.Value / reactions[reaction.Key].ResultQuantity;
      }

      return total;
    }
  }
}
