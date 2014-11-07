using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TMGLevel))]
public class TMGLevel_Inspector : Editor {
	
	TMGLevel level;
	Vector3 NodeHandleSnap = new Vector3(0.1f,0.1f,0.1f);
	bool showPlayerEditor = false;
	bool showStationEditor = false;
	bool showTerminalEditor = false;
	bool showDeviceEditor = false;
	bool showDispenserEditor = false;
	bool showCustomerEditor = false;
	bool showNodesEditor = false;
	bool showWaitingAreaEditor = false;
	bool showRegisterEditor = false;
	
	bool[] showStationConfigEditor = new bool[0];
	bool[] showTerminalConfigEditor = new bool[0];
	bool[] showDeviceConfigEditor = new bool[0];
	bool[] showDispenserConfigEditor = new bool[0];
	bool[] showCustomerConfigEditor = new bool[0];
	bool[] showNodeConfigEditor = new bool[0];
	
	// Temporary objects
	Transform SceneBG = null;
	Dictionary<string,Transform> SceneStations = new Dictionary<string, Transform>();
	
	public void OnEnable() {
		level = (TMGLevel)target;
		SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(OnSceneGUI);
	}
	
	public void OnDisable() {
		SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(OnSceneGUI);	
		
		RemoveTempObjects();
	}
	
	public Vector3 Vector3Field(string label, Vector3 vector, int labelWidth = 100) {
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(label, GUILayout.Width(labelWidth));
		vector = new Vector3(
			EditorGUILayout.FloatField(vector.x),
			EditorGUILayout.FloatField(vector.y),
			EditorGUILayout.FloatField(vector.z)
		);
		EditorGUILayout.EndHorizontal();
		return vector;
	}
	
	public void OnSceneGUI(SceneView sceneView) {
		
		Handles.color = Color.blue;
		
		for (int i = 0; i < level.Nodes.Length; i++) {
			level.Nodes[i].Position = Handles.FreeMoveHandle(level.Nodes[i].Position, Quaternion.identity, 0.02f, NodeHandleSnap, Handles.DotCap);	
		}
		
		Handles.color = Color.green;
		
		// Now draw the connections between the nodes
		for (int i = 0; i < level.Nodes.Length; i++) {
			if (level.Nodes[i].Connections == "") continue;
			
			string[] connections = level.Nodes[i].Connections.Split(',');
			
			foreach (string tag in connections) {
				
				// Find the node with the matching tag and draw a line to it
				for (int j = 0; j < level.Nodes.Length; j++) {
					if (level.Nodes[j].IDTag == tag)
						Handles.DrawLine(level.Nodes[i].Position, level.Nodes[j].Position);
						
				}
			}
		}
		
		//CreateTempObjects();
		
		if (GUI.changed)
        	EditorUtility.SetDirty (target);
	}
	
	void CreateTempObjects() {
		// Place temporary objects
		if (SceneBG != null) DestroyImmediate(SceneBG.gameObject);
		
		Object o = Resources.Load("Prefabs/Locations/" + level.Locale + "/Background");
		
		if (o != null) {
			GameObject bg = (GameObject)Instantiate(o);
				
			if (bg != null) {
				SceneBG = bg.transform;
				SceneBG.position = new Vector3(0,0,0);
			}
		}
		
		foreach (string key in SceneStations.Keys) if (SceneStations[key] != null) DestroyImmediate(SceneStations[key].gameObject);
		
		foreach (TMGLevel._Station s in level.Stations) {
					
			GameObject st = (GameObject)Instantiate(Resources.Load("Prefabs/Station"));
			
			if (st != null) {
				SceneStations[s.IDTag] = st.transform;
				SceneStations[s.IDTag].position = s.Position;
				
				//Station sta = SceneStations[s.IDTag].transform.GetComponent<Station>();
				
				//sta.sprite.spriteId = sta.sprite.GetSpriteIdByName("Station_" + s.TypeTag + "_" + s.BaseUpgradeLevel);
			}
		}
	}
	
