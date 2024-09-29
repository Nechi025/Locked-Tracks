using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public Vector3 targetPosition;
    private Vector3 correctPosition;
    public int number;
    public bool inRightPlace;

    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Suavizado de movimiento
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);

        if (targetPosition == correctPosition)
        {
            //Debug.Log("Ta bien");
            inRightPlace = true;
        }
        else
        {
            inRightPlace = false;
        }
    }
}
