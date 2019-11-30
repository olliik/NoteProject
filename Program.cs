using System;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace NoteProjekti
{
    class Program
    {
        static string defaultPath = @"C:\Users\ollii\OneDrive\Työpöytä\Notes";
        static string defaultFilename = @"\quicknote";
        static string defaultExtension = ".txt";
        static string notepadPlusPlusPath = @"C:\Program Files\Notepad++\notepad++.exe";
        static string[] fileExtensionArray = { ".txt", ".cs", ".py" };
        static string directoryName;
        static void Main(string[] args)
        {
            System.Console.WriteLine("'note ?' for help");
            if (!Directory.Exists(defaultPath)) // create default diretory if it doesn't exist.
            {
                Directory.CreateDirectory(defaultPath);
            }
            Process myProcess = new Process();

            if (args.Length <= 0) // If no arguments -> default name, folder and file extension.
            {
                createDefaultNote();
            }
            else
            {
                switch (args[0])
                {
                    case "?":
                        PrintHelp();
                        break;
                    case ".":
                        directoryName = Directory.GetCurrentDirectory();
                        if (args.Length > 1)
                        {
                            createNote(directoryName, args[1]);
                            break;
                        }
                        System.Console.WriteLine("Please specify a file name.");
                        break;
                    default:
                        directoryName = args[0];                        
                        if (args.Length > 1)
                        {
                            if (!Directory.Exists(directoryName)) //Create the folder if it doesn't exists.
                            {
                                Directory.CreateDirectory(directoryName);
                            }
                            createNote(directoryName, args[1]);
                            break;
                        }
                        System.Console.WriteLine("Please specify a file name.");
                        break;
                }


            }
            myProcess.Dispose();
            myProcess.Close();
        }

        static void createDefaultNote()
        {
            if (!File.Exists(defaultPath + defaultFilename + defaultExtension))
            {
                File.Create(defaultPath + defaultFilename + defaultExtension);
                Process.Start(notepadPlusPlusPath, defaultPath + defaultFilename + defaultExtension);
                System.Console.WriteLine("New file created: " + defaultPath + defaultFilename + defaultExtension);
            }
            // If default text file name exists -> add a running number before the file extension
            else
            {
                var directory = new DirectoryInfo(defaultPath);
                var myNote = directory.GetFiles().OrderByDescending(file => file.CreationTime).First(); // pickout the most recent file https://stackoverflow.com/a/1179987
                int start = myNote.Name.IndexOf('e');
                int end = myNote.Name.IndexOf('.') - 1;
                string fileNumber = myNote.Name.Substring(start + 1, end - start); // do some parsing -- find the running number for the most recent file.
                try
                {
                    if (!int.TryParse(fileNumber, out int number)) // if we can't parse -> no running number assigned yet - hardcode it to start from 0.
                    {
                        File.Create(defaultPath + defaultFilename + 0 + defaultExtension);
                        Process.Start(notepadPlusPlusPath, defaultPath + defaultFilename + 0 + defaultExtension);
                        System.Console.WriteLine("New file created: " + defaultPath + defaultFilename + 0 + defaultExtension);
                    }
                    else
                    {
                        number++;
                        if (!File.Exists(defaultPath + defaultFilename + number + defaultExtension)) // else we parse succesfully and +1 to our count and create a new file.
                        {
                            File.Create(defaultPath + defaultFilename + number + defaultExtension);
                            Process.Start(notepadPlusPlusPath, defaultPath + defaultFilename + number + defaultExtension);
                            System.Console.WriteLine("New file created: " + defaultPath + defaultFilename + number + defaultExtension);
                        }
                        else
                        { // we probably shouldn't need this, unless user is manually renaming or creating text files into the folder
                            Random gen = new Random(9999);
                            number = gen.Next();
                            if (!File.Exists(defaultPath + defaultFilename + number + defaultExtension))
                            {
                                File.Create(defaultPath + defaultFilename + number + defaultExtension);
                                Process.Start(notepadPlusPlusPath, defaultPath + defaultFilename + number + defaultExtension);
                                System.Console.WriteLine("New file created: " + defaultPath + defaultFilename + number + defaultExtension);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("error" + ex);
                }
            }
        }

        static void createNote(string directory, string filename)
        {
            string[] parts = filename.Split('.');
            string fileExtension = "." + parts.Last();
            if (fileExtensionArray.Contains(fileExtension))  // if supported file extension move on to create file.
            {
                if (!File.Exists(directory + @"\" + filename)) // make sure we dont overwrite an existing file
                {
                    try
                    {
                        File.Create(directory + @"\" + filename);
                        Process.Start(notepadPlusPlusPath, directory + @"\" + filename);
                        System.Console.WriteLine("New file created: " + directory + @"/" + filename);
                    }
                    catch (System.Exception)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Invalid file name.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    System.Console.WriteLine("File already exists. Do you wish to overwrite [Y/n] ?");
                    string key = Console.ReadLine();
                    switch (key.ToUpper())
                    {
                        case "Y":
                            File.Create(directory + @"\" + filename);
                            Process.Start(notepadPlusPlusPath, directory + @"\" + filename);
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("File overwritten!");
                            Console.ResetColor();
                            break;
                        case "N":
                            System.Console.WriteLine("Overwrite cancelled.");
                            break;
                        default:
                            System.Console.WriteLine("Overwrite cancelled.");
                            break;
                    }
                }
            }
            else // else go with .txt as our default file extension
            {
                if (!File.Exists(directory + @"\" + filename + defaultExtension)) // make sure we dont overwrite an existing file
                {
                    try
                    {
                        File.Create(directory + @"\" + filename + defaultExtension);
                        Process.Start(notepadPlusPlusPath, directory + @"\" + filename + defaultExtension);
                        System.Console.WriteLine("New file created: " + directory + @"/" + filename + defaultExtension);
                    }
                    catch (System.Exception)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Invalid file name.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    System.Console.WriteLine("File already exists. Do you wish to overwrite [Y/n] ?");
                    string key = Console.ReadLine();
                    switch (key.ToUpper())
                    {
                        case "Y":
                            File.Create(directory + @"\" + filename);
                            Process.Start(notepadPlusPlusPath, directory + @"\" + filename);
                            System.Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("File overwritten!");
                            Console.ResetColor();
                            break;
                        case "N":
                            System.Console.WriteLine("Overwrite cancelled.");
                            break;
                        default:
                            System.Console.WriteLine("Overwrite cancelled.");
                            break;
                    }
                }
            }
        }

        static void PrintHelp()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Usage:");
            System.Console.WriteLine("Create a quick text file in your default directory without any arguments.");
            System.Console.WriteLine("Specify a folder or use a . for the current directory");
            System.Console.WriteLine(" > note \t\t\t {default directory and default text file}");
            System.Console.WriteLine(" > note [folder] [filename]\t {file extension defaults to .txt}");
            System.Console.WriteLine(" > Example: note . note.py \t {creates a note.py file in current directory}");
            System.Console.WriteLine();
            System.Console.WriteLine("Default directory: [ " + defaultPath + " ]");
            System.Console.Write("Supported file extensions: [");
            for (int i = 0; i < fileExtensionArray.Length; i++)
            {
                System.Console.Write(" " + fileExtensionArray[i]);
            }
            System.Console.WriteLine(" ]");
            System.Console.WriteLine();
        }
    }
}
