using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SectionController))]
public class SectionControllerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		SectionController sc = (SectionController)target;

		if (GUILayout.Button("Align"))
		{
			sc.AlignSections();
		}
	}
}
