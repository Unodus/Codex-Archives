using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]

public class Snappable : MonoBehaviour
{

		//TODO: Snappable types, where only certain combinations can connect. (connectsWith dropdown)

		[Serializable]
		public struct Point
		{
			public Vector3 position;
			public Vector3 direction;
		}

		public Point[] LocalPoints = new Point[0];


		public Vector3[] WorldPoints
		{
			get
			{
				Vector3[] __worldPoints = new Vector3[LocalPoints.Length];

				for (int i = 0; i < LocalPoints.Length; i++)
				{
					//__worldPoints[i] = (points[i] + transform.position);
					__worldPoints[i] = transform.TransformPoint(LocalPoints[i].position);
				}

				return __worldPoints;
			}
		}

		[Tooltip(tooltip: "The max radius (From point to point) it will snap.")]
		public float snapRadius = 5.0f;

		//Some code stolen from my Multitons, didn't wanna use those as I'm going to re-engineer them very soon.

		#region Instancing

		 public static List<Snappable> Instances = new List<Snappable>();


		public Snappable this[in int i]
		{
			get => Instances[i];
			set => Instances[i] = value;
		}

		private bool _initialized = false;

		///<summary> Adds the Instance to <see cref="Instances"/>.</summary>
		private void Initialize()
		{
			//Don't add prefabs, please thanks.
			//if(PrefabCheckHelper.CheckIfPrefab(this)) return;

			if (this._initialized) return;

			if (Instances.Contains(item: this)) return;

			Instances.Add(item: this);
			this._initialized = true;
		}

		///<summary> Removes the Instance to <see cref="Instances"/>.</summary>
		private void DeInitialize()
		{
			Instances.Remove(item: this);

			this._initialized = false;
		}


		#endregion

		#region Initialization

		private void Reset() => Initialize();
		private void OnEnable() => Initialize();
		private void OnDisable() => DeInitialize();

		private void Update()
		{
#if UNITY_EDITOR

			Initialize();

#endif
		}

		#endregion

		private void OnDrawGizmosSelected()
		{
			foreach (Snappable __instance in Instances)
			{
				Gizmos.matrix = __instance.transform.localToWorldMatrix;

				foreach (Point __point in __instance.LocalPoints)
				{
					Gizmos.DrawRay(from: __point.position, direction: __point.direction.normalized * 0.5f);
				}
			}
		}
	}

