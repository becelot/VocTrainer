using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private Grid gridRef;

        private DataGrid evaluationGrid;

        protected class EvaluationEntry
        {
            public string Vocabulary { get; set; }
            public string Solution { get; set; }
            public string Entered { get; set; }
            public bool Correct { get; set; }
        }

        private ObservableCollection<EvaluationEntry> solutions;

        private int index = 0;

        public CheckVocabulary()
        {
            InitializeComponent();
            solutions = new ObservableCollection<EvaluationEntry>();
            gridRef = stdGrid;
            evaluationGrid = new DataGrid() { AutoGenerateColumns=false, ItemsSource=solutions, CanUserAddRows=false };
            evaluationGrid.Columns.Add(new DataGridTextColumn() { Header = "Vokabel", Width=new DataGridLength(1,DataGridLengthUnitType.Star), Binding=new Binding("Vocabulary") });
            evaluationGrid.Columns.Add(new DataGridTextColumn() { Header = "Lösung", Width = new DataGridLength(1, DataGridLengthUnitType.Star), Binding = new Binding("Solution") });
            evaluationGrid.Columns.Add(new DataGridTextColumn() { Header = "Eingabe", Width = new DataGridLength(1, DataGridLengthUnitType.Star), Binding=new Binding("Entered") });
            evaluationGrid.Columns.Add(new DataGridCheckBoxColumn() { Header = "Korrekt?", Width = new DataGridLength(1, DataGridLengthUnitType.Star), Binding =new Binding("Correct") });
        }

        private void nextVocabulary_Click(object sender, RoutedEventArgs e)
        {
            solutions.Add(new EvaluationEntry() {
                Vocabulary = labelCheck.Content.ToString(),
                Solution =actualDictionary[labelCheck.Content.ToString()],
                Entered =answerBox.Text,
                Correct =(actualDictionary[labelCheck.Content.ToString()].Equals(answerBox.Text))
            });

            index++;
            if (index < actualDictionary.Keys.Count)
            {
                answerBox.Text = "";
                answerBox.Focus();
                labelCheck.Content = actualDictionary.Keys.ToList()[index];
            }
            else // Auswerten
            {
                Helper.MainWindow.FlyoutCheckVocabulary.CloseButtonVisibility = Visibility.Visible;
                Transistor.Content = evaluationGrid;
            }
        }

        public void onCheckVocabularyOpen(bool mode)
        {
            if (mode)
            {
                Transistor.Content = this.gridRef;
                Helper.MainWindow.FlyoutCheckVocabulary.CloseButtonVisibility = Visibility.Hidden;
                solutions.Clear();
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
