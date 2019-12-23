using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day22
{
  class Program
  {
    static readonly Dictionary<string, Func<long[], int, long[]>> ShufflingTypes = new Dictionary<string, Func<long[], int, long[]>>()
    {
      {  "cut", (long[] arr, int val) => { Rotate(arr,val); return arr; } },
      {  "deal into new", (long[] arr, int val) => arr.Reverse().ToArray() },
      {  "deal with increment", (long[] arr, int val) => Incement(arr, val) },
    };

    public static void LeftShiftArray<T>(T[] arr, int shift)
    {
      shift = shift % arr.Length;
      T[] buffer = new T[shift];
      Array.Copy(arr, buffer, shift);
      Array.Copy(arr, shift, arr, 0, arr.Length - shift);
      Array.Copy(buffer, 0, arr, arr.Length - shift, shift);
    }

    public static void RightShiftArray<T>(T[] arr, int shift)
    {
      shift = shift % arr.Length;
      T[] buffer = new T[shift];
      Array.Copy(arr, arr.Length - shift, buffer, 0, shift);
      Array.Copy(arr, 0, arr, shift, arr.Length - shift);
      Array.Copy(buffer, 0, arr, 0, shift);
    }

    public static void Rotate(long[] arr, int shift)
    {
      if (shift < 0)
        RightShiftArray(arr, Math.Abs(shift));
      else
        LeftShiftArray(arr, shift);
    }

    static long[] Incement(long[] arr, int n)
    {
      long[] result = new long[arr.Length];

      for (int i = 0; i < arr.Length; i++)
        result[n * i % arr.Length] = arr[i];

      return result;
    }

    static void Main(string[] args)
    {
      const int DECK_SIZE = 10007;

      long[] deck = new long[DECK_SIZE];
      for (int i = 0; i < DECK_SIZE; i++)
        deck[i] = i;

      using (StreamReader sr = new StreamReader("TextFile1.txt"))
      {
        string input = sr.ReadToEnd();
        string[] shuffleTypes = input.Split("\r\n");

        foreach(var sf in shuffleTypes)
        {
          string[] words = sf.Split(" ");
          string shuffleType = sf.Substring(0, sf.LastIndexOf(' '));

          int parsedVal = 0;
          int.TryParse(words.Last(), out parsedVal);
          deck = ShufflingTypes[shuffleType](deck, parsedVal);
        }

        Console.WriteLine(deck.ToList().FindIndex(c => c == 2019));
      }
    }
  }
}
