using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHandlerPlayerPrefKeys
{
	public static string KEY_CHESTSTATUS = "ChestStatus"; // 0 = empty, 1 = filled, 2 = running, 3 = completed
	public static string KEY_CHESTTYPE = "ChestType"; // 0 = common, 1 = rare, 2 = epic, 3 = Legendary
	public static string KEY_CHESTLEVEL = "ChestLevel";
	public static string KEY_CHESTUNLOCKTIME = "ChestUnlockTime";
	public static string KEY_CURRENTCHESTUNLOCKINPROCESSINDEX = "ChestUnlockInProcessIndex";
}
