using Cyreto_.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cyreto_.Input;

namespace Cyreto_.Core
{
    public class Engine : IEngine
    {
        enum FolderFile
        {

        }

        public void Run()
        {
            //Initialize CYRILIC REMOVER TOTAL OFFENSE
            initCyreto();

            //variables and initialization
            HashSet<string> files = new HashSet<string>();
            HashSet<string> allFiles = new HashSet<string>();
            GlobalConstants gc = new GlobalConstants();
            getInput let = new getInput();


            Console.Write($"\nDo you want to FIX cyr symbols? [y/n] : ");
            var letRename = let.YesNo(true);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n\nDEFAULT LOCATIONS!-> \\Garments; \\Garments YA; \\Garments PS; \\Garments YAPS \\Garments MA \\SPLN;");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Do you want to use default locations? [y/n] : ");
            var isDefaultLocations = let.YesNo(false);

            Console.Write("\nDo you need to add more? [y/n] : ");
            var isCustomLocation = let.YesNo(false);
            var customLocation = "";

            if (isCustomLocation)
            {
                Console.Write("Add : ");
                customLocation = Console.ReadLine();
            }

            if (!isDefaultLocations && !isCustomLocation)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n\n\t\t   БЪЛГАРСКАТА АЗБУКА / BULGARIAN ALPHABET\n"
                    + "\n\t\t\tА а , Б б , В в , Г г , Д д , \n\t\t\tЕ е , Ж ж , З з , И и , Й й , \n\t\t\tК к , Л л , М м , Н н , О о , \n\t\t\tП п , Р р , С с , Т т , У у , \n\t\t\tФ ф , Х х , Ц ц , Ч ч , Ш ш , \n\t\t\tЩ щ , Ъ ъ , Ь ь , Ю ю , Я я\n".Replace(',', ' '));
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0);
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\nex: File extension(s): jpg .psd .obj obj");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("\nFile extension(s): "); // would be good to think about working with more than one extension
            var extensions = let.GetExtension();

            var extensionToSearch = "";
            var counterTotal = 0;
            var countFiles = 0;
            var countCurrent = 0;
            var loops = 0;

            foreach (var extension in extensions)
            {
                loops++;
                
                Console.WriteLine($"[ {extension} ] -->");

                //fix user input to proper one; -> ".jpg"
                extensionToSearch = (extension[0].ToString() == ".") ? extension : ($".{extension}");

                //keys - cyrillics | value - latin
                var dict = new Dictionary<string, string>
                {
                    {"А", "A"},
                    {"О", "O"},
                    {"Е", "E"},
                    {"И", "I"},
                    {"Н", "N"},
                    {"М", "M"},
                    {"В", "W"},
                    {"Б", "B"},
                    {"Г", "G"},
                    {"Д", "D"},
                    {"Ц", "C"},
                    {"Т", "T"},
                    {"С", "S"},
                    {"П", "P"},
                    {"У", "U"},
                    {"Ъ", "Y"},
                    {"К", "K"},
                    {"Р", "R"},
                    {"Я", "Q"},
                    {"Ф", "F"},
                    {"ѝ", "X"},
                    {"ь", "X"},
                    {"Х", "H"},
                };

                if (letRename)
                {
                    if (isDefaultLocations)
                    {
                        //WANT TO CHANGE NEXT GETFILES METHODS WITH SOMETHING SMARTER/MULTHITHREADING

                        //get the files
                        string[] adultsCollections = Directory.GetFiles(gc.adults, $"*{extensionToSearch}", SearchOption.AllDirectories); //adults
                        string[] youngCollections = Directory.GetFiles(gc.youngAthletes, $"*{extensionToSearch}", SearchOption.AllDirectories); //ya
                        string[] plusCollections = Directory.GetFiles(gc.plusSize, $"*{extensionToSearch}", SearchOption.AllDirectories); //ps
                        string[] shoesCollections = Directory.GetFiles(gc.shoes, $"*{extensionToSearch}", SearchOption.AllDirectories); //shoes
                        string[] yapsCollections = Directory.GetFiles(gc.yaps , $"*{extensionToSearch}", SearchOption.AllDirectories); //yaps
                        string[] maternityCollections = Directory.GetFiles(gc.matternity , $"*{extensionToSearch}", SearchOption.AllDirectories); //matternity


                        foreach (var path in adultsCollections)
                        {
                            allFiles.Add(path);
                        }

                        foreach (var path in youngCollections)
                        {
                            allFiles.Add(path);
                        }

                        foreach (var path in plusCollections)
                        {
                            allFiles.Add(path);
                        }

                        foreach (var path in yapsCollections)
                        {
                            allFiles.Add(path);
                        }

                        foreach (var path in maternityCollections)
                        {
                            allFiles.Add(path);
                        }

                        foreach (var path in shoesCollections)
                        {
                            allFiles.Add(path);
                        }
                    }

                    if (isCustomLocation)
                    {

                        var dirInfo = new DirectoryInfo(customLocation);
                        //var foldersCollection = new List<DirectoryInfo>();
                        //var filesCollection = new List<FileInfo>();

                        //GET ALL FOLDERS
                        var directories = dirInfo.EnumerateDirectories();

                        // WENT THROUGH EACH FOLDER
                        foreach (var dir in directories)
                        {
                            if ((dir.Attributes & FileAttributes.System) == FileAttributes.System)
                            {
                                continue;
                            }

                            //foldersCollection.Add(dir);

                            //GET THE FILES FROM CURRENT FOLDER
                            var subFoldersFiles = Directory.EnumerateFiles(dir.FullName, $"*{extensionToSearch}", SearchOption.AllDirectories);

                            foreach (var file in subFoldersFiles)
                            {
                                FileInfo ofile = new FileInfo(file);

                                if ((ofile.Attributes & FileAttributes.System) == FileAttributes.System)
                                {
                                    continue;
                                }

                                allFiles.Add(ofile.FullName);
                            }
                        }

                    }

                    //
                    foreach (var path in allFiles)
                    {
                        //get the file name from path
                        var fileName = Path.GetFileNameWithoutExtension(path);

                        var tempFileName = fileName;

                        //check each symbol
                        foreach (var symbol in fileName)
                        {
                            var currentSymbol = symbol.ToString();
                            var check = dict.ContainsKey(currentSymbol);

                            //if cyrillic exist - dictionary keys are cyrillics
                            if (check)
                            {
                                //value
                                var symbolToAdd = dict[currentSymbol];
                                tempFileName = tempFileName.Replace(currentSymbol, symbolToAdd); // replace(swap) symbols

                                countCurrent++;
                                counterTotal++;

                                printFoundCyr(fileName, currentSymbol, symbolToAdd);
                            }

                            //set + 1 file
                            if (countCurrent == 1 && check == true)
                            {
                                countFiles++;
                            }
                        }

                        //set file name which should be renamed in the hashset
                        if (countCurrent != 0)
                        {
                            var fileToRename = Path.Combine(Path.GetDirectoryName(path), tempFileName + Path.GetExtension(path));
                            files.Add(fileToRename);
                        }

                        countCurrent = 0;

                        Rename(path, tempFileName); //rename
                    }
                }

                allFiles.Clear();
            }

            //Printing
            printFinalResults();

            //Finalize
            finalize();


            //METHODS
            void initCyreto()
            {
                //set-up the console
                Console.OutputEncoding = Encoding.UTF8;
                Console.ForegroundColor = ConsoleColor.White;

                var height = 25;
                var width = 120;

                CheckAndResetWindowSize();



                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("---------***      C-Y-R-E-T-O    ***---------");
                Console.WriteLine("---------*** CYR -> [SWAP] <- LAT***---------");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" █ ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("-cyrillic | ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" █ ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-latin]");

                Console.WriteLine();

                //METHODS
                void CheckAndResetWindowSize()
                {
                    if (Console.WindowHeight != height || Console.WindowWidth != width)
                    {
                        Console.SetWindowSize(width, height);
                        //Console.SetBufferSize(width, height);
                    }
                }
            }

            void Rename(string filePath, string newFileName)
            {
                var newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + Path.GetExtension(filePath));

                //IF FILE NOT EXIST
                if (File.Exists(newFilePath) == false)
                {
                    File.Move(filePath, newFilePath);
                }
            }

            void printFoundCyr(string fileName, string currentSymbol, string symbolToAdd)
            {
                Console.Write($" +[File: {fileName + extensionToSearch} ] has [");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($" {currentSymbol} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] changed with [");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($" {symbolToAdd} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("]");
                Console.WriteLine();
            }

            void printFinalResults()
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Total cyrillic found: {counterTotal}");
                Console.WriteLine($"Total renamed files: {countFiles}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

                if (countFiles != 0)
                {
                    Console.WriteLine($"-------------");
                    Console.WriteLine();
                    Console.WriteLine("Renamed files list: ");
                    Console.WriteLine();
                    foreach (var file in files)
                    {
                        Console.WriteLine($" [ {file} ]");
                    }
                    Console.WriteLine();
                    Console.WriteLine($"-------------");
                    Console.WriteLine();
                }
            }
            void finalize()
            {
                Console.ReadLine();
            }
        }

    }
}
