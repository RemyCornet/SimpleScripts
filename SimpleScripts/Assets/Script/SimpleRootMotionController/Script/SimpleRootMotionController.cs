using UnityEngine;
using System.Collections;

public class SimpleRootMotionController : MonoBehaviour
{
	//Public
	public Transform targetPosition;
	public float angularSpeed;	
	public float luft;
	public GameObject swordObject;
	public GameObject shieldObject;

	//Private
	Animator anim;
	Vector3 targetPosVec;
	float run = 0f;
	float strafe = 0f;
	bool Atk;
	bool Fight = false;
	bool Shield = false;
	bool isPlayerRot;

	public KeyCode Draw = KeyCode.F;
	public KeyCode attack = KeyCode.Mouse0;
	public KeyCode kick = KeyCode.R;
	public KeyCode shield = KeyCode.Mouse1;
	#region Start

	void Start()
	{		
		anim = GetComponent<Animator>();	
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Locomotion();
		FightSystem();
		Attack();
		ShieldSystem();
	}
    #endregion

    #region Locomotion
    void Locomotion()
	{
		targetPosVec = targetPosition.position;


		run = Input.GetAxis("Vertical");
		strafe = Input.GetAxis("Horizontal");

		anim.SetFloat("Strafe", strafe);
		anim.SetFloat("Run", run);


		if (run != 0 || strafe != 0 || Fight == true)
		{
			Vector3 rot = transform.eulerAngles;
			transform.LookAt(targetPosVec);
			float angleBetween = Mathf.DeltaAngle(transform.eulerAngles.y, rot.y);
			if ((Mathf.Abs(angleBetween) > luft) || strafe != 0)
			{
				isPlayerRot = true;
			}
			if (isPlayerRot == true)
			{
				float bodyY = Mathf.LerpAngle(rot.y, transform.eulerAngles.y, Time.deltaTime * angularSpeed);
				transform.eulerAngles = new Vector3(0, bodyY, 0);	
			}
			else
			{
				transform.eulerAngles = new Vector3(0f, rot.y, 0f);
			}

		}
		transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
	}
	#endregion

	#region FightSystem
	void FightSystem()
	{
		if (Input.GetKeyDown(Draw) && Fight == false)
		{
			Fight = true;
			anim.SetBool("Battle", true);
			swordObject.SetActive(true);
			shieldObject.SetActive(true);

		}
		else if (Input.GetKeyDown(Draw) && Fight == true)
		{
			Fight = false;
			anim.SetBool("Battle", false);
			swordObject.SetActive(false);
			shieldObject.SetActive(false);
		}
	}
	void Attack()
	{
		if (Input.GetKey(attack) && !Atk && !Shield)
		{
			Atk = true;
			StartCoroutine(AttackAnim());
		}
		else if (Input.GetKey(kick) && !Atk && !Shield)
		{
			Atk = true;
			StartCoroutine(KickAnim());
		}
	}
	IEnumerator AttackAnim ()
	{
			anim.SetTrigger("Attack");
			yield return new WaitForSeconds(1);
			Atk = false;
	}
	IEnumerator KickAnim()
	{
			anim.SetTrigger("Kick");
			yield return new WaitForSeconds(1);
			Atk = false;		
	}
	void ShieldSystem()
	{
		if (Input.GetKey(shield) && Fight == true && Shield == false)
		{
			Shield = true;
			anim.SetBool("Shield", true);
		}
		else if (Input.GetKeyUp(shield) && Fight == true && Shield == true)
		{
			Shield = false;
			anim.SetBool("Shield", false);
		}
	}
	#endregion	
}
