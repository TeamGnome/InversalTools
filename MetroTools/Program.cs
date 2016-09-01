using System;
using LibHive;

namespace MetroTools
{
   internal class Program
   {
      private static void Main(string[] args)
      {
         RegistryHive rh = new RegistryHive();
         rh.ReadFile(@"C:\Users\Thomas\Downloads\settings.dat");

         Console.WriteLine($"Magic:\t\t{rh.Header.Magic}");
         Console.WriteLine($"Well-written:\t{rh.Header.IsWellWritten}");
         Console.WriteLine($"Last modified:\t{rh.Header.LastModified}");
         Console.WriteLine($"Format version:\t{rh.Header.FormatVersion}");
         Console.WriteLine($"Type:\t\t{rh.Header.Type}");
         Console.WriteLine($"Root type:\t{rh.Header.RootCell.Type}");
         Console.WriteLine($"Root table:\t{rh.Header.RootCell.Table}");
         Console.WriteLine($"Root block:\t{rh.Header.RootCell.Block}");
         Console.WriteLine($"Root offset:\t{rh.Header.RootCell.Offset}");
         Console.WriteLine($"Length:\t\t{rh.Header.Length}");
         Console.WriteLine($"Cluster:\t{rh.Header.Cluster}");
         Console.WriteLine($"Filename:\t{rh.Header.FileName}");

         foreach (HiveBin bin in rh.Bins)
         {
            Console.WriteLine();
            Console.WriteLine(new string('=', Console.WindowWidth));
            Console.WriteLine($"Magic:\t\t{bin.Magic}");
            Console.WriteLine($"File offset:\t{bin.FileOffset}");
            Console.WriteLine($"Size:\t\t{bin.Size}");
            Console.WriteLine($"Last modified:\t{bin.LastModified}");
            Console.WriteLine($"Cells:\t\t{bin.Cells.Length}");
         }

         Console.ReadKey();
      }
   }
}