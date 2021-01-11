using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtraChessStructures;
using System;

public static class Move_Generator
{
    /// <summary>
    /// Generate all legal moves on the board 
    /// </summary>
    /// <param name="board">Board to use</param>
    /// <param name="LookForBlackPieces">What team's turn is it?</param>
    public static List<Move> Generate(Transform[,] board, bool LookForBlackPieces)
    {
        List<Move> moves = new List<Move>();
        foreach (Transform piece in board)
        {
            try
            {
                Chess_Piece chess_Piece = piece.GetComponent<Chess_Piece>();
                if (chess_Piece.GetIsBlack() == LookForBlackPieces)
                {
                    foreach (Move move in chess_Piece.GenerateAllValidMoves())
                    {
                        moves.Add(move);
                    }
                }
            }
            catch (NullReferenceException) { }
        }
        return moves;
    }
}
