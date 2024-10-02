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
            ChangeScene.instance.LoadSceneByName("Win");
        }
        else
        {
            answerText.text = "";
            //PlaySound Error
        }
    }

    private bool VerificarClave()
    {
        string claveIngresada = "";

        while (!colaNumeros.ColaVacia())  // Recorre los números en la cola
        {
            claveIngresada += colaNumeros.Primero().ToString();
            colaNumeros.Desacolar();  // Desacola cada número
        }

        return claveIngresada == answer;  // Verifica si la secuencia es igual a la respuesta
    }

    private void LimpiarCola()
    {
        while (!colaNumeros.ColaVacia())
        {
            colaNumeros.Desacolar();
        }
    }
}
