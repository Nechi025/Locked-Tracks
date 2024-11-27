using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABB : MonoBehaviour, ArbolTDA
{

    public NodoABB raiz;

    public int Raiz()
    {
        return raiz.Puntaje;
    }

    public bool ArbolVacio()
    {
        return (raiz == null);
    }

    public void InicializarArbol()
    {
        raiz = null;
    }

    public NodoABB HijoDer()
    {
        return raiz?.hijoDer;
    }

    public NodoABB HijoIzq()
    {
        return raiz?.hijoIzq;
    }

    public void AgregarElem(int puntaje, string tiempo)
    {
        if (raiz == null)
        {
            raiz = new NodoABB(puntaje, tiempo);
        }
        else
        {
            AgregarRecursivo(raiz, puntaje, tiempo);
        }
    }

    private void AgregarRecursivo(NodoABB nodo, int puntaje, string tiempo)
    {
        if (puntaje < nodo.Puntaje)
        {
            if (nodo.hijoIzq == null)
            {
                nodo.hijoIzq = new NodoABB(puntaje, tiempo);
            }
            else
            {
                AgregarRecursivo(nodo.hijoIzq, puntaje, tiempo);
            }
        }
        else if (puntaje > nodo.Puntaje)
        {
            if (nodo.hijoDer == null)
            {
                nodo.hijoDer = new NodoABB(puntaje, tiempo);
            }
            else
            {
                AgregarRecursivo(nodo.hijoDer, puntaje, tiempo);
            }
        }
    }

    public void EliminarElem(int puntaje)
    {
        raiz = EliminarRecursivo(raiz, puntaje);
    }

    private NodoABB EliminarRecursivo(NodoABB nodo, int puntaje)
    {
        if (nodo == null) return null;

        if (puntaje < nodo.Puntaje)
        {
            nodo.hijoIzq = EliminarRecursivo(nodo.hijoIzq, puntaje);
        }
        else if (puntaje > nodo.Puntaje)
        {
            nodo.hijoDer = EliminarRecursivo(nodo.hijoDer, puntaje);
        }
        else
        {
            // Caso 1: El nodo no tiene hijos
            if (nodo.hijoIzq == null && nodo.hijoDer == null)
            {
                return null;
            }

            // Caso 2: El nodo tiene un solo hijo
            if (nodo.hijoIzq == null)
            {
                return nodo.hijoDer;
            }
            if (nodo.hijoDer == null)
            {
                return nodo.hijoIzq;
            }

        }

        return nodo;
    }



    public int Mayor(NodoABB nodo)
    {
        if (nodo.hijoDer == null)
        {
            return nodo.Puntaje;
        }
        return Mayor(nodo.hijoDer);
    }

    public int Menor(NodoABB nodo)
    {
        if (nodo == null)
        {
            throw new System.ArgumentNullException(nameof(nodo), "El nodo no puede ser nulo.");
        }

        if (nodo.hijoIzq == null)
        {
            return nodo.Puntaje;
        }
        return Menor(nodo.hijoIzq);
    }


}