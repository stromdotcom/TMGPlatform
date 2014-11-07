 using System.Xml;
 using System.Xml.Serialization;
 using System.Collections.Generic;
 using UnityEngine;
 
 public class TMGGameSave { 
	// VALUES
    [XmlAttribute("name")]
    public string Name;
    
    public int CurrentLevel;
	
	[XmlArray("UpgradesList"),XmlArrayItem("Upgrade")]
	public string[] Upgrades;
	
	
	[XmlIgnoreAttribute]
	public Dictionary<string, string> Data = new Dictionary<string, string>();
	
	public string Values {
		get {
			string ret = "";
			int count = 0;
			foreach(string key in Data.Keys) {
				if(count != 0) ret += "&";
				ret += WWW.EscapeURL(key) + "=" + WWW.EscapeURL(Data[key]);
				count++;
			}
			return ret;
		}
		set {
			Data.Clear();
			string [] parts = value.Split('&');
			for(int i = 0; i < parts.Length; i++) {
				string [] split = parts[i].Split('=');
				if(split.Length == 2) {
					string key = WWW.UnEscapeURL(split[0]);
					string val = WWW.UnEscapeURL(split[1]);
					Data[key] = val;
				}
			}
		}
	}
	// END VALUES
	
	public bool AddUpgrade(string upgrade) {
		// Add an item to the Upgrades array to store this upgrade in
		if (Upgrades == null) Upgrades = new string[1];
		else {
			// Make sure its not a duplicate
			for (int i = 0; i < Upgrades.Length; i++) 
				if (Upgrades[i] == upgrade) return false;
		
			string[] newUpgradesArray = new string[Upgrades.Length + 1];
			
			for (int i = 0; i < Upgrades.Length; i++) newUpgradesArray[i] = Upgrades[i];
			
			Upgrades = newUpgradesArray;
		}
		
		Upgrades[Upgrades.Length - 1] = upgrade;
		
		return true;
	}
	
	public bool HasUpgrade(string upgrade) {
		if (Upgrades == null) return false;
		
		bool hasUpgrade = false;
		
		for (int i = 0; i < Upgrades.Length; i++) {
			if (Upgrades[i] == upgrade) hasUpgrade = true;
		}
		
		return hasUpgrade;
	}
 }