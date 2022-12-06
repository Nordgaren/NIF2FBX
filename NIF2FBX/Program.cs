using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace NIF2FBX2FLVER
{

    // This program requires blender and this plugin for blender https://github.com/Greatness7/io_scene_mw
    // The way I installed was through steam, then I downloaded the io_scene_mw plugin.
    // Go to Edit > Preferences > Add-ons > Install and activate the plugin (Select the zip)
    // Once the program is installed and activated, close blender
    // Change the path of the Process property in this Program class to your blender path. You could use args if you want, I just used a static path.
    // ExeDir is a property that contains the path the exe is running from. Inside that path needs to be convert_nif.py.
    // The python script is automatically copied over to the build folder once you build, so you don't need to copy it, yourself.
    // This is just an example script to get you started. The reason I did it this way is that you can wait for the process to end and then grab the fbx file
    // The other option is that we just have the python script do all of it at once. I left the code Greatness7 sent me commented out at the bottom
    // Just have to change the import and export paths and figure out how to go through every directory in python
    // This program will check all subdirectories for .nif files, so you can just do the whole thing at once. It won't keep the folder structure, yet, though.
    // blender doesn't know what to do with the extra args, so it will exit with code 1, but it still converts everything fine!
    internal class Program
    {

        public static readonly string ExeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static void Main(string[] args) {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(o => {
                ConvertManyInChunks(o);
            });
           
        }

        private static List<Process> runningProcessess = new();

        private static void ConvertManyInChunks(CommandLineOptions options) {
            if (options.InputFolder is null)
                options.InputFolder = $"{options.MorrowindPath}\\\\Data Files\\meshes\\";
            
            if (options.TextureFolder is null)
                options.TextureFolder = $"{options.MorrowindPath}\\\\Data Files\\textures\\";

            if (options.ScriptPath is null)
                options.ScriptPath = $@"{ExeDir}\mass_convert_nif.py";
            
            if (!File.Exists(options.ScriptPath))
                throw new($"Script does not exist! Path: {options.ScriptPath}");

            string[] directories = Directory.GetDirectories(options.InputFolder);

            if (options.Recursive) {
                ConvertFolder(options.InputFolder, options);
            }
            else {
                foreach (string directory in directories) {
                    ConvertFolder(directory, options);
                }
            }

            foreach (Process process in runningProcessess) {
                process.WaitForExit();
            }

            Console.WriteLine("Finished converting! Press any key to continue!");
            Console.ReadKey();
        }

        private static void ConvertFolder(string folder, CommandLineOptions options) { 
            
            //the double quotes here serve to escape double quotes for the arg paths, in case of spaces. single quotes doesn't work :sadcatcry:
            string cmdArgs = 
                $@"--background --python ""{options.ScriptPath}"" ""{options.MorrowindPath}"" -i ""{folder}"" -x ""{options.TextureFolder}"" -r {options.Recursive} -s {!options.Skip}"; 
           
            var proc = new Process
            {
                StartInfo = new()
                {
                    FileName = options.BlenderPath,
                    Arguments = cmdArgs,
                    UseShellExecute = true,
                    CreateNoWindow = true,
                }
            };

            proc.Start();
            runningProcessess.Add(proc);
            
            if (!options.Wait) //Inverted because it's a switch (off by default) and by default it needs to be on.
                proc.WaitForExit();

        }

    }
}
