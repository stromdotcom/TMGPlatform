using UnityEngine;
using System.Collections;

public class TMGStation {
	
	#region Public Properties
	public string TypeTag = null;
	
	public string CustomerTag = null;
	
	public bool IsActive = false;
	public bool IsComplete = false;
	public bool IsOccupied = false;
	
	public bool AutoStarts = false;
	
	public float RunTime;
	public float MinigameChance;
	public float PatienceBonus;
	
	public int NumberOfPositions = 1;
	
	public string ParentStation = "";
	
	public float Timer {
		get { return timer; }	
	}	
	#endregion
	
	#region PrivateProperties
	private TMGManager.GenericCallback onCompleteCallback = null;
	private object onCompleteUserdata = null;
	
	private float timer = 0.0f;
	#endregion
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update (float deltaTime) {
		
		if (IsActive) {
			timer += deltaTime;
			
			if (timer >= RunTime) {
				IsActive = false;	
				IsComplete = true;
				
				if (onCompleteCallback != null)
					onCompleteCallback(onCompleteUserdata);
				
				onCompleteCallback = null;
				onCompleteUserdata = null;
			}
		}
	}
	
	public bool Run(TMGManager.GenericCallback OnComplete, object OnCompleteUserdata) {
		// Abort if station is not occupied or already active
		if (IsActive) {
			Debug.Log("Cannot run station, it is already active");
			return false;
		}
		
		if (!IsOccupied) {
			Debug.Log("Cannot run station, it is unoccupied");
			return false;
		}
		
		// Else reset and run
		timer = 0.0f;
		IsActive = true;
		IsComplete = false;
		
		onCompleteCallback = OnComplete;
		onCompleteUserdata = OnCompleteUserdata;
		
		return true;
	}
	
	public string RemoveCustomer() {
		if (!IsOccupied) return null;
		
		string ct = CustomerTag;
		CustomerTag = null;
		
		IsOccupied = false;
		IsComplete = false;
		IsActive = false;
		
		return ct;
	}
}
