using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TMGPuzzles))]
public class TMGPuzzles_Inspector : Editor {
	
	TMGPuzzles puzzles;
			
	public void OnEnable() {
		puzzles = (TMGPuzzles)target;
		
		if (puzzles.Puzzles == null) {
			puzzles.Puzzles = new TMGPuzzles._Puzzle[1];
			//puzzles.Puzzles[0] = new TMGPuzzles._Puzzle();
		}
		for (int i = 0; i < puzzles.Puzzles.Length; i++) if (puzzles.Puzzles[i].Achievements == null) {
			puzzles.Puzzles[i].Achievements = new TMGPuzzles._Achievement[1];
			//puzzles.Puzzles[i].Achievements[0] = new TMGPuzzles._Achievement();
		}
	}
	
	public void OnDisable() {
		
	}
		
	public override void OnInspectorGUI() {
		
		GUILayout.Label("Available Puzzles");
			
		if(GUILayout.Button("Add Puzzle")) {
			ArrayUtility.Add(ref this.puzzles.Puzzles, new TMGPuzzles._Puzzle());
			GUI.changed = true;
		}
			
		for (int i = 0; i < puzzles.Puzzles.Length; i++) {
			EditorGUILayout.BeginHorizontal("box");
			EditorGUILayout.BeginVertical();		
			EditorGUILayout.BeginHorizontal();
			
			EditorGUI.indentLevel = 0;
			if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this puzzle?", "Yes", "No")) {
				ArrayUtility.RemoveAt(ref this.puzzles.Puzzles, i);
				break;
			}
				
			EditorGUILayout.BeginVertical();		
			puzzles.Puzzles[i].IDTag = EditorGUILayout.TextField("Tag", puzzles.Puzzles[i].IDTag);
			puzzles.Puzzles[i].Title = EditorGUILayout.TextField("Title", puzzles.Puzzles[i].Title);
			EditorGUILayout.EndVertical();		
			
			EditorGUILayout.EndHorizontal();
			
						
			if(GUILayout.Button("Add Achievement")) {
				ArrayUtility.Add(ref puzzles.Puzzles[i].Achievements, new TMGPuzzles._Achievement());
				GUI.changed = true;
			}
			
			for (int j = 0; j < puzzles.Puzzles[i].Achievements.Length; j++) {
				EditorGUILayout.BeginHorizontal();
				EditorGUI.indentLevel = 2;
				
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this achievement?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.puzzles.Puzzles[i].Achievements, j);
					break;	
				}		
				
				EditorGUILayout.BeginVertical();
				EditorGUI.indentLevel = 4;
				if (this.puzzles.Puzzles[i].Achievements[j] == null) { 
					this.puzzles.Puzzles[i].Achievements[j] = new TMGPuzzles._Achievement();
				}
				
				this.puzzles.Puzzles[i].Achievements[j].IDTag = EditorGUILayout.TextField("Tag", puzzles.Puzzles[i].Achievements[j].IDTag);
				this.puzzles.Puzzles[i].Achievements[j].Type = (TMGPuzzleAchievementType)EditorGUILayout.EnumPopup("Type", puzzles.Puzzles[i].Achievements[j].Type);
				
				EditorGUILayout.Separator();
				
				if (this.puzzles.Puzzles[i].Achievements[j].Type == TMGPuzzleAchievementType.Combo) {
					this.puzzles.Puzzles[i].Achievements[j].NumberOfCombosRequired = EditorGUILayout.IntField("Combo Number", this.puzzles.Puzzles[i].Achievements[j].NumberOfCombosRequired);
					this.puzzles.Puzzles[i].Achievements[j].ComboTypeRequired = EditorGUILayout.IntField("Combo Type", this.puzzles.Puzzles[i].Achievements[j].ComboTypeRequired);
					
					this.puzzles.Puzzles[i].Achievements[j].UnlocksAchievement = EditorGUILayout.TextField("Unlocks Achievement", this.puzzles.Puzzles[i].Achievements[j].UnlocksAchievement);
				} else if (this.puzzles.Puzzles[i].Achievements[j].Type == TMGPuzzleAchievementType.Score) {
					this.puzzles.Puzzles[i].Achievements[j].ScoreRequired = EditorGUILayout.IntField("Score Required", this.puzzles.Puzzles[i].Achievements[j].ScoreRequired);
					this.puzzles.Puzzles[i].Achievements[j].CreditReward = EditorGUILayout.IntField("Reward", this.puzzles.Puzzles[i].Achievements[j].CreditReward);
				} else if (this.puzzles.Puzzles[i].Achievements[j].Type == TMGPuzzleAchievementType.Color) {
					this.puzzles.Puzzles[i].Achievements[j].ColorRequired = EditorGUILayout.TextField("Color Required", this.puzzles.Puzzles[i].Achievements[j].ColorRequired);
					this.puzzles.Puzzles[i].Achievements[j].ColorCountRequired = EditorGUILayout.IntField("Count", this.puzzles.Puzzles[i].Achievements[j].ColorCountRequired);
					
					this.puzzles.Puzzles[i].Achievements[j].UnlocksAchievement = EditorGUILayout.TextField("Unlocks Achievement", this.puzzles.Puzzles[i].Achievements[j].UnlocksAchievement);
				}
				
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			
			
			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		
			this.puzzles.Puzzles[i].time = EditorGUILayout.FloatField("Time", this.puzzles.Puzzles[i].time);
		}
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);
	}
}
