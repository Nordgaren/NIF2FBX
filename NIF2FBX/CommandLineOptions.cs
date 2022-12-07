using CommandLine;
using System.Windows.Markup;

namespace NIF2FBX2FLVER {
    public class CommandLineOptions {
            
        [Value(0, MetaName = "Blender Path", HelpText = "Path to Blender exe.", Required = true)]
        public string BlenderPath { get; set; }
        [Value(1, MetaName = "Morrowind Path", HelpText = "Path to unpacked Morrowind folder.", Required = true)]
        public string MorrowindPath { get; set; }
        [Option('p', "python", Required = false, HelpText = "NIF conversion python script path. Defaults to inside this exe's folder + mass_convert_nif.py")]
        public string ScriptPath { get; set; }
        [Option('i', "input", Required = false, HelpText = "Folder to convert Morrowind Path + \\Data Files\\meshes\\")]
        public string InputFolder { get; set; }
        [Option('x', "tex", Required = false, HelpText = "Morrowind Texture Folder. Defaults to Morrowind Path + \\Data Files\\textures\\")]
        public string TextureFolder { get; set; }
        [Option('r', "recursive", Required = false, HelpText = "Switch for searching subdirectories for nif files.")]
        public bool Recursive { get; set; }
        [Option('s', "skip", Required = false, HelpText = "Switch to skip files that have already been converted. On by default.")]
        public bool Skip { get; set; }
        [Option('w', "wait", Required = false, HelpText = "Switch to wait for the current blender process before going to the next one. On by default")]
        public bool Wait { get; set; }
    }

}

