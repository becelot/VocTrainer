using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VocabularyTrainer.Flyouts
{
    /// <summary>
    /// Interaction logic for AddVocabulary.xaml
    /// </summary>
    public partial class AddVocabulary
    {
        public AddVocabulary()
        {
            InitializeComponent();
        }

        private async void comboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboCategory.SelectedValue.Equals("Add.."))
            {
                var result = await Helper.MainWindow.ShowInputAsync("Neue Kategorie hinzufügen", "Wie heißt die neue Kategorie?");

                if (result != null)
                {
                    string newCat = (string)result;
                    if (Config.Instance.categories.FindIndex( (x) => { return x.Equals(newCat); }) >= 0)
                    {
                        await Helper.MainWindow.ShowMessageAsync("Neue Kategorie hinzufügen", "Kategorie existiert berets!");
                        return;
                    }
                    Config.Instance.categories.Add(result);
                    this.comboCategory.Items.Add(result);
                    this.comboCategory.SelectedIndex = this.comboCategory.Items.Count - 1;
                }
            }
        }
    }
}
