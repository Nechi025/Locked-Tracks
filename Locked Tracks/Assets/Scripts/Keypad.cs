using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    [SerializeField] private Text answerText;
    [SerializeField] private Text hintText; 
    [SerializeField] private Text password1Text;
    [SerializeField] private Text password2Text;
    [SerializeField] private Text password3Text;
    [SerializeField] private Text password4Text;
    [SerializeField] private Text password5Text;
    [SerializeField] private Text progressText; 
    [SerializeField] private Text feedbackText; 

    private List<string> passwords;
    private HashSet<string> guessedPasswords;
    private ColaTDA<int> colaNumeros;

    public CameraLook cl;
    public GameObject canvas;
    public QuickSortSolver quickSortSolver;

    private List<string> sortedHints; 
    private int currentHintIndex = 0; 

    private void Start()
    {
        colaNumeros = new QueueTDA();
        colaNumeros.InicializarCola();

        passwords = GenerateRandomPasswords(5);
        guessedPasswords = new HashSet<string>();


        password1Text.text = passwords[0];
        password2Text.text = passwords[1];
        password3Text.text = passwords[2];
        password4Text.text = passwords[3];
        password5Text.text = passwords[4];


        if (quickSortSolver != null)
        {
            sortedHints = quickSortSolver.GenerateHints(passwords);
        }
        else
        {
            Debug.LogError("QuickSortSolver is missing!");
            sortedHints = new List<string>();
        }

        UpdateProgressText();
        feedbackText.text = "";
    }


    public void Number(int num)
    {
        colaNumeros.Acolar(num);
        answerText.text += num.ToString();
    }



    public void Hint()
    {
        if (sortedHints == null || sortedHints.Count == 0)
        {
            hintText.text = "No hints available!";
            return;
        }

        hintText.text = sortedHints[currentHintIndex];

        currentHintIndex = (currentHintIndex + 1) % sortedHints.Count;
    }


    public void Enter()
    {
        string guessedPassword = GetGuessedPassword();

        // Clear the answerText after retrieving the guessed password
        answerText.text = "";

        if (guessedPasswords.Contains(guessedPassword))
        {
            feedbackText.text = "Already guessed";
            return;
        }

        if (!IsPasswordValid(guessedPassword))
        {
            feedbackText.text = "Invalid password";
            return;
        }

        if (IsCorrectPassword(guessedPassword))
        {
            guessedPasswords.Add(guessedPassword);
            UpdateProgressText();

            // Remove correctly guessed password from sortedHints
            string sortedGuessedPassword = quickSortSolver.SortDigits(guessedPassword);
            sortedHints.Remove(sortedGuessedPassword);

            if (guessedPasswords.Count == passwords.Count)
            {
                feedbackText.text = "All passwords correct";
                canvas.SetActive(false);
                cl.canMove = true;
                Cursor.lockState = CursorLockMode.None;
                ChangeScene.instance.LoadSceneByName("Level2Scene");
            }
            else
            {
                feedbackText.text = "Correct password";
            }
        }
        else
        {
            feedbackText.text = "Wrong Order";
        }
    }

    private string GetGuessedPassword()
    {
        string guessedPassword = "";

        while (!colaNumeros.ColaVacia())
        {
            guessedPassword += colaNumeros.Primero().ToString();
            colaNumeros.Desacolar();
        }

        return guessedPassword;
    }

    private bool IsPasswordValid(string guessedPassword)
    {
        return guessedPassword.Length == 5 && int.TryParse(guessedPassword, out _);
    }

    private bool IsCorrectPassword(string guessedPassword)
    {
        foreach (string password in passwords)
        {
            if (guessedPassword == password)
                return true;

            if (quickSortSolver.SortDigits(password) == quickSortSolver.SortDigits(guessedPassword))
                return false; 
        }

        return false;
    }

    private void UpdateProgressText()
    {
        progressText.text = $"{guessedPasswords.Count}/{passwords.Count}";
    }

    private List<string> GenerateRandomPasswords(int count)
    {
        HashSet<string> uniquePasswords = new HashSet<string>();
        System.Random random = new System.Random();

        while (uniquePasswords.Count < count)
        {
            string password = random.Next(10000, 100000).ToString(); 
            uniquePasswords.Add(password);
        }

        return new List<string>(uniquePasswords);
    }

    private void ClearQueue()
    {
        while (!colaNumeros.ColaVacia())
        {
            colaNumeros.Desacolar();
        }
    }

  
}
