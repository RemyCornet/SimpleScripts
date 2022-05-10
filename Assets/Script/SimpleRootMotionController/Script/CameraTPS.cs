using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTPS : MonoBehaviour {

	//Public
	public Transform camTrans;
	public Transform pivot;
	public Transform Player;
	public Transform mainTransform;
	public Transform targetLook;

	//CameraSettings
	 float turnSmooth = 0.2f;
	 float pivotSpeed = 2f;
	 float Y_rot_speed = 1.5f;
	 float X_rot_speed = 1.5f;
	 float minAngle = -35f;
	 float maxAngle = 50f;
	 float normalZ = -1.7f;
	 float normalX = 0f;
	 float normalY = 1.5f;
	//Camera Variables
	 float mouseX;
	 float mouseY;
	 float smoothX;
	 float smoothY;
	 float smoothXVelocity;
	 float smoothYVelocity;
	 float lookAngle;
	 float titlAngle;
	 float delta;
	void Start()
	{
		transform.position = camTrans.position;
		transform.forward = targetLook.forward;
	}
	void LateUpdate()
	{
		check();
	}
	void check()
	{
		delta = Time.deltaTime;
		Position ();
		Rotation ();
		Vector3 targetPosition = Vector3.Lerp (mainTransform.position, Player.position, 1);
		mainTransform.position = targetPosition;
	}
	void Position()
	{
		float targetX = normalX;
		float targetY = normalY;
		float targetZ = normalZ;
		Vector3 newPivotPosition = pivot.localPosition;
		newPivotPosition.x = targetX;
		newPivotPosition.y = targetY;
		Vector3 newCameraPosition = camTrans.localPosition;
		newCameraPosition.z = targetZ;
		float t = delta * pivotSpeed;
		pivot.localPosition = Vector3.Lerp (pivot.localPosition, newPivotPosition, t);
		camTrans.localPosition = Vector3.Lerp (camTrans.localPosition, newCameraPosition, t);
	}
	void Rotation()
	{
		mouseX = Input.GetAxis ("Mouse X");
		mouseY = Input.GetAxis ("Mouse Y");
		if (turnSmooth > 0)
		{
			smoothX = Mathf.SmoothDamp (smoothX, mouseX, ref smoothXVelocity, turnSmooth);
			smoothY = Mathf.SmoothDamp (smoothY, mouseY, ref smoothYVelocity, turnSmooth);
		}
		else
		{
			smoothX = mouseX;
			smoothY = mouseY;
		}
		lookAngle += smoothX * Y_rot_speed;
		Quaternion targetRot = Quaternion.Euler (0, lookAngle, 0);
		mainTransform.rotation = targetRot;

		titlAngle -= smoothY * X_rot_speed;
		titlAngle = Mathf.Clamp (titlAngle, minAngle, maxAngle);
		pivot.localRotation = Quaternion.Euler (titlAngle, 0, 0);
	}
}
