using System;
using System.Text;
using LibHive.Cells;
using LibHive.Structures;

namespace LibHive
{
   public class HiveBin
   {
      private HiveBinStructure _hbs;

      internal HiveBin(HiveBinStructure hbs)
      {
         _hbs = hbs;
      }

      public unsafe string Magic
      {
         get
         {
            fixed (byte* magic = _hbs.magic)
            {
               return new string((sbyte*)magic, 0, 4, Encoding.ASCII);
            }
         }
      }

      public uint FileOffset => _hbs.file_offset;
      public uint Size => _hbs.size;
      public DateTime LastModified => DateTime.FromFileTime((long)_hbs.timestamp);

      public HiveCell[] Cells { get; internal set; }
   }
}