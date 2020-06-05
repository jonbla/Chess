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

    enum GameState
    {
        BlackTurn,
        WhiteTurn
    }
}
