using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolPiece : MonoBehaviour
{
    /*public Image symbol;
    public Image triangle;
    public Image circle;
    public Image square;*/
    [SerializeField] int correctAnswer;
    private int answer = 1;
    public Text _text;

    public void ChangeSymbol()
    {
        if (answer == 3)
        {
            //symbol = triangle;
            _text.text = "triangle";
            answer = 1;
        }
        else if (answer == 1)
        {
            //symbol = circle;
            _text.text = "circle";
            answer = 2;
        }
        else
        {
            //symbol = square;
            _text.text = "square";
            answer = 3;
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
