using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabularyTrainer
{
    class Config
    {
        private static Config _config; //= new Config();

        public readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VocabularyTrainer";
        public bool SaveConfigInAppData = true;
        public bool SaveDataInAppData = true;

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

        public static Config Instance
        {
            get
            {
                if (_config == null)
                {
                    _config = new Config();
                }

                return _config;
            }
        }
    }
}
