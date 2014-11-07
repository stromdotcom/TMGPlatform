using UnityEngine;
using System.Collections;

public class TMGRegister {

	#region Public Properties				
	public TMGManager.GenericCallback QueueChangedCallback = null;
	public object QueueChangedUserdata = null;
	public float PatienceDecayMultiplier = 1.0f;
	#endregion
	
	#region Private Properties
	private string[] positionArray = null;
	#endregion
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Check whether there is a free spot at the register
	/// </summary>
	/// <returns>
	/// <c>true</c> if there is an available position
	/// </returns>
	public bool IsRegisterAvailable() {
		if (positionArray == null) {
			Debug.Log("IsRegisterAvailable: position array is null!");
			return false;
		}
		
		bool IsAvailable = false;
		
		foreach (string s in positionArray) {
			if (s == null) IsAvailable = true;	
		}
			
		return IsAvailable;
	}
	
	public bool HasWaitingCustomers() {
		if (positionArray == null) return false;
		
		bool hasCustomers = false;
		
		for (int i = 0; i < positionArray.Length; i++) {
			if (positionArray[i] != null) hasCustomers = true;
		}
		
		return hasCustomers;
	}
	
	/// <summary>
	/// Gets the available register.
	/// </summary>
	/// <returns>
	/// The available register.
	/// </returns>
	public int GetAvailableRegister() {
		if (positionArray == null) {
			Debug.Log("GetAvailableRegister: position array is null!");
			return -1;
		}
		
		for (int i = 0; i < positionArray.Length; i++) if (positionArray[i] == null) return i;
		
		return -1;
	}
	
	/// <summary>
	/// Places the customer at register.
	/// </summary>
	/// <returns>
	/// The customer at register.
	/// </returns>
	/// <param name='tag'>
	/// Tag.
	/// </param>
	public int PlaceCustomerAtRegister(string tag) {
		if (!IsRegisterAvailable()) {
			Debug.Log("PlaceCustomerAtRegister: There are no registers available!");
			return -1;
		}
		
		int position = GetAvailableRegister();
		
		if (position < 0) return -1;
		
		positionArray[position] = tag;
		
		return position;
	}
	
	/// <summary>
	/// Removes the customer from register.
	/// </summary>
	/// <returns>
	/// True on success
	/// </returns>
	/// <param name='index'>
	/// The index of the position to remove from
	/// </param>
	public bool RemoveCustomerFromRegister(int index) {
		if (positionArray[index] == null) {
			Debug.Log("RemoveCustomerFromRegister: No customer at this index!");
			return false;
		}
						
		if (positionArray[index] != null) {
						
			positionArray[index] = null;
		
			if (QueueChangedCallback != null) 
				QueueChangedCallback(QueueChangedUserdata);
				
			return true;
		}
		
		return false;
	}
	
	/// <summary>
	/// Removes the customer from waiting area by tag.
	/// </summary>
	/// <returns>
	/// The customer from waiting area by tag.
	/// </returns>
	/// <param name='tag'>
	/// If set to <c>true</c> tag.
	/// </param>
	public bool RemoveCustomerFromRegisterByTag(string tag) {
		int position = -1;
		
		for (int i = 0; i < positionArray.Length; i++) {
			if (positionArray[i] != null && positionArray[i] == tag) {
				position = i;
				break;
			}
		}
		
		if (position < 0) return false;
		
		return RemoveCustomerFromRegister(position);
	}
	
	public string GetCustomerAtPosition(int index) {
		if (index < 0 || index >= positionArray.Length) return null;
		
		return positionArray[index];
	}
	
	public bool IsCustomerAtRegister(string tag) {
		int position = -1;
		
		for (int i = 0; i < positionArray.Length; i++) {
			if (positionArray[i] != null && positionArray[i] == tag) {
				position = i;
				break;
			}
		}
		
		if (position < 0) return false;
		
		return true;
	}
	
	/// <summary>
	/// Sets the number of positions.
	/// </summary>
	/// <returns>
	/// The number of positions.
	/// </returns>
	/// <param name='positions'>
	/// If set to <c>true</c> positions.
	/// </param>
	public bool SetNumberOfPositions(int positions) {
		positionArray = new string[positions];
		
		for (int i = 0; i < positions; i++) positionArray[i] = null;
		
		return true;
	}
	
	public int GetNumberOfPositions() {
		return positionArray.Length;
	}
	
	public void SetQueueChangedCallback(TMGManager.GenericCallback cb, object data) {
		if (cb == null) Debug.Log("setting callback to null!");
		else Debug.Log("Nope all good");
		
		QueueChangedCallback = cb;
		QueueChangedUserdata = data;
	}
	
	public void SwapPositions(int position0, int position1) {
		string temp = positionArray[position0];	
		
		positionArray[position0] = positionArray[position1];
		positionArray[position1] = temp;
	}
	
	public int GetCustomerPosition(string customerTag) {
		int position = -1;

		for (int i = 0; i < positionArray.Length; i++) {
			if (positionArray[i] == customerTag) position = i;
		}
		
		return position;
	}
}
