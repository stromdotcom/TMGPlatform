using UnityEngine;
using System.Collections;

public class TMGPlayerDefinition : MonoBehaviour {
	
	[System.Serializable]
	public class _Level {
		public string UpgradeRequired = "";
		public float Speed = 5.0f;
	}
			
	public _Level[] Levels;
}
