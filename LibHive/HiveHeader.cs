using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LibHive
{
   public struct HiveHeader
   {
      internal HiveHeader(HiveHeaderInternal hhi)
      {
         unsafe
         {
            Magic = new string((sbyte*)hhi.magic, 0, 4, Encoding.ASCII);
            FileName = new string((sbyte*)hhi.filename, 0, 64, Encoding.Unicode);
         }

         IsWellWritten = hhi.sequence_one == hhi.sequence_two;
         LastModified = DateTime.FromFileTime((long)hhi.timestamp);
         FormatVersion = new Version((int)hhi.major_version, (int)hhi.minor_version);
         Type = (HiveHeaderFileType)hhi.type;
         RootCell = new HiveCellIndex(hhi.rootcell);
         Length = hhi.length;
         Cluster = hhi.cluster;
      }

      public string Magic { get; set; }
      public bool IsWellWritten { get; set; }
      public DateTime LastModified { get; set; }
      public Version FormatVersion { get; set; }
      public HiveHeaderFileType Type { get; set; }
      public HiveCellIndex RootCell { get; set; }
      public uint Length { get; set; }
      public uint Cluster { get; set; }
      public string FileName { get; set; }
   }

   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
   internal unsafe struct HiveHeaderInternal
   {
      public fixed byte magic [4];

      public uint sequence_one;
      public uint sequence_two;

      public ulong timestamp;

      public uint major_version;
      public uint minor_version;

      public uint type;
      public uint format;

      public uint rootcell;

      public uint length;
      public uint cluster;

      public fixed byte filename [64];
   }

   public enum HiveHeaderFileType : uint
   {
      Primary = 0,
      Alternate = 1,
      Log = 2,
      External = 3,
      Max = 4
   }

   public enum HiveCellIndexType
   {
      Stable = 0,
      Volatile = 1
   }
}