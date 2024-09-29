using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTDA : MonoBehaviour, ColaTDA<int>
{
    private List<int> elementos;

    public void Acolar(int x)
    {
        elementos.Add(x);
    }

    public bool ColaVacia()
    {
        return elementos.Count == 0;
    }

    public void Desacolar()
    {
        if (!ColaVacia())
        {
            elementos.RemoveAt(0);
        }
    }

    public void InicializarCola()
    {
        elementos = new List<int>();
    }

    public int Primero()
    {
        if (!ColaVacia())
        {
            return elementos[0];
        }
        else
        {
            throw new System.InvalidOperationException("La cola está vacía.");
        }
    }
}
