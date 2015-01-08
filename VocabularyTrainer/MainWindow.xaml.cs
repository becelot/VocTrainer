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
using MahApps.Metro.Controls;

namespace VocabularyTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            VocabularyDatabase.Load();
            Vocabulary voc = new Vocabulary("Guten Tag", "こんにちは", "konnichiha", "Ausdruck", "1");
            VocabularyDatabase.Instance.vocs.Add(voc);
            VocabularyDatabase.Save();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAddVoc_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
