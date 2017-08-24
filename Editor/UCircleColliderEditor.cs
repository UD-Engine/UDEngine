using UnityEngine;
using System.Collections;
using UnityEditor;
using UDEngine;
using UDEngine.Components;
using UDEngine.Components.Collision;

[CustomEditor(typeof(UCircleCollider))]
public class UColliderEditor : Editor {
	public override void OnInspectorGUI() {
		EditorUtility.SetDirty (target); //ensures repaint on value change
		UCircleCollider uCollider = (UCircleCollider)target;

		//Radius
		if (uCollider.radius < 0f) {
			uCollider.radius = 0f; //forcefully avoid negative values
		}
		bool is_radius_over_suggested_max = (uCollider.radius > UCircleCollider.GetSuggestedMaxRadius());
		if (is_radius_over_suggested_max) {
			GUI.color = Color.yellow;
		}
		uCollider.radius = EditorGUILayout.FloatField ("Collider Radius", uCollider.radius);
		GUI.color = Color.white;
		if (is_radius_over_suggested_max) {
			EditorGUILayout.HelpBox ("Exceeding UCollider suggested radius range", MessageType.Warning);
		}
		EditorGUILayout.HelpBox ("UDEngine currently requires all collider to have the shape of circle to ensure good performance. To simulate other shapes, consider multiple circle colliders", MessageType.Info);
	
		// default the .trans prop as its own parent
		// default the .trans prop as its own parent
		if (uCollider.trans == null) {
			if (uCollider.transform.parent != null) {
				uCollider.SetTransform (uCollider.transform.parent);
			} else {
				uCollider.SetTransform (uCollider.transform);
			}
		}
		uCollider.SetTransform(EditorGUILayout.ObjectField("Transform", uCollider.trans, typeof(Transform), true) as Transform);

		uCollider.SetLayer(EditorGUILayout.IntField ("Layer", uCollider.layer));

		bool isEnabled = EditorGUILayout.Toggle ("Enabled", uCollider.IsEnabled());
		uCollider.SetEnable (isEnabled);
	}

	public void OnSceneGUI() {
		UCircleCollider u_collider = (UCircleCollider)target;
		Handles.color = Color.green;
		if (u_collider.trans != null) {
			Handles.DrawWireDisc (u_collider.trans.position, u_collider.trans.forward, u_collider.radius);
		}
	}
}