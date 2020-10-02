﻿using System;

namespace Cosette.Engine.Ai.Transposition
{
    public static class PawnHashTable
    {
        private static PawnHashTableEntry[] _table;
        private static ulong _size;

        public static unsafe void Init(ulong sizeMegabytes)
        {
            var entrySize = sizeof(PawnHashTableEntry);

            _size = sizeMegabytes * 1024ul * 1024ul / (ulong)entrySize;
            _table = new PawnHashTableEntry[_size];
        }

        public static void Add(ulong hash, short score)
        {
            _table[hash % _size] = new PawnHashTableEntry(hash, score);
        }

        public static PawnHashTableEntry Get(ulong hash)
        {
            return _table[hash % _size];
        }

        public static float GetFillLevel()
        {
            var filledEntries = 0;
            for (var i = 0; i < 1000; i++)
            {
                if (_table[i].Key != 0 || _table[i].Score != 0)
                {
                    filledEntries++;
                }
            }

            return (float)filledEntries / 10;
        }

        public static void Clear()
        {
            Array.Clear(_table, 0, (int)_size);
        }
    }
}
