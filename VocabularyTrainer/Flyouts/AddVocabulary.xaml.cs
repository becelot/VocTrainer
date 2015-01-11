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
                        comboCategory.Text = "";
                        return;
                    }
                    Config.Instance.categories.Add(result);
                    this.comboCategory.Items.Add(result);
                    this.comboCategory.SelectedIndex = this.comboCategory.Items.Count - 1;
                } else
                {
                    //comboCategory.Text = "Empty";
                }
            }
            Logger.WriteLine(comboCategory.Text, "SlectionChanged");
            canAddPropertyChanged(sender, null);
        }

        private async void comboLection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboLection.SelectedValue.Equals("Add.."))
            {
                var result = await Helper.MainWindow.ShowInputAsync("Neue Lektion hinzufügen", "Wie heißt die neue Lektion?");

                if (result != null)
                {
                    string newLection = result;
                    if (Config.Instance.lections.FindIndex((x) => { return x.Equals(newLection); }) >= 0)
                    {
                        await Helper.MainWindow.ShowMessageAsync("Neue Lektion hinzufügen", "Lektion existiert berets!");
                        comboLection.Text = "";
                        return;
                    }
                    Config.Instance.lections.Add(result);
                    this.comboLection.Items.Add(result);
                    this.comboLection.SelectedIndex = this.comboLection.Items.Count - 1;
                } else
                {
                    comboLection.Text = "";
                }
            }
            canAddPropertyChanged(sender, null);
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {

            Vocabulary voc = new Vocabulary(this.textGerman.Text, this.textJap.Text, this.textRom.Text, this.comboCategory.Text, this.comboLection.Text);

            if (VocabularyDatabase.Instance.vocs.FindIndex((x) => { return (x.german.Equals(voc.german)); }) < 0) {
                VocabularyDatabase.Instance.vocs.Add(voc);
                textGerman.Text = "";
                textJap.Text = "";
                textRom.Text = "";
            } else
            {
                Helper.MainWindow.ShowMessageAsync("Vokabel existiert bereits", "Diese Vokabel ist bereits vorhanden");
            }
        }

        private void canAddPropertyChanged(object sender, TextChangedEventArgs e)
        {
            if(textGerman.Text != "" && textJap.Text != "" && textRom.Text != "" && comboCategory.SelectedValue != null && !comboCategory.SelectedValue.Equals("") && comboLection.SelectedValue != null && !comboLection.SelectedValue.Equals(""))
            {
                buttonAdd.IsEnabled = true;
            } else
            {
                buttonAdd.IsEnabled = false;
            }
        }
    }
}
