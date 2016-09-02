using System.Runtime.InteropServices;

namespace LibHive.Structures
{
   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
   internal unsafe struct HiveHeaderStructure
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

      public fixed uint reserved_one [99];

      public uint checksum;

      public fixed uint reserved_two [894];

      public uint boot_type;
      public uint boot_recover;
   }
}