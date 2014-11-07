using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TMGTerminalDefinition))]
public class TMGTerminalDefinition_Inspector : Editor {
	
	TMGTerminalDefinition definition;
	
	public void OnEnable() {
		definition = (TMGTerminalDefinition)target;
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Box("Terminal Type " + definition.TypeTag, GUILayout.Width(Screen.width));
		
		definition.TypeTag = EditorGUILayout.TextField("Type Tag", definition.TypeTag);
		
		definition.RequiresUpgrade = EditorGUILayout.Toggle("Requires Upgrade?", definition.RequiresUpgrade);
		if (definition.RequiresUpgrade) {
			EditorGUI.indentLevel = 2;
			definition.UpgradeRequired = EditorGUILayout.TextField("Upgrade Required", definition.UpgradeRequired);
			EditorGUI.indentLevel = 2;
		}
		
		EditorGUILayout.Separator();
		
		definition.RunTime = EditorGUILayout.Slider("Run Time",definition.RunTime, 0.0f,10.0f);
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);	
	}
}
