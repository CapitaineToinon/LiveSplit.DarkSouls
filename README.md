# LiveSplit.DarkSouls

This repo has 3 seperated C# solutions:

* DarkSoulsData
* DarkSoulsState
* LiveSplit.DarkSouls

# DarkSoulsData
This project is intended to just have static data like flags, zones, names, etc

# DarkSoulsState
This project is responsable to hook Dark Souls (using [PropertyHook](https://github.com/JKAnderson/PropertyHook)), to read the game's memory, detect state changes and raise events about the state changes. 

Like so, the LiveSplit autosplitter wouldn't have to do it itself and could just listen to game changes raised by DarkSoulsState such as OnQuitout, OnBossDefeated, OnItemPickedup, etc. 

# LiveSplit.DarkSouls
This is the actual LiveSplit plugin responsable for updating the splits and IGT.