	void RemoveTempObjects() {
		if (SceneBG != null) DestroyImmediate(SceneBG.gameObject);
		
		foreach (string s in SceneStations.Keys) if (SceneStations[s] != null) DestroyImmediate(SceneStations[s].gameObject);
	}
	
	
	public override void OnInspectorGUI() {
		//DrawDefaultInspector();	
		
		EditorGUILayout.Separator();
		
		Rect rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Level Editor");
		EditorGUI.indentLevel = 0;
		
		EditorGUILayout.LabelField("Level Editor","Configure core level details");
		level.LevelName = EditorGUILayout.TextField("Name", level.LevelName);
		level.RunTime = EditorGUILayout.Slider("Run Time", level.RunTime, 0.0f, 300.0f);
		level.Locale = EditorGUILayout.TextField("Locale", level.Locale);
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		EditorGUILayout.LabelField("Score Milestones","Set score milestones");
		level.BronzeScore = EditorGUILayout.FloatField("Bronze Score", level.BronzeScore);
		level.SilverScore = EditorGUILayout.FloatField("Silver Score", level.SilverScore);
		level.GoldScore = EditorGUILayout.FloatField("Gold Score", level.GoldScore);
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		EditorGUILayout.LabelField("Intro Objects","Story and intro screens and popups");
		level.StoryScreens = EditorGUILayout.TextField("Story Screens", level.StoryScreens);
		level.OutroScreens = EditorGUILayout.TextField("Outro Screens", level.OutroScreens);
		level.IntroPopups = EditorGUILayout.TextField("Intro Popups", level.IntroPopups);
		
		if (level.Player == null) level.Player = new TMGLevel._Player();
		if (level.Stations == null) level.Stations = new TMGLevel._Station[0];
		if (level.Terminals == null) level.Terminals = new TMGLevel._Terminal[0];
		if (level.Devices == null) level.Devices = new TMGLevel._Device[0];
		if (level.Dispensers == null) level.Dispensers = new TMGLevel._Dispenser[0];
		if (level.Customers == null) level.Customers = new TMGLevel._Customer[0];
		if (level.Nodes == null) level.Nodes = new TMGLevel._Node[0];
		
		if (level.WaitingPositions == null) level.WaitingPositions = new TMGLevel._WaitingPosition[0];
		if (level.RegisterPositions == null) level.RegisterPositions = new TMGLevel._RegisterPosition[0];
		
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		#region Player Editor
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Player Editor");
				
		showPlayerEditor = EditorGUILayout.Foldout(showPlayerEditor, "Player Configuration");
				
		if (showPlayerEditor) {
			level.Player.StartPosition = EditorGUILayout.Vector3Field("Start Position", level.Player.StartPosition);
			level.Player.ReadyPosition = EditorGUILayout.Vector3Field("Ready Position", level.Player.ReadyPosition);
		}
		
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Waiting Area Editor
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Waiting Area Editor");
				
		showWaitingAreaEditor = EditorGUILayout.Foldout(showWaitingAreaEditor, "Waiting Area Configuration");
				
		if (showWaitingAreaEditor) {
			level.WaitingAreaPosition = Vector3Field("Position", level.WaitingAreaPosition);
			level.WaitingAreaOverflowPosition = Vector3Field("Overflow Position", level.WaitingAreaOverflowPosition);
		
			GUILayout.Label("Waiting Positions");
			
			if(GUILayout.Button("Add Position")) {
				ArrayUtility.Add(ref level.WaitingPositions, new TMGLevel._WaitingPosition());
				GUI.changed = true;
			}
			
			for (int i = 0; i < level.WaitingPositions.Length; i++) {
				EditorGUILayout.BeginHorizontal("box");
				EditorGUILayout.BeginVertical();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this position?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.WaitingPositions, i);
					break;
				}
				level.WaitingPositions[i].Position = Vector3Field("Position " + i, level.WaitingPositions[i].Position);	
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUI.indentLevel = 3;
				level.WaitingPositions[i].RequiresUpgrade = EditorGUILayout.Toggle("Requires upgrade?", level.WaitingPositions[i].RequiresUpgrade);
				
				if (level.WaitingPositions[i].RequiresUpgrade) {
					level.WaitingPositions[i].UpgradeRequired = EditorGUILayout.TextField("Upgrade", level.WaitingPositions[i].UpgradeRequired);	
				}
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				
			}
		}
					
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Waiting Area Editor OLD
		/*
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Waiting Area Editor");
		
		showWaitingAreaEditor = EditorGUILayout.Foldout(showWaitingAreaEditor, "Waiting Area Configuration");
				
		if (showWaitingAreaEditor) {
			level.WaitingAreaPosition = Vector3Field("Position", level.WaitingAreaPosition);
			
			GUILayout.Label("Waiting Positions");
			
			if(GUILayout.Button("Add Position")) {
				ArrayUtility.Add(ref level.WaitingPositions, Vector3.zero);
				GUI.changed = true;
			}
			
			for (int i = 0; i < level.WaitingPositions.Length; i++) {
				EditorGUILayout.BeginHorizontal("box");
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this position?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.WaitingPositions, i);
					break;
				}
				level.WaitingPositions[i] = Vector3Field("Position " + i, level.WaitingPositions[i]);	
				EditorGUILayout.EndHorizontal();
			}
		}
					
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		*/
		#endregion
		
		#region Register Editor
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Register Editor");
		
		showRegisterEditor = EditorGUILayout.Foldout(showRegisterEditor, "Register Configuration");
				
		if (showRegisterEditor) {
			level.RegisterPosition = Vector3Field("Position", level.RegisterPosition);	
									
			GUILayout.Label("Register Positions");
			
			if(GUILayout.Button("Add Position")) {
				ArrayUtility.Add(ref level.RegisterPositions, new TMGLevel._RegisterPosition());
				GUI.changed = true;
			}
			
			for (int i = 0; i < level.RegisterPositions.Length; i++) {
				EditorGUILayout.BeginHorizontal("box");
				EditorGUILayout.BeginVertical();
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this position?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.RegisterPositions, i);
					break;
				}
				level.RegisterPositions[i].Position = Vector3Field("Position " + i, level.RegisterPositions[i].Position);	
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUI.indentLevel = 3;
				level.RegisterPositions[i].RequiresUpgrade = EditorGUILayout.Toggle("Requires upgrade?", level.RegisterPositions[i].RequiresUpgrade);
				
				if (level.RegisterPositions[i].RequiresUpgrade) {
					level.RegisterPositions[i].UpgradeRequired = EditorGUILayout.TextField("Upgrade", level.RegisterPositions[i].UpgradeRequired);	
				}
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
		}
		
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Register Editor OLD
		/*
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Register Editor");
		
		showRegisterEditor = EditorGUILayout.Foldout(showRegisterEditor, "Register Configuration");
				
		if (showRegisterEditor) {
			level.RegisterPosition = Vector3Field("Position", level.RegisterPosition);	
									
			GUILayout.Label("Register Positions");
			
			if(GUILayout.Button("Add Position")) {
				ArrayUtility.Add(ref level.RegisterPositions, Vector3.zero);
				GUI.changed = true;
			}
			
			for (int i = 0; i < level.RegisterPositions.Length; i++) {
				EditorGUILayout.BeginHorizontal("box");
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this position?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.RegisterPositions, i);
					break;
				}
				level.RegisterPositions[i] = Vector3Field("Position " + i, level.RegisterPositions[i]);	
				EditorGUILayout.EndHorizontal();
			}
		}
		
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		*/			
		#endregion
		
		#region Stations Editor
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Stations Editor");
		
		showStationEditor = EditorGUILayout.Foldout(showStationEditor, "Station Configuration");
		
		if (showStationEditor) {	
			// Check station count and recreate the Station array if it changed (copying over the old values)
			if(GUILayout.Button("Add Station")) {
				ArrayUtility.Add(ref level.Stations, new TMGLevel._Station());
				ArrayUtility.Add(ref showStationConfigEditor, true);
				GUI.changed = true;
			}
						
			if (showStationConfigEditor.Length != level.Stations.Length) showStationConfigEditor = new bool[level.Stations.Length];
			
		
			//EditorGUI.indentLevel = 2;
			for (int i = 0; i < level.Stations.Length; i++) {
				EditorGUILayout.BeginVertical("box");
				//EditorGUI.indentLevel = 1;			
				
				TMGLevel._Station s = level.Stations[i];
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this station?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.Stations, i);
					break;
				}
				showStationConfigEditor[i] = EditorGUILayout.Foldout((bool)showStationConfigEditor[i], "Station " + s.IDTag + " (Type " + s.TypeTag + " )" + (s.ParentTag != ""?" (Parent " + s.ParentTag + ")":""));
				EditorGUILayout.EndHorizontal();
				
				if (showStationConfigEditor[i]) {
					//EditorGUI.indentLevel = 2;
					
					//GUILayout.Box("Staton " + s.IDTag + " (Type " + s.TypeTag + ")", GUILayout.ExpandWidth(true));
					s.IDTag = EditorGUILayout.TextField("Tag", s.IDTag);
					s.TypeTag = EditorGUILayout.TextField("Station Type", s.TypeTag);
					s.ParentTag = EditorGUILayout.TextField("Parent Station", s.ParentTag);
					
					s.Position = Vector3Field("Position", s.Position, 147);
					s.ActivationPosition = Vector3Field("Acivation Position", s.ActivationPosition, 147);
					s.CustomerPosition = Vector3Field("Customer Position", s.CustomerPosition, 147);
					
					s.OverrideDefaultSettings = EditorGUILayout.Toggle("Override defaults", s.OverrideDefaultSettings);
					
					if (s.OverrideDefaultSettings) {
						EditorGUI.indentLevel = 3;
						s.RunTime = EditorGUILayout.Slider("Run Time", s.RunTime, 0.0f, 10.0f);
						s.MinigameChance = EditorGUILayout.Slider("Minigame Chance", s.MinigameChance, 0.0f, 10.0f);
						s.PatienceBonus = EditorGUILayout.FloatField("Patience Bonus", s.PatienceBonus);
						EditorGUI.indentLevel = 0;
					}
					
					s.IsAutomatic = EditorGUILayout.Toggle("Is Automatic", s.IsAutomatic);
					s.HasAssistant = EditorGUILayout.Toggle("Has Assistant", s.HasAssistant);
					if (s.HasAssistant) {
						EditorGUI.indentLevel = 3;
						EditorGUILayout.BeginVertical();
						s.AssistantPosition = EditorGUILayout.Vector3Field("Pos:", s.AssistantPosition);
						
						EditorGUILayout.BeginHorizontal();
						s.AssistantRequiresUpgrade = EditorGUILayout.Toggle("Requires Upgrade", s.AssistantRequiresUpgrade);
						
						if (s.AssistantRequiresUpgrade) {
							s.AssistantUpgradeRequired = EditorGUILayout.TextField("Upgrade Required", s.AssistantUpgradeRequired);
						}
						
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.EndVertical();
						EditorGUI.indentLevel = 0;
					}
										
					EditorGUILayout.Separator();
										
					EditorGUILayout.LabelField("Upgrade settings");
					
					EditorGUI.indentLevel = 3;
					s.RequiresUpgrade = EditorGUILayout.Toggle("Requires upgrade", s.RequiresUpgrade);
					
					if (s.RequiresUpgrade) {
						EditorGUI.indentLevel = 4;
						s.UpgradeRequired = EditorGUILayout.TextField("Upgrade", s.UpgradeRequired);	
						EditorGUI.indentLevel = 3;
					}
					
					s.BaseUpgradeLevel = EditorGUILayout.IntField("Base level", s.BaseUpgradeLevel);
					EditorGUI.indentLevel = 0;
				}
				EditorGUILayout.EndVertical();
			}
			
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Terminals Editor
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Terminals Editor");
		EditorGUI.indentLevel = 0;
		showTerminalEditor = EditorGUILayout.Foldout(showTerminalEditor, "Terminals Configuration");
		
		if (showTerminalEditor) {	
			// Check station count and recreate the Station array if it changed (copying over the old values)
			if(GUILayout.Button("Add Terminal")) {
				ArrayUtility.Add(ref level.Terminals, new TMGLevel._Terminal());
				ArrayUtility.Add(ref showTerminalConfigEditor, true);
				GUI.changed = true;
			}
						
			if (showTerminalConfigEditor.Length != level.Terminals.Length) showTerminalConfigEditor = new bool[level.Terminals.Length];
			
			EditorGUI.indentLevel = 0;
			for (int i = 0; i < level.Terminals.Length; i++) {
				EditorGUILayout.BeginVertical("box");
				//EditorGUI.indentLevel = 0;			
				
				TMGLevel._Terminal t = level.Terminals[i];
				
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this terminal?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.Terminals, i);
					break;
				}
				showTerminalConfigEditor[i] = EditorGUILayout.Foldout((bool)showTerminalConfigEditor[i], "Terminal " + t.IDTag + " (Type " + t.TypeTag + " )");
				EditorGUILayout.EndHorizontal();
				
				if (showTerminalConfigEditor[i]) {
					//EditorGUI.indentLevel = 2;
					
					t.IDTag = EditorGUILayout.TextField("Tag", t.IDTag);
					t.TypeTag = EditorGUILayout.TextField("Terminal Type", t.TypeTag);
					
					t.OverrideDefaultSettings = EditorGUILayout.Toggle("Override defaults", t.OverrideDefaultSettings);
					
					if (t.OverrideDefaultSettings) {
						EditorGUI.indentLevel = 3;
						t.RunTime = EditorGUILayout.Slider("Run Time", t.RunTime, 0.0f, 10.0f);
						EditorGUI.indentLevel = 0;
					}
					
					t.IsAutomatic = EditorGUILayout.Toggle("Is Automatic", t.IsAutomatic);
					t.Position = Vector3Field("Position", t.Position, 147);
					EditorGUILayout.Separator();
										
					EditorGUILayout.LabelField("Upgrade settings");
					
					EditorGUI.indentLevel = 3;
					t.RequiresUpgrade = EditorGUILayout.Toggle("Requires upgrade", t.RequiresUpgrade);
					
					if (t.RequiresUpgrade) {
						EditorGUI.indentLevel = 4;
						t.UpgradeRequired = EditorGUILayout.TextField("Upgrade", t.UpgradeRequired);	
						EditorGUI.indentLevel = 3;
					}
					
					t.BaseUpgradeLevel = EditorGUILayout.IntField("Base level", t.BaseUpgradeLevel);
					EditorGUI.indentLevel = 0;
				}
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Devices Editor
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Devices Editor");
		
		showDeviceEditor = EditorGUILayout.Foldout(showDeviceEditor, "Devices Configuration");
		
		if (showDeviceEditor) {	
			// Check device count and recreate the Device array if it changed (copying over the old values)
			if(GUILayout.Button("Add Device")) {
				ArrayUtility.Add(ref level.Devices, new TMGLevel._Device());
				ArrayUtility.Add(ref showDeviceConfigEditor, true);
				GUI.changed = true;
			}
						
			if (showDeviceConfigEditor.Length != level.Devices.Length) showDeviceConfigEditor = new bool[level.Devices.Length];
			
		
			//EditorGUI.indentLevel = 2;
			for (int i = 0; i < level.Devices.Length; i++) {
				EditorGUILayout.BeginVertical("box");
				//EditorGUI.indentLevel = 1;			
				
				TMGLevel._Device d = level.Devices[i];
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this device?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.Devices, i);
					break;
				}
				showDeviceConfigEditor[i] = EditorGUILayout.Foldout((bool)showDeviceConfigEditor[i], "Device " + d.IDTag + " (Type " + d.TypeTag + " )");
				EditorGUILayout.EndHorizontal();
				
				if (showDeviceConfigEditor[i]) {
					//EditorGUI.indentLevel = 2;
					
					//GUILayout.Box("Staton " + s.IDTag + " (Type " + s.TypeTag + ")", GUILayout.ExpandWidth(true));
					d.IDTag = EditorGUILayout.TextField("Tag", d.IDTag);
					d.TypeTag = EditorGUILayout.TextField("Device Type", d.TypeTag);
					
					d.OverrideDefaultSettings = EditorGUILayout.Toggle("Override defaults", d.OverrideDefaultSettings);
					
					if (d.OverrideDefaultSettings) {
						EditorGUI.indentLevel = 3;
						d.WarmupTime = EditorGUILayout.Slider("Warmup Time", d.WarmupTime, 0.0f, 10.0f);
						d.CooldownTime = EditorGUILayout.Slider("Cooldown Time", d.CooldownTime, 0.0f, 10.0f);
						d.RunTime = EditorGUILayout.Slider("Run Time", d.RunTime, 0.0f, 10.0f);
						EditorGUI.indentLevel = 0;
					}
										
					d.Position = Vector3Field("Position", d.Position, 147);
					EditorGUILayout.Separator();
										
					EditorGUILayout.LabelField("Upgrade settings");
					
					EditorGUI.indentLevel = 3;
					d.RequiresUpgrade = EditorGUILayout.Toggle("Requires upgrade", d.RequiresUpgrade);
					
					if (d.RequiresUpgrade) {
						EditorGUI.indentLevel = 4;
						d.UpgradeRequired = EditorGUILayout.TextField("Upgrade", d.UpgradeRequired);	
						EditorGUI.indentLevel = 3;
					}
					
					d.BaseUpgradeLevel = EditorGUILayout.IntField("Base level", d.BaseUpgradeLevel);
					EditorGUI.indentLevel = 0;
				}
				EditorGUILayout.EndVertical();
			}
			
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Dispensers Editor
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Dispensers Editor");
		
		showDispenserEditor = EditorGUILayout.Foldout(showDispenserEditor, "Dispensers Configuration");
		
		if (showDispenserEditor) {	
			// Check device count and recreate the Device array if it changed (copying over the old values)
			if(GUILayout.Button("Add Dispenser")) {
				ArrayUtility.Add(ref level.Dispensers, new TMGLevel._Dispenser());
				ArrayUtility.Add(ref showDispenserConfigEditor, true);
				GUI.changed = true;
			}
						
			if (showDispenserConfigEditor.Length != level.Dispensers.Length) showDispenserConfigEditor = new bool[level.Dispensers.Length];
			
		
			//EditorGUI.indentLevel = 2;
			for (int i = 0; i < level.Dispensers.Length; i++) {
				EditorGUILayout.BeginVertical("box");
				//EditorGUI.indentLevel = 1;			
				
				TMGLevel._Dispenser d = level.Dispensers[i];
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this dispenser?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.Dispensers, i);
					break;
				}
				showDispenserConfigEditor[i] = EditorGUILayout.Foldout((bool)showDispenserConfigEditor[i], "Dispenser " + d.IDTag + " (Type " + d.TypeTag + " )");
				EditorGUILayout.EndHorizontal();
				
				if (showDispenserConfigEditor[i]) {
					//EditorGUI.indentLevel = 2;
					
					//GUILayout.Box("Staton " + s.IDTag + " (Type " + s.TypeTag + ")", GUILayout.ExpandWidth(true));
					d.IDTag = EditorGUILayout.TextField("Tag", d.IDTag);
					d.TypeTag = EditorGUILayout.TextField("Dispenser Type", d.TypeTag);
					
					d.OverrideDefaultSettings = EditorGUILayout.Toggle("Override defaults", d.OverrideDefaultSettings);
					
					if (d.OverrideDefaultSettings) {
						EditorGUI.indentLevel = 3;
						d.WarmupTime = EditorGUILayout.Slider("Warmup Time", d.WarmupTime, 0.0f, 10.0f);
						d.CooldownTime = EditorGUILayout.Slider("Cooldown Time", d.CooldownTime, 0.0f, 10.0f);
						d.RunTime = EditorGUILayout.Slider("Run Time", d.RunTime, 0.0f, 10.0f);
						
						EditorGUILayout.Separator();
						
						d.Units = EditorGUILayout.IntField("Units", d.Units);
						d.MaxUnits = EditorGUILayout.IntField("Max Units", d.MaxUnits);
						d.UnitsPerCompletion = EditorGUILayout.IntField("Units per Completion", d.UnitsPerCompletion);
						
						EditorGUI.indentLevel = 0;
					}
										
					d.Position = Vector3Field("Position", d.Position, 147);
					EditorGUILayout.Separator();
										
					EditorGUILayout.LabelField("Upgrade settings");
					
					EditorGUI.indentLevel = 3;
					d.RequiresUpgrade = EditorGUILayout.Toggle("Requires upgrade", d.RequiresUpgrade);
					
					if (d.RequiresUpgrade) {
						EditorGUI.indentLevel = 4;
						d.UpgradeRequired = EditorGUILayout.TextField("Upgrade", d.UpgradeRequired);	
						EditorGUI.indentLevel = 3;
					}
					
					d.BaseUpgradeLevel = EditorGUILayout.IntField("Base level", d.BaseUpgradeLevel);
					EditorGUI.indentLevel = 0;
				}
				EditorGUILayout.EndVertical();
			}
			
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Customers Editor
		EditorGUI.indentLevel = 0;
		
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Customer Editor");
		
		showCustomerEditor = EditorGUILayout.Foldout(showCustomerEditor, "Customer Configuration");
				
		if (showCustomerEditor) {
			level.CustomerStartPosition = EditorGUILayout.Vector3Field("Customer Start Position", level.CustomerStartPosition);
			level.CustomerReadyPosition = EditorGUILayout.Vector3Field("Customer Ready Position", level.CustomerReadyPosition);
			level.CustomerReadyToLeavePosition = EditorGUILayout.Vector3Field("Customer Ready To Leave Position", level.CustomerReadyToLeavePosition);
			level.CustomerEndPosition = EditorGUILayout.Vector3Field("Customer End Position", level.CustomerEndPosition);
			
			// Check station count and recreate the Station array if it changed (copying over the old values)
			
			if(GUILayout.Button("Add Customer")) {
				ArrayUtility.Add(ref level.Customers, new TMGLevel._Customer());
				ArrayUtility.Add(ref showCustomerConfigEditor, true);
				GUI.changed = true;
			}
			/*
				int newArraySize = EditorGUILayout.IntField("Customer count", level.Customers.Length);
				if (newArraySize != level.Customers.Length) {
				TMGLevel._Customer[] newCustomerArray = new TMGLevel._Customer[newArraySize];
						
				for (int i = 0; i < newCustomerArray.Length; i++) {
					if (i < level.Customers.Length)
						newCustomerArray[i] = level.Customers[i];
					else 
						newCustomerArray[i] = new TMGLevel._Customer();
				}
			
				level.Customers = newCustomerArray;
			}
						
			
			*/
			
			if (showCustomerConfigEditor.Length != level.Customers.Length) showCustomerConfigEditor = new bool[level.Customers.Length];
			
			
			
			for (int i = 0; i < level.Customers.Length; i++) {
				EditorGUILayout.BeginVertical("box");
				EditorGUI.indentLevel = 0;
				
				TMGLevel._Customer c = level.Customers[i];
				
				string requestString = "(Reqs: ";
				for (int j = 0; j < c.StationsRequested.Length; j++) requestString += c.StationsRequested[j] + (j == c.StationsRequested.Length - 1?")":",");
				if (c.StationsRequested.Length == 0) requestString = "";
				
				GUILayout.BeginHorizontal();
				
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this customer?", "Yes", "No")) {
					DeleteCustomer(i);	
				}
				
				showCustomerConfigEditor[i] = EditorGUILayout.Foldout((bool)showCustomerConfigEditor[i], "Customer " + c.IDTag + " (Type " + c.TypeTag + ") " + requestString);				
				
				GUILayout.EndHorizontal();
								
				if (showCustomerConfigEditor[i]) {
					EditorGUI.indentLevel = 0;
					
					GUI.contentColor = Color.white;
				
					/*GUILayout.BeginHorizontal();
					GUILayout.Box("Customer " + c.IDTag + " " + requestString, GUILayout.Width(Screen.width - 10));
					GUILayout.EndHorizontal();*/
					
					c.IDTag = EditorGUILayout.TextField("Tag", c.IDTag);
					c.TypeTag = EditorGUILayout.TextField("Type Tag", c.TypeTag);
					
					c.OverrideDefaultSettings = EditorGUILayout.Toggle("Override defaults", c.OverrideDefaultSettings);
					
					if (c.OverrideDefaultSettings) {
						EditorGUI.indentLevel = 3;
						c.Patience = EditorGUILayout.Slider("Patience",c.Patience, 0.0f,10.0f);
						c.MaxPatience = EditorGUILayout.Slider("Max Patience",c.MaxPatience, c.Patience,10.0f);
						c.PatienceDecay = EditorGUILayout.Slider("Patience Decay",c.PatienceDecay, 0.0f,1.0f);
						
						EditorGUILayout.Separator();
		
						c.PatienceCheckoutMultiplier = EditorGUILayout.FloatField("Checkout Multiplier", c.PatienceCheckoutMultiplier);
						c.PatienceCheckoutBonus = EditorGUILayout.FloatField("Checkout Bonus", c.PatienceCheckoutBonus);
						
						EditorGUI.indentLevel = 2;
					}
					
					c.EntryDelay = EditorGUILayout.FloatField("Entry Delay", c.EntryDelay);
					
					bool foldout = true;
					c.StationsRequested = EditorGUIExtension.ArrayFoldout("Station Requests", c.StationsRequested, ref foldout);
					//c.Position = Vector3Field("Position", c.Position, 147);
					if(GUILayout.Button("Insert new customer after this one.")) {
						ArrayUtility.Insert(ref level.Customers, i + 1, new TMGLevel._Customer());
						ArrayUtility.Insert(ref showCustomerConfigEditor, i + 1, true);
						GUI.changed = true;
					}
				}
				EditorGUILayout.EndVertical();
			}
			
		}
		
		EditorGUILayout.EndVertical();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		#endregion
		
		#region Nodes Editor		
		EditorGUI.indentLevel = 0;
		rect = EditorGUILayout.BeginVertical("window", GUILayout.Height(20));
		GUI.Label(rect, " Nodes Editor");
		
		showNodesEditor = EditorGUILayout.Foldout(showNodesEditor, "Nodes Configuration");
		
		if (showNodesEditor) {
			
			if(GUILayout.Button("Add Node")) {
				ArrayUtility.Add(ref level.Nodes, new TMGLevel._Node());
				ArrayUtility.Add(ref showNodeConfigEditor, true);
				GUI.changed = true;
			}
			
			if (showNodeConfigEditor.Length != level.Nodes.Length) showNodeConfigEditor = new bool[level.Nodes.Length];
			
		
			EditorGUI.indentLevel = 2;
			for (int i = 0; i < level.Nodes.Length; i++) {
				EditorGUILayout.BeginVertical("box");
				EditorGUI.indentLevel = 0;			
				
				TMGLevel._Node n = level.Nodes[i];
				
				GUILayout.BeginHorizontal();
				
				if (GUILayout.Button("-", GUILayout.Width(16),GUILayout.Height(12)) && EditorUtility.DisplayDialog("Remove?", "Are you sure you want to remove this node?", "Yes", "No")) {
					ArrayUtility.RemoveAt(ref this.level.Nodes, i);
					break;
				}
				showNodeConfigEditor[i] = EditorGUILayout.Foldout((bool)showNodeConfigEditor[i], "Node " + n.IDTag);
				
				GUILayout.EndHorizontal();
				
				if (showNodeConfigEditor[i]) {
					EditorGUI.indentLevel = 0;
					
					//GUILayout.Box("Node " + n.IDTag, GUILayout.Width(Screen.width));
					n.IDTag = EditorGUILayout.TextField("Tag", n.IDTag);
					n.Connections = EditorGUILayout.TextField("Connections", n.Connections);
					n.Position = Vector3Field("Position", n.Position, 147);
					EditorGUILayout.Separator();
					
					if(GUILayout.Button("Insert new node after this one.")) {
						ArrayUtility.Insert(ref level.Nodes, i + 1, new TMGLevel._Node());
						ArrayUtility.Insert(ref showNodeConfigEditor, i + 1, true);
						GUI.changed = true;
					}
				}
				EditorGUILayout.EndVertical();
			}
			
			
		}
		EditorGUILayout.EndVertical();
		#endregion
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);		
		
	}
	
	void DeleteCustomer(int index) {
		if (index >= 0 && index < level.Customers.Length) {
			int newArraySize = level.Customers.Length - 1;
			
			TMGLevel._Customer[] newCustomerArray = new TMGLevel._Customer[newArraySize];
						
			for (int i = 0; i < newCustomerArray.Length; i++) {
				if (i < index) newCustomerArray[i] = level.Customers[i];
				else newCustomerArray[i] = level.Customers[i + 1];
			}
			
			level.Customers = newCustomerArray;
		}
	}
}
