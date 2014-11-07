using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TMGPlayerDefinition))]
public class TMGPlayerDefinition_Inspector : Editor {
	
	TMGPlayerDefinition definition;
	
	private bool showLevelsEditor = true;
	
	public void OnEnable() {
		definition = (TMGPlayerDefinition)target;
		
		if (definition.Levels == null) definition.Levels = new TMGPlayerDefinition._Level[1];
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Box("Player settings", GUILayout.Width(Screen.width));
				
		EditorGUILayout.Separator();
		
		//definition.RunTime = EditorGUILayout.Slider("Run Time",definition.RunTime, 0.0f,10.0f);
		
		// Define levels here
		#region Level Configuration Editor
		Rect rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Levels Editor");
		
		showLevelsEditor = EditorGUILayout.Foldout(showLevelsEditor, "Levels Configuration");
				
		if (showLevelsEditor) {
					
			if(GUILayout.Button("Add Level")) {
				ArrayUtility.Add(ref definition.Levels, new TMGPlayerDefinition._Level());
				GUI.changed = true;
			}
			
			for (int i = 0; i < definition.Levels.Length; i++) {
				EditorGUILayout.BeginHorizontal("box");
				EditorGUILayout.BeginVertical();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this level?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.definition.Levels, i);
					break;
				}
				EditorGUILayout.LabelField("Level " + i);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				EditorGUI.indentLevel = 3;
				
				if (i != 0)
					definition.Levels[i].UpgradeRequired = EditorGUILayout.TextField("Upgrade Required", definition.Levels[i].UpgradeRequired);
				definition.Levels[i].Speed = EditorGUILayout.FloatField("Speed", definition.Levels[i].Speed);
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				
			}
		}
					
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);	
	}
}
