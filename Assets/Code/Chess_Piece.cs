using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Universal Chess Piece
/// </summary>
public class Chess_Piece : MonoBehaviour
{
    /// <summary>
    /// Handles piece specific functions
    /// </summary>
    Piece_Middle_manager middleMan;

    /// <summary>
    /// Used for moving the piece while mouse is being held down
    /// </summary>
    private bool mouseIsClicked = false;

    /// <summary>
    /// Struct describing collition status
    /// </summary>
    public ColInfo CollisionInfo;

    /// <summary>
    /// Used for determining offset
    /// </summary>
    Vector3 startPos;

    /// <summary>
    /// Last move in chessboard units
    /// </summary>
    public Vector2Int lastMove; 

    /// <summary>
    /// Flag showing if this piece is in play or dead
    /// </summary>
    public bool isDead = false;

    /// <summary>
    /// Reference to TeamManager class
    /// </summary>
    Team_Manager team;



    // Start is called before the first frame update
    void Start()
    {
        middleMan = transform.GetComponent<Piece_Middle_manager>();
        team = transform.parent.GetComponent<Team_Manager>();

        //On start, make sure all the pieces are center
        CenterPiece();
    }

    // Update is called once per frame
    void Update()
    {
        //Keep running this function while mouse is being held down
        if (mouseIsClicked)
        {
            MovePieceWithMouse();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Locate the nearest cell and centre the piece into it
    /// </summary>
    void CenterPiece()
    {
        Grid grid = transform.parent.parent.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.LocalToCell(transform.localPosition);
        transform.localPosition = grid.GetCellCenterLocal(cellPosition);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
    }

    /// <summary>
    /// Checks if piece is in playable area
    /// </summary>
    /// <returns>True if on board, False otherwise</returns>
    bool IsInBounds()
    {
        if(Mathf.Abs(transform.localPosition.x) > 4.1 || Mathf.Abs(transform.localPosition.y) > 4.1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Checks if the last move was valid
    /// </summary>
    /// <returns>Validity of last move</returns>
    bool IsValidMove()
    {

        //is the piece on the board?
        if (!IsInBounds())
        {
            Feedback.SetText("Out of Bounds");
            return false;
        }

        //is it your turn to go?
        if (!team.hasTurn)
        {
            Feedback.SetText("Not your turn");
            return false;
        }

        CollisionInfo = Coord_Manager.CheckCollition(transform);

        bool isColliding = CollisionInfo.isColliding;
        bool isCollidingWithOwnTeam = CollisionInfo.isCollidingWithOwnTeam;

        //is the piece trying to kill it's own team?
        if (isCollidingWithOwnTeam)
        {
            Feedback.SetText("Can't kill your own piece");
            return false;
        }

        lastMove = Coord_Manager.GetPositionDifference();

        if(lastMove == Vector2Int.zero)
        {
            Feedback.SetText("Opps, dropped your piece");
            return false;
        }

        //is the piece making a move that particular piece is able to do?
        if (!middleMan.IsPieceSpecificMoveValid())
        {
            return false;
        }

        if (team.IsInCheck)
        {
            Feedback.SetText("King can be attacked there");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Function called by other pieces when this piece is being killed
    /// </summary>
    public void GetKilled()
    {
        Coord_Manager.KillPiece(transform.name);
        isDead = true;
        transform.localPosition = new Vector3(-5.22f, transform.localPosition.y, transform.localPosition.z);
    }

    /// <summary>
    /// Move the piece with the mouse
    /// </summary>
    void MovePieceWithMouse()
    {
        mouseIsClicked = true;
        Vector2 temp = Mouse_Manager.GetMouseDelta();
        transform.position = new Vector3(transform.position.x + temp.x, transform.position.y + temp.y, -1);
    }

    /// <summary>
    /// End piece move, calculate move made and execute it
    /// </summary>
    void DropPiece()
    {
        print("timestart: "+Time.time);
        mouseIsClicked = false;
        Mouse_Manager.ResetMouseDelta();
        CenterPiece();
        Coord_Manager.UpdatePosition(transform.name, transform.localPosition);
        if (!IsValidMove())
        {
            transform.position = startPos;
            Coord_Manager.RevertMove();
        }
        else
        {            
            Coord_Manager.CommitPositionUpdate(transform.name, transform.localPosition);
            team.EndTurn();
        }
        print("timeend: " + Time.time);
    }

    /// <summary>
    /// Event called when mouse is down
    /// </summary>
    private void OnMouseDown()
    {
        
        startPos = transform.position;
        MovePieceWithMouse();
    }

    /// <summary>
    /// Event called when mouse is let go
    /// </summary>
    private void OnMouseUp()
    {
        DropPiece();
    }
}
