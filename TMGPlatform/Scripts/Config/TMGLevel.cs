using UnityEngine;
using System.Collections;

public class TMGLevel : MonoBehaviour {
	
	[System.Serializable]
	public class _Player {
		public Vector3 StartPosition = new Vector3(0,0,0);
		public Vector3 ReadyPosition = new Vector3(0,0,0);
	}
	
	[System.Serializable]
	public class _Station {
		public string IDTag ="";
		public string TypeTag ="";
		public string ParentTag = "";
		
		public bool OverrideDefaultSettings = false;
		public float RunTime = 5.0f;
		public float MinigameChance = 1.0f;
		public float PatienceBonus = 1.0f;
		
		public bool IsAutomatic = false;
		public bool HasAssistant = false;
		public bool AssistantRequiresUpgrade = false;
		public Vector3 AssistantPosition = new Vector3(0,0,0);
		public string AssistantUpgradeRequired = "";
		
		public int BaseUpgradeLevel = 0;
		
		public bool RequiresUpgrade = false;
		public string UpgradeRequired = "";
		
		public Vector3 Position = new Vector3(0,0,0);
		public Vector3 ActivationPosition = new Vector3(0,0,0);
		public Vector3 CustomerPosition = new Vector3(0,0,0);
	}
	
	[System.Serializable]
	public class _Terminal {
		public string IDTag ="";
		public string TypeTag ="";
		
		public bool OverrideDefaultSettings = false;
		public float RunTime = 5.0f;
		
		public bool IsAutomatic = false;
		
		public int BaseUpgradeLevel = 0;
		
		public bool RequiresUpgrade = false;
		public string UpgradeRequired = "";
		
		public Vector3 Position = new Vector3(0,0,0);
	}
	
	[System.Serializable]
	public class _Device {
		public string IDTag ="";
		public string TypeTag ="";
		
		public bool OverrideDefaultSettings = false;
		public float WarmupTime = 5.0f;
		public float CooldownTime = 5.0f;
		public float RunTime = 5.0f;
		
		public int BaseUpgradeLevel = 0;
		
		public bool RequiresUpgrade = false;
		public string UpgradeRequired = "";
		
		public Vector3 Position = new Vector3(0,0,0);
	}
	
	[System.Serializable]
	public class _Dispenser {
		public string IDTag ="";
		public string TypeTag ="";
		
		public bool OverrideDefaultSettings = false;
		public float WarmupTime = 5.0f;
		public float CooldownTime = 5.0f;
		public float RunTime = 5.0f;
		
		public int Units = 0;
		public int MaxUnits = 0;
		public int UnitsPerCompletion = 0;
		
		public int BaseUpgradeLevel = 0;
		
		public bool RequiresUpgrade = false;
		public string UpgradeRequired = "";
		
		public Vector3 Position = new Vector3(0,0,0);
	}
	
	[System.Serializable]
	public class _Customer {
		public string IDTag = "";
		public string TypeTag = "";
		
		public bool OverrideDefaultSettings = false;
		public float Patience = 5.0f;
		public float MaxPatience = 5.0f;
		public float PatienceDecay = 0.1f;
		
		public float PatienceCheckoutMultiplier = 1.0f;
		public float PatienceCheckoutBonus = 0.0f;
		
		public float EntryDelay = 0.0f;
		
		public string[] StationsRequested = new string[] {};
		
		public Vector3 Position = new Vector3(0,0,0);
	}
	
	[System.Serializable]
	public class _Node {
		public string IDTag = "";
		
		public Vector3 Position = new Vector3(0,0,0);
		
		public string Connections = "";
	}
	
	[System.Serializable]
	public class _RegisterPosition {
		public bool RequiresUpgrade = false;
		public string UpgradeRequired = "";
		
		public Vector3 Position = new Vector3(0,0,0);
	}
	
	[System.Serializable]
	public class _WaitingPosition {
		public bool RequiresUpgrade = false;
		public string UpgradeRequired = "";
		
		public Vector3 Position = new Vector3(0,0,0);
	}
	
	#region Public Properties
	public string Locale = "";
	public string LevelName = "";
	public float RunTime = 60.0f;
	
	public Vector3 WaitingAreaPosition = new Vector3(0,0,0);
	public Vector3 WaitingAreaOverflowPosition = new Vector3(0,0,0); // Where the customer stands if the waiting area is full
	
	public Vector3 CustomerStartPosition = new Vector3(0,0,0);
	public Vector3 CustomerEndPosition = new Vector3(0,0,0);
	public Vector3 CustomerReadyPosition = new Vector3(0,0,0);
	public Vector3 CustomerReadyToLeavePosition = new Vector3(0,0,0);
	
	public Vector3 RegisterPosition = new Vector3(0,0,0);
	public _WaitingPosition[] WaitingPositions;
	public _RegisterPosition[] RegisterPositions;
	
	public float BronzeScore = 0.0f;
	public float SilverScore = 0.0f;
	public float GoldScore = 0.0f;
	
	public string StoryScreens = "";
	public string OutroScreens = "";
	public string IntroPopups = "";

	public _Player Player;
	public _Station[] Stations;
	public _Terminal[] Terminals;
	public _Device[] Devices;
	public _Dispenser[] Dispensers;
	public _Customer[] Customers;
	public _Node[] Nodes;
	#endregion
	
	#region Private Properties
	
	#endregion
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
