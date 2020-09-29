﻿using System;
using Cosette.Engine.Ai.Score;
using Cosette.Engine.Ai.Score.Evaluators;
using Cosette.Engine.Fen;

namespace Cosette.Interactive.Commands
{
    public class EvaluateCommand : ICommand
    {
        public string Description { get; }
        private InteractiveConsole _interactiveConsole;

        public EvaluateCommand(InteractiveConsole interactiveConsole)
        {
            _interactiveConsole = interactiveConsole;
            Description = "Evaluate the specified position";
        }

        public void Run(params string[] parameters)
        {
            var fen = string.Join(' ', parameters);
            var boardState = FenToBoard.Parse(fen);
            var evaluationStatistics = new EvaluationStatistics();

            var openingPhase = boardState.GetPhaseRatio();
            var endingPhase = 1 - openingPhase;

            var materialEvaluation = MaterialEvaluator.Evaluate(boardState);
            var castlingEvaluation = CastlingEvaluator.Evaluate(boardState, openingPhase, endingPhase);
            var positionEvaluation = PositionEvaluator.Evaluate(boardState, openingPhase, endingPhase);
            var pawnStructureEvaluation = PawnStructureEvaluator.Evaluate(boardState, evaluationStatistics, openingPhase, endingPhase);
            var mobility = MobilityEvaluator.Evaluate(boardState, openingPhase, endingPhase);
            var kingSafety = KingSafetyEvaluator.Evaluate(boardState, openingPhase, endingPhase);
            var pieces = PiecesEvaluator.Evaluate(boardState, openingPhase, endingPhase);

            var total = materialEvaluation + castlingEvaluation + positionEvaluation + pawnStructureEvaluation +
                        mobility + kingSafety + pieces;

            _interactiveConsole.WriteLine($"Evaluation for board with hash {boardState.Hash} (phase {openingPhase:F}, " +
                                          $"{boardState.IrreversibleMovesCount} irreversible moves)");

            _interactiveConsole.WriteLine($" = Material: {materialEvaluation}");
            _interactiveConsole.WriteLine($" = Castling: {castlingEvaluation}");
            _interactiveConsole.WriteLine($" = Position: {positionEvaluation}");
            _interactiveConsole.WriteLine($" = Pawns: {pawnStructureEvaluation}");
            _interactiveConsole.WriteLine($" = Mobility: {mobility}");
            _interactiveConsole.WriteLine($" = King safety: {kingSafety}");
            _interactiveConsole.WriteLine($" = Pieces evaluation: {pieces}");
            _interactiveConsole.WriteLine();
            _interactiveConsole.WriteLine($" = Total: {total}");
        }
    }
}