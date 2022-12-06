# Original script by Greatness7

import argparse
import bpy
from contextlib import redirect_stdout
from es3 import nif
import pathlib
import io_scene_mw.nif_import
# from io_scene_mw.lib.es3 import nif
import io
from os.path import exists
import sys



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


def patch_NiStream_load():
    NiStream_load = nif.NiStream.load

    def patched_load(self, filepath):
        NiStream_load(self, filepath)

        for obj in self.objects_of_type(nif.NiStringExtraData):
            if obj.string_data.startswith("NC"):
                self.root.children.append(nif.NiNode(name="NCO"))
                break

    nif.NiStream.load = patched_load


def export_nif_to_fbx(args, import_path):  # truly shitcode, now. I'm sorry, Greatness! :mechands:
    export_path = import_path.with_suffix(".fbx")
    print("Converting: ", import_path)

    if args.skip & exists(export_path):
        return

    try:
        bpy.ops.wm.read_homefile(load_ui=False, use_empty=True)
        # bpy.context.preferences.addons["io_scene_mw"].scale_correction = 1.0
        try:
            bpy.ops.import_scene.mw(filepath=str(import_path), ignore_animations=True)
        except Exception as e:
            print("Failed: ", import_path)
            print(e)

        replace_mw_textures(args.tex)

        bpy.ops.export_scene.fbx(filepath=str(export_path), embed_textures=False)  # , global_scale=0.01)
    except Exception as e:
        print("Export Failed: ", import_path)
        print(e)
        # inp = input()


def begin_conversion(args):
    patch_NiStream_load()

    if args.input is None:
        args.input = f"Texture Path: {args.mw_path}\\Data Files\\meshes\\"

    if args.tex is None:
        args.tex = f"Texture Path: {args.mw_path}\\Data Files\\textures\\"

    print(f"Input Path: {args.input}")
    print(f"Texture Path: {args.tex}")

    input_path = pathlib.Path(args.input)

    count = 1
    total = len(list(input_path.rglob("*.nif") if args.recurs else input_path.glob("*.nif"))) #I hate this. IDK why I can't just get a count and keep the generator

    files = input_path.rglob("*.nif") if args.recurs else input_path.glob("*.nif")

    for file in files:
        print(f"Converting number: {count}\{total}")
        count += 1
        export_nif_to_fbx(args, file)
        print("")

def parse_program_args():
    parser = argparse.ArgumentParser()
    parser.add_argument("mw_path")
    parser.add_argument("-i", "--input", type=str, required=False)
    parser.add_argument("-x", "--tex", type=str, required=False)
    parser.add_argument("-r", "--recurs", type=bool, default=True)
    parser.add_argument("-s", "--skip", type=bool, default=True)

    #parser.add_argument("--background", action="store_true")
    #parser.add_argument("--python", action="store_true")

    return parser.parse_known_args()[0] #god why

if __name__ == "__main__":
    args = parse_program_args()
    begin_conversion(args)
    print(f"Finished converting folder {args.input}")
