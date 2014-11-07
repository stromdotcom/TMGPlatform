using UnityEngine;
using System.Collections;

public class TMGUpgrades : MonoBehaviour {
	
	[System.Serializable]
	public class _Upgrade {
		public string IDTag ="";
		
		public int AvailableAtLevel = 0;
		public string RequiresUpgrade = "";
		public string RequiresAchievement = "";
		public float Cost = 1000.0f;
		
		public string Title = "";
		public string Description = "";
		public string RightMainText = "";
		public string UpgradeTypeName = "";
		public string URL = "";
		
		public string LeftLabel = "";
		public string RightLabel = "";
		public string CenterLabel = "";
		
		public string LeftIcon = "";
		public string RightIcon = "";
		public string CenterIcon = "";
	}
	
	
	#region Public Properties
	public _Upgrade[] Upgrades;
	#endregion
	
	#region Private Properties
	
	#endregion
	
	#region Helpers
	public bool IsUpgradeAvailableInLevel(string tag, int level) {
		
		if (Upgrades.Length == 0) {
			Debug.Log("IsUpgradeAvailableInLevel: no upgrades found");
			return false;
		}
		
		_Upgrade u = GetUpgradeIndex(tag);
		
		if (u == null) {
			Debug.Log("IsUpgradeAvailableInLevel: upgrade " + tag + " not found");
			return false;
		}
		
		return level >= u.AvailableAtLevel;
	}
	
	public _Upgrade GetUpgradeIndex(string tag) {
		if (Upgrades.Length == 0) return null;
		
		_Upgrade u = null;	
		
		for (int i = 0; i < Upgrades.Length; i++) {
			if (Upgrades[i].IDTag == tag) {
				u = Upgrades[i];	
				break;
			}
		}
		
		return u;
	}
	
	public string GetUpgradeTitle(string tag) {
		_Upgrade u = GetUpgradeIndex(tag);
		if (u == null) {
			Debug.Log("GetUpgradeTitle: upgrade " + tag + " not found");
			return "";
		}
		
		return u.Title;
	}
	
	public string GetUpgradeLeftLabel(string tag) {
		_Upgrade u = GetUpgradeIndex(tag);
		if (u == null) {
			Debug.Log("GetUpgradeLeftLabel: upgrade " + tag + " not found");
			return "";
		}
		
		return u.LeftLabel;
	}
	
	public string GetUpgradeRightLabel(string tag) {
		_Upgrade u = GetUpgradeIndex(tag);
		if (u == null) {
			Debug.Log("GetUpgradeRightLabel: upgrade " + tag + " not found");
			return "";
		}
		
		return u.RightLabel;
	}
	
	public string GetUpgradeDescription(string tag) {
		_Upgrade u = GetUpgradeIndex(tag);
		if (u == null) {
			Debug.Log("GetUpgradeDescription: upgrade " + tag + " not found");
			return "";
		}
		
		return u.Description;
	}
	
	public float GetUpgradeCost(string tag) {
		_Upgrade u = GetUpgradeIndex(tag);
		if (u == null) {
			Debug.Log("GetUpgradeCost: upgrade " + tag + " not found");
			return 0.0f;
		}
		
		return u.Cost;
	}
	
	public int GetUpgradeAvailableLevel(string tag) {
		_Upgrade u = GetUpgradeIndex(tag);
		if (u == null) {
			Debug.Log("GetUpgradeAvailableLevel: upgrade " + tag + " not found");
			return -1;
		}
		
		return u.AvailableAtLevel;
	}
	#endregion
}
