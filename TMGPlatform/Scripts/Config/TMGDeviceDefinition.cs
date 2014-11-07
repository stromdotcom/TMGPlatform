using UnityEngine;
using System.Collections;

public class TMGDeviceDefinition : MonoBehaviour {
	
	public string TypeTag = "";
	public bool RequiresUpgrade = false;
	public string UpgradeRequired = "";
	
	public float WarmupTime = 5.0f;
	public float CooldownTime = 5.0f;
	public float RunTime = 5.0f;
}
