using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

    //Puntos de referencia que se utilizan para que giren los enemigos.
    public static Transform[] points;

    void Awake()
    {
        //Asigna un Transform a cada punto.
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

}
