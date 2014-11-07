using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TMGStationDefinition))]
public class TMGStationDefinition_Inspector : Editor {
	
	TMGStationDefinition definition;
	
	private bool showLevelsEditor = true;
	
	public void OnEnable() {
		definition = (TMGStationDefinition)target;
		
		if (definition.Levels == null) definition.Levels = new TMGStationDefinition._Level[1];
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Box("Station Type " + definition.TypeTag, GUILayout.Width(Screen.width));
		
		definition.TypeTag = EditorGUILayout.TextField("Type Tag", definition.TypeTag);
		
		definition.RequiresUpgrade = EditorGUILayout.Toggle("Requires Upgrade?", definition.RequiresUpgrade);
		if (definition.RequiresUpgrade) {
			EditorGUI.indentLevel = 2;
			definition.UpgradeRequired = EditorGUILayout.TextField("Upgrade Required", definition.UpgradeRequired);
			EditorGUI.indentLevel = 2;
		}
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.Separator();
		
		definition.MinimumLevel = EditorGUILayout.IntField("Minimum Level", definition.MinimumLevel);
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.Separator();
		
		definition.HasAssistant = EditorGUILayout.Toggle("Has Assistant", definition.HasAssistant);
				
		if (definition.HasAssistant) {
			EditorGUI.indentLevel = 2;
			definition.AssistantRequiresUpgrade = EditorGUILayout.Toggle("Requires Upgrade", definition.AssistantRequiresUpgrade);
					
			if (definition.AssistantRequiresUpgrade) {
				definition.AssistantUpgradeRequired = EditorGUILayout.TextField("Upgrade Required", definition.AssistantUpgradeRequired);	
			}
			EditorGUI.indentLevel = 0;
		}
		
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.Separator();
		
		definition.HasParent = EditorGUILayout.Toggle("Has Parent", definition.HasParent);
				
		if (definition.HasParent) {
			EditorGUI.indentLevel = 2;
			definition.Parent = EditorGUILayout.TextField("Parent", definition.Parent);	
			
			EditorGUI.indentLevel = 0;
		}
		
		EditorGUILayout.Separator();
		
		EditorGUILayout.Separator();
		
		EditorGUI.indentLevel = 2;	
						
		//definition.RunTime = EditorGUILayout.Slider("Run Time",definition.RunTime, 0.0f,10.0f);
		
		// Define levels here
		#region Level Configuration Editor
		Rect rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Levels Editor");
		
		showLevelsEditor = EditorGUILayout.Foldout(showLevelsEditor, "Levels Configuration");
				
		if (showLevelsEditor) {
					
			if(GUILayout.Button("Add Level")) {
				ArrayUtility.Add(ref definition.Levels, new TMGStationDefinition._Level());
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
				
				definition.Levels[i].DescriptionTitle = EditorGUILayout.TextField("Title", definition.Levels[i].DescriptionTitle);
				definition.Levels[i].DescriptionText = EditorGUILayout.TextField("Description", definition.Levels[i].DescriptionText);
				definition.Levels[i].StationUpgradeType = (TMGStationUpgradeType)EditorGUILayout.EnumPopup("Type", definition.Levels[i].StationUpgradeType);
				definition.Levels[i].StationUpgradeLevel = EditorGUILayout.IntField("Level", definition.Levels[i].StationUpgradeLevel);
				
				if (i != 0)
					definition.Levels[i].UpgradeRequired = EditorGUILayout.TextField("Upgrade Required", definition.Levels[i].UpgradeRequired);
				definition.Levels[i].RunTime = EditorGUILayout.FloatField("Run Time", definition.Levels[i].RunTime);
				definition.Levels[i].MinigameChance = EditorGUILayout.Slider("Minigame Chance", definition.Levels[i].MinigameChance, 0.0f,1.0f);
				definition.Levels[i].PatienceBonus = EditorGUILayout.FloatField("Patience Bonus", definition.Levels[i].PatienceBonus);
				definition.Levels[i].Positions = EditorGUILayout.IntField("Positions", definition.Levels[i].Positions);
				
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
