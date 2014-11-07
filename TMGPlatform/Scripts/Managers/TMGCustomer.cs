using UnityEngine;
using System.Collections;

public class TMGCustomer {
	
	#region Public Properties
	
	public string TypeTag = null;
	
	/// <summary>
	/// The tag of the station this customer is currently at (if any)
	/// </summary>
	public string StationTag = null;
	
	/// <summary>
	/// A stack containing the list of stations this customer will request (empty when he is done)
	/// You can peek at the top element to see where he wants to go next.
	/// </summary>
	public Stack StationsRequested = new Stack();
	
	/// <summary>
	/// The amount of patience a customer has
	/// </summary>
	public float Patience = 0.0f;
	
	/// <summary>
	/// The maximum patience a customer can have
	/// </summary>
	public float MaxPatience = 0.0f;
	
	/// <summary>
	/// The rate at which the customers patience decays over time (as deltaTime * PatienceDecayRate)
	/// </summary>
	public float PatienceDecayRate = 0.0f;
	
	/// <summary>
	/// The amount the customer's patience is multiplied by on checkout
	/// </summary>
	public float CheckoutMultiplier = 1.0f;
	
	/// <summary>
	/// The amount added to the customer's patience on checkout, after the multiplier is applied
	/// </summary>
	public float CheckoutBonus = 0.0f;
	
	/// <summary>
	/// How long the customer should delay before entering (triggers a callback)
	/// </summary>
	public float EntryDelay = 0.0f;
	
	/// <summary>
	/// True is the customer has been checked out (must be set by TMGManager)
	/// </summary>
	public bool IsCheckedOut = false;
	#endregion
	
	#region Private Properties
	private TMGManager.GenericCallback onPatienceRanOutCallback = null;
	private object onPatienceRanOutUserdata = null;
	
	private TMGManager.GenericCallback onEntryDelayRanOutCallback = null;
	private object onEntryDelayRanOutUserdata = null;
	
	private bool patienceShouldDecay = false;
	#endregion
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update (float dt) {
		if (EntryDelay > 0.0f) {
			EntryDelay -= dt;
			
			if (EntryDelay <= 0.0f) {
				EntryDelay = 0.0f;
				if (onEntryDelayRanOutCallback != null) {
					onEntryDelayRanOutCallback(onEntryDelayRanOutUserdata);	
				}
			}
		}
		
		if (Patience > 0.0f && patienceShouldDecay) {
			Patience -= PatienceDecayRate * dt;
			
			if (Patience <= 0.0f) {
				Patience = 0.0f;
				if (onPatienceRanOutCallback != null) {
					onPatienceRanOutCallback(onPatienceRanOutUserdata);	
					
					onPatienceRanOutCallback = null;
				}
			}
		}
	}
	
	public void SetPatienceRanOutCallback(TMGManager.GenericCallback cb, object userdata) {
		onPatienceRanOutCallback = cb;
		onPatienceRanOutUserdata = userdata;
	}
	
	public void SetEntryDelayRanOutCallback(TMGManager.GenericCallback cb, object userdata) {
		onEntryDelayRanOutCallback = cb;
		onEntryDelayRanOutUserdata = userdata;
	}
	
	public void AddPatience(float amount) {
		Patience += amount;
		
		if (Patience > MaxPatience) Patience = MaxPatience;
		
		if (Patience <= 0 && onPatienceRanOutCallback != null) {
			onPatienceRanOutCallback(onPatienceRanOutUserdata);	
					
			onPatienceRanOutCallback = null;
		}
	}
	
	public void SetPatienceShouldDecay(bool shouldDecay) {
		patienceShouldDecay = shouldDecay;	
	}
}
