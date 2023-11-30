using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "ScriptableObjects/AbilityData")]
public class AbilitiesDataScriptableObject : ScriptableObject
{
	public string str_AbilityName;
	public Sprite sprite_AbilityIcon;
	public bool isAbilityUnlocked;
	public string str_Description;
	public int numberOfProperties;
	public string[] all_PropertiesNames;
	public float[] all_PropertyOneValues;
	public float[] all_PropertyTwoValues;
}
