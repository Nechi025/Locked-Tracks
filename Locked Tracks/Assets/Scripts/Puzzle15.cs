using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle15 : MonoBehaviour
{
    [SerializeField] Transform emptySpace;
    private Camera _camera;

    private PilaTDA<Movimiento> stackTDA;

    private void Start()
    {
        _camera = Camera.main;
        stackTDA = new StackTDA();
        stackTDA.InicializarPila();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                if (Vector3.Distance(emptySpace.position, hit.transform.position) < 0.1)
                {
                    Movimiento movimiento = new Movimiento
                    {
                        piezaMovida = hit.transform,
                        posicionAnterior = hit.transform.position,
                        posicionEspacioVacio = emptySpace.position
                    };
                    stackTDA.Apilar(movimiento);

                    Vector3 lastEmptySpacePosition = emptySpace.position;
                    emptySpace.position = hit.transform.position;
                    hit.transform.position = lastEmptySpacePosition;
                }
            }
        }
    }
    public void RevertirMovimiento()
    {
        if (!stackTDA.PilaVacia())
        {
            Movimiento ultimoMovimiento = stackTDA.Tope();
            stackTDA.Desapilar();

            // Restauramos las posiciones
            ultimoMovimiento.piezaMovida.position = ultimoMovimiento.posicionAnterior;
            emptySpace.position = ultimoMovimiento.posicionEspacioVacio;
        }
    }

}
