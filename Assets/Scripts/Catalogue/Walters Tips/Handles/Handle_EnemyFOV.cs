using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class Handle_EnemyFOV : MonoBehaviour
{
	#region Fields

	[Tooltip(tooltip: "Field Of View in degrees")]
	[SerializeField] private float fov = 80.0f;

	[Tooltip(tooltip: "Maximum Checking Distance for the player.")]
	[SerializeField] private float maxCheckDistance = 9.0f;

	#endregion

	#region Properties

	/// <summary> Checks if the enemy can see the player at all. </summary>
	private bool IsSeeingPlayer
	{
		get
		{
			//Note: this is kinda a crappy way of doing it, because we only check for the centre of the character, not the actual mesh or bounds.

			Transform __enemyTransform = this.transform;
			Vector3 __enemyPosition = __enemyTransform.position;

			Vector3 __playerDirection = Player.position - __enemyPosition;

			if (__playerDirection.magnitude >= maxCheckDistance) return false; //Distance to player is too far away.

			float __angleToPlayer = Vector3.Angle(from: __playerDirection.normalized, to: __enemyTransform.right);

			Debug.DrawRay(start: __enemyPosition, dir: __playerDirection, color: Color.yellow); //Direction to player.
			Debug.DrawRay(start: __enemyPosition, dir: __enemyTransform.right * __playerDirection.magnitude, color: Color.cyan); //Enemy.right direction.

			float __detectionAngle = (fov / 2.0f);

			return ((__angleToPlayer >= -__detectionAngle) && (__angleToPlayer <= __detectionAngle)); // -45 to 45 with a 90° FOV
		}
	}

	private Transform _cachedPlayer;
	private Transform Player => _cachedPlayer ??= GameObject.FindWithTag(tag: "Player").transform;

	#endregion

	#region Custom Editor

	//Don't forget to put the #if UNITY_EDITOR checks, otherwise you can't build.
#if UNITY_EDITOR
	[CustomEditor(typeof(Handle_EnemyFOV))]
	private sealed class EnemyCharacterEditor : Editor
	{
		private Handle_EnemyFOV _enemyCharacter;

		private void OnEnable() => _enemyCharacter = (Handle_EnemyFOV)target;

		private void OnSceneGUI()
		{
			if (_enemyCharacter == null) return;

			Transform __enemyTransform = _enemyCharacter.transform;

			//Colour based on whether the enemy can see the player or not.
			Handles.color = _enemyCharacter.IsSeeingPlayer
				? new Color(r: 0, g: 255, b: 0, a: 0.1f)  //green 
				: new Color(r: 255, g: 0, b: 0, a: 0.1f); //red

			Handles.DrawSolidArc(
				center: __enemyTransform.position,
				normal: __enemyTransform.forward,
				from: Quaternion.Euler(x: 0, y: 0, z: -(_enemyCharacter.fov / 2.0f)) * __enemyTransform.right,
				angle: _enemyCharacter.fov,
				radius: _enemyCharacter.maxCheckDistance);
		}

	}
#endif

	#endregion

}