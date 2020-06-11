using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtraChessStructures
{
    public struct ColInfo
    {
        public bool isColliding;
        public bool isCollidingWithOwnTeam;
        public bool isCollidingWithKing;

        public ColInfo(bool col, bool colTeam, bool colKing)
        {
            isColliding = col;
            isCollidingWithOwnTeam = colTeam;
            isCollidingWithKing = colKing;
        }
        
    }

    public struct CheckFlags
    {
        public bool isInCheck;
        public bool isInCheckmate;

        public CheckFlags(bool isInCheck, bool isInCheckmate)
        {
            this.isInCheck = isInCheck;
            this.isInCheckmate = isInCheckmate;
        }
    }

    enum GameState
    {
        BlackTurn,
        WhiteTurn
    }
}
