using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace LibHive
{
   public class RegistryHive
   {
      private const int BLOCK_SIZE = 4096;
      public HiveHeader Header { get; internal set; }

      public HiveBin[] Bins { get; internal set; }

      public void ReadFile(string path)
      {
         FileInfo fi = new FileInfo(path);
         using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open))
         {
            using (MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor(0, BLOCK_SIZE))
            {
               HiveHeaderInternal obj;
               mmva.Read(0, out obj);

               Header = new HiveHeader(obj);
            }

            List<HiveBin> bins = new List<HiveBin>();

            for (int i = BLOCK_SIZE; i < fi.Length - BLOCK_SIZE; i += BLOCK_SIZE)
            {
               using (MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor(i, BLOCK_SIZE))
               {
                  HiveBinInternal obj;
                  mmva.Read(0, out obj);

                  HiveBin hb = new HiveBin(obj);

                  // malformed/empty block
                  if (hb.Magic != "hbin") continue;

                  List<HiveCell> cells = new List<HiveCell>();

                  int cellStart = Marshal.SizeOf(obj);
                  for (int j = cellStart; j < BLOCK_SIZE - cellStart;)
                  {
                     int sizeRecord = mmva.ReadInt32(j);
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