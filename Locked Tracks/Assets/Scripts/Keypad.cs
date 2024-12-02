using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    [SerializeField] private Text answerText; 
    [SerializeField] private Text password1Text; 
    [SerializeField] private Text password2Text; 
    [SerializeField] private Text password3Text;

    private List<string> passwords;
    private HashSet<string> guessedPasswords;
    private ColaTDA<int> colaNumeros; 
    public CameraLook cl;
    public GameObject canvas;

    public QuickSortSolver quickSortSolver;
    private List<string> sortedHints;
    [SerializeField] private Text hintText;
    private int currentHintIndex = 0;

    private void Start()
    {
        colaNumeros = new QueueTDA();
        colaNumeros.InicializarCola();

        passwords = GenerateRandomPasswords(3);
        guessedPasswords = new HashSet<string>();

        password1Text.text = passwords[0];
        password2Text.text = passwords[1];
        password3Text.text = passwords[2];

        if (quickSortSolver != null)
        {
            sortedHints = quickSortSolver.GenerateHints(passwords);
        }
        else
        {
            sortedHints = new List<string>();
        }

    }

    public void Number(int num)
    {
        colaNumeros.Acolar(num);
        answerText.text += num.ToString();
    }

    public void Enter()
    {
        string enteredPassword = GetEnteredPassword();

        if (enteredPassword == "")
        {
            Debug.Log("Nada");
            return;
        }

        if (passwords.Contains(enteredPassword) && !guessedPasswords.Contains(enteredPassword))
        {
            guessedPasswords.Add(enteredPassword);
            Debug.Log($"Correcto: {enteredPassword}");
            answerText.text = "";

            if (guessedPasswords.Count == passwords.Count)
            {
                
                Debug.Log("Respuesta Correctas!");
                canvas.SetActive(false);
                cl.canMove = true;
                Cursor.lockState = CursorLockMode.None;
                ChangeScene.instance.LoadSceneByName("Level2Scene");
            }
        }
        else
        {
            Debug.Log("Contraseña Incorrecta o Ya puesta!");
            answerText.text = ""; // Clear input for retry
        }

        ClearQueue(); // Clear the queue for new input
    }

    private string GetEnteredPassword()
    {
        string enteredPassword = "";

        while (!colaNumeros.ColaVacia())
        {
            enteredPassword += colaNumeros.Primero().ToString();
            colaNumeros.Desacolar();
        }

        return enteredPassword;
    }

    private void ClearQueue()
    {
        while (!colaNumeros.ColaVacia())
        {
            colaNumeros.Desacolar();
        }
    }

    private List<string> GenerateRandomPasswords(int count)
    {
        HashSet<string> uniquePasswords = new HashSet<string>();
        System.Random random = new System.Random();

        while (uniquePasswords.Count < count)
        {
            string password = random.Next(100, 1000).ToString();
            uniquePasswords.Add(password);
        }

        return new List<string>(uniquePasswords);
    }

    public void Hint()
    {
        if (sortedHints == null || sortedHints.Count == 0)
        {
            Debug.LogError("No hints available!");
            return;
        }


        hintText.text = sortedHints[currentHintIndex];


        currentHintIndex = (currentHintIndex + 1) % sortedHints.Count;
    }
}
