using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSlot : MonoBehaviour
{
    public SlotState slotState;
    public int chestLevelIndex; // represents the stage of this chest
    public ChestLevelsScriptableObject chestInThisSlot;

    public void FillChestSlot(int _chestLevelIndex, ChestLevelsScriptableObject _chestInfo)
	{
        slotState = SlotState.Filled;
        chestLevelIndex = _chestLevelIndex;
        chestInThisSlot = _chestInfo;
	}

    public void GotChestSlotDataFromDatabase(SlotState _state, int _chestLevelIndex, ChestLevelsScriptableObject _chestInfo)
	{
        slotState = _state;
        chestLevelIndex = _chestLevelIndex;
        chestInThisSlot = _chestInfo;
    }
}

public enum SlotState
{
    Empty,
    Filled,
    ChestUnlockInProgress,
    ChestUnlockCompleted
}