using System.Collections.Generic;
using UnityEngine;

namespace Model.JSON {

	[System.Serializable]
	public class CharacterSaveData
	{
    	public string type; 
    	public string name;
    	public int hitPoints;
    	public int damageMin;
    	public int damageMax;
    	public float attackSpeed;
    	public float moveSpeed;
    	public float chanceToCrit;
    	public Vector3 position;

    	public float chanceToHeal;
    	public int minHeal;
    	public int maxHeal;
    
    	public List<string> collectedPillars;
	}
}