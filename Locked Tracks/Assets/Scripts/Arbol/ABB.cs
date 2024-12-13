using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABB : MonoBehaviour, ArbolTDA
{

    public NodoABB raiz;

    private void Start()
    {
        TestArbol();
    }
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
            Debug.Log("Nuevo puntaje agregado como raíz: " + puntaje);
        }
        else
        {
            AgregarRecursivo(raiz, puntaje, tiempo);
        }
    }

    private void AgregarRecursivo(NodoABB nodo, int puntaje, string tiempo)
    {
        if (puntaje == nodo.Puntaje)
        {
            Debug.LogWarning("Puntaje duplicado ignorado: " + puntaje);
            return;
        }
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
        else
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
            
            if (nodo.hijoIzq == null && nodo.hijoDer == null)
            {
                return null;
            }

            
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

    public void GuardarArbolEnPlayerPrefs()
    {
        List<int> puntajes = new List<int>();
        RecorrerInOrder(raiz, puntajes, int.MaxValue); // Obtener todos los valores del árbol
        string json = JsonUtility.ToJson(new Wrapper { scores = puntajes });
        PlayerPrefs.SetString("ArbolPuntajes", json);
        PlayerPrefs.Save();
    }
    public void CargarArbolDePlayerPrefs()
    {
        string json = PlayerPrefs.GetString("ArbolPuntajes", "[]");
        List<int> puntajes = JsonUtility.FromJson<Wrapper>(json)?.scores ?? new List<int>();
        foreach (int puntaje in puntajes)
        {
            AgregarElem(puntaje, FormatearTiempo(puntaje)); // Usa tu lógica existente
        }
    }
    public List<int> TopHighScore()
    {
        List<int> mejores = new List<int>();
        RecorrerInOrder(raiz, mejores, 3); // Recolecta los tres menores
        return mejores;
    }

    private void RecorrerInOrder(NodoABB nodo, List<int> lista, int limite)
    {
        if (nodo == null || lista.Count >= limite)
            return;

        // Recorremos primero los hijos izquierdos (valores menores)
        RecorrerInOrder(nodo.hijoIzq, lista, limite);

        // Agregamos el valor actual si no hemos alcanzado el límite
        if (lista.Count < limite)
        {
            lista.Add(nodo.Puntaje);
        }

        // Luego recorremos los hijos derechos
        RecorrerInOrder(nodo.hijoDer, lista, limite);
    }

    private void TopHighScoreRecursivo(NodoABB nodo, List<int> mejores)
    {
        if (nodo == null || mejores.Count >= 3)
        {
            return;
        }

        // Recorre el hijo izquierdo (valores más pequeños)
        TopHighScoreRecursivo(nodo.hijoIzq, mejores);

        // Si el puntaje no está ya en la lista, lo agregamos
        if (mejores.Count < 3 && !mejores.Contains(nodo.Puntaje)) 
        {
            mejores.Add(nodo.Puntaje);
        }
        
        // Recorre el hijo derecho (valores más grandes)
        TopHighScoreRecursivo(nodo.hijoDer, mejores);
    }

    public bool Existe(int puntaje)
    {
        return ExisteRecursivo(raiz, puntaje);
    }

    private bool ExisteRecursivo(NodoABB nodo, int puntaje)
    {
        if (nodo == null) return false;
        if (nodo.Puntaje == puntaje) return true;
        if (puntaje < nodo.Puntaje) return ExisteRecursivo(nodo.hijoIzq, puntaje);
        return ExisteRecursivo(nodo.hijoDer, puntaje);
    }

    public void ImprimirArbol()
    {
        ImprimirRecursivo(raiz);
    }

    private void ImprimirRecursivo(NodoABB nodo)
    {
        if (nodo == null) return;

        ImprimirRecursivo(nodo.hijoIzq);
        Debug.Log("Puntaje: " + nodo.Puntaje);
        ImprimirRecursivo(nodo.hijoDer);
    }

    public void TestArbol()
    {
        ABB arbol = new ABB();
        arbol.InicializarArbol();

        // Añadir algunos puntajes de prueba
        arbol.AgregarElem(30, "00:30");
        arbol.AgregarElem(20, "00:20");
        arbol.AgregarElem(40, "00:40");
        arbol.AgregarElem(25, "00:25");
        arbol.AgregarElem(35, "00:35");

        // Ver la estructura del árbol
        Debug.Log("Estructura del árbol:");
        arbol.ImprimirArbol();

        // Obtener el Top 3
        List<int> top = arbol.TopHighScore();
        Debug.Log("Top 3: " + string.Join(", ", top));
    }

    private string FormatearTiempo(int tiempoEnSegundos)
    {
        int minutos = tiempoEnSegundos / 60;
        int segundos = tiempoEnSegundos % 60;
        return string.Format("{0:D2}:{1:D2}", minutos, segundos);
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<int> scores;
    }

}