using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class Handle_Explosion : MonoBehaviour
{
	#region Fields

	[SerializeField] private float areaOfEffect = 10.0f;

	#endregion

	#region Properties

	private (Transform[], int colliderCount) CollidersInAreaOfEffect
	{
		get
		{
			const int __MAX_OBJECTS = 50;

			Collider[] __hitColliders = new Collider[__MAX_OBJECTS];

			int __colliderCount = Physics.OverlapSphereNonAlloc(position: transform.position, radius: areaOfEffect, results: __hitColliders);

			Transform[] __hitTransforms = new Transform[__colliderCount];

			for (int __index = 0; __index < __colliderCount; __index++)
			{
				__hitTransforms[__index] = __hitColliders[__index].transform;
			}

			__hitTransforms = __hitTransforms.OrderBy(keySelector: a => Vector3.Distance(a: a.position, b: transform.position)).ToArray(); //Sort by distance.

			return (__hitTransforms, __colliderCount);
		}
	}

	#endregion

	#region Custom Editor

	//Don't forget to put the #if UNITY_EDITOR checks, otherwise you can't build.
#if UNITY_EDITOR
	[CustomEditor(typeof(Handle_Explosion))]
	private sealed class ExplosionEditor : Editor
	{
		private Handle_Explosion _explosion;

		private void OnEnable() => _explosion = (Handle_Explosion)target;

		private void OnSceneGUI()
		{
			if (_explosion == null) return;

			DrawRadiusHandles();
			DrawDistances();
		}

		private void DrawRadiusHandles()
		{
			Handles.color = Color.cyan;

			Transform __explosionTransform = _explosion.transform;

			EditorGUI.BeginChangeCheck();
			float __areaOfEffect = Handles.RadiusHandle(Quaternion.identity, __explosionTransform.position, _explosion.areaOfEffect);

			if (!EditorGUI.EndChangeCheck()) return; //Record changes so you can undo them.

			Undo.RecordObject(target, name: "Changed Explosion 'Area Of Effect'. ");
			_explosion.areaOfEffect = __areaOfEffect;
		}

		private void DrawDistances()
		{
			(Transform[] __transforms, int __colliderCount) = _explosion.CollidersInAreaOfEffect;

			if (__colliderCount == 0) return;

			Transform __explosionTransform = _explosion.transform;

			float __step = 1.0f / __colliderCount;

			for (int __index = 0; __index < __colliderCount; __index++)
			{
				Handles.color = Color.Lerp(a: Color.green, b: Color.red, t: __index * __step);

				Handles.DrawDottedLine(
					p1: __explosionTransform.position,
					p2: __transforms[__index].position,
					screenSpaceSize: 5.0f);
			}
		}

	}
#endif

	#endregion

}