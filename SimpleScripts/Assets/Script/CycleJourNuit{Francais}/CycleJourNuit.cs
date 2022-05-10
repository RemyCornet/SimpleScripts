using UnityEngine;

[System.Serializable]
public class CouleurDujour
{
	public Color CouleurduCiel;
	public Color CouleurdeLequateur;
	public Color CouleurdeLhorizon;
}

public class CycleJourNuit : MonoBehaviour {
	public GameObject DomeEtoiles;
	public GameObject LunePivot;
	public GameObject Lune;
	public CouleurDujour CouleurdeLaube;
	public CouleurDujour CouleurduJour;
	public CouleurDujour CouleurdelaNuit;
	public Light Lumiereprincipale; 
	public float SecondepourUneJournee = 120f; 
	[Range(0,1)]
	public float heureActuelle = 0; 
	[HideInInspector]
	public float timeMultiplier = 1f; 
	float IntensitedelaLumiere;
	Material MaterielsdesEtoiles;

	void Start () {
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
		IntensitedelaLumiere = Lumiereprincipale.intensity;
		MaterielsdesEtoiles = DomeEtoiles.GetComponentInChildren<MeshRenderer> ().material;		
	}
	
	void Update () {
		MettreajourlaLumiere();
		heureActuelle += (Time.deltaTime / SecondepourUneJournee) * timeMultiplier;
		if (heureActuelle >= 1) {
			heureActuelle = 0;			
		}
	}

	void MettreajourlaLumiere()
	{
		DomeEtoiles.transform.Rotate (new Vector3 (0, 2f * Time.deltaTime, 0));
		Lumiereprincipale.transform.localRotation = Quaternion.Euler ((heureActuelle * 360f) - 90, 170, 0);
		LunePivot.transform.localRotation = Quaternion.Euler ((heureActuelle * 360f) - 100, 170, 0);


		float intensityMultiplier = 1;

		if (heureActuelle <= 0.23f || heureActuelle >= 0.75f) 
		{
			intensityMultiplier = 0;
			MaterielsdesEtoiles.color = new Color(1,1,1,Mathf.Lerp(1,0,Time.deltaTime));
		}
		else if (heureActuelle <= 0.25f) 
		{
			intensityMultiplier = Mathf.Clamp01((heureActuelle - 0.23f) * (1 / 0.02f));
			MaterielsdesEtoiles.color = new Color(1,1,1,Mathf.Lerp(0,1,Time.deltaTime));
		}
		else if (heureActuelle <= 0.73f) 
		{
			intensityMultiplier = Mathf.Clamp01(1 - ((heureActuelle - 0.73f) * (1 / 0.02f)));
		}



		if (heureActuelle <= 0.2f) {
			RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, CouleurdelaNuit.CouleurduCiel, Time.deltaTime);
			RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, CouleurdelaNuit.CouleurdeLequateur, Time.deltaTime);
			RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, CouleurdelaNuit.CouleurdeLhorizon, Time.deltaTime);
		}
		if (heureActuelle > 0.2f && heureActuelle < 0.4f) {
			RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, CouleurdeLaube.CouleurduCiel, Time.deltaTime);
			RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, CouleurdeLaube.CouleurdeLequateur, Time.deltaTime);
			RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, CouleurdeLaube.CouleurdeLhorizon, Time.deltaTime);
		}
		if (heureActuelle > 0.4f && heureActuelle < 0.75f) {
			RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, CouleurduJour.CouleurduCiel, Time.deltaTime);
			RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, CouleurduJour.CouleurdeLequateur, Time.deltaTime);
			RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, CouleurduJour.CouleurdeLhorizon, Time.deltaTime);
		}
		if (heureActuelle > 0.75f) {
			RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, CouleurduJour.CouleurduCiel, Time.deltaTime);
			RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, CouleurduJour.CouleurdeLequateur, Time.deltaTime);
			RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, CouleurduJour.CouleurdeLhorizon, Time.deltaTime);
		}

		Lumiereprincipale.intensity = IntensitedelaLumiere * intensityMultiplier;
	}
}
