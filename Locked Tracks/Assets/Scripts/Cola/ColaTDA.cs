using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ColaTDA<T>
{
    void InicializarCola();
    void Acolar(T x);     
    void Desacolar();      
    bool ColaVacia();       
    T Primero();
}
