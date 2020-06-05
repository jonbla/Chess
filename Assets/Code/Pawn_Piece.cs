﻿using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public class Pawn_Piece : Custom_Mono
{
    bool firstMoveDone = false;

    //is this a valid move for a pawn?
    public bool isValidPawnMove(bool isBlack = false)
    {
        Vector2Int lastMove = CP.lastMove;
        ColInfo flags = CP.CollisionInfo;

        if (isBlack)
        {
            lastMove = new Vector2Int(lastMove.x, lastMove.y * -1);
        }

        if(lastMove.x != 0) //if there was a horizontal move
        {
            if(Mathf.Abs(lastMove.x) == 1 && lastMove.y == 1) //was it one of (1,1) or (-1,1)?
            {
                Debug.Log("pass1");
                if(!flags.isCollidingWithKing) //make sure you didn't try to kill a king
                {
                    Debug.Log("pass2");
                    if(flags.isColliding) //make sure you actually are colliding with something
                    {
                        Debug.Log("pass3");
                        KillAtLocation(); //kill piece colliding with
                        firstMoveDone = true;
                        return true; //if all these were true, then the move is valid
                    }
                }
            }
            Debug.Log("fail1");
            return false; //if there was a horizontal move and any of these conditions are false, then this move was invalid
        }

        if(lastMove.y >= 2 || lastMove.y < 0) //if you tried to move more than 1 space forward
        {
            if(lastMove.y == 2 && !firstMoveDone) //is this you first move?
            {
                firstMoveDone = true;
                return true; //if so, then this move is valid
            }
            Debug.Log("fail2");
            return false; //otherwise it isn't
        }

        if(lastMove.y == 1 && flags.isColliding) //can't move forward when there is a piece infront of you as a pawn
        {
            Debug.Log("fail3");
            return false;
        }

        firstMoveDone = true;
        return true; //all other moves are fine
    }
}