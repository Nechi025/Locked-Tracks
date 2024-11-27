using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodoABB
{
    public int Puntaje { get; set; }
    public string Tiempo { get; set; }

    public NodoABB hijoIzq;
    public NodoABB hijoDer;

    public NodoABB(int puntaje, string tiempo)
    {
        Puntaje = puntaje;
        Tiempo = tiempo;
        hijoIzq = null;
        hijoDer = null;
    }
}
