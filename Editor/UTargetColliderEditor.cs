using UnityEngine;
using System.Collections;
using UnityEditor;
using UDEngine;
using UDEngine.Components;
using UDEngine.Components.Collision;

[CustomEditor(typeof(UTargetCollider))]
public class UTargetColliderEditor : Editor {
	public override void OnInspectorGUI() {
		EditorUtility.SetDirty (target); //ensures repaint on value change
		UTargetCollider uCollider = (UTargetCollider)target;

		//Radius
		if (uCollider.radius < 0f) {
			uCollider.radius = 0f; //forcefully avoid negative values
		}
		bool is_radius_over_suggested_max = (uCollider.radius > UTargetCollider.GetSuggestedMaxRadius());
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

		uCollider.SetEnable (EditorGUILayout.Toggle ("Enabled", uCollider.IsEnabled()));
	}

	public void OnSceneGUI() {
		UTargetCollider u_collider = (UTargetCollider)target;
		Handles.color = Color.green;
		if (u_collider.trans != null) {
			Handles.DrawWireDisc (u_collider.trans.position, u_collider.trans.forward, u_collider.radius);
		}
	}
}

