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

        public ColInfo(bool col, bool colTeam, bool colKing)
        {
            isColliding = col;
            isCollidingWithOwnTeam = colTeam;
            isCollidingWithKing = colKing;
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

    /// <summary>
    /// Indicates the current state of the game
    /// </summary>
    enum GameState
    {
        BlackTurn,
        WhiteTurn
    }
}
