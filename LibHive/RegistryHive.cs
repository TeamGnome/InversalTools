using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using LibHive.Cells;
using LibHive.Structures;

namespace LibHive
{
   public class RegistryHive
   {
      private const int BLOCK_SIZE = 4096;

      public HiveHeader Header { get; }
      public HiveBin[] Bins { get; }

      public RegistryHive(string path)
      {
         FileInfo fi = new FileInfo(path);

         using (MemoryMappedFile mmfHive = MemoryMappedFile.CreateFromFile(path, FileMode.Open))
         {
            using (MemoryMappedViewAccessor mmvaInitialBlock = mmfHive.CreateViewAccessor(0, BLOCK_SIZE))
            {
               HiveHeaderStructure head;
               mmvaInitialBlock.Read(0, out head);

               Header = new HiveHeader(head);
            }

            List<HiveBin> bins = new List<HiveBin>();

            for (int i = BLOCK_SIZE; i < fi.Length - BLOCK_SIZE; i += BLOCK_SIZE)
            {
               using (MemoryMappedViewAccessor mmvaBlock = mmfHive.CreateViewAccessor(i, BLOCK_SIZE))
               {
                  HiveBinStructure bin;
                  mmvaBlock.Read(0, out bin);

                  HiveBin hb = new HiveBin(bin);

                  // malformed or empty bin
                  if (hb.Magic != "hbin")
                  {
                     continue;
                  }

                  List<HiveCell> cells = new List<HiveCell>();

                  int cellStart = Marshal.SizeOf(bin);
                  for (int j = cellStart; j < BLOCK_SIZE - cellStart;)
                  {
                     int sizeRecord = mmvaBlock.ReadInt32(j);
                     HiveCell hc = new HiveCell
                     {
                        Size = Math.Abs(sizeRecord),
                        Allocated = sizeRecord < 0
                     };

                     cells.Add(hc);

                     j += hc.Size;
                  }

                  hb.Cells = cells.ToArray();
                  bins.Add(hb);
               }
            }

            Bins = bins.ToArray();
         }
      }
   }
}