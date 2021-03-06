﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using UDEngine;
using UDEngine.Core;
using UDEngine.Core.Actor;
using UDEngine.Core.Collision;
using UDEngine.Core.Bullet;

[CustomEditor(typeof(UBulletCollider))]
public class UBulletColliderEditor : Editor {
	public override void OnInspectorGUI() {
		EditorUtility.SetDirty (target); //ensures repaint on value change
		UBulletCollider uCollider = (UBulletCollider)target;

		//Radius
		if (uCollider.radius < 0f) {
			uCollider.radius = 0f; //forcefully avoid negative values
		}
		bool is_radius_over_suggested_max = (uCollider.radius > UBulletCollider.GetSuggestedMaxRadius());
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
		if (uCollider.trans == null) {
			if (uCollider.transform.parent != null) {
				uCollider.SetTransform (uCollider.transform.parent);
			} else {
				uCollider.SetTransform (uCollider.transform);
			}
		}
		uCollider.SetTransform(EditorGUILayout.ObjectField("Transform", uCollider.trans, typeof(Transform), true) as Transform);

		uCollider.bulletObject = EditorGUILayout.ObjectField ("Bullet Object", uCollider.bulletObject, typeof(UBulletObject), true) as UBulletObject;

		uCollider.actor = EditorGUILayout.ObjectField ("Actor", uCollider.actor, typeof(UBulletActor), true) as UBulletActor;

		uCollider.SetLayer(EditorGUILayout.IntField ("Layer", uCollider.layer));

		uCollider.SetEnable (EditorGUILayout.Toggle ("Enabled", uCollider.IsEnabled()));

		uCollider.SetRecyclable (EditorGUILayout.Toggle ("Recyclable", uCollider.IsRecyclable ()), false);
	}

	public void OnSceneGUI() {
		UBulletCollider u_collider = (UBulletCollider)target;
		Handles.color = Color.green;
		if (u_collider.trans != null) {
			Handles.DrawWireDisc (u_collider.trans.position, u_collider.trans.forward, u_collider.radius);
		}
	}
}
