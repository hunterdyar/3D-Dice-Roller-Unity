using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace HDyar.DiceRoller
{
	public class DiceWorldCreator : MonoBehaviour
	{
		[SerializeField]
		private new Camera camera;
		public float depth;
		public float spawnHeightAboveDepth;
		public GameObject shadowCatcherCube;
		
		public List<Transform> DiceSpawnPoints;
		
		public Vector2Int DiceSpawnPointGridSize = new Vector2Int(10,10);

		public PhysicMaterial _colliderMat;

		private void Start()
		{
			CreateDiceColliders();
			CreateSpawnPoints();
		}

		private void CreateSpawnPoints()
		{
			var spawnParent = new GameObject();
			SceneManager.MoveGameObjectToScene(spawnParent,gameObject.scene);
			spawnParent.name = "Spawn Points";
			DiceSpawnPoints = new List<Transform>();
			var bottomLeftW = camera.ScreenToWorldPoint(new Vector3(0, 0, depth-spawnHeightAboveDepth)) + new Vector3(1,0,1);
			var topRightW = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, depth-spawnHeightAboveDepth)) - new Vector3(1,0,1);
			//these are world space? but x/y is local?
			
			for (int x = 0; x < DiceSpawnPointGridSize.x; x++)
			{
				for (int y = 0; y < DiceSpawnPointGridSize.y; y++)
				{
					GameObject spawn = new GameObject();
					spawn.transform.SetParent(spawnParent.transform);
					var pos = new Vector3(
						Mathf.Lerp(bottomLeftW.x, topRightW.x, x / (float)DiceSpawnPointGridSize.x),
						bottomLeftW.y,
						Mathf.Lerp(bottomLeftW.z, topRightW.z, y / (float)DiceSpawnPointGridSize.y)
					);
					spawn.transform.position = pos + new Vector3(0.5f,0,0.5f);//move one half... of a thickness...
					DiceSpawnPoints.Add(spawn.transform);
				}
			}

			// Shuffle the list of points.
			for (int i = 0; i < DiceSpawnPoints.Count; ++i)
			{
				var randomIndex = Random.Range(0,DiceSpawnPoints.Count);
				(DiceSpawnPoints[randomIndex], DiceSpawnPoints[i]) = (DiceSpawnPoints[i], DiceSpawnPoints[randomIndex]);
			}
		}

		[ContextMenu("Create Dice Colliders")]
		public void CreateDiceColliders()
		{
			ClearChildren();
			float thick = 1;

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
			
			//create floor
			var f = new GameObject().AddComponent<BoxCollider>();
			f.gameObject.name = "Floor";
			f.transform.SetParent(transform);
			f.transform.localPosition = Vector3.forward * depth;
			f.center = new Vector3(0,-thick/2,0);
			f.size = new Vector3(size.x+thick,thick,size.z+thick);
			f.material = _colliderMat;
			
			var shadow=Instantiate(shadowCatcherCube, transform);
			shadow.transform.localPosition = f.transform.localPosition;
			shadow.transform.localPosition += f.center;
			shadow.transform.localScale = f.size;
			
			//ceiling
			var c = new GameObject().AddComponent<BoxCollider>();
			c.gameObject.name = "Ceiling";
			c.transform.SetParent(transform);
			c.transform.localPosition = new Vector3(0,0,camera.nearClipPlane);//thickness means we stay half a unit away.
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
			up.center = new Vector3(0, 0, -thick / 2);
			up.size = new Vector3(size.x+thick, size.y, thick);
			
			var down = new GameObject().AddComponent<BoxCollider>(); 
			down.gameObject.name = "down"; 
			down.transform.SetParent(transform); 
			down.transform.localPosition = Vector3.forward * depth / 2 + Vector3.down * size.y / 2;
			down.center = new Vector3(0, 0, thick / 2); 
			down.size = new Vector3(size.x+thick, size.y, thick);
		}

		private void ClearChildren()
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}

		public Transform[] GetSpawnPoints(int count)
		{
			if (count > DiceSpawnPoints.Count)
			{
				Debug.LogError("Can't roll this many dice at once. Not enough spawn points. ");
			}else if (count == DiceSpawnPoints.Count)
			{
				return DiceSpawnPoints.ToArray();
			}
			//the points has been shuffled! nice.
			return DiceSpawnPoints.GetRange(0,count).ToArray();
			
			//Shuffle again? so it doesn't keep picking the same points?
		}
	}
}