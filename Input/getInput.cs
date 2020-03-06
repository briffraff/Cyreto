using System;

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

        public string GetExtension()
        {
            //get extension to work with
            var extension = Console.ReadLine();
            Console.WriteLine();

            while (extension != null)
            {
                if (extension == "")
                {
                    Console.WriteLine("*Please , don't leave empty extension!");

                    extension = Console.ReadLine();
                    Console.WriteLine();
                }
                else if (extension == ".")
                {
                    Console.WriteLine("You must type something after .");

                    extension = Console.ReadLine();
                    Console.WriteLine();
                }
                else if (extension.Length >= 2)
                {
                    break;
                }
            }

            return extension;
        }
    }
}
