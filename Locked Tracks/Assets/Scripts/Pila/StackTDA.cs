using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackTDA : MonoBehaviour, PilaTDA<Movimiento>
{
    private List<Movimiento> pila;

    public void Apilar(Movimiento movimiento)
    {
        pila.Add(movimiento);
    }

    public void Desapilar()
    {
        if (!PilaVacia())
        {
            pila.RemoveAt(pila.Count - 1);
        }
    }

    public void InicializarPila()
    {
        pila = new List<Movimiento>();
    }

    public bool PilaVacia()
    {
        return pila.Count == 0;
    }

    public Movimiento Tope()
    {
        if (!PilaVacia())
        {
            return pila[pila.Count - 1];
        }
        else
        {
            throw new System.InvalidOperationException("La pila está vacía.");
        }
    }
}
