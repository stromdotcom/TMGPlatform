using UnityEngine;
using System.Collections;

public enum TMGPuzzleAchievementType {Score, Combo, Color};

public class TMGPuzzles : MonoBehaviour {
	
	[System.Serializable]
	public class _Achievement {
		public string IDTag;
				
		public TMGPuzzleAchievementType Type;
		
				
		public int NumberOfCombosRequired;
		public int ComboTypeRequired;
		public string UnlocksAchievement;
		
		public int ScoreRequired;
		public int CreditReward;
		
		public string ColorRequired;
		public int ColorCountRequired;
	}
	
	[System.Serializable]
	public class _Puzzle {
		public string IDTag;
		public string Title;
		
		public _Achievement[] Achievements = new _Achievement[2];
		
		public float time = 30.0f;
	}
	
	public _Puzzle[] Puzzles;
	
	#region Helpers
	
	#endregion
}
