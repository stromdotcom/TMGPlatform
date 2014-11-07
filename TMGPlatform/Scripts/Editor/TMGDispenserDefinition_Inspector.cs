using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TMGDispenserDefinition))]
public class TMGDispenserDefinition_Inspector : Editor {
	
	TMGDispenserDefinition definition;
	
	public void OnEnable() {
		definition = (TMGDispenserDefinition)target;
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Box("Dispenser Type " + definition.TypeTag, GUILayout.Width(Screen.width));
		
		definition.TypeTag = EditorGUILayout.TextField("Type Tag", definition.TypeTag);
		
		definition.RequiresUpgrade = EditorGUILayout.Toggle("Requires Upgrade?", definition.RequiresUpgrade);
		if (definition.RequiresUpgrade) {
			EditorGUI.indentLevel = 2;
			definition.UpgradeRequired = EditorGUILayout.TextField("Upgrade Required", definition.UpgradeRequired);
			EditorGUI.indentLevel = 2;
		}
		
		EditorGUILayout.Separator();
		
		definition.WarmupTime = EditorGUILayout.Slider("Warmup Time",definition.WarmupTime, 0.0f,60.0f);
		definition.CooldownTime = EditorGUILayout.Slider("Cooldown Time",definition.CooldownTime, 0.0f,60.0f);
		definition.RunTime = EditorGUILayout.Slider("Run Time",definition.RunTime, 0.0f,60.0f);
		
		definition.Units = EditorGUILayout.IntField("Units",definition.Units);
		definition.MaxUnits = EditorGUILayout.IntField("Max Units",definition.MaxUnits);
		definition.UnitsPerCompletion = EditorGUILayout.IntField("Units per Completion",definition.UnitsPerCompletion);
		
		definition.PatienceBonus = EditorGUILayout.FloatField("Patience Bonus", definition.PatienceBonus);
		
		definition.IsAutomatic = EditorGUILayout.Toggle("Automatic", definition.IsAutomatic);
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);	
	}
}
