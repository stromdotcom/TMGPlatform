using UnityEngine;
using System.Collections;

public class TMGCustomerDefinition : MonoBehaviour {
	
	public string TypeTag = "";
	
	public float Patience = 5.0f;
	public float MaxPatience = 5.0f;
	public float PatienceDecay = 0.1f;
	
	public float PatienceCheckoutMultiplier = 1.0f;
	public float PatienceCheckoutBonus = 0.0f;
}