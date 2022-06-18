#!/usr/bin/python3
# Thanks you to Greatness7 for 90% of this script

import sys
import bpy
from pathlib import Path

import_path = Path(sys.argv[4].strip("\""))
export_path = import_path.with_suffix(".fbx")

print(export_path)

bpy.ops.object.delete()
bpy.ops.import_scene.mw(filepath=str(import_path))
bpy.ops.export_scene.fbx(filepath=str(export_path))
# bpy.ops.object.delete() # I don't know if this is necessary, as we will be calling a new instance of blender every time


# Original script by Greatness7
# import bpy
# import pathlib
#
# root = pathlib.Path("C:/Users/Admin/Games/Morrowind/Data Files/Meshes/g7/test")
#
# for import_path in root.rglob("*.nif"):
#     export_path = import_path.with_suffix(".fbx")
#
#     bpy.ops.import_scene.mw(filepath=str(import_path))
#     bpy.ops.export_scene.fbx(filepath=str(export_path))
#     bpy.ops.object.delete()