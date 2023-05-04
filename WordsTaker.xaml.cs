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

namespace TestingTranslate
{
    /// <summary>
    /// Interaction logic for WordsTaker.xaml
    /// </summary>
    public partial class WordsTaker : Window
    {
        public WordsTaker()
        {
            InitializeComponent();
            ShowWords();
        }

        

        private void UKRWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (sender == UKRWords)
            {
                var selectedWord = UKRWords.SelectedItem as string;
                InputUKR.Text = selectedWord;
                InputENG.Text = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord)?.eng;
            }
            else if (sender == ENGWords)
            {
                var selectedWord = ENGWords.SelectedItem as string;
                InputENG.Text = selectedWord;
                InputUKR.Text = WordsDictionary.storage.FirstOrDefault(x => x.eng == selectedWord)?.ukr;
            }
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            WordsDictionary.SaveData();
        }

        private void CancelSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            UKRWords.SelectedItem = null;
            ENGWords.SelectedItem = null;
        }


        private void UpdateWord_Click(object sender, RoutedEventArgs e)
        {
            var selectedWord = UKRWords.SelectedItem as string;

            
            var wordToUpdate = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord || x.eng == selectedWord);
            if (wordToUpdate != null)
            {
                var ukr = InputUKR.Text.Trim();
                var eng = InputENG.Text.Trim();

                if (!string.IsNullOrWhiteSpace(ukr) && !string.IsNullOrWhiteSpace(eng))
                {
                    wordToUpdate.ukr = ukr;
                    wordToUpdate.eng = eng;

                    WordsDictionary.SaveData();
                    ShowWords();
                }
                else
                {
                    MessageBox.Show("Both Ukrainian and English words must be filled in.");
                }
            }

           
            var wordToUpdateInOtherList = WordsDictionary.storage.FirstOrDefault(x => x.ukr == InputUKR.Text.Trim() || x.eng == InputENG.Text.Trim());
            if (wordToUpdateInOtherList != null && wordToUpdateInOtherList != wordToUpdate)
            {
                var ukr = InputUKR.Text.Trim();
                var eng = InputENG.Text.Trim();

                if (!string.IsNullOrWhiteSpace(ukr) && !string.IsNullOrWhiteSpace(eng))
                {
                    wordToUpdateInOtherList.ukr = ukr;
                    wordToUpdateInOtherList.eng = eng;

                    WordsDictionary.SaveData();
                    ShowWords();
                }
                else
                {
                    MessageBox.Show("Both Ukrainian and English words must be filled in.");
                }
            }
        }



        private void AddWordToDictionary_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputUKR.Text) || string.IsNullOrWhiteSpace(InputENG.Text) ||
               InputUKR.Text.Trim().Length == 0 || InputENG.Text.Trim().Length == 0)
            {
                MessageBox.Show("Both fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WordsDictionary.Add(InputUKR.Text.Trim(), InputENG.Text.Trim());
                ShowWords();
                InputENG.Clear();
                InputUKR.Clear();
            }
        }
        private void ShowWords()
        {
            UKRWords.ItemsSource = WordsDictionary.storage.Select(x => x.ukr);
            ENGWords.ItemsSource = WordsDictionary.storage.Select(x => x.eng);
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }
        private void DeleteWordFromDictionary_Click(object sender, RoutedEventArgs e)
        {
            if (WordsDictionary.storage.Count == 0)
            {
                MessageBox.Show("There are no words in the dictionary to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WordsDictionary.storage.Clear();
                ShowWords();
                WordsDictionary.SaveData(); 
            }
        }
    }
}
    
