# ShareCart1000-Unity
A [ShareCart1000](http://sharecart1000.com/) utility class for Unity3D.

## Dependencies:
https://github.com/rickyah/ini-parser


## Download:
https://github.com/Pyredrid/ShareCart1000-Unity/releases/download/v2/ShareCart1000-Unity_v2.unitypackage


## Info

Every function that sets a value will also immediately save to file.  This means there is no buffer and multiple calls to setting values will save the file each time.  I know its inefficient, if you want to make it better make a pull request.

Every call to get and set a value will throw an exception if the value you are getting or setting is invalid.  However, if the section named Main does not exist in the save file, it will completely overwrite the save file with a new default save file.  This is because the entire save file is invalid at this point.

This class follows the guide here as best as it can: http://sharecart1000.com/img/SHARECART1000guide.png


## Usage:
```C#
//What to do
ShareCart1000.CreateDefaultShareCart(); //Creates a new save file with default data
ShareCart1000.SetMiscByIndex(1337, 2); //Sets Misc2 to the value 1337
ShareCart1000.SetMisc2(1337); //Also sets Misc2 to the value 1337
ShareCart1000.SetSwitchByIndex(true, 6); //Sets Switch6 to the value TRUE
ShareCart1000.SetSwitch3(false); //Sets Switch3 to the value FALSE

//What not to do
ShareCart1000.SetSwitch(false, 100); //Throws an ArgumentOutOfRangeException since there are only 8 switches
ShareCart1000.GetMisc(6); //Throws an ArgumentOutOfRangeException since there are only 4 misc
ShareCart1000.SetMapX(1337); //Throws an Exception since MapX has to be between 0-1023
```
