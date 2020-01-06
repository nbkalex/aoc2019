using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

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
      return (BIG_DECK_SIZE + search + n) % BIG_DECK_SIZE;
    }

    //const int DECK_SIZE = 10007;
    const long BIG_DECK_SIZE = /*10007*/119315717514047;
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

        long b = 0;
        foreach (var shuffle in shuffleTypesInverted)
          b = shuffle.Item1(shuffle.Item2, b);

        Console.WriteLine(b);

        long a = 1;

        foreach (var shuffle in shuffleTypesInverted)
          a = shuffle.Item1(shuffle.Item2, a);

        a += BIG_DECK_SIZE - b;
        Console.WriteLine(a);

        BigInteger res = 2020 * a + b;
        Console.WriteLine(res % BIG_DECK_SIZE);

        long c = 2020;

        foreach (var shuffle in shuffleTypesInverted)
          c = shuffle.Item1(shuffle.Item2, c);

        foreach (var shuffle in shuffleTypesInverted)
          c = shuffle.Item1(shuffle.Item2, c);

        foreach (var shuffle in shuffleTypesInverted)
          c = shuffle.Item1(shuffle.Item2, c);

        Console.WriteLine(c);

        BigInteger a1 = a;
        BigInteger b1 = b;
        BigInteger x = 2020;

        // 101741582076661
        BigInteger times= 101741582076661;

        BigInteger res_2 = (BigInteger.ModPow(a1, 3, BIG_DECK_SIZE) * x + BigInteger.ModPow(a1, 2, BIG_DECK_SIZE) * b1 + a1 * b1 + b1)% BIG_DECK_SIZE;
        Console.WriteLine(res_2);


        var a_a_times_1 = (a1 * (BigInteger.ModPow(a1, times-1, BIG_DECK_SIZE) - 1) * BigInteger.ModPow(a1-1, BIG_DECK_SIZE - 2, BIG_DECK_SIZE) + 1);
        BigInteger resFinal = (BigInteger.ModPow(a1, times, BIG_DECK_SIZE) * x % BIG_DECK_SIZE +  b1 * a_a_times_1) % BIG_DECK_SIZE;

        Console.WriteLine(resFinal);
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
