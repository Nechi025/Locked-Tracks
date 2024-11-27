using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GrafoTDA<T>
{
    void InicializarGrafo();
    void AgregarVertice(T v);
    void EliminarVertice(T v);
    void AgregarArista(T v1, T v2, int peso);
    void EliminarArista(T v1, T v2);
    bool ExisteArista(T v1, T v2);
    int PesoArista(T v1, T v2);
}
