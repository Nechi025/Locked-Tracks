using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle15 : MonoBehaviour
{
    [SerializeField] Transform emptySpace;
    private Camera _camera;
    public GamePiece[] gamePieces;
    public PanelPuzzle gameBoard;
    public CameraLook cl;
    public GameObject canvas;
    bool _shuffle = true;
    int emptySpaceIndex = 15;
    
    private bool isFinished = false;

    private PilaTDA<Movimiento> stackTDA;

    [SerializeField] Door _door;

    private void Start()
    {
        _camera = Camera.main;
        stackTDA = new StackTDA();
        stackTDA.InicializarPila();
    }

    private void Update()
    {
        if (gameBoard.isInteracting)
        {
            if (_shuffle)
            {
                Shuffle();
                _shuffle = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Tiro un raycast desde el mouse para averiguar cual ficha se esta tocando
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                //Si la pieza alcanzada esta al lado del espacio vacio
                if (Vector3.Distance(emptySpace.position, hit.transform.position) < 0.1f)
                {
                    //Guardo el movimiento y lo apilo
                    Movimiento movimiento = new Movimiento
                    {
                        piezaMovida = hit.transform.GetComponent<GamePiece>(),
                        posicionAnterior = hit.transform.position,
                        posicionEspacioVacio = emptySpace.position
                    };
                    stackTDA.Apilar(movimiento);

                    //Mueve la pieza
                    Vector3 lastEmptySpacePosition = emptySpace.position;
                    GamePiece thisPiece = hit.transform.GetComponent<GamePiece>();
                    emptySpace.position = thisPiece.targetPosition;
                    thisPiece.targetPosition = lastEmptySpacePosition;

                    int pieceIndex = findIndex(thisPiece);
                    gamePieces[emptySpaceIndex] = gamePieces[pieceIndex];
                    gamePieces[pieceIndex] = null;
                    emptySpaceIndex = pieceIndex;
                }
            }
        }

        if (!isFinished)
        {
            int correctPieces = 0;
            foreach (var a in gamePieces)
            {
                if (a != null)
                {
                    if (a.inRightPlace)
                    {
                        correctPieces++;
                    }
                }
            }

            if (correctPieces == gamePieces.Length - 1)
            {
                isFinished = true;
                canvas.SetActive(false);
                cl.canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else _door.DoorOpen();

    }

    public void Shuffle()
    {
        if (emptySpaceIndex != 15)
        {
            var pieceOn15LastPos = gamePieces[15].targetPosition;
            gamePieces[15].targetPosition = emptySpace.position;
            emptySpace.position = pieceOn15LastPos;
            gamePieces[emptySpaceIndex] = gamePieces[15];
            gamePieces[15] = null;
            emptySpaceIndex = 15;
        }

        int invertion;
        do
        {
            for (int i = 0; i <= 14; i++)
            {

                if (gamePieces[i] != null)
                {
                    Debug.Log("a");
                    var lastPos = gamePieces[i].targetPosition;
                    int randomIndex = Random.Range(0, 14);
                    gamePieces[i].targetPosition = gamePieces[randomIndex].targetPosition;
                    gamePieces[randomIndex].targetPosition = lastPos;

                    var piece = gamePieces[i];
                    gamePieces[i] = gamePieces[randomIndex];
                    gamePieces[randomIndex] = piece;
                }

            }

            invertion = GetInversions();
            Debug.Log("mezcla");
        }while(invertion % 2 != 0);
    }

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < gamePieces.Length; i++)
        {
            int thisPieceInvertion = 0;
            for (int j = i; j < gamePieces.Length; j++)
            {
                if (gamePieces[j] != null)
                {
                    if (gamePieces[i].number > gamePieces[j].number)
                    {
                        thisPieceInvertion++;
                    }
                }
            }
            inversionsSum += thisPieceInvertion;
        }
        return inversionsSum;
    }

    public int findIndex(GamePiece ts)
    {
        for (int i = 0; i < gamePieces.Length; i++)
        {
            if (gamePieces[i] != null)
            {
                if (gamePieces[i] == ts)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public void RevertirMovimiento()
    {
        if (!stackTDA.PilaVacia())
        {
            //Saco el ultimo movimiento
            Movimiento ultimoMovimiento = stackTDA.Tope();
            stackTDA.Desapilar();

            //Restauro las posiciones
            ultimoMovimiento.piezaMovida.targetPosition = ultimoMovimiento.posicionAnterior;
            emptySpace.position = ultimoMovimiento.posicionEspacioVacio;
        }
    }

}
