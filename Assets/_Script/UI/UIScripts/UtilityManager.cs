using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityManager : MonoBehaviour
{
    public static UtilityManager Instance;

	private void Awake()
	{
        Instance = this;
	}

	public string FormatTimeToString(TimeSpan _time)
	{
  
        // If more than or equal to 1 hour
        if (_time.TotalHours >= 1)
        {
            int totalHours = (int)Math.Floor(_time.TotalHours);
            return string.Format("{0:D2}h {1:D2}m {2:D2}s", totalHours, _time.Minutes, _time.Seconds);
        }
        // If less than 1 hour but more than or equal to 1 minute
        else if (_time.TotalMinutes >= 1)
        {
            return string.Format("{0:D2}m {1:D2}s", _time.Minutes, _time.Seconds);
        }
        // If less than 1 minute
        else
        {
            return string.Format("{0:D2}s", _time.Seconds);
        }
    }

    public string FormatTimeToSingularValue(TimeSpan _time)
	{
      
        if (_time.TotalHours >= 1)
        {
            int totalHours = (int)Math.Floor(_time.TotalHours);

            return string.Format("{0:D1}h", totalHours);
        }
        // If less than 1 hour but more than or equal to 1 minute
        else if (_time.TotalMinutes >= 1)
        {
            return string.Format("{0:D2}m", _time.Minutes);
        }
        // If less than 1 minute
        else
        {
            return string.Format("{0:D2}s", _time.Seconds);
        }
    }

    public string FormatIntegerValueToStringWithComma(int _value)
	{
        return _value.ToString("N0");
    }

    public string FormatFloatToTrimDecimalPlacesIfTheyAreNotNeeded(float _value)
	{
        string formattedValue = "";

        if (_value == Mathf.Floor(_value))
        {
            // If the value is a whole number, format without decimals
            return formattedValue = _value.ToString("F0");
        }

        // Otherwise, format to two decimal places
        return formattedValue = _value.ToString("F2").TrimEnd('0').TrimEnd('.');
        

    }
}
