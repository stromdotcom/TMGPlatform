/**
 * @brief  Brief description: no more than one line
 *
 * This is the longer description. There must be a blank line separating
 * the brief comment from the longer description.
 */

/**
 * @file   hello.c
 * @author Carla Schroder
 * @date   July 22, 2011
.*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TMGManager : MonoBehaviour {
	
	public class ScheduledCallback {
		public float Delay = 0.0f;
		public GenericCallback Callback;
		public object Userdata;
	}
	
	#region Public Properties
	
	// Accessors
	/// <summary>
	/// The master time variable for the manager.  How long the simulation has been running.
	/// </summary>
	public float Timer {
		get { return timer; }	
	}
	
	public float Score {
		get { return score; }
		set { score = value; }
	}
	public bool IsRunning {
		get { return isRunning; }
	}
	
	public float LevelDuration = 0.0f;
	
	public List<TMGStation> Stations = new List<TMGStation>();
	public List<TMGTerminal> Terminals = new List<TMGTerminal>();
	public List<TMGCustomer> Customers = new List<TMGCustomer>();
	public List<TMGDevice> Devices = new List<TMGDevice>();
	public List<TMGDispenser> Dispensers = new List<TMGDispenser>();
	public TMGWaitingArea WaitingArea = null;
	public TMGRegister Register = null;
	#endregion
	
	#region Private Properties
	
	private float timer = 0.0f;
	private float score = 0.0f;
	
	private bool isRunning = false;
	
	private Dictionary<string,int> stationTags = new Dictionary<string, int>();
	private Dictionary<string,int> terminalTags = new Dictionary<string, int>();
	private Dictionary<string,int> customerTags = new Dictionary<string, int>();
	private Dictionary<string,int> deviceTags = new Dictionary<string, int>();
	private Dictionary<string,int> dispenserTags = new Dictionary<string, int>();
	
	private ArrayList scheduledCallbacks = new ArrayList();
	#endregion
	
	#region Delegate Defintions
	
	public delegate void GenericCallback(object obj);
	
	#endregion
	
	#region Event Delegate Definitions
	
	public delegate void GenericEventHandler(object sender, EventArgs e);
	
	#endregion
	
	#region Event Declarations
	
	// A generic, all purpose event
	public event GenericEventHandler GenericEvent;
	protected virtual void OnGenericEvent(EventArgs e) {		
		if (GenericEvent != null)
			GenericEvent(this, e);
	}
	
	// An event to be fired when the level timer exceeds or equals the level duration (ie time is up)
	public event GenericEventHandler TimeUpEvent;
	protected virtual void OnTimeUpEvent(EventArgs e) {		
		if (TimeUpEvent != null)
			TimeUpEvent(this, e);
	}
	
	#endregion
	
	#region Core Methods
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isRunning) {
			float dt = Time.deltaTime;
			
			// Update all stations,terminals, devices and customers
			foreach (TMGStation s in Stations) s.Update(dt);
			foreach (TMGCustomer c in Customers) c.Update(dt);
			foreach (TMGDevice d in Devices) d.Update(dt);
			foreach (TMGDispenser d in Dispensers) d.Update(dt);
			foreach (TMGTerminal t in Terminals) t.Update(dt);
			
			float oldTimer = timer;
		
			timer += Time.deltaTime;
		
			// Fire an event if time has run out
			if (oldTimer < LevelDuration && timer >= LevelDuration) OnTimeUpEvent(EventArgs.Empty);
			
			// Demo: fire a generic event every 10 seconds
			if ((int)(oldTimer / 10.0f) != (int)(timer / 10.0f)) OnGenericEvent(EventArgs.Empty);
			
			// Run through the scheduler and look for any callbacks that need to be called
			
			ArrayList deletedCallbacks = new ArrayList();
			
			foreach (ScheduledCallback s in scheduledCallbacks) {
				s.Delay -= dt;
				
				if (s.Delay <= 0.0f) {
					if (s.Callback != null) {
						s.Callback(s.Userdata);	
					}
					
					deletedCallbacks.Add(s);
				}
			}
			
			foreach (ScheduledCallback s in deletedCallbacks) scheduledCallbacks.Remove(s);
		}
	}
	#endregion
	
	#region Initialization Methods
	
	/// <summary>
	/// Adds a station tot he station array, and store its tag
	/// </summary>
	/// <returns>
	/// True if station was added successfully.
	/// </returns>
	/// <param name='tag'>
	/// The identifying tag of the station
	/// </param>
	/// <param name='typeTag'>
	/// The type tag of the station
	/// </param>
	/// <param name='RunTime'>
	/// How long the station runs for when activated
	/// </param>
	public bool AddStation(string tag, string typeTag, float RunTime) {
		if (tag != null && stationTags.ContainsKey(tag)) {
			Debug.Log("AddStation: Attempted to add station using existing tag!");
			
			return false;
		}
		
		int index = Stations.Count;
		TMGStation station = new TMGStation();
		station.RunTime = RunTime;
		station.TypeTag = typeTag;
		
		Stations.Insert(index, station);
		
		if (tag != null) 
			stationTags[tag] = index;
		
		return true;
	}	
	
	/// <summary>
	/// Add a station to the station array, and store its tag
	/// </summary>
	/// <param name="RunTime">
	/// How long the station runs for when activated
	/// </param>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <param name="typeTag">
	/// The type tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the station was added
	/// </returns>
	[Obsolete("Use AddStation(string, string, float) instead", false)]
	public bool AddStation(float RunTime, string tag, string typeTag) {
		if (tag != null && stationTags.ContainsKey(tag)) {
			Debug.Log("AddStation: Attempted to add station using existing tag!");
			
			return false;
		}
		
		int index = Stations.Count;
		TMGStation station = new TMGStation();
		station.RunTime = RunTime;
		station.TypeTag = typeTag;
		
		Stations.Insert(index, station);
		
		if (tag != null) 
			stationTags[tag] = index;
		
		return true;  
	}	
	
	/// <summary>
	/// Add a terminal to the terminal array, and store its tag
	/// </summary>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the terminal was added
	/// </returns>
	/// <param name='tag'>
	/// The identifying tag of the terminal
	/// </param>
	/// <param name='typeTag'>
	/// The type tag of the terminal
	/// </param>
	/// <param name='RunTime'>
	/// How long the terminal runs for when activated
	/// </param>
	public bool AddTerminal(string tag, string typeTag, float RunTime) {
		if (tag != null && terminalTags.ContainsKey(tag)) {
			Debug.Log("AddTerminal: Attempted to add terminal using existing tag!");
			
			return false;
		}
		
		int index = Terminals.Count;
		TMGTerminal terminal = new TMGTerminal();
		terminal.RunTime = RunTime;
		terminal.TypeTag = typeTag;
		
		Terminals.Insert(index, terminal);
		
		if (tag != null) 
			terminalTags[tag] = index;
		
		return true;
	}	
	
	/// <summary>
	/// Add a device to the device array, and store its tag
	/// </summary>
	/// <returns>
	/// True on success
	/// </returns>
	/// <param name='tag'>
	/// The identifying tag of the device
	/// </param>
	/// <param name='typeTag'>
	/// The type tag of the device
	/// </param>
	/// <param name='warmUpTime'>
	/// The amount of time the device should take to warm up
	/// </param>
	/// <param name='coolDownTime'>
	/// The amount of time the device should take to cool down after being run
	/// </param>
	public bool AddDevice(string tag, string typeTag, float warmUpTime, float coolDownTime, float runTime) {
		if (tag != null && deviceTags.ContainsKey(tag)) {
			Debug.Log("AddDevice: Attempted to add device using existing tag!");
			
			return false;
		}
		
		int index = Devices.Count;
		TMGDevice device = new TMGDevice();
		device.CoolDownTime = coolDownTime;
		device.WarmUpTime = warmUpTime;
		device.RunTime = runTime;
		device.TypeTag = typeTag;
		
		// We call initialize here because the warmup accumulator needs to be set up
		device.Initialize();
		
		Devices.Insert(index, device);
		
		if (tag != null) 
			deviceTags[tag] = index;
		
		return true;
	}	
	
	public bool AddDispenser(string tag, string typeTag, float warmUpTime, float coolDownTime, float runTime, int units, int maxUnits, int unitsPerCompletion, bool isAutomatic) {
		if (tag != null && dispenserTags.ContainsKey(tag)) {
			Debug.Log("AddDispenser: Attempted to add dispenser using existing tag!");
			
			return false;
		}
		
		int index = Dispensers.Count;
		TMGDispenser dispenser = new TMGDispenser();
		dispenser.CoolDownTime = coolDownTime;
		dispenser.WarmUpTime = warmUpTime;
		dispenser.RunTime = runTime;
		dispenser.TypeTag = typeTag;
		
		dispenser.Units = units;
		dispenser.MaxUnits = maxUnits;
		dispenser.UnitsPerCompletion = unitsPerCompletion;
		
		dispenser.IsAutomatic = isAutomatic;
		
		// We call initialize here because the warmup accumulator needs to be set up
		dispenser.Initialize();
		
		Dispensers.Insert(index, dispenser);
		
		if (tag != null) 
			dispenserTags[tag] = index;
		
		return true;
	}
	
	/// <summary>
	/// Add a customer to the customer array, and store its tag 
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the customer was added
	/// </returns>
	public bool AddCustomer(string tag, string typeTag, float entryDelay) {
		if (tag != null && customerTags.ContainsKey(tag)) {
			Debug.Log("AddCustomer: Attempted to add customer using existing tag!");
			return false;	
		}
		
		int index = Customers.Count;
		TMGCustomer customer = new TMGCustomer();
		
		customer.EntryDelay = entryDelay;
		customer.TypeTag = typeTag;
		
		Customers.Insert(index, customer);
		
		if (tag != null)
			customerTags[tag] = index;
		
		return true;
	}
	
	/// <summary>
	/// Adds a waiting area to the simulation.
	/// </summary>
	/// <returns>
	/// True on success
	/// </returns>
	/// <param name='positions'>
	/// The number of positions for this waiting area
	/// </param>
	public bool AddWaitingArea(int positions) {
		if (WaitingArea != null) {
			Debug.Log("AddWaitingArea: Cannot add waiting area, one already exists!");
			return false;
		}
		
		WaitingArea = new TMGWaitingArea();
		WaitingArea.SetNumberOfPositions(positions);
		
		return true;
	}
	
	public bool AddRegister(int positions) {
		if (Register != null) {
			Debug.Log("AddRegister: Cannot add register, one already exists!");
			return false;
		}
		
		Register = new TMGRegister();
		Register.SetNumberOfPositions(positions);
		
		return true;
	}
	
	/// <summary>
	/// Set a station's auto-start property
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <param name="IsAutomatic">
	/// true if the station should autostart
	/// </param>
	public void SetStationAuto(string tag, bool IsAutomatic) {
		Stations[stationTags[tag]].AutoStarts = IsAutomatic;
	}
	
	/// <summary>
	/// Sets the station parent.
	/// </summary>
	/// <returns>
	/// True on success
	/// </returns>
	/// <param name='tag'>
	/// the indentifying tag of the station
	/// </param>
	/// <param name='parentTag'>
	/// the indentifying tag of the parent station
	/// </param>
	public bool SetStationParent(string tag, string parentTag) {
		if (!stationTags.ContainsKey(tag)) {
			Debug.Log("SetStationParent: no station exists with tag " + tag + ".");
			return false;
		} else if (!stationTags.ContainsKey(parentTag)) {
			Debug.Log("SetStationParent: no station exists with tag " + parentTag + ".");
			return false;
		}
		
		Stations[stationTags[tag]].ParentStation = parentTag;
		
		return true;
	}
	
	/// <summary>
	/// Gets the parent of the station
	/// </summary>
	/// <returns>
	/// The station's parent tag, or empty string if no parent.
	/// </returns>
	/// <param name='tag'>
	/// The indentifying tag of the station
	/// </param>
	public string GetStationParent(string tag) {
		if (!stationTags.ContainsKey(tag)) {
			Debug.Log("GetStationParent: no station exists with tag " + tag + ".");
			return "";
		}
		
		return Stations[stationTags[tag]].ParentStation;
	}
	
	public ArrayList GetStationChildren(string tag) {
		if (!stationTags.ContainsKey(tag)) {
			Debug.Log("GetStationParent: no station exists with tag " + tag + ".");
			return null;
		}	
		
		ArrayList list = new ArrayList();
		
		foreach (string t in stationTags.Keys) {
			if (Stations[stationTags[t]].ParentStation == "tag") list.Add(t);	
		}
		
		return list;
	}
	
	/// <summary>
	/// Set the station request list for the given customer
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="requestList">
	/// A stack containing the requested stations
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the request list stack was applied to the customer
	/// </returns>
	public bool SetCustomerStationsRequested(string tag, Stack requestList) {
		if (requestList == null) {
			Debug.Log("SetCustomerStationsRequested: stack is null.");
			return false;
		}
		
		if (tag == null || !customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerStationsRequested: no customer exists with that tag, or tag is null!");
			return false;
		}
		
		Customers[customerTags[tag]].StationsRequested = requestList;
		
		return true;
	}
	#endregion
	
	#region Access Methods
	
	/// <summary>
	/// Get the TMGStation object for a given tag
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// The TMGStation object for the given tag
	/// </returns>
	public TMGStation GetStationByTag(string tag) {
		if (stationTags.ContainsKey(tag)) {
			return Stations[stationTags[tag]];	
		} else {
			Debug.Log("GetStationByTag: Station with tag " + tag + " does not exist!");			
			return null;
		}
	}
	
	/// <summary>
	/// Get the TMGDispenser object for a given tag
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the dispenser
	/// </param>
	/// <returns>
	/// The TMGDispenser object for the given tag
	/// </returns>
	public TMGDispenser GetDispenserByTag(string tag) {
		if (dispenserTags.ContainsKey(tag)) {
			return Dispensers[dispenserTags[tag]];	
		} else {
			Debug.Log("GetDispenserByTag: Dispenser with tag " + tag + " does not exist!");			
			return null;
		}
	}
	
	/// <summary>
	/// Get the TMGCustomer object for a given tag
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <returns>
	/// The TMGCustomer object for the given tag
	/// </returns>
	public TMGCustomer GetCustomerByTag(string tag) {
		if (customerTags.ContainsKey(tag)) {
			return Customers[customerTags[tag]];
		} else {
			Debug.Log("GetCustomerByTag: Customer with tag " + tag + " does not exist");			
			return null;
		}
	}
	
	/// <summary>
	/// Gets the TMGWaitingArea
	/// </summary>
	/// <returns>
	/// The waiting area.
	/// </returns>
	public TMGWaitingArea GetWaitingArea() {
		return WaitingArea;	
	}
	
	#endregion
	
	#region Station Queries
	
	public bool SetStationMinigameChance(string tag, float chance) {
		if (stationTags.ContainsKey(tag)) {
			Stations[stationTags[tag]].MinigameChance = chance;
			return true;
		} else {
			Debug.Log("SetStationMinigameChance: Station with tag " + tag + " does not exist");			
			return false;
		}
	}
	
	public bool SetStationPatienceBonus(string tag, float bonus) {
		if (stationTags.ContainsKey(tag)) {
			Stations[stationTags[tag]].PatienceBonus = bonus;
			return true;
		} else {
			Debug.Log("SetStationPatienceBonus: Station with tag " + tag + " does not exist");			
			return false;
		}
	}
	
	public float GetStationMinigameChance(string tag) {
		if (stationTags.ContainsKey(tag)) {
			return Stations[stationTags[tag]].MinigameChance;
		} else {
			Debug.Log("GetStationMinigameChance: Station with tag " + tag + " does not exist");			
			return -1.0f;;
		}	
	}
	
	public float GetStationPatienceBonus(string tag) {
		if (stationTags.ContainsKey(tag)) {
			return Stations[stationTags[tag]].PatienceBonus;
		} else {
			Debug.Log("GetStationPatienceBonus: Station with tag " + tag + " does not exist");			
			return -1.0f;;
		}	
	}
	
	/// <summary>
	/// Check to see if a station is active
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the station is active
	/// </returns>
	public bool StationIsActive(string tag) {
		if (stationTags.ContainsKey(tag)) {
			return Stations[stationTags[tag]].IsActive;
		} else {
			Debug.Log("StationIsActive: Station with tag " + tag + " does not exist");			
			return false;
		}
	}
	
	/// <summary>
	/// Check to see if a station is occupied
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the station is occupied
	/// </returns>
	public bool StationIsOccupied(string tag) {
		if (stationTags.ContainsKey(tag)) {
			return Stations[stationTags[tag]].IsOccupied;
		} else {
			Debug.Log("StationIsOccupied: Station with tag " + tag + " does not exist");			
			return false;
		}
	}
	
	/// <summary>
	/// Check to see if a station is complete, i.e. occupied and finished running
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the station is complete
	/// </returns>
	public bool StationIsComplete(string tag) {
		if (stationTags.ContainsKey(tag)) {
			return Stations[stationTags[tag]].IsComplete;
		} else {
			Debug.Log("StationIsComplete: Station with tag " + tag + " does not exist");			
			return false;
		}
	}
	
	/// <summary>
	/// Check to see if a station is set to run automatically
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the station is set to run automatically
	/// </returns>
	public bool StationIsAutomatic(string tag) {
		return Stations[stationTags[tag]].AutoStarts;	
	}
	
	/// <summary>
	/// Get the type tag of a station
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// The type tag for the station
	/// </returns>
	public string GetStationTypeTag(string tag) {
		if (!stationTags.ContainsKey(tag)) {
			Debug.Log("GetStationTypeTag: no station with tag " + tag + " exists!");
			return null;
		}
		
		return Stations[stationTags[tag]].TypeTag;
	}
	#endregion
	
	#region Device Queries
	
	/// <summary>
	/// Set a callback to be called when a device is cooled down
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the device
	/// </param>
	/// <param name="callback">
	/// The callback method
	/// </param>
	/// <param name="userdata">
	/// The data to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating success
	/// </returns>
	public bool SetDeviceCooledDownCallback(string tag, GenericCallback callback, object userdata) {
		if (!deviceTags.ContainsKey(tag)) {
			Debug.Log("SetDeviceCooledDownCallback: no device with tag " + tag + " exists!");
			return false;
		}
		
		Devices[deviceTags[tag]].SetCooledDownCallback(callback, userdata);
		
		return true;
	}
	
	/// <summary>
	/// Set a callback to be called when a device is warmed up
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the device
	/// </param>
	/// <param name="callback">
	/// The callback method
	/// </param>
	/// <param name="userdata">
	/// The data to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating success
	/// </returns>
	public bool SetDeviceWarmedUpCallback(string tag, GenericCallback callback, object userdata) {
		if (!deviceTags.ContainsKey(tag)) {
			Debug.Log("SetDeviceWarmedUpCallback: no device with tag " + tag + " exists!");
			return false;
		}
		
		Devices[deviceTags[tag]].SetWarmedUpCallback(callback, userdata);
		
		return true;
	}
	
	#endregion
	
	#region Dispenser Queries
	
	public bool SetDispenserPatienceBonus(string tag, float bonus) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("SetDispenserPatienceBonus: no dispenser with tag " + tag + " exists!");
			return false;
		}
	
		Dispensers[dispenserTags[tag]].PatienceBonus = bonus;
		return true;
	}
	
	public float GetDispenserPatienceBonus(string tag) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("GetDispenserPatienceBonus: no dispenser with tag " + tag + " exists!");
			return 0.0f;
		}
		
		return Dispensers[dispenserTags[tag]].PatienceBonus;
	}
	
	/// <summary>
	/// Set a callback to be called when a dispenser is cooled down
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the dispenser
	/// </param>
	/// <param name="callback">
	/// The callback method
	/// </param>
	/// <param name="userdata">
	/// The data to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating success
	/// </returns>
	public bool SetDispenserCooledDownCallback(string tag, GenericCallback callback, object userdata) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("SetDispenserCooledDownCallback: no dispenser with tag " + tag + " exists!");
			return false;
		}
		
		Dispensers[dispenserTags[tag]].SetCooledDownCallback(callback, userdata);
		
		return true;
	}
	
	/// <summary>
	/// Set a callback to be called when a dispenser is warmed up
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the dispenser
	/// </param>
	/// <param name="callback">
	/// The callback method
	/// </param>
	/// <param name="userdata">
	/// The data to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating success
	/// </returns>
	public bool SetDispenserWarmedUpCallback(string tag, GenericCallback callback, object userdata) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("SetDispenserWarmedUpCallback: no dispenser with tag " + tag + " exists!");
			return false;
		}
		
		Dispensers[dispenserTags[tag]].SetWarmedUpCallback(callback, userdata);
		
		return true;
	}
	
	/// <summary>
	/// Set a callback to be called when a dispenser is complete
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the dispenser
	/// </param>
	/// <param name="callback">
	/// The callback method
	/// </param>
	/// <param name="userdata">
	/// The data to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating success
	/// </returns>
	public bool SetDispenserCompleteCallback(string tag, GenericCallback callback, object userdata) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("SetDispenserCompleteCallback: no dispenser with tag " + tag + " exists!");
			return false;
		}
		
		Dispensers[dispenserTags[tag]].SetCompleteCallback(callback, userdata);
		
		return true;
	}
	
	public int GetNumberOfDispenserUnits(string tag) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("GetNumberOfDispenserUnits: no dispenser with tag " + tag + " exists!");
			return 0;
		}
		
		return Dispensers[dispenserTags[tag]].Units;
		
	}
	
	public bool ConsumeDispenserUnit(string tag) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("ConsumeDispenserUnit: no dispenser with tag " + tag + " exists!");
			return false;
		}
				
		if (Dispensers[dispenserTags[tag]].Units <= 0) return false;
		
		Dispensers[dispenserTags[tag]].Units -= 1;
		Debug.Log("Consumed.  Units = " + Dispensers[dispenserTags[tag]].Units + " and MaxUnits = " + Dispensers[dispenserTags[tag]].MaxUnits);
		return true;
		
	}
	
	public bool DispenserHasUnits(string tag) {
		if (!dispenserTags.ContainsKey(tag)) {
			Debug.Log("DispenserHasUnits: no dispenser with tag " + tag + " exists!");
			return false;
		}
		
		return Dispensers[dispenserTags[tag]].Units > 0;	
	}
	#endregion
	
	#region Terminal Queries
	
	/// <summary>
	/// Check to see if a terminal is active
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the terminal
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the terminal is active
	/// </returns>
	public bool TerminalIsActive(string tag) {
		if (terminalTags.ContainsKey(tag)) {
			return Terminals[terminalTags[tag]].IsActive;
		} else {
			Debug.Log("TerminalIsActive: Terminal with tag " + tag + " does not exist");			
			return false;
		}
	}
			
	/// <summary>
	/// Check to see if a terminal is complete, i.e. finished running
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the terminal
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the terminal is complete
	/// </returns>
	public bool TerminalIsComplete(string tag) {
		if (terminalTags.ContainsKey(tag)) {
			return Terminals[terminalTags[tag]].IsComplete;
		} else {
			Debug.Log("TerminalIsComplete: Terminal with tag " + tag + " does not exist");			
			return false;
		}
	}
	
	/// <summary>
	/// Check to see if a terminal is set to run automatically
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the terminal
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the terminal is set to run automatically
	/// </returns>
	public bool TerminalIsAutomatic(string tag) {
		return Terminals[terminalTags[tag]].AutoStarts;	
	}
	
	/// <summary>
	/// Get the type tag of a terminal
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the terminal
	/// </param>
	/// <returns>
	/// The type tag for the terminal
	/// </returns>
	public string GetTerminalTypeTag(string tag) {
		if (!terminalTags.ContainsKey(tag)) {
			Debug.Log("GetTerminalTypeTag: no terminal with tag " + tag + " exists!");
			return null;
		}
		
		return Terminals[terminalTags[tag]].TypeTag;
	}
	#endregion
	
	#region Customer Queries
	
	/// <summary>
	/// Set the patience, max patience and patience decay rate for a customer
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="patience">
	/// The patience value for the customer
	/// </param>
	/// <param name="maxPatience">
	/// The customer's maximum patience level
	/// </param>
	/// <param name="patienceDecayRate">
	/// The rate at which the customer's patience should decay (per second)
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating success
	/// </returns>
	public bool SetCustomerPatienceAndDecay(string tag, float patience, float maxPatience, float patienceDecayRate) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerPatienceAndDecay: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].Patience = patience;
		Customers[customerTags[tag]].MaxPatience = maxPatience;
		Customers[customerTags[tag]].PatienceDecayRate = patienceDecayRate;
		
		return true;
	}
	
	public bool SetCustomerCheckedOut(string tag, bool isCheckedOut) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerCheckedOut: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].IsCheckedOut = isCheckedOut;
		Customers[customerTags[tag]].SetPatienceShouldDecay(false);
		
		return true;
	}
	
	public bool SetCustomerCheckoutBonusAndMultiplier(string tag, float bonus, float multiplier) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerCheckoutBonusAndMultiplier: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].CheckoutBonus = bonus;
		Customers[customerTags[tag]].CheckoutMultiplier = multiplier;
				
		return true;
	}
	
	
	/// <summary>
	/// Sets the customer checkout bonuses and multiplier
	/// </summary>
	/// <returns>
	/// True on success
	/// </returns>
	/// <param name='tag'>
	/// The identifying tag of the customer
	/// </param>
	/// <param name='mutliplier'>
	/// The amount to multiply the customer's patience by on checkout
	/// </param>
	/// <param name='bonus'>
	/// The amount to add to the customer's patience (after multiplier) on checkout
	/// </param>
	
	[Obsolete("SetCustomerCheckoutBonuses is obsolete.  Use SetCustomerCheckoutBonusAndMultiplier instead", false)]
	public bool SetCustomerCheckoutBonuses(string tag, float mutliplier, float bonus) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerCheckoutBonuses: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].CheckoutBonus = bonus;
		Customers[customerTags[tag]].CheckoutMultiplier = mutliplier;
				
		return true;
	}
	
	/// <summary>
	/// Set a callback to be called when a customers patience runs out
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="callback">
	/// The callback method
	/// </param>
	/// <param name="userdata">
	/// The data to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating success
	/// </returns>
	public bool SetCustomerPatienceRanOutCallback(string tag, GenericCallback callback, object userdata) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerPatienceRanOutCallback: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].SetPatienceRanOutCallback(callback, userdata);
		
		return true;
	}
	
	/// <summary>
	/// Set a callback to be called when the customer is to enter the store
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="callback">
	/// A <see cref="GenericCallback"/> to be called on the event
	/// </param>
	/// <param name="userdata">
	/// A <see cref="System.Object"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/>
	/// </returns>
	public bool SetCustomerEntryDelayRanOutCallback(string tag, GenericCallback callback, object userdata) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerEntryDelayRanOutCallback: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].SetEntryDelayRanOutCallback(callback, userdata);
		
		return true;
	}
	
	/// <summary>
	/// Increment a customer's patience by a value
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="amount">
	/// The amount to increase (or decrease) the customer's patience
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/>
	/// </returns>
	public bool AddCustomerPatience(string tag, float amount) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("AddCustomerPatience: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].AddPatience(amount);
	
		return true;
	}
	
	public float GetCustomerPatience(string tag) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerPatience: no customer with tag " + tag + " exists!");
			return 0.0f;
		}
	
		return Customers[customerTags[tag]].Patience;
	}
	
	public float GetCustomerPatienceDecayRate(string tag) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerPatienceDecayRate: no customer with tag " + tag + " exists!");
			return 0.0f;
		}
	
		return Customers[customerTags[tag]].PatienceDecayRate;
	}
	
	public bool SetCustomerPatienceDecayRate(string tag, float decayRate) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerPatienceDecayRate: no customer with tag " + tag + " exists!");
			return false;
		}
	
		Customers[customerTags[tag]].PatienceDecayRate = decayRate;
		return true;
	}
	
	public float GetCustomerCheckoutBonus(string tag) {
			if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerCheckoutBonus: no customer with tag " + tag + " exists!");
			return 0.0f;
		}
	
		return Customers[customerTags[tag]].CheckoutBonus;
	}
	
	public float GetCustomerCheckoutMultiplier(string tag) {
			if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerCheckoutMultiplier: no customer with tag " + tag + " exists!");
			return 0.0f;
		}
	
		return Customers[customerTags[tag]].CheckoutMultiplier;
	}
	
	/// <summary>
	/// Get the current pending station request for a given customer
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <returns>
	/// The station type tag that the customer is requesting, or null if none exists
	/// </returns>
	public string GetCustomerStationRequest(string tag) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerStationRequest: no customer with tag " + tag + " exists!");
			return null;
		}
		
		if (Customers[customerTags[tag]].StationsRequested.Count == 0) return null;
		
		return (string)Customers[customerTags[tag]].StationsRequested.Peek();
	}
	
	public string GetCustomerStationRequest(string tag, bool includeStationTag, bool includeMinigameTag) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerStationRequest: no customer with tag " + tag + " exists!");
			return null;
		}
		
		if (Customers[customerTags[tag]].StationsRequested.Count == 0) return null;
		
		string stationReq = GetCustomerStationRequest(tag);
		
		if (!includeStationTag && !includeMinigameTag) return "";
		if (includeStationTag && includeMinigameTag) return stationReq;
				
		string[] strs = stationReq.Split('.');
		
		if (includeStationTag) return strs[0];
		if (includeMinigameTag && strs.Length > 1) return strs[1];
		
		return (string)Customers[customerTags[tag]].StationsRequested.Peek();
	}
	
	public string GetCustomerTypeTag(string tag) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerTypeTag: no customer with tag " + tag + " exists!");
			return null;
		}
		
		return Customers[customerTags[tag]].TypeTag;
	}
	
	public string GetStationCustomerIsAt(string tag) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("GetStationCustomerIsAt: no customer with tag " + tag + " exists!");
			return null;
		}
	
		return Customers[customerTags[tag]].StationTag;
	}
	
	public string GetCustomerAtStation(string tag) {
			if (!stationTags.ContainsKey(tag)) {
			Debug.Log("GetCustomerAtStation: no station with tag " + tag + " exists!");
			return null;
		}
	
		return Stations[stationTags[tag]].CustomerTag;
	}
	
	/// <summary>
	/// Turn the patience decay for the given customer on or off
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="shouldDecay">
	/// True if patience should decay, false otherwise
	/// </param>
	public bool SetCustomerPatienceShouldDecay(string tag, bool shouldDecay) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("SetCustomerPatienceShouldDecay: no customer with tag " + tag + " exists!");
			return false;
		}
		
		Customers[customerTags[tag]].SetPatienceShouldDecay(shouldDecay);
		
		return true;
	}
	#endregion
	
	#region Waiting Area Queries
	
	/// <summary>
	/// Places the customer at waiting area.
	/// </summary>
	/// <returns>
	/// The index of the position, or -1 on failure
	/// </returns>
	/// <param name='tag'>
	/// The identifying tag of the customer
	/// </param>
	public int PlaceCustomerAtWaitingArea(string tag) {
		if (WaitingArea == null) {
			Debug.Log("PlaceCustomerAtWaitingArea: waiting area is null");
			return -1;
		}
		
		return WaitingArea.PlaceCustomerAtWaitingArea(tag);
	}
	
	/// <summary>
	/// Determine if customer is in waiting area
	/// </summary>
	/// <returns>
	/// <c>true</c> if customer is in the waiting area
	/// </returns>
	/// <param name='tag'>
	/// The identifying tag of the customer
	/// </param>
	public bool IsCustomerAtWaitingArea(string tag) {
		if (WaitingArea == null) {
			Debug.Log("IsCustomerAtWaitingArea: waiting area is null");
			return false;
		}

		return WaitingArea.IsCustomerAtWaitingArea(tag);	
	}
	
	/// <summary>
	/// Removes the customer from waiting area.
	/// </summary>
	/// <returns>
	/// True if customer was removed
	/// </returns>
	/// <param name='tag'>
	/// The identifying tag of the customer
	/// </param>
	public bool RemoveCustomerFromWaitingArea(string tag) {
		if (WaitingArea == null) {
			Debug.Log("RemoveCustomerFromWaitingArea: waiting area is null");
			return false;
		}
		
		return WaitingArea.RemoveCustomerFromWaitingAreaByTag(tag);	
	}
	
	#endregion
	
	#region Register Queries
	public bool SetRegisterPatienceDecayMultiplier(float multiplier) {
		if (Register == null) {
			Debug.Log("SetRegisterPatienceDecayMultiplier: register has not be instantiated yet!");
			return false;
		}
		
		Register.PatienceDecayMultiplier = multiplier;
		return true;
	}
	
	public float GetRegisterPatienceDecayMultiplier() {
		if (Register == null) {
			Debug.Log("GetRegisterPatienceDecayMultiplier: register has not be instantiated yet!");
			return 0.0f;
		}
		
		return Register.PatienceDecayMultiplier;
	}
	
	public bool SetRegisterQueueChangedCallback(GenericCallback callback, object userdata) {
		if (Register == null) {
			Debug.Log("SetRegisterQueueChangedCallback: register has not be instantiated yet!");
			return false;
		}
		
		Register.SetQueueChangedCallback(callback, userdata);
		
		return true;
	}
	
	public int PlaceCustomerAtRegister(string tag) {
		if (Register == null) {
			Debug.Log("PlaceCustomerAtRegister: Register is null");
			return -1;
		}
		
		return Register.PlaceCustomerAtRegister(tag);
	}
	
	public bool RegisterHasWaitingCustomers() {
		if (Register == null) {
			Debug.Log("RegisterHasWaitingCustomers: Register is null");
			return false;
		}
		
		return Register.HasWaitingCustomers();
	}
	
	public int GetNumberOfActiveCustomers() {
		int count = 0;
		
		foreach (string key in customerTags.Keys) {
			if (!Customers[customerTags[key]].IsCheckedOut && Customers[customerTags[key]].Patience > 0.0f) count++;
		}
		
		return count;
	}
	
	public int GetNumberOfCheckedOutCustomers() {
		int count = 0;
		
		foreach (string key in customerTags.Keys) {
			if (Customers[customerTags[key]].IsCheckedOut) count++;
		}
		
		return count;
	}
	
	public int GetNumberOfLostCustomers() {
		int count = 0;
		
		foreach (string key in customerTags.Keys) {
			if (!Customers[customerTags[key]].IsCheckedOut && Customers[customerTags[key]].Patience <= 0.0f) count++;
		}
		
		return count;
	}
	
	public bool IsCustomerAtRegister(string tag) {
		if (Register == null) {
			Debug.Log("IsCustomerAtRegister: Register is null");
			return false;
		}

		return Register.IsCustomerAtRegister(tag);	
	}
	
	public string GetCustomerAtRegisterPosition(int index) {
		if (Register == null) {
			Debug.Log("GetCustomerAtRegisterByIndex: Register is null");
			return null;
		}
		
		return Register.GetCustomerAtPosition(index);
	}
	
	public bool RemoveCustomerFromRegister(string tag) {
		if (Register == null) {
			Debug.Log("RemoveCustomerFromRegister: Register is null");
			return false;
		}
		
		return Register.RemoveCustomerFromRegisterByTag(tag);	
	}
	
	public void SwapRegisterPositions(int position0, int position1) {
		if (Register == null) {
			Debug.Log("SwapRegisterPositions: Register is null");
			return;
		}
		
		Register.SwapPositions(position0, position1);
	}
	
	public int GetAvailableRegisterPosition() {
		if (Register == null) {
			Debug.Log("GetAvailableRegisterPosition: Register is null");
			return -1;
		}
		
		return Register.GetAvailableRegister();	
	}
	
	public int GetCustomerPositionAtRegister(string tag) {
		if (Register == null) {
			Debug.Log("GetCustomerPositionAtRegister: Register is null");
			return -1;
		}	
		
		return Register.GetCustomerPosition(tag);
	}
	
	public int GetNumberOfRegisterPositions() {
		if (Register == null) {
			Debug.Log("GetNumberOfRegisterPositions: Register is null");
			return -1;
		}
	
		return Register.GetNumberOfPositions();
	}
	
	#endregion
	
	#region Actions	
	
	/// <summary>
	/// Attempt to place a customer in a station.  If the station is occupied, or customer does not have the station as the next requested in his station request stack, this will fail.
	/// </summary>
	/// <param name="customerTag">
	/// Identifying tag of the customer
	/// </param>
	/// <param name="stationTag">
	/// Identifying tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if placement was succefful
	/// </returns>
	public bool PlaceCustomerInStation(string customerTag, string stationTag) {
		if (!stationTags.ContainsKey(stationTag)) {
			Debug.Log("PlaceCustomerInStation: Station with tag " + stationTag + " does not exist");			
			return false;
		} else if (!customerTags.ContainsKey(customerTag)) {
			Debug.Log("PlaceCustomerInStation: Customer with tag " + customerTag + " does not exist");			
			return false;
		}
		
		TMGCustomer customer = Customers[customerTags[customerTag]];
		TMGStation station = Stations[stationTags[stationTag]];
		
		if (station.IsOccupied) {
			Debug.Log("PlaceCustomerInStation: Station " + stationTag + " is already occupied!  You should call StationIsOccupied() to check for this first.");
			return false;
		}
		
		if (customer.StationsRequested == null || customer.StationsRequested.Count == 0) {
			Debug.Log("PlaceCustomerInStation: Customer " + customerTag + " has no requested stations, or request list is null");
			return false;
		}
		
		string stationReq = (string)customer.StationsRequested.Peek();
		stationReq = stationReq.Split('.')[0];
		
		if (stationReq != station.TypeTag) {
			Debug.Log("PlaceCustomerInStation: Customer " + customerTag + " is not requesting this station.  He is requesting " + customer.StationsRequested.Peek());
			return false;
		}
		
		if (customer.StationTag != null) {
			RemoveCustomerFromStation(stationTag);	
		}	
		
		station.CustomerTag = customerTag;
		customer.StationTag = stationTag;
		
		station.IsOccupied = true;
				
		return true;
	}
	
	/// <summary>
	/// Remove the customer from a station, if it is occupied
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// True if successful
	/// </returns>
	public bool RemoveCustomerFromStation(string tag) {
		if (!stationTags.ContainsKey(tag)) {
			Debug.Log("RemoveCustomerFromStation: Station with tag " + tag + " does not exist");
			
			return false;
		}
		
		TMGStation station = Stations[stationTags[tag]];
		
		if (!station.IsOccupied) {
			Debug.Log("RemoveCustomerFromStation: Station " + tag + " is unoccupied, so no customer can be removed");
			
			return false;
		}
		
		string customerTag = station.RemoveCustomer();
		
		if (customerTags.ContainsKey(customerTag)) {
			Customers[customerTags[customerTag]].StationTag = null;	
		} 
		
		return true;
		
	}
	
	/// <summary>
	/// Pop a requested station from a customer's request stack.  Generally called after a station finishes running.
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of the customer
	/// </param>
	/// <returns>
	/// The popped request string
	/// </returns>
	public string PopCustomerStationRequest(string tag) {
		if (!customerTags.ContainsKey(tag)) {
			Debug.Log("PopCustomerStationRequest: no customer with tag " + tag + " exists!");
			return null;
		}
		
		if (Customers[customerTags[tag]].StationsRequested.Count == 0) {
			Debug.Log("PopCustomerStationRequest: customer with tag " + tag + " has no requests to pop!");
			return null;
		}
		
		return (string)Customers[customerTags[tag]].StationsRequested.Pop();
	}
	
	/// <summary>
	/// Check if a customer is requesting a given station
	/// </summary>
	/// <param name="customerTag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="stationTag">
	/// The identifying tag of the station
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the customer is requesting this station
	/// </returns>
	public bool IsCustomerRequestingStation(string customerTag, string stationTag) {
		if (!customerTags.ContainsKey(customerTag)) {
			Debug.Log("IsCustomerRequestingStation: no customer with tag " + customerTag + " exists!");
			return false;
		}
		
		if (!stationTags.ContainsKey(stationTag)) {
			Debug.Log("IsCustomerRequestingStation: no station with tag " + stationTag + " exists!");
			return false;
		}
		
		string stationReq = (string)Customers[customerTags[customerTag]].StationsRequested.Peek();
		stationReq = stationReq.Split('.')[0];
		
		return stationReq == Stations[stationTags[stationTag]].TypeTag;
	}
	
	/// <summary>
	/// Check if a customer is requesting a given station type
	/// </summary>
	/// <param name="customerTag">
	/// The identifying tag of the customer
	/// </param>
	/// <param name="stationTag">
	/// The identifying tag of the station type
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating if the customer is requesting this station type
	/// </returns>
	public bool IsCustomerRequestingStationType(string customerTag, string stationTypeTag) {
		if (!customerTags.ContainsKey(customerTag)) {
			Debug.Log("IsCustomerRequestingStation: no customer with tag " + customerTag + " exists!");
			return false;
		}			
		
		string stationReq = (string)Customers[customerTags[customerTag]].StationsRequested.Peek();
		stationReq = stationReq.Split('.')[0];
		
		return stationReq == stationTypeTag;
	}
	
	/// <summary>
	/// Start running a station
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of a station
	/// </param>
	/// <param name="callback">
	/// A <see cref="GenericCallback"/> to be called when the station finished running
	/// </param>
	/// <param name="userdata">
	/// A <see cref="System.String"/> of userdata to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating that the station was started successfully
	/// </returns>
	public bool RunStation(string tag, GenericCallback callback, string userdata) {
		if (stationTags.ContainsKey(tag)) {
			return Stations[stationTags[tag]].Run(callback, userdata);
		} else {
			Debug.Log("RunStation: Station with tag " + tag + " does not exist!");
			return false;
		}
	}
	
	/// <summary>
	/// Start running a terminal
	/// </summary>
	/// <param name="tag">
	/// The identifying tag of a terminal
	/// </param>
	/// <param name="callback">
	/// A <see cref="GenericCallback"/> to be called when the terminal finished running
	/// </param>
	/// <param name="userdata">
	/// A <see cref="System.String"/> of userdata to be returned with the callback
	/// </param>
	/// <returns>
	/// A <see cref="System.Boolean"/> indicating that the terminal was started successfully
	/// </returns>
	public bool RunTerminal(string tag, GenericCallback callback, string userdata) {
		if (terminalTags.ContainsKey(tag)) {
			return Terminals[terminalTags[tag]].Run(callback, userdata);
		} else {
			Debug.Log("RunTerminal: Terminal with tag " + tag + " does not exist!");
			return false;
		}
	}
	
	public bool RunDevice(string tag, GenericCallback callback, string userdata) {
		if (deviceTags.ContainsKey(tag)) {
			return Devices[deviceTags[tag]].Run(callback, userdata);
		} else {
			Debug.Log("RunDevice: Device with tag " + tag + " does not exist!");
			return false;
		}
	}
	
	public bool RunDispenser(string tag, GenericCallback callback, string userdata) {
		if (dispenserTags.ContainsKey(tag)) {
			return Dispensers[dispenserTags[tag]].Run(callback, userdata);
		} else {
			Debug.Log("RunDispenser: Dispenser with tag " + tag + " does not exist!");
			return false;
		}
	}
	#endregion
	
	#region Transport Methods
	
	/// <summary>
	/// Start running the simulation
	/// </summary>
	public void Run() {
		isRunning = true;	
	}
	
	/// <summary>
	/// Stop running the simulation
	/// </summary>
	public void Stop() {
		isRunning = false;	
	}
		
	/// <summary>
	///  Reset the timer of the simulation
	/// </summary>
	public void Reset() {
		timer = 0.0f;	
	}
	#endregion
	
	#region Scheduler
	public bool AddScheduledCallback(float delay, GenericCallback callback, object userdata) {
		ScheduledCallback s = new ScheduledCallback();
		s.Delay = delay;
		s.Callback = callback;
		s.Userdata = userdata;
		
		scheduledCallbacks.Add(s);
		
		return true;
	}
	
	#endregion
	
}
