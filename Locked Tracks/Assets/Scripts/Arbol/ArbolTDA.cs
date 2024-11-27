using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ArbolTDA
{
    int Raiz();
    NodoABB HijoIzq();
    NodoABB HijoDer();
    bool ArbolVacio();
    void InicializarArbol();
    void AgregarElem(int x, string y);
    void EliminarElem(int x);
}