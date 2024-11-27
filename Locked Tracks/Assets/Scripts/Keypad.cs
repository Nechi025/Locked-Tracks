using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    [SerializeField] private Text answerText;
    private string answer = "528";
    private ColaTDA<int> colaNumeros;
    public CameraLook cl;
    public GameObject canvas;
    public QuickSortSolver quickSortSolver; 

    private void Start()
    {
        colaNumeros = new QueueTDA();
        colaNumeros.InicializarCola();
    }

    public void Number(int num)
    {
        colaNumeros.Acolar(num);
        answerText.text += num.ToString();
    }

    public void Enter()
    {
        if (VerificarClave())
        {
            canvas.SetActive(false);
            cl.canMove = true;
            Cursor.lockState = CursorLockMode.None;
            GameManager.instance.TerminarPartida();
            //ChangeScene.instance.LoadSceneByName("Win");
        }
        else
        {
            answerText.text = "";
        }
    }

    private bool VerificarClave()
    {
        string claveIngresada = "";

        while (!colaNumeros.ColaVacia())
        {
            claveIngresada += colaNumeros.Primero().ToString();
            colaNumeros.Desacolar();
        }

        return claveIngresada == answer;
    }

    public void AutoSolve()
    {
        if (quickSortSolver == null || answerText == null)
        {
            Debug.LogError("QuickSortSolver or AnswerText is not assigned!");
            return;
        }

        
        quickSortSolver.Solve(answer, answerText);
    }
}
