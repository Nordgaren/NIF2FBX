# Original script by Greatness7

import sys
import bpy
import pathlib
from contextlib import redirect_stdout
from os.path import exists
import io

def replace_mw_textures(tex):
    for ob in bpy.context.selected_objects:
        if ob.type != "MESH":
            continue

        try:
            base_texture_image = ob.active_material.mw.base_texture.image
            base_texture_uv_map = ob.data.uv_layers.active.name
        except AttributeError:
            continue

        # create/assign new material

        # Some Debug Stuff. Uncomment to see
        # print(ob.name)
        # print(str(base_texture_image))
        # print(ob.active_material.name)
        # read = input()
        # End Debug Stuff
        material = bpy.data.materials.new(ob.active_material.name + " | " + ob.name)  # shader name
        material.use_nodes = True
        ob.data.materials[ob.active_material_index] = material
        nodes = material.node_tree.nodes
        links = material.node_tree.links

        # create image texture node
        node = nodes.new("ShaderNodeTexImage")
        node.image = base_texture_image

        links.new(node.outputs["Color"], nodes.get("Principled BSDF").inputs["Base Color"])

        # create uv map node
        node = material.node_tree.nodes.new("ShaderNodeUVMap")
        node.uv_map = base_texture_uv_map
        links.new(node.outputs["UV"], nodes.get("Image Texture").inputs["Vector"])

    for image in bpy.data.images:
        if not image.filepath:
            continue
        img = pathlib.Path(image.filepath).with_suffix(".dds").name
        path = tex.joinpath(img)
        image.filepath = str(path)

def export_nif_to_fbx():
    root = pathlib.Path(sys.argv[6].strip("\""))
    tex = pathlib.Path(sys.argv[7].strip("\""))
    print(root)
    print(tex)

    count = 1
    for import_path in root.rglob("*.nif"):
        export_path = import_path.with_suffix(".fbx")
        print("Converting: ",import_path)
        print("Converting number: ",count)
        count += 1

        if skip & exists(export_path):
            continue

        try:
            bpy.ops.wm.read_homefile(load_ui=False, use_empty=True)
            #bpy.context.preferences.addons["io_scene_mw"].scale_correction = 1.0
            try:
                bpy.ops.import_scene.mw(filepath=str(import_path), ignore_animations=True)
            except Exception as e:
                print("Failed: ", import_path)
                print(e)

            replace_mw_textures(tex)
            bpy.ops.export_scene.fbx(filepath=str(export_path), embed_textures=False)#, global_scale=0.01)
        except Exception as e:
            print("Export Failed: ", import_path)
            print(e)
            #inp = input()
        
        print("")


skip = False

if __name__ == "__main__":
    skip = sys.argv[8] == 'True'
    export_nif_to_fbx()
    read = input("Press the any key to end the program")
