using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPlayerPrefKeys
{
	// Daily Reward
	public static string KEY_NEXTDAILYREWARDTIME = "Daily Reward Time";
	public static string KEY_CURRENTACTIVEDAYFORDAILYREWARD = "Current Active Day Index";

	// Wheel Roulette
	public static string KEY_NEXTWHEELSPINREWARDTIME = "Wheel Spin Time";

	// Daily Runs Reward
	public static string KEY_NEXTDAILYRUNSRESETTIME = "Daily Runs Reset Time";
	public static string KEY_CURRENTRUNSPROGRESS = "Daily Runs Current Progress";
	public static string KEY_CURRENTDAILYRUNSCLAIMEDSTATUS = "Daily Runs Reward Claimed Status";
}
