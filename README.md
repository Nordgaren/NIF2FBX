# NIF2FBX
Converts .floppa files to .bingus

# Use

First you will need to download blender and install this plugin by [Greatness7](https://github.com/Greatness7) - [io_scene_mw](https://github.com/Greatness7/io_scene_mw)  
Go to Edit > Preferences > Add-ons > Install and activate the plugin (Select the zip)  
Once you have installed that plugin in blender you will extract the tool and all of it's contents to a folder you choose.
The command basic for this tool is `NIF2FBX.exe  "P:/ath/to/morrowwind/Data Files/textures" "P:/ath/to/morrowwind/Data Files/Data Files/meshes" "P:/ath/to/blender.exe"`

Options:  
```
	-p, --python                NIF conversion python script path. Defaults to inside this exe's folder + mass_convert_nif.py

  	-i, --input                 Folder to convert Morrowwind Path + \Data Files\meshes\

 	-x, --tex                   Morrowwind Texture Folder. Defaults to Morrowwind Path + \Data Files\textures\

 	-r, --recursive             Switch for searching subdirectories for nif files.

 	-s, --skip                  Switch to skip files that have already been converted. On by default.

 	-w, --wait                  Switch to wait for the current blender process before going to the next one. Ony by default
```

The tool will work recursively through the folder you give it, so it will do all subfolders, as well. It will split up all of the subfolders into multiple calls to blender.  

I don't recommend doing all of them at once, as the tool even makes a new scene in blender, and it does get a bit bogged down. I will try to fix this in the future.  

# Credits
[Greatness7](https://github.com/Greatness7) - Gave me most of the script that I needed

# Change Log
### 1.1
* Made a nice CLI interface with some switches

* Changes default behaviour to split up folders and call blender multiple times.

### 1.0

* Initial Release
