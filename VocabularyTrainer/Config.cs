using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VocabularyTrainer
{
    public class Config
    {
        private static Config _config; //= new Config();

        public readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VocabularyTrainer";

        [DefaultValue(true)]
        public bool? SaveConfigInAppData = null;

        [DefaultValue(false)]
        public bool? SaveDataInAppData = null;

        [XmlArray("Categories")]
        [XmlArrayItem("Categorie")]
        public List<string> categories { get; set; }

        [XmlArray("Lections")]
        [XmlArrayItem("Lection")]
        public List<string> lections { get; set; }

        private Config() {
            this.DefaultLang = System.Windows.Forms.InputLanguage.CurrentInputLanguage;
        }

        [XmlIgnore]
        public System.Windows.Forms.InputLanguage DefaultLang {
            get; set;
        }

        public static Config Instance
        {
            get
            {
                if (_config == null)
                {
                    _config = new Config();
                    if (_config.categories == null)
                    {
                        _config.categories = new List<string>();
                    }

                    if (_config.lections == null)
                    {
                        _config.lections = new List<string>();
                    }
                }

                return _config;
            }
        }


        public string ConfigPath
        {
            get { return Instance.ConfigDir + "config.xml"; }
        }

        public string ConfigDir
        {
            get { return Instance.SaveConfigInAppData == false ? string.Empty : AppDataPath + "\\"; }
        }

        public string DataDir
        {
            get { return Instance.SaveDataInAppData == false ? string.Empty : AppDataPath + "\\"; }
        }

        public static void SaveBackup(bool deleteOriginal = false)
        {
            var configPath = Instance.ConfigPath;

            if (File.Exists(configPath))
            {
                File.Copy(configPath, configPath + DateTime.Now.ToFileTime());

                if (deleteOriginal)
                    File.Delete(configPath);
            }
        }

        public static void Load()
        {
            var foundConfig = false;
            try
            {
                if (File.Exists("config.xml"))
                {
                    _config = XmlManager<Config>.Load("config.xml");
                    foundConfig = true;
                }
                else if (File.Exists(Instance.AppDataPath + @"\config.xml"))
                {
                    _config = XmlManager<Config>.Load(Instance.AppDataPath + @"\config.xml");
                    foundConfig = true;
                }
                else if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))
                    //save locally if appdata doesn't exist (when e.g. not on C)
                    Instance.SaveConfigInAppData = false;
            }
            catch (Exception e)
            {
            }

            if (!foundConfig)
            {
                if (Instance.ConfigDir != string.Empty)
                    Directory.CreateDirectory(Instance.ConfigDir);
                using (var sr = new StreamWriter(Instance.ConfigPath, false))
                    sr.WriteLine("<Config></Config>");
            }
            else if (Instance.SaveConfigInAppData != null)
            {
                if (Instance.SaveConfigInAppData.Value) //check if config needs to be moved
                {
                    if (File.Exists("config.xml"))
                    {
                        Directory.CreateDirectory(Instance.ConfigDir);
                        SaveBackup(true); //backup in case the file already exists
                        File.Move("config.xml", Instance.ConfigPath);
                        Logger.WriteLine("Moved config to appdata");
                    }
                }
                else if (File.Exists(Instance.AppDataPath + @"\config.xml"))
                {
                    SaveBackup(true); //backup in case the file already exists
                    File.Move(Instance.AppDataPath + @"\config.xml", Instance.ConfigPath);
                    Logger.WriteLine("Moved config to local");
                }
            }
        }

        public static void Save()
        {
            Logger.WriteLine(Instance.categories.Count.ToString());
            XmlManager<Config>.Save(Instance.ConfigPath, Instance);
        }
    }
}
