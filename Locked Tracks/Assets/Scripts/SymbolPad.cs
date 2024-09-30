using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolPad : MonoBehaviour
{
    public SymbolPiece[] pieces;
    public CameraLook cl;
    public GameObject canvas;
    bool isFinished = false;

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {
            int correctPieces = 0;
            foreach (var piece in pieces)
            {
                if (piece.isCorrect())
                {
                    correctPieces++;
                }
            }

            if (correctPieces == pieces.Length)
            {
                isFinished = true;
                canvas.SetActive(false);
                cl.canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
    }
}
