using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_Mono : MonoBehaviour
{
    [HideInInspector]
    public Chess_Piece CP;

    // Start is called before the first frame update
    void Start()
    {
        CP = transform.GetComponent<Chess_Piece>();
    }

    public void KillAtLocation()
    {
        bool isBlack = GetIsBlack();

        string targetName = Coord_Manager.GetNameAt(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
        //Coord_Manager.getTransformAt()
        if (isBlack)
        {
            transform.parent.parent.Find("White").Find(targetName).GetComponent<Chess_Piece>().getKilled();
        }
        else
        {
            transform.parent.parent.Find("Black").Find(targetName).GetComponent<Chess_Piece>().getKilled();
        }//
        
    }

    public bool GetIsBlack()
    {
        bool isBlack = false;
        if (transform.parent.name == "Black")
        {
            isBlack = true;
        }
        return isBlack;
    }
}
