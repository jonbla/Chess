using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public class Chess_Piece : MonoBehaviour
{
    Piece_Middle_manager middleMan; // handles piece specific functions

    private bool mouseIsClicked = false; //used for making the piece move while mouse is being held down

    public ColInfo CollisionInfo; //3 flags describing collition status
    Vector3 startPos; //used for determining offset
   

    public Vector2Int lastMove; //last move in chessboard units

    public bool isDead = false;

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
            MovePiece();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //Centre the piece in nearest cell 
    void CenterPiece()
    {
        Grid grid = transform.parent.parent.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.LocalToCell(transform.localPosition);
        transform.localPosition = grid.GetCellCenterLocal(cellPosition);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
    }

    //check if piece is on the board
    bool isInBounds()
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

    //employ various constraints to check if the move just made was valid
    bool isValidMove()
    {

        //is the piece on the board?
        if (!isInBounds())
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

        lastMove = Coord_Manager.GetPositionDifference(transform.name, transform.localPosition);
        //is the piece making a move that particular piece is able to do?
        if (!middleMan.IsPieceSpecificMoveValid())
        {
            return false;
        }

        if (team.isInCheck)
        {
            Feedback.SetText("King can be attacked there");
            return false;
        }

        return true;
    }

    //function called by other pieces when this piece is being killed
    public void getKilled()
    {
        Coord_Manager.KillPiece(transform.name);
        isDead = true;
        transform.localPosition = new Vector3(-5.22f, transform.localPosition.y, transform.localPosition.z);
    }

    //Move the piece with the mouse
    void MovePiece()
    {
        mouseIsClicked = true;
        Vector2 temp = Mouse_Manager.getMouseDelta();
        transform.position = new Vector3(transform.position.x + temp.x, transform.position.y + temp.y, -1);
    }

    //Drop piece into closest cell and reset mouse delta
    void DropPiece()
    {
        print("timestart: "+Time.time);
        mouseIsClicked = false;
        Mouse_Manager.resetMouseDelta();
        CenterPiece();
        Coord_Manager.UpdatePosition(transform.name, transform.localPosition);
        if (!isValidMove())
        {
            transform.position = startPos;
        }
        else
        {            
            Coord_Manager.CommitPositionUpdate(transform.name, transform.localPosition);
            team.EndTurn();
        }
        print("timeend: " + Time.time);
    }

    //This is an event sent to piece affected
    //mouse has just been pressed down, begin moving the piece
    private void OnMouseDown()
    {
        
        startPos = transform.position;
        MovePiece();
    }

    //The mouse has just been lifted, drop the piece into the closest cell
    private void OnMouseUp()
    {
        DropPiece();
    }
}
