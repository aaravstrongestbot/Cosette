﻿using Cosette.Engine.Moves;

namespace Cosette.Engine.Ai
{
    public class SearchStatistics
    {
        public int Depth { get; set; }
        public int Score { get; set; }
        public ulong Leafs { get; set; }
        public ulong Nodes { get; set; }
        public ulong SearchTime { get; set; }
        public ulong NodesPerSecond { get; set; }
        public int BranchingFactor { get; set; }
        public ulong BetaCutoffs { get; set; }
        public ulong TTHits { get; set; }
        public ulong TTCollisions { get; set; }
        public Move BestMove { get; set; }

        public void Clear()
        {
            Depth = 0;
            Score = 0;
            Leafs = 0;
            Nodes = 0;
            SearchTime = 0;
            NodesPerSecond = 0;
            BranchingFactor = 0;
            BetaCutoffs = 0;
            TTHits = 0;
            TTCollisions = 0;
            BestMove = new Move();
        }
    }
}
