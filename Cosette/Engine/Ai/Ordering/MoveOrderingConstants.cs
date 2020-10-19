﻿namespace Cosette.Engine.Ai.Ordering
{
    public static class MoveOrderingConstants
    {
        public const int HashMove = 10000;
        public const int Promotion = 5000;
        public const int Castling = 1000;
        public const int PawnNearPromotion = 150;
        public const int Capture = 100;
        public const int EnPassant = 100;
        public const int KillerMove = 90;
        public const int HistoryHeuristicMaxScore = 80;

        public const int KillerSlots = 3;
    }
}
