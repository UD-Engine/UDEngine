using UnityEngine;
using System.Collections;
using UnityEditor;
using UDEngine;
using UDEngine.Core;
using UDEngine.Core.Collision;

[CustomEditor(typeof(UCircleBulletRegionTrigger))]
public class UCircleBulletRegionTriggerEditor : Editor {
	public override void OnInspectorGUI() {
		EditorUtility.SetDirty (target); //ensures repaint on value change
		UCircleBulletRegionTrigger uTrigger = (UCircleBulletRegionTrigger)target;

		if (uTrigger.radius < 0f) {
			uTrigger.radius = 0f;
		}

		uTrigger.centerX = EditorGUILayout.FloatField ("Center X", uTrigger.centerX);
		uTrigger.centerX = EditorGUILayout.FloatField ("Center Y", uTrigger.centerY);
		uTrigger.radius = EditorGUILayout.FloatField ("Width", uTrigger.radius);
	}

	public void OnSceneGUI() {
		UCircleBulletRegionTrigger u_trigger = (UCircleBulletRegionTrigger)target;
		Handles.color = Color.green;
		Handles.DrawWireDisc (new Vector3(u_trigger.centerX, u_trigger.centerY, 0f), Vector3.forward, u_trigger.radius);
	}
}
