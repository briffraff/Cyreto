using System;
using System.Collections.Generic;
using System.Linq;

namespace Cyreto_.Input
{
    public class getInput
    {
        public bool YesNo()
        {
            //validate answer for renaming
            string yesOrNo = Console.ReadLine().ToUpper();
            var answer = yesOrNo == "Y" ? true : false;

            while (yesOrNo != null)
            {
                if (answer)
                {
                    break;
                }
                else if (yesOrNo == "N")
                {
                    //answer = false;
                    //break;
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("*You must say 'Y' or 'N' !");

                    yesOrNo = Console.ReadLine().ToUpper();
                    answer = yesOrNo == "Y" ? true : false;
                }
            }

            return answer;
        }

        public List<string> GetExtension()
        {
            List<string> problems = new List<string>();
            List<string> goods = new List<string>();

            //get extension to work with
            var extensionInput = Console.ReadLine();
            Console.WriteLine();

            while (extensionInput != null)
            {
                if (extensionInput == "")
                {
                    Console.WriteLine("*Please , don't leave empty extension!");

                    extensionInput = Console.ReadLine();
                    Console.WriteLine();
                }
                else
                {
                    break;
                }
            }

            //split input
            var extensionCollection = extensionInput.Split(new char[] {' ',','},StringSplitOptions.RemoveEmptyEntries).ToList();

            //check for problems
            foreach(var ext in extensionCollection)
            {
                if (ext == ".")
                {
                    Console.WriteLine($"You must type something after '{ext}'");
                    problems.Add(ext); //if there is problem add to problems list
                }
                else if (ext.Length > 10)
                {
                    Console.WriteLine($"'{ext}' is too long extension name");
                    problems.Add(ext);
                }
                else
                {
                    //if there no problem add to goods list
                    goods.Add(ext);
                }
            }

            //ask how to proceed next
            if(problems.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nDo you want to remove those ? [y/n] : ");
                Console.ForegroundColor = ConsoleColor.White;

                //wait for answer
                var toRemove = Console.ReadLine().ToLower();

                //then load extension list with goods or row input
                extensionCollection = toRemove == "y" ? goods : extensionCollection;

                Console.WriteLine($"\nFile extension(s): {string.Join(" ", extensionCollection)}");
                Console.WriteLine("\n\tSearching ...");

                //but if there is only problems just exit
                if (goods.Count == 0)
                {
                    Console.WriteLine("\nThere is not valid extension ! Adios!");
                    Environment.Exit(0);
                }
            }

            Console.WriteLine("\n\tSearching ...");

            return extensionCollection;
        }
    }
}
