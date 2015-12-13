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

        private void comboCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            canAddPropertyChanged(sender, null);
        }

        private void comboLection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            canAddPropertyChanged(sender, null);
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Vocabulary voc = new Vocabulary(this.textGerman.Text, this.textJap.Text, this.textRom.Text, this.comboCategory.Text, this.comboLection.Text);

            if (!VocabularyDatabase.Instance.vocs.Contains(voc)) {
                VocabularyDatabase.Instance.vocs.Add(voc);
                textGerman.Text = "";
                textJap.Text = "";
                textRom.Text = "";
                textGerman.Focus();
            } else
            {
                Helper.MainWindow.ShowMessageAsync("Vokabel existiert bereits", "Diese Vokabel ist bereits vorhanden");
            }
        }

        private void canAddPropertyChanged(object sender, TextChangedEventArgs e)
        {
            if(textGerman.Text != "" && textJap.Text  != "" && comboCategory.SelectedValue != null && !comboCategory.SelectedValue.Equals("") && comboLection.SelectedValue != null && !comboLection.SelectedValue.Equals(""))
            {
                buttonAdd.IsEnabled = true;
            } else
            {
                buttonAdd.IsEnabled = false;
            }
        }

        private void textGerman_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (buttonAdd.IsEnabled)
                {
                    this.buttonAdd_Click(sender, null);
                }
            }
        }

        private void textJap_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Helper.SetInputLanguageByName("japanese");
        }

        private void textRom_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Helper.SetInputLanguageByName("japanese");
            //Helper.SetInputLanguage(Config.Instance.DefaultLang);
        }
    }
}
