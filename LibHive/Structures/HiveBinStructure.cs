using System.Runtime.InteropServices;

namespace LibHive.Structures
{
   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
   internal unsafe struct HiveBinStructure
   {
      public fixed byte magic [4];

      public uint file_offset;
      public uint size;

      public uint free_space;
      public uint free_list;

      public ulong timestamp;

      public uint memory_alloc;
   }
}