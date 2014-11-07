using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TMGMinigameDefinition))]
public class TMGMinigameDefinition_Inspector : Editor {
	
	TMGMinigameDefinition defintion;
	
	public void OnEnable() {
		defintion = (TMGMinigameDefinition)target;
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Box("Minigame Type " + defintion.TypeTag, GUILayout.Width(Screen.width));
		
		defintion.TypeTag = EditorGUILayout.TextField("Type Tag", defintion.TypeTag);
		
		EditorGUILayout.Separator();
		
		defintion.SuccessBonus = EditorGUILayout.Slider("Success Bonus",defintion.SuccessBonus, 0.0f,10.0f);
		defintion.FailurePenalty = EditorGUILayout.Slider("Failure Penalty",defintion.FailurePenalty, 0.0f,10.0f);
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);	
	}
}
