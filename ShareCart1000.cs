using UnityEngine;
using System;
using System.Collections;
using System.IO;
using IniParser;
using IniParser.Model;

/// <summary>
/// <para>A static utility class for the ShareCart1000 save file.</para>
/// <para></para>
/// <para>This class will create a new save file and corresponding directories if one does not exist.</para>
/// <para></para>
/// <para>Every function that modifies a value in the save file will throw at least one error upon failure, it is up to you to handle these.</para>
/// <para></para>
/// <para>It is also assumed that the save file has a [Main] section and will create a new save file if it does not.</para>
/// </summary>
public static class ShareCart1000 {
	private static FileIniDataParser parser;
	private static string saveFilePath;
	private static string saveFileDir;
	static ShareCart1000() {
		parser = new FileIniDataParser ();

		saveFileDir = Directory.GetParent(Directory.GetParent (Application.dataPath).FullName).FullName + "/dat/";
		saveFilePath = saveFileDir+"/o_o.ini";

		if(!Directory.Exists(saveFileDir)){
			Directory.CreateDirectory(saveFileDir);
		}
		if(!File.Exists(saveFilePath)){
			CreateAndOverrideSave();
		}
	}
	/// <summary>
	/// Sets the switch at the given index.  Will throw errors if the index given is out of the range 0-7.
	/// 
	/// Switch will be set to 'TRUE' if true and 'FALSE' if false, case sensitive.
	/// </summary>
	/// <param name="newValue">Sets the switch as index to this.</param>
	/// <param name="switchIndex">Switch index to set.</param>
	public static void SetSwitchByIndex(bool newValue, int switchIndex){
		if(switchIndex < 0 || switchIndex > 7){
			Debug.LogWarning("The switch index of "+switchIndex+" is not in range of 0-7");
			throw new ArgumentOutOfRangeException("The switch index of "+switchIndex+" is not in range of 0-7");
		}
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(newValue){
				data["Main"]["Switch"+switchIndex] = "TRUE";
			} else{
				data["Main"]["Switch"+switchIndex] = "FALSE";
			}
		} else {
			CreateAndOverrideSave();
		}
		parser.WriteFile (saveFilePath, data);
	}
	/// <summary>
	/// Gets the switch at the given index.  Will throw errors if the index given is out of the range 0-7, or the stored switch has invalid data.
	/// 
	/// The result is true if switch is 'TRUE' and false if switch is 'FALSE', case insensitive.
	/// </summary>
	/// <returns><c>true</c>, if switch at index was true, <c>false</c> if switch at index was false</returns>
	/// <param name="switchIndex">Switch index to retrieve.</param>
	public static bool GetSwitchByIndex(int switchIndex){
		if(switchIndex < 0 || switchIndex > 7){
			Debug.LogWarning("The switch index of "+switchIndex+" is not in range of 0-7");
			throw new ArgumentOutOfRangeException("The switch index of "+switchIndex+" is not in range of 0-7");
		}
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(data ["Main"] ["Switch" + switchIndex] != null){
				if (data ["Main"] ["Switch" + switchIndex].ToUpper ().Equals ("TRUE")) {
					return true;
				} else if (data ["Main"] ["Switch" + switchIndex].ToUpper ().Equals ("FALSE")) {
					return false;
				} else {
					throw new Exception("The key Switch"+switchIndex+" has invalid data");
				}
			} else{
				throw new Exception("The key Switch"+switchIndex+" does not exist");
			}
		} else {
			CreateAndOverrideSave();
			return GetSwitchByIndex(switchIndex);
		}
	}

	/// <summary>
	/// Sets the misc at the given index.  Will throw errors if the index given is out of range.
	/// </summary>
	/// <param name="newValue">Sets the misc at the given index to this</param>
	/// <param name="miscIndex">Misc index to set</param>
	public static void SetMiscByIndex(ushort newValue, int miscIndex){
		if(miscIndex < 0 || miscIndex > 3){
			Debug.LogWarning("The misc index of "+miscIndex+" is not in range of 0-3");
			throw new ArgumentOutOfRangeException("The Misc index of "+miscIndex+" is not in range of 0-3");
		}
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			data["Main"]["Misc"+miscIndex] = newValue.ToString();
		} else {
			CreateAndOverrideSave();
		}
		parser.WriteFile (saveFilePath, data);
	}

	/// <summary>
	/// Gets the Misc at the given index.  Will throw errors if the index given is out of the range 0-3, or the stored misc has invalid data.
	/// </summary>
	/// <returns>The Misc key at the given index</returns>
	/// <param name="miscIndex">Misc index to retrieve</param>
	public static ushort GetMiscByIndex(int miscIndex){
		if(miscIndex < 0 || miscIndex > 3){
			Debug.LogWarning("The misc index of "+miscIndex+" is not in range of 0-3");
			throw new ArgumentOutOfRangeException("The misc index of "+miscIndex+" is not in range of 0-3");
		}
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(data ["Main"] ["Misc" + miscIndex] != null){
				long miscVal = Convert.ToInt64 (data ["Main"] ["Misc" + miscIndex]);
				if (data ["Main"] ["Misc" + miscIndex] != null && miscVal <= ushort.MaxValue && miscVal >= 0) {
					return (ushort) miscVal;
				} else {
					throw new Exception ("The key Misc" + miscIndex + " has invalid data");
				}
			} else{
				throw new Exception("The key Misc"+miscIndex+" does not exist");
			}
		} else {
			CreateAndOverrideSave();
			return GetMiscByIndex(miscIndex);
		}
	}

	/// <summary>
	/// Sets the key MapX to the given value.  The given value mut be in range 0-1023.  Will throw errors if MapX is not in range or section Main does not exist.
	/// </summary>
	/// <param name="newValue">Value to set MapX to.</param>
	public static void SetMapX(ushort newValue){
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(newValue < 1024){
				data["Main"]["MapX"] = newValue.ToString();
			} else{
				throw new ArgumentOutOfRangeException("The MapX of "+newValue+" is not in range of 0-1023");
			}
		} else {
			CreateAndOverrideSave();
		}
		parser.WriteFile (saveFilePath, data);
	}
	/// <summary>
	/// Retrieves the MapX key in the save file.  Will throw errors if MapX has not been set, MapX has invalid data, or section Main does not exist.
	/// </summary>
	/// <returns>The MapX key in the save file.</returns>
	public static int GetMapX(){
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(data["Main"]["MapX"] != null){
				ushort mapValue = Convert.ToUInt16(data["Main"]["MapX"]);
				if(mapValue < 1024){
					return mapValue;
				} else{
					throw new Exception("The key MapX has invalid data");
				}
			} else{
				throw new Exception("The key MapX does not exist");
			}
		} else {
			CreateAndOverrideSave();
			return GetMapX();
		}
	}
	
	/// <summary>
	/// Sets the key MapY to the given value.  The given value mut be in range 0-1023.  Will throw errors if MapY is not in range or section Main does not exist.
	/// </summary>
	/// <param name="newValue">Value to set MapY to.</param>
	public static void SetMapY(ushort newValue){
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(newValue < 1024){
				data["Main"]["MapY"] = newValue.ToString();
			} else{
				throw new ArgumentOutOfRangeException("The MapY of "+newValue+" is not in range of 0-1023");
			}
		} else {
			CreateAndOverrideSave();
		}
		parser.WriteFile (saveFilePath, data);
	}
	/// <summary>
	/// Retrieves the MapY key in the save file.  Will throw errors if MapY has not been set, MapY has invalid data, or section Main does not exist.
	/// </summary>
	/// <returns>The MapY key in the save file.</returns>
	public static int GetMapY(){
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(data["Main"]["MapY"] != null){
				ushort mapValue = Convert.ToUInt16(data["Main"]["MapY"]);
				if(mapValue < 1024){
					return mapValue;
				} else{
					throw new Exception("The key MapY has invalid data");
				}
			} else{
				throw new Exception("The key MapY does not exist");
			}
		} else {
			CreateAndOverrideSave();
			return GetMapY();
		}
	}

	/// <summary>
	/// Sets the PlayerName key in the save file.  Will throw errors if the player name is longer than 1023 characters or section Main does not exist.
	/// </summary>
	/// <param name="newValue">Value to set PlayerName to.</param>
	public static void SetPlayerName(string newValue){
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(newValue.Length < 1024){
				data["Main"]["PlayerName"] = newValue;
			} else{
				throw new ArgumentException("The PlayerName of "+newValue+" is longer than 1023 characters");
			}
		} else {
			CreateAndOverrideSave();
		}
		parser.WriteFile (saveFilePath, data);
	}
	/// <summary>
	/// Gets the PlayerName key in the save file.  Will throw errors if the PlayerName key is invalid in the save file, the PlayeName key does not exist, or the section Main does not exist.
	/// </summary>
	/// <returns>The PlayerName key in the save file.</returns>
	public static string GetPlayerName(){
		IniData data = parser.ReadFile(saveFilePath);
		if (data ["Main"] != null) {
			if(data["Main"]["PlayerName"] != null){
				string playerNameValue = data["Main"]["PlayerName"];
				if(playerNameValue.Length < 1024){
					return playerNameValue;
				} else{
					throw new Exception("The key PlayerName has invalid data");
				}
			} else{
				throw new Exception("The key PlayerName does not exist");
			}
		} else {
			CreateAndOverrideSave();
			return GetPlayerName();
		}
	}
	
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetMiscByIndex(newValue, 0)"/>
	/// </summary>
	public static void SetMisc0(ushort newValue){
		SetMiscByIndex (newValue, 0);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetMiscByIndex(newValue, 1)"/>
	/// </summary>
	public static void SetMisc1(ushort newValue){
		SetMiscByIndex (newValue, 1);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetMiscByIndex(newValue, 2)"/>
	/// </summary>
	public static void SetMisc2(ushort newValue){
		SetMiscByIndex (newValue, 2);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetMiscByIndex(newValue, 3)"/>
	/// </summary>
	public static void SetMisc3(ushort newValue){
		SetMiscByIndex (newValue, 3);
	}

	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetMiscByIndex(0)"/>
	/// </summary>
	public static ushort GetMisc0(){
		return GetMiscByIndex (0);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetMiscByIndex(1)"/>
	/// </summary>
	public static ushort GetMisc1(){
		return GetMiscByIndex (1);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetMiscByIndex(2)"/>
	/// </summary>
	public static ushort GetMisc2(){
		return GetMiscByIndex (2);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetMiscByIndex(3)"/>
	/// </summary>
	public static ushort GetMisc3(){
		return GetMiscByIndex (3);
	}
	
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 0)"/>
	/// </summary>
	public static void SetSwitch0(bool newValue){
		SetSwitchByIndex (newValue, 0);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 1)"/>
	/// </summary>
	public static void SetSwitch1(bool newValue){
		SetSwitchByIndex (newValue, 1);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 2)"/>
	/// </summary>
	public static void SetSwitch2(bool newValue){
		SetSwitchByIndex (newValue, 2);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 3)"/>
	/// </summary>
	public static void SetSwitch3(bool newValue){
		SetSwitchByIndex (newValue, 3);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 4)"/>
	/// </summary>
	public static void SetSwitch4(bool newValue){
		SetSwitchByIndex (newValue, 4);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 5)"/>
	/// </summary>
	public static void SetSwitch5(bool newValue){
		SetSwitchByIndex (newValue, 5);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 6)"/>
	/// </summary>
	public static void SetSwitch6(bool newValue){
		SetSwitchByIndex (newValue, 6);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.SetSwitchByIndex(newValue, 7)"/>
	/// </summary>
	public static void SetSwitch7(bool newValue){
		SetSwitchByIndex (newValue, 7);
	}
	
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(0)"/>
	/// </summary>
	public static bool GetSwitch0(){
		return GetSwitchByIndex (0);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(1)"/>
	/// </summary>
	public static bool GetSwitch1(){
		return GetSwitchByIndex (1);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(2)"/>
	/// </summary>
	public static bool GetSwitch2(){
		return GetSwitchByIndex (2);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(3)"/>
	/// </summary>
	public static bool GetSwitch3(){
		return GetSwitchByIndex (3);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(4)"/>
	/// </summary>
	public static bool GetSwitch4(){
		return GetSwitchByIndex (4);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(5)"/>
	/// </summary>
	public static bool GetSwitch5(){
		return GetSwitchByIndex (5);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(6)"/>
	/// </summary>
	public static bool GetSwitch6(){
		return GetSwitchByIndex (6);
	}
	/// <summary>
	/// Equivalent to <see cref="ShareCart1000.GetSwitchByIndex(7)"/>
	/// </summary>
	public static bool GetSwitch7(){
		return GetSwitchByIndex (7);
	}

	/// <summary>
	/// Creates and saves a new ShareCart1000 save file with the following default values:
	/// <para></para>
	/// <para>MapX = "0"</para>
	/// <para>MapY = "0"</para>
	/// <para>Misc0 = "0"</para>
	/// <para>Misc1 = "0"</para>
	/// <para>Misc2 = "0"</para>
	/// <para>Misc3 = "0"</para>
	/// <para>PlayerName = ""</para>
	/// <para>Switch0 = "FALSE"</para>
	/// <para>Switch1 = "FALSE"</para>
	/// <para>Switch2 = "FALSE"</para>
	/// <para>Switch3 = "FALSE"</para>
	/// <para>Switch4 = "FALSE"</para>
	/// <para>Switch5 = "FALSE"</para>
	/// <para>Switch6 = "FALSE"</para>
	/// <para>Switch7 = "FALSE"</para>
	/// </summary>
	public static void CreateAndOverrideSave(){
		parser.WriteFile (saveFilePath, CreateDefaultShareCart());
	}
	
	private static IniData CreateDefaultShareCart(){
		IniData defaultShareCart = new IniData();
		defaultShareCart.Sections.AddSection("Main");
		defaultShareCart["Main"]["MapX"] = "0";
		defaultShareCart["Main"]["MapY"] = "0";
		defaultShareCart["Main"]["Misc0"] = "0";
		defaultShareCart["Main"]["Misc1"] = "0";
		defaultShareCart["Main"]["Misc2"] = "0";
		defaultShareCart["Main"]["Misc3"] = "0";
		defaultShareCart["Main"]["PlayerName"] = "";
		defaultShareCart["Main"]["Switch0"] = "FALSE";
		defaultShareCart["Main"]["Switch1"] = "FALSE";
		defaultShareCart["Main"]["Switch2"] = "FALSE";
		defaultShareCart["Main"]["Switch3"] = "FALSE";
		defaultShareCart["Main"]["Switch4"] = "FALSE";
		defaultShareCart["Main"]["Switch5"] = "FALSE";
		defaultShareCart["Main"]["Switch6"] = "FALSE";
		defaultShareCart["Main"]["Switch7"] = "FALSE";
		return defaultShareCart;
	}
}