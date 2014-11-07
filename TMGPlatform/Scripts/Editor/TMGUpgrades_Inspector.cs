using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TMGUpgrades))]
public class TMGUpgrades_Inspector : Editor {
	
	TMGUpgrades upgrades;
		
	bool showUpgradesEditor = true;
	
	public void OnEnable() {
		upgrades = (TMGUpgrades)target;
		
	}
	
	public void OnDisable() {
		
	}
		
	public override void OnInspectorGUI() {
			
		EditorGUILayout.Separator();
		
		Rect rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Upgrades Editor");
		EditorGUI.indentLevel = 0;
		
		EditorGUILayout.LabelField("Upgrades Editor","Configure core upgrade details");
		
		if (upgrades.Upgrades == null) upgrades.Upgrades = new TMGUpgrades._Upgrade[0];
				
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Available Upgrades Editor");
		
		showUpgradesEditor = EditorGUILayout.Foldout(showUpgradesEditor, "Upgrades Configuration");
				
		if (showUpgradesEditor) {
												
			GUILayout.Label("Available Upgrades");
			
			if(GUILayout.Button("Add Upgrade")) {
				ArrayUtility.Add(ref upgrades.Upgrades, new TMGUpgrades._Upgrade());
				GUI.changed = true;
			}
			
			for (int i = 0; i < upgrades.Upgrades.Length; i++) {
				EditorGUILayout.BeginHorizontal("box");
				EditorGUILayout.BeginVertical();		
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this upgrade?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.upgrades.Upgrades, i);
					break;
				}
				
				upgrades.Upgrades[i].IDTag = EditorGUILayout.TextField("Tag", upgrades.Upgrades[i].IDTag);
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginVertical();
				
				upgrades.Upgrades[i].Title = EditorGUILayout.TextField("Title", upgrades.Upgrades[i].Title);
				upgrades.Upgrades[i].Description = EditorGUILayout.TextField("Description", upgrades.Upgrades[i].Description);
				//upgrades.Upgrades[i].RightMainText = EditorGUILayout.TextArea("Main Text", upgrades.Upgrades[i].RightMainText);
				upgrades.Upgrades[i].RightMainText = EditorGUILayout.TextArea(upgrades.Upgrades[i].RightMainText, GUILayout.Height(64));
				upgrades.Upgrades[i].UpgradeTypeName = EditorGUILayout.TextArea(upgrades.Upgrades[i].UpgradeTypeName, GUILayout.Height(64));
				upgrades.Upgrades[i].URL = EditorGUILayout.TextField("URL", upgrades.Upgrades[i].URL);
				
				upgrades.Upgrades[i].LeftLabel = EditorGUILayout.TextField("Left Label", upgrades.Upgrades[i].LeftLabel);
				upgrades.Upgrades[i].RightLabel = EditorGUILayout.TextField("Right Label", upgrades.Upgrades[i].RightLabel);
				upgrades.Upgrades[i].CenterLabel = EditorGUILayout.TextField("Center Label", upgrades.Upgrades[i].CenterLabel);
				
				EditorGUILayout.Separator();
				
				upgrades.Upgrades[i].LeftIcon = EditorGUILayout.TextField("Left Icon", upgrades.Upgrades[i].LeftIcon);
				upgrades.Upgrades[i].RightIcon = EditorGUILayout.TextField("Right Icon", upgrades.Upgrades[i].RightIcon);
				upgrades.Upgrades[i].CenterIcon = EditorGUILayout.TextField("Center Icon", upgrades.Upgrades[i].CenterIcon);
				
				EditorGUILayout.Separator();
				
				EditorGUI.indentLevel = 3;
								
				upgrades.Upgrades[i].AvailableAtLevel = EditorGUILayout.IntField("Available at", upgrades.Upgrades[i].AvailableAtLevel);
				upgrades.Upgrades[i].Cost = EditorGUILayout.FloatField("Cost", upgrades.Upgrades[i].Cost);
				upgrades.Upgrades[i].RequiresUpgrade = EditorGUILayout.TextField("Requires Upgrade", upgrades.Upgrades[i].RequiresUpgrade);
				upgrades.Upgrades[i].RequiresAchievement = EditorGUILayout.TextField("Requires Achievement", upgrades.Upgrades[i].RequiresAchievement);
				EditorGUI.indentLevel = 0;
				
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				
			}
		}
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}
}
