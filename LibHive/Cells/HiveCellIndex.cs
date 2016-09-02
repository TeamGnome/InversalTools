namespace LibHive.Cells
{
   public class HiveCellIndex
   {
      private readonly uint _cellIndex;

      public HiveCellIndexType Type => (HiveCellIndexType)((_cellIndex & 0x80000000) >> 31);
      public int Table => (int)((_cellIndex & 0x7fe00000) >> 21);
      public int Block => (int)((_cellIndex & 0x001ff000) >> 12);
      public int Offset => (int)(_cellIndex & 0x00000fff);

      public HiveCellIndex(uint cellIndex)
      {
         _cellIndex = cellIndex;
      }
   }
}