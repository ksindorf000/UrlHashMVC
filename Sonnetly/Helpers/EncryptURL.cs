using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Sonnetly.Helpers
{
    public class EncryptURL
    {
        private static int indexStart = 0;
        private static int lineNum = 0;
        public static string results;

        static void Main(string[] args)
        {
        }

        public static string AndGo(string uInput, string ownerId)
        {
            string original = uInput + ownerId;

            string hashKey = Encrypt256(original);

            //Get lines
            List<string> lineList = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                lineNum = GetLineNum(hashKey);
                lineList.Add(GetLine(lineNum));
            }

            //Get random words
            List<string> rWords = new List<string>();
            foreach (var line in lineList)
            {
                rWords.Add(GetRandWord(line));
            }

            results = Results(rWords);

            return results;

        }

        /*****************************************
         * Encrypt256()
         * Hashing code written by Joel in class
         *****************************************/
        static string Encrypt256(string input)
        {
            string output;
            byte[] byteData = Encoding.ASCII.GetBytes(input);
            Stream inputStream = new MemoryStream(byteData);

            using (SHA256 shaM = new SHA256Managed())
            {
                var result = shaM.ComputeHash(inputStream);
                output = BitConverter.ToString(result);
            }

            //Strip special characters
            output = output.Replace("-", "");
            //Strip letters
            output = Regex.Replace(output, "[A-Za-z ]", "");

            return output;
        }

        /*****************************************
         * GetLineNum()
         * Gets line num based on digits in hash
         *****************************************/
        private static int GetLineNum(string hashKey)
        {
            int lineNum = 0;
            bool invalid = true;

            //https://msdn.microsoft.com/en-us/library/system.web.httpserverutility.mappath(v=vs.110).aspx
            var path = HttpContext.Current.Server.MapPath(@"\TextFiles\TheSonnets.txt");

            //If first three numbers are out of bounds,
            //get next three numbers
            while (invalid)
            {
                lineNum = int.Parse(hashKey.Substring(indexStart, 3));
                invalid = lineNum > File.ReadLines(path).Count() ? true : false;
                indexStart++;
            }

            return lineNum;
        }


        /*****************************************
         * GetLine()
         * Gets line based on hash 
         * Orders words in line, Desc
         *****************************************/
        private static string GetLine(int lineNum)
        {
            //Get Line from File
            string line = File.ReadLines(@"..\..\TheSonnets.txt").Skip(lineNum).Take(1).First();

            return line;
        }

        /*****************************************
         * GetRandWord()
         * Gets a random word from the given line
         *****************************************/
        private static string GetRandWord(string line)
        {
            string word;
            Random rng = new Random();

            List<string> sList = new List<string>(
                line.Split(new string[] { " " }, StringSplitOptions.None));

            int index = new Random().Next(sList.Count());
            word = sList[index];

            return word;
        }


        /*****************************************
         * Results()
         *****************************************/
        private static string Results(List<string> rWords)
        {
            string results = string.Join(" ", rWords.ToArray());

            //http://stackoverflow.com/questions/1943273/convert-all-first-letter-to-upper-case-rest-lower-for-each-word
            results = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(results.ToLower());

            results = results.Replace(" ", "");

            return results;
        }
    }
}
