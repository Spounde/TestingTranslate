using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace TestingTranslate
{

    public partial class MainWindow : Window
    {
        private List<string> wordsList; // Список слов для отображения

        public MainWindow()
        {
            InitializeComponent();
            wordsList = WordsDictionary.storage.Select(x => x.ukr).ToList();
            WordsBox.ItemsSource = wordsList;
            SynonymsTextBox.Visibility = Visibility.Collapsed;
            TranslationBox.Visibility = Visibility.Collapsed;
            Synonymslist.Visibility = Visibility.Collapsed;
            Translation.Visibility = Visibility.Collapsed;
            AlternateTranslationsBox.Visibility = Visibility.Collapsed;
            AlternateTranslation.Visibility = Visibility.Collapsed;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            WordsDictionary.SaveData();
            this.Close();
            
        }
        private string selectedWord;

        private void WordsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedWord = WordsBox.SelectedItem as string;
            SearchTextBox.Text = selectedWord;

            if (string.IsNullOrEmpty(selectedWord))
            {
                SynonymsTextBox.Visibility = Visibility.Collapsed;
                TranslationBox.Visibility = Visibility.Collapsed;
                Synonymslist.Visibility = Visibility.Collapsed;
                Translation.Visibility = Visibility.Collapsed;
                AlternateTranslationsBox.Visibility = Visibility.Collapsed;
                AlternateTranslation.Visibility = Visibility.Collapsed;
                ClearSelectionButton.Visibility = Visibility.Collapsed; // Скрываем кнопку снятия выделения
            }
            else
            {
                if (UKR_RB.IsChecked == true)
                {
                    var word = WordsDictionary.storage.FirstOrDefault(x => x.ukr.Equals(selectedWord, StringComparison.OrdinalIgnoreCase));
                    if (word != null)
                    {
                        SynonymsTextBox.Text = string.Join(", ", word.Synonyms.OrderBy(synonym => synonym));
                        TranslationBox.Text = word.eng;

                        AlternateTranslationsBox.ItemsSource = word.AlternateEngTranslations;
                    }
                }
                else if (ENG_RB.IsChecked == true)
                {
                    var word = WordsDictionary.storage.FirstOrDefault(x => x.eng.Equals(selectedWord, StringComparison.OrdinalIgnoreCase));
                    if (word != null)
                    {
                        SynonymsTextBox.Text = string.Join(", ", word.Homonyms);
                        TranslationBox.Text = word.ukr;

                        AlternateTranslationsBox.ItemsSource = word.AlternateUkrTranslations;
                    }
                }
                SynonymsTextBox.Visibility = Visibility.Visible;
                TranslationBox.Visibility = Visibility.Visible;
                Synonymslist.Visibility = Visibility.Visible;
                Translation.Visibility = Visibility.Visible;
                AlternateTranslationsBox.Visibility = Visibility.Visible;
                AlternateTranslation.Visibility = Visibility.Visible;
                ClearSelectionButton.Visibility = Visibility.Visible; // Показываем кнопку снятия выделения
            }
        }

        private void ClearSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            WordsBox.SelectedItem = null; // Снимаем выделение
        }




        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = SearchTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Please enter a word to search.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UKR_RB.IsChecked == true)
            {
                var matchingWords = WordsDictionary.storage
                    .Where(word => word.ukr.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)
                        || word.Synonyms.Any(synonym => synonym.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                        || word.ukr.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                        || word.Synonyms.Any(synonym => synonym.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                    .Select(word => word.ukr);
                WordsBox.ItemsSource = matchingWords;
            }
            else if (ENG_RB.IsChecked == true)
            {
                var matchingWords = WordsDictionary.storage
                    .Where(word => word.eng.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)
                        || word.Homonyms.Any(homonym => homonym.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase))
                        || word.eng.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                        || word.Homonyms.Any(homonym => homonym.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                    .Select(word => word.eng);
                WordsBox.ItemsSource = matchingWords;
            }
        }


        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo ukrainianCulture = new CultureInfo("uk-UA");
            wordsList.Sort(StringComparer.Create(ukrainianCulture, false));

            if (UKR_RB.IsChecked == true)
            {
                WordsBox.ItemsSource = wordsList;
            }
            else if (ENG_RB.IsChecked == true)
            {
                var engWords = wordsList.Select(word => WordsDictionary.storage.First(x => x.ukr.Equals(word, StringComparison.OrdinalIgnoreCase)).eng);
                WordsBox.ItemsSource = engWords;
            }
        }


        private void UKR_RB_Checked(object sender, RoutedEventArgs e)
        {
            WordsBox.ItemsSource = WordsDictionary.storage.Select(x => x.ukr);
            
        }

        private void ENG_RB_Checked(object sender, RoutedEventArgs e)
        {
            WordsBox.ItemsSource = WordsDictionary.storage.Select(x => x.eng);
            
        }

        private void IncreasWords_Click(object sender, RoutedEventArgs e)
        {
            WordsTaker wt = new WordsTaker();
            wt.Show();
            this.Close();
        }

        private void TestMe_Click(object sender, RoutedEventArgs e)
        {
            TestMe t = new TestMe();
            t.Show();
            this.Close();
        }
    }
}
