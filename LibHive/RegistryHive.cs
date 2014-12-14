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
                    HiveHeaderInternal obj = new HiveHeaderInternal();
                    mmva.Read<HiveHeaderInternal>(0, out obj);

                    Header = new HiveHeader(obj);
                }

                List<HiveBin> Bins = new List<HiveBin>();

                for (int i = BLOCK_SIZE; i < (fi.Length - BLOCK_SIZE); i += BLOCK_SIZE)
                {
                    using (MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor(i, BLOCK_SIZE))
                    {
                        HiveBinInternal obj = new HiveBinInternal();
                        mmva.Read<HiveBinInternal>(0, out obj);

                        HiveBin hb = new HiveBin(obj);
                        List<HiveCell> Cells = new List<HiveCell>();

                        int cellStart = Marshal.SizeOf(obj);
                        for (int j = cellStart; j < (BLOCK_SIZE - cellStart);)
                        {
                            int sizeRecord = mmva.ReadInt32(j);
                            HiveCell hc = new HiveCell()
                            {
                                Size = Math.Abs(sizeRecord),
                                Allocated = sizeRecord < 0
                            };

                            Cells.Add(hc);

                            j += hc.Size;
                        }

                        hb.Cells = Cells.ToArray();
                        Bins.Add(hb);
                    }
                }

                this.Bins = Bins.ToArray();
            }
        }
    }
}
