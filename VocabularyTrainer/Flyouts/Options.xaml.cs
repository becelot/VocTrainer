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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VocabularyTrainer.Flyouts
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : UserControl
    {
        public Options()
        {
            InitializeComponent();
        }

        private async void buttonAddLection_Click(object sender, RoutedEventArgs e)
        {
            var result = await Helper.MainWindow.ShowInputAsync("Neue Lektion hinzufügen", "Wie heißt die neue Lektion?");

            if (result != null && !result.Equals(""))
            {
                string newLection = result;
                if (Config.Instance.lections.FindIndex((x) => { return x.Equals(newLection); }) >= 0)
                {
                    await Helper.MainWindow.ShowMessageAsync("Neue Lektion hinzufügen", "Lektion existiert berets!");
                    return;
                }
                Config.Instance.lections.Add(result);
                Helper.MainWindow.AddVocabulary.comboLection.Items.Add(result);
                this.comboLection.Items.Add(result);
                this.comboLection.SelectedIndex = this.comboLection.Items.Count - 1;
            }
        }

        private void buttonDeleteLection_Click(object sender, RoutedEventArgs e)
        {
            if (Config.Instance.lections.FindIndex((x) => { return x.Equals(comboLection.SelectedValue); }) >= 0)
            {
                Config.Instance.lections.Remove(comboLection.SelectedValue.ToString());
                Helper.MainWindow.AddVocabulary.comboLection.Items.Remove(comboLection.SelectedValue);
                comboLection.Items.Remove(comboLection.SelectedValue);
                //TODO: How to handle vocabulary from that category?
            }
                
        }

        private async void buttonCategoryAdd_Click(object sender, RoutedEventArgs e)
        {
            var result = await Helper.MainWindow.ShowInputAsync("Neue Kategorie hinzufügen", "Wie heißt die neue Kategorie?");

            if (result != null && !result.Equals(""))
            {
                string newCat = (string)result;
                if (Config.Instance.categories.FindIndex((x) => { return x.Equals(newCat); }) >= 0)
                {
                    await Helper.MainWindow.ShowMessageAsync("Neue Kategorie hinzufügen", "Kategorie existiert berets!");
                    return;
                }
                Config.Instance.categories.Add(result);
                Helper.MainWindow.AddVocabulary.comboCategory.Items.Add(result);
                this.comboCategory.Items.Add(result);
                this.comboCategory.SelectedIndex = this.comboCategory.Items.Count - 1;
            }
        }

        private void buttonCategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Config.Instance.categories.FindIndex((x) => { return x.Equals(comboCategory.SelectedValue); }) >= 0)
            {
                Config.Instance.categories.Remove(comboCategory.SelectedValue.ToString());
                Helper.MainWindow.AddVocabulary.comboCategory.Items.Remove(comboCategory.SelectedValue);
                comboCategory.Items.Remove(comboCategory.SelectedValue);
                //TODO: How to handle vocabulary from that category?
            }
        }
    }
}
