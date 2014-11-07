using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TMGDeviceDefinition))]
public class TMGDeviceDefinition_Inspector : Editor {
	
	TMGDeviceDefinition definition;
	
	public void OnEnable() {
		definition = (TMGDeviceDefinition)target;
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Box("Device Type " + definition.TypeTag, GUILayout.Width(Screen.width));
		
		definition.TypeTag = EditorGUILayout.TextField("Type Tag", definition.TypeTag);
		
		definition.RequiresUpgrade = EditorGUILayout.Toggle("Requires Upgrade?", definition.RequiresUpgrade);
		if (definition.RequiresUpgrade) {
			EditorGUI.indentLevel = 2;
			definition.UpgradeRequired = EditorGUILayout.TextField("Upgrade Required", definition.UpgradeRequired);
			EditorGUI.indentLevel = 2;
		}
		
		EditorGUILayout.Separator();
		
		definition.WarmupTime = EditorGUILayout.Slider("Warmup Time",definition.WarmupTime, 0.0f,10.0f);
		definition.CooldownTime = EditorGUILayout.Slider("Cooldown Time",definition.CooldownTime, 0.0f,10.0f);
		definition.RunTime = EditorGUILayout.Slider("Run Time",definition.RunTime, 0.0f,10.0f);
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);	
	}
}
