using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSortSolver : MonoBehaviour
{
    public void Solve(string targetAnswer, Text outputText)
    {
       
        List<string> permutations = GeneratePermutations(3);

        QuickSort(permutations, 0, permutations.Count - 1);

       
        foreach (string perm in permutations)
        {
            if (perm == targetAnswer)
            {
                
                outputText.text = perm;
                return;
            }
        }

    }

    private List<string> GeneratePermutations(int length)
    {
        List<string> permutations = new List<string>();

        
        for (int i = 0; i < 1000; i++)
        {
            
            string number = i.ToString("000");
            permutations.Add(number);
        }

        return permutations;
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
