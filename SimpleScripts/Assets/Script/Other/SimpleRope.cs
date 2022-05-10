using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRope : MonoBehaviour
{
    public LineRenderer Line;
    public GameObject Sart;
    public GameObject End;
    public float Size = 4.5f;

    private void Start()
    {
        Line.startWidth = Size;
        Line.endWidth = Size;
        Line.useWorldSpace = true;
    }
    private void Update()
    {
        List<Vector3> posA = new List<Vector3>();
        posA.Add(Sart.transform.position);
        posA.Add(End.transform.position);
        Line.SetPositions(posA.ToArray());
    }
}
