using LibHive;
using System;

namespace MetroTools
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistryHive rh = new RegistryHive();
            rh.ReadFile(@"C:\Users\Thomas\Downloads\settings.dat");

            Console.WriteLine("Magic:\t\t{0}", rh.Header.Magic);
            Console.WriteLine("Well-written:\t{0}", rh.Header.IsWellWritten);
            Console.WriteLine("Last modified:\t{0}", rh.Header.LastModified);
            Console.WriteLine("Format version:\t{0}", rh.Header.FormatVersion);
            Console.WriteLine("Type:\t\t{0}", rh.Header.Type);
            Console.WriteLine("Root type:\t{0}", rh.Header.RootCell.Type);
            Console.WriteLine("Root table:\t{0}", rh.Header.RootCell.Table);
            Console.WriteLine("Root block:\t{0}", rh.Header.RootCell.Block);
            Console.WriteLine("Root offset:\t{0}", rh.Header.RootCell.Offset);
            Console.WriteLine("Length:\t\t{0}", rh.Header.Length);
            Console.WriteLine("Cluster:\t{0}", rh.Header.Cluster);
            Console.WriteLine("Filename:\t{0}", rh.Header.FileName);

            foreach(var bin in rh.Bins)
            {
                Console.WriteLine();
                Console.WriteLine(new string('=', Console.WindowWidth));
                Console.WriteLine("Magic:\t\t{0}", bin.Magic);
                Console.WriteLine("File offset:\t{0}", bin.FileOffset);
                Console.WriteLine("Size:\t\t{0}", bin.Size);
                Console.WriteLine("Last modified:\t{0}", bin.LastModified);
                Console.WriteLine("Cells:\t\t{0}", bin.Cells.Length);
            }

            Console.ReadKey();
        }
    }
}
