using UnityEngine;
using System.Collections;

	public class TransferMotion : MonoBehaviour {

		public Transform Item;

		float transferMotion = 1f;
		
		private Vector3 lastPosition;

		void OnEnable() 
		{
			lastPosition = transform.position;
		}		
		void Update() 
		{
			Vector3 delta = transform.position - lastPosition;

			Item.position += delta * transferMotion;

			lastPosition = transform.position;
		}
}
