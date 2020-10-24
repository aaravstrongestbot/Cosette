﻿using System;
using Cosette.Engine.Ai.Score;
using Cosette.Engine.Common;
using Cosette.Engine.Moves;

namespace Cosette.Engine.Board.Operators
{
    public static class RookOperator
    {
        public static int GetAvailableMoves(BoardState boardState, int color, Span<Move> moves, int offset)
        {
            var enemyColor = ColorOperations.Invert(color);
            var rooks = boardState.Pieces[color][Piece.Rook];

            while (rooks != 0)
            {
                var piece = BitOperations.GetLsb(rooks);
                rooks = BitOperations.PopLsb(rooks);

                var from = BitOperations.BitScan(piece);
                var availableMoves = RookMovesGenerator.GetMoves(boardState.OccupancySummary, from) & ~boardState.Occupancy[color];

                while (availableMoves != 0)
                {
                    var field = BitOperations.GetLsb(availableMoves);
                    var fieldIndex = BitOperations.BitScan(field);
                    availableMoves = BitOperations.PopLsb(availableMoves);

                    var flags = (field & boardState.Occupancy[enemyColor]) != 0 ? MoveFlags.Capture : MoveFlags.Quiet;
                    moves[offset++] = new Move(from, fieldIndex, flags);
                }
            }

            return offset;
        }

        public static int GetAvailableQMoves(BoardState boardState, int color, Span<Move> moves, int offset)
        {
            var enemyColor = ColorOperations.Invert(color);
            var rooks = boardState.Pieces[color][Piece.Rook];

            while (rooks != 0)
            {
                var piece = BitOperations.GetLsb(rooks);
                rooks = BitOperations.PopLsb(rooks);

                var from = BitOperations.BitScan(piece);
                var availableMoves = RookMovesGenerator.GetMoves(boardState.OccupancySummary, from) & boardState.Occupancy[enemyColor];

                while (availableMoves != 0)
                {
                    var field = BitOperations.GetLsb(availableMoves);
                    var fieldIndex = BitOperations.BitScan(field);
                    availableMoves = BitOperations.PopLsb(availableMoves);

                    moves[offset++] = new Move(from, fieldIndex, MoveFlags.Capture);
                }
            }

            return offset;
        }

        public static int GetMobility(BoardState boardState, int color)
        {
            var centerMobility = 0;
            var extendedCenterMobility = 0;
            var outsideMobility = 0;

            var rooks = boardState.Pieces[color][Piece.Rook];

            while (rooks != 0)
            {
                var piece = BitOperations.GetLsb(rooks);
                rooks = BitOperations.PopLsb(rooks);

                var from = BitOperations.BitScan(piece);
                var availableMoves = RookMovesGenerator.GetMoves(boardState.OccupancySummary, from);

                centerMobility += (int)BitOperations.Count(availableMoves & EvaluationConstants.Center);
                extendedCenterMobility += (int)BitOperations.Count(availableMoves & EvaluationConstants.ExtendedCenter);
                outsideMobility += (int)BitOperations.Count(availableMoves & EvaluationConstants.Outside);
            }

            return EvaluationConstants.CenterMobilityModifier * centerMobility +
                   EvaluationConstants.ExtendedCenterMobilityModifier * extendedCenterMobility +
                   EvaluationConstants.OutsideMobilityModifier * outsideMobility;
        }
    }
}
