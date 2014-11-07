using UnityEngine;
using System.Collections;

public class TMGRegisterDefinition : MonoBehaviour {
	
	
	[System.Serializable]
	public class _Level {
		
		public string UpgradeRequired = "";
				
		public float PatienceDecayMultiplier = 1.0f;
		
	}
		
	public _Level[] Levels;
}
