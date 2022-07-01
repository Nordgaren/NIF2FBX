# NIF2FBX
Converts .floppa files to .bingus

# Use

First you will need to download blender and install this plugin by [Greatness7](https://github.com/Greatness7) - [io_scene_mw](https://github.com/Greatness7/io_scene_mw)  
Go to Edit > Preferences > Add-ons > Install and activate the plugin (Select the zip)  
Once you have installed that plugin in blender you will extract the tool and all of it's contents to a folder you choose.
The command for this tool is `NIF2FBX.exe "P:/ath/to/textures" "P:/ath/to/nifs" "P:/ath/to/blender.exe"`

Optionally, you can add `True` to the end of the command to skip files that have already been converted. Example: `NIF2FBX.exe "P:/ath/to/textures" "P:/ath/to/nifs" "P:/ath/to/blender.exe" True`  

The tool will work recursively through the folder you give it, so it will do all subfolders, as well.

I don't recommend doing all of them at once, as the tool even restarts blender, and it does get a bit bogged down. I will try to fix this in the future.  

# Credits
[Greatness7](https://github.com/Greatness7) - Gave me most of the script that I needed

# Change Log
### 1.0

* Initial Release
