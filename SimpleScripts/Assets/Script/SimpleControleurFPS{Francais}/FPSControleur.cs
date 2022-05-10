using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControleur: MonoBehaviour
{
    bool peutcourir = true;

    public bool entraindecourir { get; private set; }
    public bool estagenou { get; private set; }

    public float Vitesse = 5;
    public float VitessedeCourse = 9;
    public float VitesseaGenou = 2;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode crouchkey = KeyCode.LeftControl;

    Rigidbody rb;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();
    FPSSol ToucheleSol;

    public float ForcedeSaut = 2;

    Transform positiondeTete;

    [HideInInspector]
    public float? defaultHeadYLocalPosition;

    public float positiondeTeteGenou = 1;

    
    CapsuleCollider colliderJoueur;
    float? defaultColliderHeight;

    void Reset()
    {
        ToucheleSol = GetComponentInChildren<FPSSol>();
        positiondeTete = GetComponentInChildren<Camera>().transform;
        colliderJoueur = this.gameObject.GetComponent<CapsuleCollider>();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ToucheleSol = GetComponentInChildren<FPSSol>();
        positiondeTete = GetComponentInChildren<Camera>().transform;
        colliderJoueur = this.gameObject.GetComponent<CapsuleCollider>();
    }
    #region Mouvement
    void FixedUpdate()
    {
        entraindecourir = peutcourir && Input.GetKey(runKey);

        float targetMovingSpeed = entraindecourir ? VitessedeCourse : Vitesse;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }
        if (ToucheleSol.EstaTerre)
        {
            Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
            rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);

        }

        
    }
    #endregion
    #region Saut + Genou
    void LateUpdate()
    {

        if (Input.GetButtonDown("Jump") && (!ToucheleSol || ToucheleSol.EstaTerre) && !estagenou)
        {
            rb.AddForce(Vector3.up * 100 * ForcedeSaut);
        }
        if (Input.GetKey(crouchkey))
        {
            if (positiondeTete)
            {
                if (!defaultHeadYLocalPosition.HasValue)
                {
                    defaultHeadYLocalPosition = positiondeTete.localPosition.y;
                }

                positiondeTete.localPosition = new Vector3(positiondeTete.localPosition.x, positiondeTeteGenou, positiondeTete.localPosition.z);
            }

            if (colliderJoueur)
            {

                if (!defaultColliderHeight.HasValue)
                {
                    defaultColliderHeight = colliderJoueur.height;
                }

                float loweringAmount;
                if (defaultHeadYLocalPosition.HasValue)
                {
                    loweringAmount = defaultHeadYLocalPosition.Value - positiondeTeteGenou;
                }
                else
                {
                    loweringAmount = defaultColliderHeight.Value * .5f;
                }

                colliderJoueur.height = Mathf.Max(defaultColliderHeight.Value - loweringAmount, 0);
                colliderJoueur.center = Vector3.up * colliderJoueur.height * .5f;
            }

            if (!estagenou)
            {
                estagenou = true;
                peutcourir = false;
                SetSpeedOverrideActive(true);
            }
        }
        else
        {
            if (estagenou)
            {
                if (positiondeTete)
                {
                    positiondeTete.localPosition = new Vector3(positiondeTete.localPosition.x, defaultHeadYLocalPosition.Value, positiondeTete.localPosition.z);
                }

                if (colliderJoueur)
                {
                    colliderJoueur.height = defaultColliderHeight.Value;
                    colliderJoueur.center = Vector3.up * colliderJoueur.height * .5f;
                }

                estagenou = false;
                peutcourir = true;
                SetSpeedOverrideActive(false);
            }
        }
    }
    #endregion
    #region Vitesse
   void SetSpeedOverrideActive(bool state)
    {
        if (state)
        {
            
            if (speedOverrides.Contains(SpeedOverride))
            {
                speedOverrides.Add(SpeedOverride);
            }
        }
        else
        {
            
            if (speedOverrides.Contains(SpeedOverride))
            {
                speedOverrides.Remove(SpeedOverride);
            }
        }
    }
    float SpeedOverride() => VitesseaGenou;
    #endregion

}
