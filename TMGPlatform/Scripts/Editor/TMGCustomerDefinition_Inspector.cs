using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TMGCustomerDefinition))]
public class TMGCustomerDefinition_Inspector : Editor {
	
	TMGCustomerDefinition defintion;
	
	public void OnEnable() {
		defintion = (TMGCustomerDefinition)target;
	}
	
	public override void OnInspectorGUI() {
		GUILayout.Box("Customer Type " + defintion.TypeTag, GUILayout.Width(Screen.width));
		
		defintion.TypeTag = EditorGUILayout.TextField("Type Tag", defintion.TypeTag);
		
		EditorGUILayout.Separator();
		
		defintion.Patience = EditorGUILayout.Slider("Patience",defintion.Patience, 0.0f,10.0f);
		defintion.MaxPatience = EditorGUILayout.Slider("Max Patience",defintion.MaxPatience, defintion.Patience,10.0f);
		defintion.PatienceDecay = EditorGUILayout.Slider("Patience Decay",defintion.PatienceDecay, 0.0f,1.0f);
		
		EditorGUILayout.Separator();
		
		defintion.PatienceCheckoutMultiplier = EditorGUILayout.FloatField("Checkout Multiplier", defintion.PatienceCheckoutMultiplier);
		defintion.PatienceCheckoutBonus = EditorGUILayout.FloatField("Checkout Bonus", defintion.PatienceCheckoutBonus);
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);	
	}
}
