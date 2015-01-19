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
    /// Interaction logic for CheckVocabulary.xaml
    /// </summary>
    public partial class CheckVocabulary : UserControl
    {
        public Dictionary<string, string> actualDictionary { get; set; }
        public Dictionary<string, string> answers { get; set; }

        private int index = 0;

        public CheckVocabulary()
        {
            InitializeComponent();
            answers = new Dictionary<string, string>();
        }

        private void nextVocabulary_Click(object sender, RoutedEventArgs e)
        {
            answers.Add(labelCheck.Content.ToString(), answerBox.Text);
            index++;
            if (index < actualDictionary.Keys.Count)
            {
                answerBox.Text = "";
                answerBox.Focus();
                labelCheck.Content = actualDictionary.Keys.ToList()[index];
            }
            else // Auswerten
            {
                Helper.MainWindow.FlyoutCheckVocabulary.IsOpen = false;
            }
        }

        private void answerBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.nextVocabulary_Click(sender, null);
            }
        }
    }
}
