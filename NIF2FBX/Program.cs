using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

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
        //public static readonly string Process = @"G:\Steam\steamapps\common\Blender\blender.exe"; // Put the path to the blender.exe that has the mw plugin activated
        private static void Main(string[] args)
        {
            ConvertMany(args);
            //ConvertOneByOne(args);
        }

        private static void ConvertOneByOne(string[] args)
        {
            if (args.Length < 3)
                throw new ArgumentOutOfRangeException(nameof(args),
                    "You must provide at least three arguments: Arg 1: Folder Path of Texture files. Arg 2: Folder Path of Nif files. Arg 3: Blender exe location");

            string scriptPath = $@"{ExeDir}\convert_nif.py";

            if (!File.Exists(scriptPath))
                throw new Exception($"Script does not exist! Path: {scriptPath}");

            string textureFiles = args[0];

            string nifFolderPath = args[1];
            string[] nifFiles = Directory.GetFiles(nifFolderPath, "*.nif", SearchOption.AllDirectories);

            if (nifFiles.Length <= 0)
                throw new Exception($"No .nif files found in {nifFolderPath} or subdirectories");


            string blenderProc = args[2];

            if (!File.Exists(blenderProc))
                throw new Exception($"Blender path does not exist! Path: {blenderProc}");

            bool skip = false;
            
            if (args.Length > 3 && !bool.TryParse(args[3], out skip))
            {
                throw new Exception($"4th argument needs to be a bool (true or false)");
            }

            Console.WriteLine($"Converting {nifFiles.Length} files");

            foreach (string filePath in nifFiles)
            {
                Console.WriteLine($"Processing: {filePath}");
                string cmdArgs = $@"--background --python ""{scriptPath}"" ""{filePath}"" ""{textureFiles}"" {skip}"; //the double quotes here serve to provide double quotes to the arg paths, in case of spaces.
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = blenderProc,
                        Arguments = cmdArgs,
                        UseShellExecute = true,

                        CreateNoWindow = true,
                    }
                };

                proc.Start();
                proc.WaitForExit();
                Console.WriteLine(proc.StandardOutput); 
                Console.WriteLine($"Complete: {filePath}");
            }

            Console.ReadLine();
        }

        private static void ConvertMany(string[] args)
        {
            if (args.Length < 3)
                throw new ArgumentOutOfRangeException(nameof(args),
                    "You must provide at least three arguments: Arg 1: Folder Path of Texture files. Arg 2: Folder Path of Nif files. Arg 3: Blender exe location");

            string scriptPath = $@"{ExeDir}\mass_convert_nif.py";

            if (!File.Exists(scriptPath))
                throw new Exception($"Script does not exist! Path: {scriptPath}");

            string textureFiles = args[0];
            Console.WriteLine(textureFiles);

            string nifFolderPath = args[1];
            string[] nifFiles = Directory.GetFiles(nifFolderPath, "*.nif", SearchOption.AllDirectories);

            if (nifFiles.Length <= 0)
                throw new Exception($"No .nif files found in {nifFolderPath} or subdirectories");

            string blenderProc = args[2];

            if (!File.Exists(blenderProc))
                throw new Exception($"Blender path does not exist! Path: {blenderProc}");

            bool skip = false;

            if (args.Length > 3 && !bool.TryParse(args[3], out skip)) 
            {
                throw new Exception($"4th argument needs to be a bool (true or false)");
            }

            Console.WriteLine($"Converting {nifFiles.Length} files");
            
            string cmdArgs = $@"--background  --log ""*"" --python ""{scriptPath}"" ""{nifFolderPath}"" ""{textureFiles}"" {skip}"; //the double quotes here serve to provide double quotes to the arg paths, in case of spaces.
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = blenderProc,
                    Arguments = cmdArgs,
                    UseShellExecute = true,
                    CreateNoWindow = true,
                }
            };

            proc.Start();
            proc.WaitForExit();

            Console.ReadLine();
        }
    }
}
