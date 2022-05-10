using System;
using UnityEngine;

public class SimpleControleurDeVoiture : MonoBehaviour
{
    #region Public
      public int VitesseMax = 90; // VitesseMax de la voiture en marche avant
      public int MarchearrièreMax = 45;  // VitesseMax de la voiture en marche arriere

      public int Acceleration = 2; // multiplicateur d'Acceleration
      public int Deceleration = 2; // multiplicateur de Deceleration

      public int BraquageMax = 27; // Braquage Max
      public float Vitessededirection = 0.5f; // Vitesse de la direction
    #endregion
    #region Roues
    //Mesh des roue de la voiture  
    public GameObject AvantGaucheMesh;
      public GameObject AvantDroiteMesh;     
      public GameObject ArriereGaucheMesh;     
      public GameObject ArriereDroiteMesh;
    //Composant WheelCollider
      public WheelCollider AvantGaucheCo;
      public WheelCollider AvantDroiteCo;
      public WheelCollider ArriereGaucheCo;
      public WheelCollider ArriereDroiteCo;
    #endregion

    #region Privé 
      float Vitesse;    
      Rigidbody VoitureRb; 
      float axededirection;
      float Axedesgaz;
      float VelociteZ;
      float VelociteX;
      bool endeceleration;    
      WheelFrictionCurve AvantGaucheFriction;
      float AvantGaucheGlissade;
      WheelFrictionCurve AvantDroiteFriction;
      float AvantDroiteGlissade;
      WheelFrictionCurve ArriereGaucheFriction;
      float ArriereGaucheGlissade;
      WheelFrictionCurve ArriereDroiteFriction;
      float ArriereDroiteGlissade;

