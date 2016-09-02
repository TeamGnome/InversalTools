using System;
using System.Text;
using LibHive.Cells;
using LibHive.Structures;

namespace LibHive
{
   public struct HiveHeader
   {
      private HiveHeaderStructure _hhi;

      internal HiveHeader(HiveHeaderStructure hhi)
      {
         _hhi = hhi;
         RootCell = new HiveCellIndex(hhi.rootcell);
      }

      public unsafe string Magic
      {
         get
         {
            fixed (byte* magic = _hhi.magic)
            {
               return new string((sbyte*)magic, 0, 4, Encoding.ASCII);
            }
         }
      }

      public unsafe string FileName
      {
         get
         {
            fixed (byte* filename = _hhi.filename)
            {
               return new string((sbyte*)filename, 0, 64, Encoding.Unicode);
            }
         }
      }

      public bool IsWellWritten => _hhi.sequence_one == _hhi.sequence_two;
      public DateTime LastModified => DateTime.FromFileTime((long)_hhi.timestamp);
      public Version FormatVersion => new Version((int)_hhi.major_version, (int)_hhi.minor_version);
      public HiveHeaderFileType Type => (HiveHeaderFileType)_hhi.type;
      public HiveCellIndex RootCell { get; set; }
      public uint Length => _hhi.length;
      public uint Cluster => _hhi.cluster;
      public uint CheckSum => _hhi.checksum;
      public HiveHeaderBootType BootType => (HiveHeaderBootType)_hhi.boot_type;
      public bool BootRecovered => _hhi.boot_recover > 0;
   }
}