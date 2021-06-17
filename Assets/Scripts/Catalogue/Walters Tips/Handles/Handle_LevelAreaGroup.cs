
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class Handle_LevelAreaGroup : MonoBehaviour
{
	[Serializable]
	private class LevelArea
	{
		public string name;
		public Color color;

		public Vector3 offset = Vector3.zero;
		public Vector2 size = Vector2.one;
	}

	[SerializeField] private LevelArea[] levelAreas;

	#region Custom Editor

	//Don't forget to put the #if UNITY_EDITOR checks, otherwise you can't build.
#if UNITY_EDITOR
	[CustomEditor(typeof(Handle_LevelAreaGroup))]
	private sealed class LevelAreaGroupEditor : Editor
	{
		private Handle_LevelAreaGroup _areaGroup;

		private void OnEnable()
		{
			_areaGroup = (Handle_LevelAreaGroup)target;
		}

		private void OnSceneGUI()
		{
			if (_areaGroup == null) return;

			if (_areaGroup.levelAreas == null) return;

			for (int __index = 0; __index < _areaGroup.levelAreas.Length; __index++)
			{
				_areaGroup.levelAreas[__index] = DrawOffset(__index);
				_areaGroup.levelAreas[__index] = DrawName(__index);
				_areaGroup.levelAreas[__index] = DrawSize(__index);
			}
		}

		private LevelArea DrawName(in int areaIndex)
		{
			LevelArea __area = _areaGroup.levelAreas[areaIndex];

			Handles.color = Color.white;

			Vector3 __pos = (_areaGroup.transform.position + __area.offset);

			Handles.Label(
				position: __pos,
				text: $"Level Area: \n" +
					  $"{__area.name}");

			return __area;
		}
		private LevelArea DrawOffset(in int areaIndex)
		{
			LevelArea __area = _areaGroup.levelAreas[areaIndex];

			Handles.color = __area.color;

			Vector3 __pos = (_areaGroup.transform.position + __area.offset);

			Vector3 __offsetPos = Handles.FreeMoveHandle(
				position: __pos,
				rotation: Quaternion.identity,
				size: .1f,
				snap: new Vector3(.5f, .5f, .5f),
				capFunction: Handles.DotHandleCap);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Free Move LookAt Point");
				__area.offset = (__offsetPos - _areaGroup.transform.position);
			}

			return __area;
		}
		private LevelArea DrawSize(in int areaIndex)
		{
			LevelArea __area = _areaGroup.levelAreas[areaIndex];
			Handles.color = __area.color;

			Vector3 __pos = (_areaGroup.transform.position + __area.offset);

			Vector3 __horizontal = new Vector3(
				x: __pos.x + __area.size.x / 2.0f,
				y: __pos.y,
				z: __pos.z);

			Vector3 __vertical = new Vector3(
				x: __pos.x,
				y: __pos.y + __area.size.y / 2.0f,
				z: __pos.z);

			Rect __rect = new Rect(position: __pos - (Vector3)(__area.size / 2f), __area.size);

			Handles.DrawSolidRectangleWithOutline(
				rectangle: __rect,
				faceColor: new Color(0.5f, 0.5f, 0.5f, 0.05f),
				outlineColor: new Color(0, 0, 0, 1));

			__area.size.x = Mathf.Abs(Handles.ScaleValueHandle(
				value: __area.size.x,
				position: __horizontal,
				rotation: Quaternion.identity,
				size: 0.5f,
				capFunction: Handles.RectangleHandleCap,
				snap: 1.0f));

			__area.size.y = Mathf.Abs(Handles.ScaleValueHandle(
				value: __area.size.y,
				position: __vertical,
				rotation: Quaternion.identity,
				size: 0.5f,
				capFunction: Handles.RectangleHandleCap,
				snap: 1.0f));

			return __area;
		}

	}
#endif

	#endregion
}