using UnityEngine;
using System.Collections;

public enum TMGStationUpgradeType {Default, Service, Efficiency, Assistant, Position};

public class TMGStationDefinition : MonoBehaviour {
	
	
	[System.Serializable]
	public class _Level {
		
		public string UpgradeRequired = "";
				
		public float RunTime = 5.0f;
		public float MinigameChance = 1.0f;
		public float PatienceBonus = 1.0f;
		
		public int Positions = 1;
		
		public string DescriptionTitle = "";
		public string DescriptionText = "";
		
		public TMGStationUpgradeType StationUpgradeType = TMGStationUpgradeType.Default;
		public int StationUpgradeLevel = 0;
	}
			
	public string TypeTag = "";
	public bool RequiresUpgrade = false;
	public string UpgradeRequired = "";
	
	public int MinimumLevel = 0;

	public bool HasAssistant = false;
	public bool AssistantRequiresUpgrade = false;
	public string AssistantUpgradeRequired = "";
	//public float RunTime = 5.0f;

	public bool HasParent = false;
	public string Parent = "";
	
	public _Level[] Levels;
}
