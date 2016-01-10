using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Parser {
	
	static public List<T> GetAllValidObjectFromList<T>(List<T> list) where T : IObjectWithTime
	{
		var validList = new List<T>();
		foreach (T bns in list) {
			if (IsValidInTime(bns.StartDate(), bns.EndDate())) {
				validList.Add(bns);
			}
		}
		return validList;
	}

	static public List<string> GetListString(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			List<object> list = (List<object>) dict[key];
			List<string> listString = new List<string>();
			foreach (object obj in list) {
				listString.Add(obj.ToString());
			}
			return listString;
		} else {
			return new List<string>();
		}
	}

	static public List<int> GetListInt(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			List<object> list = (List<object>) dict[key];
			List<int> listInt = new List<int>();
			foreach (object obj in list) {
				listInt.Add(int.Parse(obj.ToString()));
			}
			return listInt;
		} else {
			return new List<int>();
		}
	}

	static public Dictionary<string, object> GetDict(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			return (Dictionary<string, object>)dict[key];
		} else {
			return null;
		}
	}

	static public Dictionary<string, int> GetDictInt (Dictionary<string, object> dict, string key) {
		Dictionary<string, object> dictObj = GetDict(dict, key);
		Dictionary<string, int> dictInt = new Dictionary<string, int>();
		foreach(KeyValuePair<string, object> ent in dictObj) {
			dictInt.Add(ent.Key, int.Parse(ent.Value.ToString()));
		}

		return dictInt;
	}

	static public string GetString(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			return dict[key].ToString();
		} else {
			return "";
		}
	}

	static public bool GetBool(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			return (bool)dict[key];
		} else {
			return false;
		}
	}

	static public int GetInt(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			return int.Parse(dict[key].ToString());
		} else {
			return 0;
		}
	}

	static public long GetLong(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			return long.Parse(dict[key].ToString());
		} else {
			return 0;
		}
	}

	static public float GetFloat(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			return float.Parse(dict[key].ToString());
		} else {
			return 0;
		}
	}

	static public List<Dictionary<string, object>> GetListDict(Dictionary<string, object> dict, string key)
	{
		if (dict.ContainsKey(key)) {
			List<object> list =  (List<object>) dict[key];
			List<Dictionary<string, object>> listDict = new List<Dictionary<string, object>>();
			foreach (object obj in list) {
				listDict.Add((Dictionary<string, object>)obj);
			}
			return listDict;
		} else {
			return new List<Dictionary<string,object>>();
		}
	}

	static public DateTime GetDateTime (Dictionary<string, object> dict, string key) {
		string timeInString = GetString(dict, key);
		return DateTime.Parse(timeInString);
	}

	static public TimeSpan GetTimeSpan (Dictionary<string, object> dict, string key) {
		string timeInString = GetString(dict, key);
		return TimeSpan.Parse(timeInString);
	}

	static public bool IsValidInTime(string startDate, string endDate)
	{
		DateTime now = DateTime.Now;
		DateTime start = new DateTime (int.Parse (startDate.Substring (0, 4)), int.Parse (startDate.Substring (4, 2)), int.Parse (startDate.Substring (6, 2)),
                          int.Parse (startDate.Substring (9, 2)), int.Parse (startDate.Substring (12, 2)), int.Parse (startDate.Substring (15, 2)));
		DateTime end = new DateTime (int.Parse (endDate.Substring (0, 4)), int.Parse (endDate.Substring (4, 2)), int.Parse (endDate.Substring (6, 2)),
                        int.Parse (endDate.Substring (9, 2)), int.Parse (endDate.Substring (12, 2)), int.Parse (endDate.Substring (15, 2)));
		//var start = DateTime.Parse(startDate);
		//var end = DateTime.Parse(endDate);
		if (DateTime.Compare (now, start) >= 0 && DateTime.Compare (now, end) <= 0) {
				return true;
		} else {
				return false;
		}
	}

	// yyyyMMdd HH:mm:ss
	static string FormatDate(string input)
	{
		return null;
	}

	public static long GetMax(List<long> list) {
		long a = Int64.MinValue;
		foreach (long l in list) {
			if (l>a) a = l;
		}
		return a;
	}

	public static T ParseEnum<T>(string value)
	{
		try {
			return (T)System.Enum.Parse(typeof(T), value, true);
		}catch {
			return default(T);
		}
	}

	static private void AddTime(Dictionary<string, object> dict)
	{
		var str = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
		dict.Add("time", str);
	}
}

public interface IObjectWithTime {
	string StartDate();
	string EndDate();
}
