using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolPiece : MonoBehaviour
{

    [SerializeField] int correctAnswer;
    private int answer = 1;
    public Animator anim;
    public void ChangeSymbol()
    {
        if (answer == 3)
        {
            answer = 1;
            anim.SetInteger("Number", 1);
            Debug.Log(anim.GetInteger("Number"));
        }
        else if (answer == 1)
        {  
            answer = 2;
            anim.SetInteger("Number", 2);
            Debug.Log(anim.GetInteger("Number"));
        }
        else
        {
            answer = 3;
            anim.SetInteger("Number", 3);
            Debug.Log(anim.GetInteger("Number"));
        }
    }

    public bool isCorrect()
    {
        if (answer == correctAnswer)
        {
            return true;
        }
        else return false;
    }
}
