using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day22
{
  class Program
  {
    static readonly Dictionary<string, Func<int, long, long>> ShufflingInvertedTypes = new Dictionary<string, Func<int, long, long>>()
    {
      {  "cut", RevertRotate },
      {  "deal into new", RevertReverse },
      {  "deal with increment", RevertIncrement },
    };

    static long RevertIncrement(int n, long search)
    {
      for (int c = 0; c < n; c++)
      {
        double i = ((double)c * BIG_DECK_SIZE + search) / n;
        if (i == (long)i && (i * n) % BIG_DECK_SIZE == search)
          return (long)i;
      }

      throw new Exception();
    }

    static long RevertReverse(int n, long search)
    {
      return BIG_DECK_SIZE - search - 1;
    }

    static long RevertRotate(int n, long search)
    {
        return search + n % BIG_DECK_SIZE;
    }

    const int DECK_SIZE = 10007;
    const long BIG_DECK_SIZE = /*10007;*/119315717514047;
    const int SEARCH = 1;

    static void Main(string[] args)
    {
      List<Tuple<Func<int, long, long>, int>> shuffleTypesInverted = new List<Tuple<Func<int, long, long>, int>>();

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        string[] shuffleTypes = input.Split("\r\n");

        foreach (var sf in shuffleTypes)
        {
          string[] words = sf.Split(" ");
          string shuffleType = sf.Substring(0, sf.LastIndexOf(' '));

          int parsedVal = -1;
          int.TryParse(words.Last(), out parsedVal);

          shuffleTypesInverted.Add(new Tuple<Func<int, long, long>, int>(ShufflingInvertedTypes[shuffleType], parsedVal));

          //List<Tuple<Func<int, long, long>, int>> shuffleTypesInvertedTest = new List<Tuple<Func<int, long, long>, int>>(shuffleTypesInverted);
          //shuffleTypesInvertedTest.Reverse();
          //Test(shuffleTypesInvertedTest);
        }

        shuffleTypesInverted.Reverse();

        long current = 2020;

        for (long i = 0; i < 101741582076661; i++)
        {
          foreach (var shuffle in shuffleTypesInverted)
            current = shuffle.Item1(shuffle.Item2, current);

            Console.WriteLine(current);
        }
      }
    }

    static void Test(List<Tuple<Func<int, long, long>, int>> shuffleTypesInverted)
    {
      long current = SEARCH;

      foreach (var shuffle in shuffleTypesInverted)
        current = shuffle.Item1(shuffle.Item2, current);

      Console.WriteLine(current);
    }
  }
}