      Vector3 CenterdeMasse; // Si besoin rendre publique
    #endregion
    void Awake()
    {
        VoitureRb = this.gameObject.GetComponent<Rigidbody>();
        VoitureRb.centerOfMass = CenterdeMasse;

        AvantGaucheFriction = new WheelFrictionCurve ();
        AvantGaucheFriction.extremumSlip = AvantGaucheCo.sidewaysFriction.extremumSlip;
        AvantGaucheGlissade = AvantGaucheCo.sidewaysFriction.extremumSlip;
        AvantGaucheFriction.extremumValue = AvantGaucheCo.sidewaysFriction.extremumValue;
        AvantGaucheFriction.asymptoteSlip = AvantGaucheCo.sidewaysFriction.asymptoteSlip;
        AvantGaucheFriction.asymptoteValue = AvantGaucheCo.sidewaysFriction.asymptoteValue;
        AvantGaucheFriction.stiffness = AvantGaucheCo.sidewaysFriction.stiffness;
        AvantDroiteFriction = new WheelFrictionCurve ();
        AvantDroiteFriction.extremumSlip = AvantDroiteCo.sidewaysFriction.extremumSlip;
        AvantDroiteGlissade = AvantDroiteCo.sidewaysFriction.extremumSlip;
        AvantDroiteFriction.extremumValue = AvantDroiteCo.sidewaysFriction.extremumValue;
        AvantDroiteFriction.asymptoteSlip = AvantDroiteCo.sidewaysFriction.asymptoteSlip;
        AvantDroiteFriction.asymptoteValue = AvantDroiteCo.sidewaysFriction.asymptoteValue;
        AvantDroiteFriction.stiffness = AvantDroiteCo.sidewaysFriction.stiffness;
        ArriereGaucheFriction = new WheelFrictionCurve ();
        ArriereGaucheFriction.extremumSlip = ArriereGaucheCo.sidewaysFriction.extremumSlip;
        ArriereGaucheGlissade = ArriereGaucheCo.sidewaysFriction.extremumSlip;
        ArriereGaucheFriction.extremumValue = ArriereGaucheCo.sidewaysFriction.extremumValue;
        ArriereGaucheFriction.asymptoteSlip = ArriereGaucheCo.sidewaysFriction.asymptoteSlip;
        ArriereGaucheFriction.asymptoteValue = ArriereGaucheCo.sidewaysFriction.asymptoteValue;
        ArriereGaucheFriction.stiffness = ArriereGaucheCo.sidewaysFriction.stiffness;
        ArriereDroiteFriction = new WheelFrictionCurve ();
        ArriereDroiteFriction.extremumSlip = ArriereDroiteCo.sidewaysFriction.extremumSlip;
        ArriereDroiteGlissade = ArriereDroiteCo.sidewaysFriction.extremumSlip;
        ArriereDroiteFriction.extremumValue = ArriereDroiteCo.sidewaysFriction.extremumValue;
        ArriereDroiteFriction.asymptoteSlip = ArriereDroiteCo.sidewaysFriction.asymptoteSlip;
        ArriereDroiteFriction.asymptoteValue = ArriereDroiteCo.sidewaysFriction.asymptoteValue;
        ArriereDroiteFriction.stiffness = ArriereDroiteCo.sidewaysFriction.stiffness;
    }
    void Update()
    {
        Vitesse = (2 * Mathf.PI * AvantGaucheCo.radius * AvantGaucheCo.rpm * 60) / 1000;

        VelociteX = transform.InverseTransformDirection(VoitureRb.velocity).x;

        VelociteZ = transform.InverseTransformDirection(VoitureRb.velocity).z;
              
        if(Input.GetKey(KeyCode.W)){
          CancelInvoke("Ralentir");
            endeceleration = false;
            Enavant();
        }
        if(Input.GetKey(KeyCode.S)){
          CancelInvoke("Ralentir");
            endeceleration = false;
            Enarriere();
        }
        if(Input.GetKey(KeyCode.A)){
            Tourneragauche();
        }
        if(Input.GetKey(KeyCode.D)){
            Tourneradroite();
        }       
        if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))){
            MoteurOff();
        }
        if(!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !endeceleration)
        {
          InvokeRepeating("Ralentir", 0f, 0.1f);
            endeceleration = true;
        }
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && axededirection != 0f){
            ResetDirection();
        }
        Animerlesroues();
    }

    #region direction
    public void Enavant()
    {

        Axedesgaz = Axedesgaz + (Time.deltaTime * 3f);
        if (Axedesgaz > 1f)
        {
            Axedesgaz = 1f;
        }

        else
        {
            if (Mathf.RoundToInt(Vitesse) < VitesseMax)
            {

                AvantGaucheCo.brakeTorque = 0;
                AvantGaucheCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
                AvantDroiteCo.brakeTorque = 0;
                AvantDroiteCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
                ArriereGaucheCo.brakeTorque = 0;
                ArriereGaucheCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
                ArriereDroiteCo.brakeTorque = 0;
                ArriereDroiteCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
            }
            else
            {
                AvantGaucheCo.motorTorque = 0;
                AvantDroiteCo.motorTorque = 0;
                ArriereGaucheCo.motorTorque = 0;
                ArriereDroiteCo.motorTorque = 0;
            }
        }
    }
    public void Enarriere()
    {

        Axedesgaz = Axedesgaz - (Time.deltaTime * 3f);
        if (Axedesgaz < -1f)
        {
            Axedesgaz = -1f;

        }
        else
        {
            if (Mathf.Abs(Mathf.RoundToInt(Vitesse)) < MarchearrièreMax)
            {
                AvantGaucheCo.brakeTorque = 0;
                AvantGaucheCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
                AvantDroiteCo.brakeTorque = 0;
                AvantDroiteCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
                ArriereGaucheCo.brakeTorque = 0;
                ArriereGaucheCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
                ArriereDroiteCo.brakeTorque = 0;
                ArriereDroiteCo.motorTorque = (Acceleration * 50f) * Axedesgaz;
            }
            else
            {
                AvantGaucheCo.motorTorque = 0;
                AvantDroiteCo.motorTorque = 0;
                ArriereGaucheCo.motorTorque = 0;
                ArriereDroiteCo.motorTorque = 0;
            }
        }
    }
    public void Tourneragauche()
    {
        axededirection = axededirection - (Time.deltaTime * 10f * Vitessededirection);
        if (axededirection < -1f)
        {
            axededirection = -1f;
        }
        var steeringAngle = axededirection * BraquageMax;
        AvantGaucheCo.steerAngle = Mathf.Lerp(AvantGaucheCo.steerAngle, steeringAngle, Vitessededirection);
        AvantDroiteCo.steerAngle = Mathf.Lerp(AvantDroiteCo.steerAngle, steeringAngle, Vitessededirection);
    }
    public void Tourneradroite()
    {
        axededirection = axededirection + (Time.deltaTime * 10f * Vitessededirection);
        if (axededirection > 1f)
        {
            axededirection = 1f;
        }
        var steeringAngle = axededirection * BraquageMax;
        AvantGaucheCo.steerAngle = Mathf.Lerp(AvantGaucheCo.steerAngle, steeringAngle, Vitessededirection);
        AvantDroiteCo.steerAngle = Mathf.Lerp(AvantDroiteCo.steerAngle, steeringAngle, Vitessededirection);
    }
    public void ResetDirection()
    {
        if (axededirection < 0f)
        {
            axededirection = axededirection + (Time.deltaTime * 10f * Vitessededirection);
        }
        else if (axededirection > 0f)
        {
            axededirection = axededirection - (Time.deltaTime * 10f * Vitessededirection);
        }
        if (Mathf.Abs(AvantGaucheCo.steerAngle) < 1f)
        {
            axededirection = 0f;
        }
        var steeringAngle = axededirection * BraquageMax;
        AvantGaucheCo.steerAngle = Mathf.Lerp(AvantGaucheCo.steerAngle, steeringAngle, Vitessededirection);
        AvantDroiteCo.steerAngle = Mathf.Lerp(AvantDroiteCo.steerAngle, steeringAngle, Vitessededirection);
    }

    #endregion
    #region Autres
    void Animerlesroues()
    {
        try
        {
            Quaternion FLWRotation;
            Vector3 FLWPosition;
            AvantGaucheCo.GetWorldPose(out FLWPosition, out FLWRotation);
            AvantGaucheMesh.transform.position = FLWPosition;
            AvantGaucheMesh.transform.rotation = FLWRotation;

            Quaternion FRWRotation;
            Vector3 FRWPosition;
            AvantDroiteCo.GetWorldPose(out FRWPosition, out FRWRotation);
            AvantDroiteMesh.transform.position = FRWPosition;
            AvantDroiteMesh.transform.rotation = FRWRotation;

            Quaternion RLWRotation;
            Vector3 RLWPosition;
            ArriereGaucheCo.GetWorldPose(out RLWPosition, out RLWRotation);
            ArriereGaucheMesh.transform.position = RLWPosition;
            ArriereGaucheMesh.transform.rotation = RLWRotation;

            Quaternion RRWRotation;
            Vector3 RRWPosition;
            ArriereDroiteCo.GetWorldPose(out RRWPosition, out RRWRotation);
            ArriereDroiteMesh.transform.position = RRWPosition;
            ArriereDroiteMesh.transform.rotation = RRWRotation;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
        }
    }
    public void MoteurOff()
    {
        AvantGaucheCo.motorTorque = 0;
        AvantDroiteCo.motorTorque = 0;
        ArriereGaucheCo.motorTorque = 0;
        ArriereDroiteCo.motorTorque = 0;
    }
    public void Ralentir()
    {

        if (Axedesgaz != 0f)
        {
            if (Axedesgaz > 0f)
            {
                Axedesgaz = Axedesgaz - (Time.deltaTime * 10f);
            }
            else if (Axedesgaz < 0f)
            {
                Axedesgaz = Axedesgaz + (Time.deltaTime * 10f);
            }
            if (Mathf.Abs(Axedesgaz) < 0.15f)
            {
                Axedesgaz = 0f;
            }
        }
        VoitureRb.velocity = VoitureRb.velocity * (1f / (1f + (0.025f * Deceleration)));

        AvantGaucheCo.motorTorque = 0;
        AvantDroiteCo.motorTorque = 0;
        ArriereGaucheCo.motorTorque = 0;
        ArriereDroiteCo.motorTorque = 0;

        if (VoitureRb.velocity.magnitude < 0.25f)
        {
            VoitureRb.velocity = Vector3.zero;
            CancelInvoke("Ralentir");
        }
    }
    #endregion
}
