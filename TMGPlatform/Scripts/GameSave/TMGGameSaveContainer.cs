using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
 
 [XmlRoot("SaveGameCollection")]
 public class TMGGameSaveContainer {
	
	// VALUES
    [XmlArray("GameSaveList"),XmlArrayItem("GameSave")]
    public TMGGameSave[] GameSaves;
    
	public int ActiveGameSave = -1;
	// END VALUES
	
	private static string path = Path.Combine(Application.persistentDataPath, "GameSaves.xml");
	
	#region Load and Save
	
	/// <summary>
	/// Save this TMGGameSaveContainer to disk
	/// </summary>
    public void Save() {
		//string path = Path.Combine(Application.persistentDataPath, "GameSaves.xml");
	
#if !UNITY_WEBPLAYER		
        var serializer = new XmlSerializer(typeof(TMGGameSaveContainer));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
#else
		PlayerPrefs.SetString("GameSave", SaveToText());
		PlayerPrefs.Save();
#endif
    }
    
	/// <summary>
	/// Load the TMGGameSaveContainer from disk
	/// </summary>
	public static TMGGameSaveContainer Load() {
		//string path = Path.Combine(Application.persistentDataPath, "GameSaves.xml");
#if !UNITY_WEBPLAYER				
		if (!File.Exists(path)) {
			TMGGameSaveContainer container = new TMGGameSaveContainer();
			container.Save();
			
			return container;
		}
		
        var serializer = new XmlSerializer(typeof(TMGGameSaveContainer));
        using(var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as TMGGameSaveContainer;
        }
#else
		string loadXML = PlayerPrefs.GetString("GameSave");
		return TMGGameSaveContainer.LoadFromText(loadXML);	
#endif
    }
 
	/// <summary>
	/// Saves the TMGGameSaveContainer to a string.  Use to post the serialized object to a web server.
	/// </summary>
	/// <returns>
	/// THe serialized TMGGameSaveContainer as a string of text.
	/// </returns>
	public string SaveToText() {
		var serializer = new XmlSerializer(typeof(TMGGameSaveContainer));
        
		StringWriter stringStream = new StringWriter();
		
        serializer.Serialize(stringStream, this);
        	
		return stringStream.ToString();
	}
	
    /// <summary>
    /// Loads a TMGGameSaveContainer from text.  Use after loading the XML from a web server.
    /// </summary>
    /// <returns>
    /// The deserialized TMGGameSaveContainer
    /// </returns>
    /// <param name='text'>
    /// The XML text describing the TMGContainer
    /// </param>
    public static TMGGameSaveContainer LoadFromText(string text) {
		if (text == null || text == "") {
			TMGGameSaveContainer container = new TMGGameSaveContainer();
			container.Save();
			//PlayerPrefs.SetString("GameSave", container.SaveToText());
			
			return container;	
		}
		
        var serializer = new XmlSerializer(typeof(TMGGameSaveContainer));
        return serializer.Deserialize(new StringReader(text)) as TMGGameSaveContainer;
    }
	
	#endregion
	
	public bool DeleteSaves() {
#if UNITY_IPHONE || UNITY_ANDROID		
		if (File.Exists(path)) {
			File.Delete(path);
			return true;
		} else return false;
#else
		PlayerPrefs.SetString("GameSave","");
		return true;
#endif
	}
	
	public bool SetActiveGameSave(int index) {
		if (GameSaves == null) {
			ActiveGameSave = -1;
			return false;
		}
		
		if (index >= 0 && index < GameSaves.Length) {
			ActiveGameSave = index;	
			return true;
		} 
		
		ActiveGameSave = -1;
		
		return false;
	}
	
	public int AddNewGameSave(string name, int currentLevel) {
		if (GameSaves == null) {
			GameSaves = new TMGGameSave[1];
		} else {
			TMGGameSave[] newGameSaveArray = new TMGGameSave[GameSaves.Length + 1];
			
			for (int i = 0; i < GameSaves.Length; i++) newGameSaveArray[i] = GameSaves[i];
			
			GameSaves = newGameSaveArray;			
		}				
		
		TMGGameSave newGameSave = new TMGGameSave();
		newGameSave.Name = name;
		newGameSave.CurrentLevel = currentLevel;
		
		GameSaves[GameSaves.Length - 1] = newGameSave;
		
		return GameSaves.Length - 1;
	}
	
	public TMGGameSave ActiveSave() {
		if (GameSaves == null || ActiveGameSave < 0 || ActiveGameSave >= GameSaves.Length) return null;
		
		return GameSaves[ActiveGameSave];
	}
	
	#region GUID
	
	public static string GetGUID() {
		if (PlayerPrefs.GetString("UserID") != "") return PlayerPrefs.GetString("UserID");
		
		System.Guid guid = System.Guid.NewGuid();
		
		PlayerPrefs.SetString("UserID", "" + guid);
		
		return "" + guid;
	}
	
	#endregion
 }