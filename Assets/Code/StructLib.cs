using UnityEngine;

/// <summary>
/// Structs some classes may use
/// </summary>
namespace ExtraChessStructures
{
    /// <summary>
    /// Collision info
    /// </summary>
    public struct ColInfo
    {
        /// <summary>
        /// Indicates if piece is colliding with another piece
        /// </summary>
        public bool isColliding;

        /// <summary>
        /// Indicates if piece is colliding with another piece of the same team
        /// </summary>
        public bool isCollidingWithOwnTeam;

        /// <summary>
        /// Indicates if piece is colliding with a King
        /// </summary>
        public bool isCollidingWithKing;

        /// <summary>
        /// Name of piece object is colliding with
        /// </summary>
        public string nameOfColObject;


        public ColInfo(bool col, bool colTeam, bool colKing)
        {
            isColliding = col;
            isCollidingWithOwnTeam = colTeam;
            isCollidingWithKing = colKing;
            nameOfColObject = "";
        }

    }

    /// <summary>
    /// Struct reprisenting info about Kings mate status
    /// </summary>
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

    public struct Move
    {
        public string name;
        public string type;
        public Vector2Int newMoveOffset;
        public Vector2Int finalPos;
        public Vector2 finalPosRaw;

        public Move(string name, string type, Vector2Int newMoveOffset, Vector2 finalPosRaw)
        {
            this.name = name;
            this.type = type;
            this.newMoveOffset = newMoveOffset;
            this.finalPos = Coord_Manager.ConvertCoordsToChessUnits(finalPosRaw);
            this.finalPosRaw = finalPosRaw;
        }

        public Move(string name, string type, Vector2Int newMoveOffset, Vector2Int finalPos)
        {
            this.name = name;
            this.type = type;
            this.newMoveOffset = newMoveOffset;
            this.finalPos = finalPos;
            this.finalPosRaw = Coord_Manager.ConvertChessUnitsToCoords(finalPos);
        }
    }

    /// <summary>
    /// Indicates the current state of the game
    /// </summary>
    enum GameState
    {
        BlackTurn,
        WhiteTurn
    }
}
