using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VocabularyTrainer
{
    class Helper
    {

        public static MainWindow MainWindow { get; set; }

        public static string RemoveInvalidPathChars(string s)
        {
            var invalidChars = new string(Path.GetInvalidPathChars());
            var regex = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            return regex.Replace(s, "");
        }

        public static string RemoveInvalidFileNameChars(string s)
        {
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            var regex = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            return regex.Replace(s, "");
        }

        public static string GetValidFilePath(string dir, string name, string extension)
        {
            var validDir = RemoveInvalidPathChars(dir);
            if (!Directory.Exists(validDir))
                Directory.CreateDirectory(validDir);

            if (!extension.StartsWith("."))
                extension = "." + extension;

            var path = validDir + "\\" + RemoveInvalidFileNameChars(name);
            if (File.Exists(path + extension))
            {
                var num = 1;
                while (File.Exists(path + "_" + num + extension))
                    num++;
                path += "_" + num;
            }

            return path + extension;
        }

        public static void SetInputLanguageByName(string inputName)
        {
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (lang.Culture.EnglishName.ToLower().StartsWith(inputName.ToLower()))
                {
                    Logger.WriteLine(lang.Culture.EnglishName, "Changed layout");
                    InputLanguage.CurrentInputLanguage = lang;
                }
            }
        }

        public static void SetInputLanguage(InputLanguage lang)
        {
            InputLanguage.CurrentInputLanguage = lang;
        }
    }
}
