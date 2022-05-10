using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFPS : MonoBehaviour
{
    [SerializeField]
    Transform Joueur;
    public float sensibilite = 2;
    public float lissage = 1.5f;

    Vector2 velocite;
    Vector2 framevelocite;

    Camera cam;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)]
    public float zoomactuel;
    public float Zoomsensibilite = 1;
    void Reset()
    {
        Joueur = GetComponentInParent<FPSControleur>().transform;
    }
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //
        cam = GetComponent<Camera>();
        if (cam)
        {
            defaultFOV = cam.fieldOfView;
        }
    }
    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensibilite);
        framevelocite = Vector2.Lerp(framevelocite, rawFrameVelocity, 1 / lissage);
        velocite += framevelocite;
        velocite.y = Mathf.Clamp(velocite.y, -90, 90);

        
        transform.localRotation = Quaternion.AngleAxis(-velocite.y, Vector3.right);
        Joueur.localRotation = Quaternion.AngleAxis(velocite.x, Vector3.up);

        zoomactuel += Input.mouseScrollDelta.y * Zoomsensibilite * .05f;
        zoomactuel = Mathf.Clamp01(zoomactuel);
        cam.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, zoomactuel);
    }
}
