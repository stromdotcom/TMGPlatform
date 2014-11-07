using UnityEngine;
using System.Collections;

public class TMGWaitingArea {
	
	#region Public Properties				
	
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
	/// Check if a position is available
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is waiting area available; otherwise, <c>false</c>.
	/// </returns>
	public bool IsWaitingAreaAvailable() {
		if (positionArray == null) {
			Debug.Log("IsWaitingAreaAvailable: position array is null!");
			return false;
		}
		
		bool IsAvailable = false;
		
		foreach (string s in positionArray) {
			if (s == null) IsAvailable = true;	
		}
			
		return IsAvailable;
	}
	
	/// <summary>
	/// Get the index of the first free position
	/// </summary>
	/// <returns>
	/// The available waiting area, or -1 if none exists.
	/// </returns>
	public int GetAvailableWaitingArea() {
		if (positionArray == null) {
			Debug.Log("GetAvailableWaitingArea: position array is null!");
			return -1;
		}
		
		for (int i = 0; i < positionArray.Length; i++) if (positionArray[i] == null) return i;
		
		return -1;
	}
	
	/// <summary>
	/// Places the customer at waiting area.
	/// </summary>
	/// <returns>
	/// The index of the waiting area
	/// </returns>
	/// <param name='tag'>
	/// Tag.
	/// </param>
	public int PlaceCustomerAtWaitingArea(string tag) {
		if (!IsWaitingAreaAvailable()) {
			Debug.Log("PlaceCustomerAtWaitingArea: There are no waiting areas available!");
			return -1;
		}
		
		int position = GetAvailableWaitingArea();
		
		if (position < 0) return -1;
		
		positionArray[position] = tag;
		
		return position;
	}
	
	/// <summary>
	/// Removes the customer from waiting area.
	/// </summary>
	/// <returns>
	/// The customer from waiting area.
	/// </returns>
	/// <param name='index'>
	/// The index of the position to remove from
	/// </param>
	public bool RemoveCustomerFromWaitingArea(int index) {
		if (positionArray[index] == null) {
			Debug.Log("RemoveCustomerFromWaitingArea: No customer waiting there!");
			return false;
		}
		
		positionArray[index] = null;
		
		return true;
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
	public bool RemoveCustomerFromWaitingAreaByTag(string tag) {
		int position = -1;
		
		for (int i = 0; i < positionArray.Length; i++) {
			if (positionArray[i] != null && positionArray[i] == tag) {
				position = i;
				break;
			}
		}
		
		if (position < 0) return false;
		
		return RemoveCustomerFromWaitingArea(position);
	}
	
	public bool IsCustomerAtWaitingArea(string tag) {
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
}
