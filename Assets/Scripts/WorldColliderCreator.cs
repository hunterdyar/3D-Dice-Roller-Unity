using System;
using Unity.VisualScripting;
using UnityEngine;

namespace HDyar.DiceRoller
{
	public class WorldColliderCreator : MonoBehaviour
	{
		public float depth;
		public Bounds bounds;
		//todo: use unique physics scene option. in roller, i guess.

		private void Awake()
		{
			CreateDiceColliders();
		}

		[ContextMenu("Create Dice Colliders")]
		public void CreateDiceColliders()
		{
			ClearChildren();
			float thick = 1;
			Camera camera = Camera.main;
			if (camera == null)
			{
				camera = Camera.main;
			}

			transform.position = camera.transform.position;
			transform.rotation = camera.transform.rotation;

			var bottomLeftW = camera.ScreenToWorldPoint(new Vector3(0, 0, depth));
			var topRightW = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, 0));
			if(!camera.orthographic)
			{
				topRightW = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
				topRightW = topRightW - Vector3.forward * depth;
			}

			bottomLeftW = transform.InverseTransformVector(bottomLeftW);
			topRightW = transform.InverseTransformVector(topRightW);
			//work in local space will keep bounds axis-aligned.
			var center = Vector3.Lerp(bottomLeftW, topRightW, .5f);
			var size = topRightW - bottomLeftW;
			size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
			bounds = new Bounds(center, size);
			
			//create floor
			var f = new GameObject().AddComponent<BoxCollider>();
			f.gameObject.name = "Floor";
			f.transform.SetParent(transform);
			f.transform.localPosition = Vector3.forward * depth;
			f.center = new Vector3(0,-thick/2,0);
			f.size = new Vector3(size.x+thick,thick,size.z+thick);
			
			//ceiling
			var c = new GameObject().AddComponent<BoxCollider>();
			c.gameObject.name = "Ceiling";
			c.transform.SetParent(transform);
			c.transform.localPosition = new Vector3(0,0,camera.nearClipPlane+0.01f);
			c.center = new Vector3(0,thick/2,0);
			c.size = new Vector3(size.x+thick,thick,size.z+thick);
			
			//Create Walls
			var right = new GameObject().AddComponent<BoxCollider>();
			right.gameObject.name = "right";
			right.transform.SetParent(transform);
			right.transform.localPosition = Vector3.forward * depth / 2 + Vector3.right * size.x / 2;
			right.center = new Vector3(thick / 2, 0, 0);
			right.size = new Vector3(thick, size.y, size.z+thick);
			
			var left = new GameObject().AddComponent<BoxCollider>(); 
			left.gameObject.name = "left"; 
			left.transform.SetParent(transform); 
			left.transform.localPosition = Vector3.forward * depth / 2 + Vector3.left * size.x / 2; 
			left.center = new Vector3(-thick / 2, 0, 0); 
			left.size = new Vector3(thick, size.y, size.z+thick);

			var up = new GameObject().AddComponent<BoxCollider>();
			up.gameObject.name = "up";
			up.transform.SetParent(transform);
			up.transform.localPosition = Vector3.forward * depth / 2 + Vector3.up * size.y / 2;
			up.center = new Vector3(0, 0, thick / 2);
			up.size = new Vector3(size.x+thick, size.y, thick);
			
			var down = new GameObject().AddComponent<BoxCollider>(); 
			down.gameObject.name = "down"; 
			down.transform.SetParent(transform); 
			down.transform.localPosition = Vector3.forward * depth / 2 + Vector3.down * size.y / 2;
			down.center = new Vector3(0, 0,- thick / 2); 
			down.size = new Vector3(size.x+thick, size.y, thick);
		}

		private void ClearChildren()
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}
	}
}