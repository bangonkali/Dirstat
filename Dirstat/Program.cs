using System;
using Dirstat.Data;

namespace Dirstat
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Root!");

      Scanner scanner = new Scanner(new ScannerOptions()
      {
        Root = "/Users/gilmichael/Downloads/Test Assessment",
      });

      scanner.Scan();
    }
  }
}