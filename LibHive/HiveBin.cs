using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LibHive
{
   public class HiveBin
   {
      public string Magic { get; set; }
      public uint FileOffset { get; set; }
      public uint Size { get; set; }
      public DateTime LastModified { get; set; }

      public HiveCell[] Cells { get; set; }

      internal HiveBin(HiveBinInternal hbi)
      {
         unsafe
         {
            Magic = new string((sbyte*)hbi.magic, 0, 4, Encoding.ASCII);
         }

         FileOffset = hbi.file_offset;
         Size = hbi.size;
         LastModified = DateTime.FromFileTime((long)hbi.timestamp);
      }
   }

   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
   internal unsafe struct HiveBinInternal
   {
      public fixed byte magic [4];

      public uint file_offset;
      public uint size;

      public uint free_space;
      public uint free_list;

      public ulong timestamp;

      public uint memory_alloc;
   }

   public struct HiveCellIndex
   {
      public HiveCellIndex(uint cellIndex)
      {
         Type = (HiveCellIndexType)((cellIndex & 0x80000000) >> 31);
         Table = (int)((cellIndex & 0x7fe00000) >> 21);
         Block = (int)((cellIndex & 0x001ff000) >> 12);
         Offset = (int)(cellIndex & 0x00000fff);
      }

      public HiveCellIndexType Type { get; set; }
      public int Table { get; set; }
      public int Block { get; set; }
      public int Offset { get; set; }
   }
}