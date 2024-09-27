using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PilaTDA<T>
{
    void InicializarPila();
    void Apilar(T x);
    void Desapilar();
    bool PilaVacia();
    T Tope();
}
