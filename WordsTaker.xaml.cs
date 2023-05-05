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
                var selectedWord = UKRWords.SelectedItems.OfType<string>().FirstOrDefault();
                InputUKR.Text = selectedWord;
                InputENG.Text = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord)?.eng;
            }
            else if (sender == ENGWords)
            {
                var selectedWord = ENGWords.SelectedItems.OfType<string>().FirstOrDefault();
                InputENG.Text = selectedWord;
                InputUKR.Text = WordsDictionary.storage.FirstOrDefault(x => x.eng == selectedWord)?.ukr;
            }
        }

     

        private void CancelSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            UKRWords.SelectedItem = null;
            ENGWords.SelectedItem = null;
        }


        private void UpdateWord_Click(object sender, RoutedEventArgs e)
        {
            var selectedWord = UKRWords.SelectedItem as string;

           
            var ukr = InputUKR.Text.Trim();
            var eng = InputENG.Text.Trim();
            if (string.IsNullOrWhiteSpace(ukr) || string.IsNullOrWhiteSpace(eng))
            {
                MessageBox.Show("Both Ukrainian and English words must be filled in.");
                return;
            }

            
            if (WordsDictionary.storage.Any(x => x.ukr == ukr || x.eng == eng))
            {
                MessageBox.Show("This word already exists in the dictionary.");
                return;
            }

            
            var wordToUpdate = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord || x.eng == selectedWord);
            if (wordToUpdate != null)
            {
                wordToUpdate.ukr = ukr;
                wordToUpdate.eng = eng;
            }

            // Обновляем слово в другом списке, если оно есть там тоже
            var wordToUpdateInOtherList = WordsDictionary.storage.FirstOrDefault(x => x.ukr == ukr || x.eng == eng);
            if (wordToUpdateInOtherList != null && wordToUpdateInOtherList != wordToUpdate)
            {
                wordToUpdateInOtherList.ukr = ukr;
                wordToUpdateInOtherList.eng = eng;
            }

            WordsDictionary.SaveData();
            ShowWords();
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            WordsDictionary.SaveData();
        }

        private void AddWordToDictionary_Click(object sender, RoutedEventArgs e)
        {
            var ukr = InputUKR.Text.Trim();
            var eng = InputENG.Text.Trim();

            if (string.IsNullOrWhiteSpace(ukr) || string.IsNullOrWhiteSpace(eng))
            {
                MessageBox.Show("Both fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (WordsDictionary.storage.Any(x => x.ukr == ukr || x.eng == eng))
            {
                MessageBox.Show("Word already exists in dictionary.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                WordsDictionary.Add(ukr, eng);
                ShowWords();
                WordsDictionary.SaveData();
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
    
