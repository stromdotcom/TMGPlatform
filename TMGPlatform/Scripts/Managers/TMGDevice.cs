using UnityEngine;
using System.Collections;

public class TMGDevice {
	
	#region Public Properties
	
	public string TypeTag = null;
		
	public float WarmUpTime = 1.0f;
	public float CoolDownTime = 0.5f;
	public float RunTime = 1.0f;
	public bool IsReady = false;
	public bool IsRunning = false;
	#endregion
	
	#region Private Properties
	private TMGManager.GenericCallback onWarmedUpCallback = null;
	private object onWarmedUpUserdata = null;
	
	private TMGManager.GenericCallback onCooledDownCallback = null;
	private object onCooledDownUserdata = null;
	
	private TMGManager.GenericCallback onCompleteCallback = null;
	private object onCompleteUserdata = null;
	
	private float warmUpAccumulator = 0.0f;
	private float coolDownAccumulator = 0.0f;
	private float runAccumulator = 0.0f;
	#endregion
	
	// Use this for initialization
	public void Initialize() {
		IsReady = false;
		IsRunning = false;
		warmUpAccumulator = WarmUpTime;
	}
	
	// Update is called once per frame
	public void Update (float dt) {
		
		if (IsRunning) {
			
			runAccumulator -= dt;
			
			if (runAccumulator < 0.0f) {
				IsRunning = false;	
				coolDownAccumulator = CoolDownTime;
				
				if (onCompleteCallback != null) {
					onCompleteCallback(onCompleteUserdata);	
				}
			}
		} else if (warmUpAccumulator > 0.0f) {
			warmUpAccumulator -= dt;
			
			if (warmUpAccumulator <= 0.0f) {
				IsReady = true;
		
				if (onWarmedUpCallback != null) {
					onWarmedUpCallback(onWarmedUpUserdata);	
				}
			}
		} else if (coolDownAccumulator > 0.0f) {
			coolDownAccumulator -= dt;
			
			if (coolDownAccumulator <= 0.0f) {
				IsReady = true;
		
				if (onCooledDownCallback != null) {
					onCooledDownCallback(onCooledDownUserdata);	
				}
			}
		} 
	}
	
	public void SetWarmedUpCallback(TMGManager.GenericCallback cb, object userdata) {
		onWarmedUpCallback = cb;
		onWarmedUpUserdata = userdata;
	}
	
	public void SetCooledDownCallback(TMGManager.GenericCallback cb, object userdata) {
		onCooledDownCallback = cb;
		onCooledDownUserdata = userdata;
	}
		
	public bool Run(TMGManager.GenericCallback OnComplete, object OnCompleteUserdata) {
		if (!IsReady || IsRunning) return false;
				
		runAccumulator = RunTime;
		IsReady = false;
		IsRunning = true;
		
		onCompleteCallback = OnComplete;
		onCompleteUserdata = OnCompleteUserdata;
		
		return true;
	}
	
}