using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace HDyar.DiceRoller
{
	[RequireComponent(typeof(Rigidbody))]
	public class Dice : MonoBehaviour
	{
		[Tooltip("Check velocity and update up-face whenever not in motion, continuously. IsStill is only ensured to be useful if this is true.")]
		[FormerlySerializedAs("rolling")] public bool activeRolling = true;
		public int Sides => _faces.Count;
		[SerializeField] 
		private List<DiceFaceOnModel> _faces;
		private Rigidbody _rigidbody;
		public DiceFace currentUpFace;
		//custom editor to choose "1 and highest" or "select face"
		private DiceFace lowestValueFace;
		private DiceFace highestValueFace; 
		[SerializeField] private float stillTime;
		private float _currentstillTime;
		public bool isStill = false;
		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
		}

		public DiceFace GetWorldUpFace()
		{
			return GetClosestAlignedFace(Vector3.up);
		}

		public void RandomizeOrientation()
		{
			transform.rotation = Random.rotation;
		}

		public void AddFace(DiceFaceOnModel face)
		{
			if (face != null)
			{
				if (_faces == null)
				{
					_faces = new List<DiceFaceOnModel>();
				}
				_faces.Add(face);
			}
		}
		private void Update()
		{
			if (activeRolling)
			{
				if (_rigidbody.velocity.sqrMagnitude < 0.01f && _rigidbody.angularVelocity.sqrMagnitude < 0.01f)
				{
					_currentstillTime += Time.deltaTime;
					// _rigidbody.useGravity = true;
					if (_currentstillTime >= stillTime)
					{
						currentUpFace = GetWorldUpFace();
						//is it worth it to cache the orientation and only start checking again if it changes?
						_currentstillTime = 0;
						isStill = true;
						GetComponent<MeshRenderer>().material.color = Color.gray;
					}
				}
				else
				{
					isStill = false;
					_currentstillTime = 0;
					GetComponent<MeshRenderer>().material.color = Color.white;
				}
			}
		}

		public void Roll(Vector3 throwForce, Vector3 spinForce)
		{
			isStill = false;
			currentUpFace = null;
			_rigidbody.AddForce(throwForce,ForceMode.Impulse);
			_rigidbody.AddTorque(spinForce,ForceMode.Impulse);
			_currentstillTime = 0;
		}

		public DiceFace GetClosestAlignedFace(Vector3 direction)
		{
			//var closestAligned = _faces.Select(x => (Vector3.Dot(x.ModelNormal, direction),x)).Min().Item2;
			var dir = transform.InverseTransformDirection(direction);
			var closestAligned = _faces.OrderByDescending(x => Vector3.Dot(x.ModelNormal, dir)).First();
			//current normal, find closest normal from diceFaceOnModel list.
			return closestAligned.Face;
		}

		[ContextMenu("Sort")]
		public void SortFaces()
		{
			_faces.OrderBy(x => x.Face.Value);
		}
	}
}