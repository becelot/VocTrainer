using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace VocabularyTrainer
{
    public class VocabularyDatabase
    {
        private static VocabularyDatabase _instance;

        [XmlArray(ElementName = "Database")]
        [XmlArrayItem(ElementName = "Vocabulary")]
        public List<Vocabulary> vocs;

        public VocabularyDatabase()
        {
            vocs = new List<Vocabulary>();
        }

        public static VocabularyDatabase Instance
        {
            get { return _instance ?? (_instance = new VocabularyDatabase()); }
        }

        public static void Load()
        {
            var file = Config.Instance.DataDir + "Database.xml";
            if (!File.Exists(file))
                return;
            try
            {
                _instance = XmlManager<VocabularyDatabase>.Load(file);
            }
            catch (Exception)
            {
                //failed loading deckstats 
                var corruptedFile = Helper.GetValidFilePath(Config.Instance.DataDir, "Database_corrupted", "xml");
                try
                {
                    File.Move(file, corruptedFile);
                }
                catch (Exception)
                {
                    throw new Exception("Can not load or move Database.xml file. Please manually delete the file in \"%appdata\\VocabularyTrainer\".");
                }

                //get latest backup file
                var backup =
                    new DirectoryInfo(Config.Instance.DataDir).GetFiles("Database_backup*")
                                                              .OrderByDescending(x => x.CreationTime)
                                                              .FirstOrDefault();
                if (backup != null)
                {
                    try
                    {
                        File.Copy(backup.FullName, file);
                        _instance = XmlManager<VocabularyDatabase>.Load(file);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Error restoring Database backup. Please manually rename \"Database_backup.xml\" to \"Database.xml\" in \"%appdata\\VocabularyTrainer\".");
                    }
                }
                else
                {
                    //can't call ShowMessageAsync on MainWindow at this point. todo: Add something like a message queue.
                    //MessageBox.Show("Your Database file got corrupted and there was no backup to restore from.", "Error restoring Database backup");
                    Logger.WriteLine("Your Database file got corrupted and there was no backup to restore from.", "Error restoring Database backup");
                }
            }
        }

        public static void Save()
        {
            var file = Config.Instance.DataDir + "Database.xml";
            XmlManager<VocabularyDatabase>.Save(file, Instance);
        }
    }
}