using System;

namespace LibHive
{
   [Flags]
   public enum HiveHeaderBootType : uint
   {
      Normal = 0,
      Repair = 1,
      Backup = 2,
      SelfHeal = 4
   }
}