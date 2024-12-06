using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSortSolver : MonoBehaviour
{
    public List<string> GenerateHints(List<string> passwords)
    {
        List<string> sortedPasswords = new List<string>();

        foreach (string password in passwords)
        {
            string sortedPassword = SortDigits(password);
            sortedPasswords.Add(sortedPassword);
        }

        QuickSort(sortedPasswords, 0, sortedPasswords.Count - 1);

        return sortedPasswords;
    }

    public string SortDigits(string input)
    {
        char[] digits = input.ToCharArray();
        QuickSort(digits, 0, digits.Length - 1);
        return new string(digits);
    }


    private void QuickSort(char[] array, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(array, low, high);
            QuickSort(array, low, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, high);
        }
    }

    private int Partition(char[] array, int low, int high)
    {
        char pivot = array[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (array[j] < pivot)
            {
                i++;
                Swap(array, i, j);
            }
        }

        Swap(array, i + 1, high);
        return i + 1;
    }

    private void Swap(char[] array, int i, int j)
    {
        char temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    private void QuickSort(List<string> list, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(list, low, high);
            QuickSort(list, low, pivotIndex - 1);
            QuickSort(list, pivotIndex + 1, high);
        }
    }

    private int Partition(List<string> list, int low, int high)
    {
        string pivot = list[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (string.Compare(list[j], pivot) < 0)
            {
                i++;
                Swap(list, i, j);
            }
        }

        Swap(list, i + 1, high);
        return i + 1;
    }

    private void Swap(List<string> list, int i, int j)
    {
        string temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}


