using UnityEngine;
using System.Collections;
using UnityEditor;
using UDEngine;
using UDEngine.Core;
using UDEngine.Core.Collision;

[CustomEditor(typeof(URectBulletRegionTrigger))]
public class URectBulletRegionTriggerEditor : Editor {
	public override void OnInspectorGUI() {
		EditorUtility.SetDirty (target); //ensures repaint on value change
		URectBulletRegionTrigger uTrigger = (URectBulletRegionTrigger)target;

		if (uTrigger.width < 0f) {
			uTrigger.width = 0f;
		}

		if (uTrigger.height < 0f) {
			uTrigger.height = 0f;
		}

		uTrigger.xMin = EditorGUILayout.FloatField ("X Min", uTrigger.xMin);
		uTrigger.yMin = EditorGUILayout.FloatField ("Y Min", uTrigger.yMin);
		uTrigger.width = EditorGUILayout.FloatField ("Width", uTrigger.width);
		uTrigger.height = EditorGUILayout.FloatField ("Height", uTrigger.height);
	}

	public void OnSceneGUI() {
		URectBulletRegionTrigger u_trigger = (URectBulletRegionTrigger)target;
		Handles.color = Color.green;

		Handles.DrawWireCube(new Vector3(u_trigger.xMin + (u_trigger.width / 2f), u_trigger.yMin + (u_trigger.height / 2f)), 
			new Vector3(u_trigger.width, u_trigger.height, 0f));
	}
}
