using UnityEngine;
using System.Collections;

public class TMGDispenserDefinition : MonoBehaviour {
	
	public string TypeTag = "";
	public bool RequiresUpgrade = false;
	public string UpgradeRequired = "";
	
	public float WarmupTime = 5.0f;
	public float CooldownTime = 5.0f;
	public float RunTime = 5.0f;
	
	public int Units = 0;
	public int MaxUnits = 0;
	public int UnitsPerCompletion = 0;
	
	public float PatienceBonus = 0.0f;
	
	public bool IsAutomatic = false;
}
