using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UDEngine.Interface;
using UDEngine.Internal;
using UDEngine.Core;
using UDEngine.Core.Actor;
using UDEngine.Core.Collision;

[CustomEditor(typeof(UCollisionMonitor))]
public class UCollisionMonitorEditor : Editor {
	public override void OnInspectorGUI() {
		EditorUtility.SetDirty (target); //ensures repaint on value change
		UCollisionMonitor uCollisionMonitor = (UCollisionMonitor) target;

		uCollisionMonitor.boundXMin = EditorGUILayout.FloatField ("X Min", uCollisionMonitor.boundXMin);
		uCollisionMonitor.boundYMin = EditorGUILayout.FloatField ("Y Min", uCollisionMonitor.boundYMin);
		uCollisionMonitor.boundWidth = EditorGUILayout.FloatField ("Width", uCollisionMonitor.boundWidth);
		uCollisionMonitor.boundHeight = EditorGUILayout.FloatField ("Height", uCollisionMonitor.boundHeight);
	}

	public void OnSceneGUI() {
		UCollisionMonitor uCollisionMonitor = (UCollisionMonitor)target;
		Handles.color = Color.green;

		Handles.DrawWireCube(new Vector3(uCollisionMonitor.boundXMin + (uCollisionMonitor.boundWidth / 2f), uCollisionMonitor.boundYMin + (uCollisionMonitor.boundHeight / 2f)), 
			new Vector3(uCollisionMonitor.boundWidth, uCollisionMonitor.boundHeight, 0f));
	}
}
