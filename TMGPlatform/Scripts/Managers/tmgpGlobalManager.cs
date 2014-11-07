using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A singleton class to be used for any global management tasks
/// </summary>
public class TMGGlobalManagement {
	
	#region Public Properties
	
	#endregion
	
	#region Private Properties

	#endregion
	
	#region Singleton Instanciation
	private static TMGGlobalManagement instance = null;
	public TMGGlobalManagement() {
		if (instance != null) {
			Debug.LogError("Multiple instanciation of GlobalManager");
			return;
		}
		
		instance = this;
	}
	
	public static TMGGlobalManagement Instance {
		get {
			if (instance == null) {
				new TMGGlobalManagement();
			}
			
			return instance;
		}
	}	
	#endregion
	
	
	
}