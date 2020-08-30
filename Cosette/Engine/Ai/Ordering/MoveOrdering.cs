﻿using System;
using Cosette.Engine.Ai.Ordering;
using Cosette.Engine.Ai.Score;
using Cosette.Engine.Ai.Transposition;
using Cosette.Engine.Board;
using Cosette.Engine.Common;
using Cosette.Engine.Moves;

namespace Cosette.Engine.Ai.Search
{
    public static class MoveOrdering
    {
        public static void AssignValues(BoardState board, Span<Move> moves, Span<int> moveValues, int movesCount, TranspositionTableEntry entry)
        {
            var enemyColor = ColorOperations.Invert(board.ColorToMove);
            for (var i = 0; i < movesCount; i++)
            {
                if (entry.Type != TranspositionTableEntryType.Invalid && entry.BestMove == moves[i])
                {
                    moveValues[i] = MoveOrderingConstants.HashMove;
                }
                else if ((moves[i].Flags & MoveFlags.Kill) != 0)
                {
                    var attackingPiece = moves[i].Piece;
                    var capturedPiece = board.GetPiece(enemyColor, moves[i].To);

                    var attackers = board.GetAttackingPiecesWithColor(enemyColor, moves[i].To);
                    var defenders = board.GetAttackingPiecesWithColor(board.ColorToMove, moves[i].To);
                    moveValues[i] = StaticExchangeEvaluation.Evaluate(attackingPiece, capturedPiece, attackers, defenders);

                    //moveValues[i] = EvaluationConstants.Pieces[(int) capturedPiece] - EvaluationConstants.Pieces[(int) attackingPiece];
                    //moveValues[i] = board.GetAttackingPiecesWithColor(enemyColor, moves[i].To);
                }
                else if ((int)moves[i].Flags >= 16)
                {
                    moveValues[i] = MoveOrderingConstants.Promotion;
                }
            }
        }

        public static void SortNextBestMove(Span<Move> moves, Span<int> moveValues, int movesCount, int currentIndex)
        {
            var max = int.MinValue;
            var maxIndex = -1;

            for (var i = currentIndex; i < movesCount; i++)
            {
                if (moveValues[i] > max)
                {
                    max = moveValues[i];
                    maxIndex = i;
                }
            }

            var tempMove = moves[maxIndex];
            moves[maxIndex] = moves[currentIndex];
            moves[currentIndex] = tempMove;

            var tempMoveValue = moveValues[maxIndex];
            moveValues[maxIndex] = moveValues[currentIndex];
            moveValues[currentIndex] = tempMoveValue;
        }
    }
}
