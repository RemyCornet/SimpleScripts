using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSol : MonoBehaviour
{
    public float distanceaveclesolMax = .15f;

    public bool EstaTerre = true;

    public event System.Action Sol;

    const float Decalagedorigine = .001f;
    Vector3 RaycastOrigin => transform.position + Vector3.up * Decalagedorigine;


    void LateUpdate()
    {
        
        bool EstaTerreMaintenant = Physics.Raycast(RaycastOrigin, Vector3.down, distanceaveclesolMax * 2);

        
        if (EstaTerreMaintenant && !EstaTerre)
        {
            Sol?.Invoke();
        }


        EstaTerre = EstaTerreMaintenant;
    }
}
