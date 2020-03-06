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
        public void Run()
        {
            //Initialize CYRILIC REMOVER TOTAL OFFENSE
            initCyreto();

            //variables and initialization
            HashSet<string> files = new HashSet<string>();
            HashSet<string> allFiles = new HashSet<string>();
            GlobalConstants gc = new GlobalConstants();
            getInput let = new getInput();

            Console.WriteLine($"\nDo you want to FIX cyr symbols? [y/n]");
            var letRename = let.YesNo();

            Console.Write("\nFile extension: "); // would be good to think about to work with more than one extension
            var extension = let.GetExtension();

            //fix user input to proper one; -> ".jpg"
            var extensionToSearch = (extension[0].ToString() == ".") ? extension : ($".{extension}");

            var counterTotal = 0;
            var countFiles = 0;
            var countCurrent = 0;

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
                {"Х", "H"},
            };

            if(letRename)
            {
                //WANT TO CHANGE NEXT GETFILES METHODS WITH SOMETHING SMARTER/MULTHITHREADING
                
                //get the files
                string[] adultsCollections = Directory.GetFiles(gc.adults, $"*{extensionToSearch}", SearchOption.AllDirectories); //adults
                string[] youngCollections = Directory.GetFiles(gc.youngAthletes, $"*{extensionToSearch}", SearchOption.AllDirectories); //ya
                string[] plusCollections = Directory.GetFiles(gc.plusSize, $"*{extensionToSearch}", SearchOption.AllDirectories); //ps
                string[] shoesCollections = Directory.GetFiles(gc.shoes, $"*{extensionToSearch}", SearchOption.AllDirectories); //ps

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

                foreach (var path in shoesCollections)
                {
                    allFiles.Add(path);
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

                    ////Rename(path, tempFileName); //rename
                }
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

                var height = 45;
                var width = 75;

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
                        Console.SetBufferSize(width, height);
                    }
                }
            }
            void Rename(string filePath, string newFileName)
            {
                var newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + Path.GetExtension(filePath));
                File.Move(filePath, newFilePath);
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
