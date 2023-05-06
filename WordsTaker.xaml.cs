﻿using System;
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
                if (selectedWord != null)
                {
                    var word = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord);
                    if (word != null)
                    {
                        InputUKR.Text = word.ukr;
                        InputENG.Text = word.eng;
                        InputUKRSynonyms.Text = string.Join(", ", word.Synonyms);
                        InputENGSynonyms.Text = string.Join(", ", word.Homonyms);
                        InputAlternateUKR.Text = string.Join(", ", word.AlternateUkrTranslations);
                        InputAlternateENG.Text = string.Join(", ", word.AlternateEngTranslations);
                    }
                }
            }
            else if (sender == ENGWords)
            {
                var selectedWord = ENGWords.SelectedItems.OfType<string>().FirstOrDefault();
                if (selectedWord != null)
                {
                    var word = WordsDictionary.storage.FirstOrDefault(x => x.eng == selectedWord);
                    if (word != null)
                    {
                        InputUKR.Text = word.ukr;
                        InputENG.Text = word.eng;
                        InputUKRSynonyms.Text = string.Join(", ", word.Synonyms);
                        InputENGSynonyms.Text = string.Join(", ", word.Homonyms);
                        InputAlternateUKR.Text = string.Join(", ", word.AlternateUkrTranslations);
                        InputAlternateENG.Text = string.Join(", ", word.AlternateEngTranslations);
                    }
                }
            }

            if (UKRWords.SelectedItem != null || ENGWords.SelectedItem != null)
            {
                ApplyChangeButton.Visibility = Visibility.Visible;
                CancelSelectionButton.Visibility = Visibility.Visible;
            }
            else
            {
                ApplyChangeButton.Visibility = Visibility.Collapsed;
                CancelSelectionButton.Visibility = Visibility.Collapsed;
            }
        }



        private void CancelSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            UKRWords.SelectedItem = null;
            ENGWords.SelectedItem = null;
            InputUKRSynonyms.Clear();
            InputENGSynonyms.Clear();
            InputAlternateENG.Clear();
            InputAlternateUKR.Clear();
            ApplyChangeButton.Visibility = Visibility.Collapsed;
            CancelSelectionButton.Visibility = Visibility.Collapsed;
        }

        private void UpdateWord_Click(object sender, RoutedEventArgs e)
        {
            var selectedWord = UKRWords.SelectedItem as string;

            var ukr = InputUKR.Text.Trim();
            var eng = InputENG.Text.Trim();
            var ukrSynonyms = InputUKRSynonyms.Text.Trim().Split(',').Select(s => s.Trim()).ToList();
            var engSynonyms = InputENGSynonyms.Text.Trim().Split(',').Select(s => s.Trim()).ToList();
            var alternateUkrTranslations = InputAlternateUKR.Text.Trim().Split(',').Select(s => s.Trim()).ToList();
            var alternateEngTranslations = InputAlternateENG.Text.Trim().Split(',').Select(s => s.Trim()).ToList();

            if (string.IsNullOrWhiteSpace(ukr) || string.IsNullOrWhiteSpace(eng))
            {
                MessageBox.Show("Both Ukrainian and English words must be filled in.");
                return;
            }

            var wordToUpdate = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord || x.eng == selectedWord);
            if (wordToUpdate != null)
            {
                if (wordToUpdate.ukr != ukr && WordsDictionary.storage.Any(x => x.ukr == ukr))
                {
                    MessageBox.Show("This Ukrainian word already exists in the dictionary.");
                    return;
                }

                wordToUpdate.ukr = ukr;
                wordToUpdate.eng = eng;
                wordToUpdate.Synonyms = ukrSynonyms;
                wordToUpdate.Homonyms = engSynonyms;
                wordToUpdate.AlternateUkrTranslations = alternateUkrTranslations;
                wordToUpdate.AlternateEngTranslations = alternateEngTranslations;
            }

            WordsDictionary.SaveData();
            ShowWords();

            UKRWords.SelectedItem = null;
            ENGWords.SelectedItem = null;
            ApplyChangeButton.Visibility = Visibility.Collapsed;
            CancelSelectionButton.Visibility = Visibility.Collapsed;
        }






            private void SearchUKR_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterWords(UKRWords, SearchUKR.Text);
        }

        private void SearchENG_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterWords(ENGWords, SearchENG.Text);
        }
        private void FilterWords(ListBox listBox, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                listBox.ItemsSource = WordsDictionary.storage.Select(x => x.ukr);
                return;
            }

            var filteredWords = WordsDictionary.storage
                .Where(x => x.ukr.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                            x.eng.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.ukr)
                .ToList();
            listBox.ItemsSource = filteredWords;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            WordsDictionary.SaveData();
            this.Close();

        }

        private void AddWordToDictionary_Click(object sender, RoutedEventArgs e)
        {
            var ukr = InputUKR.Text.Trim();
            var eng = InputENG.Text.Trim();
            var ukrSynonyms = InputUKRSynonyms.Text.Trim().Split(',').Select(s => s.Trim()).ToList();
            var engSynonyms = InputENGSynonyms.Text.Trim().Split(',').Select(s => s.Trim()).ToList();

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
                WordsDictionary.Add(ukr, eng, ukrSynonyms, engSynonyms);
                ShowWords();
                WordsDictionary.SaveData();
                InputENG.Clear();
                InputUKR.Clear();
                InputUKRSynonyms.Clear();
                InputENGSynonyms.Clear();
            }
        }

        private void ShowWords()
        {
            UKRWords.ItemsSource = WordsDictionary.storage.Select(x => x.ukr);
            ENGWords.ItemsSource = WordsDictionary.storage.Select(x => x.eng);

            var selectedWord = UKRWords.SelectedItems.OfType<string>().FirstOrDefault();
            if (selectedWord != null)
            {
                var word = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord);
                if (word != null)
                {
                    InputUKR.Text = word.ukr;
                    InputENG.Text = word.eng;
                    InputUKRSynonyms.Text = string.Join(", ", word.Synonyms);
                    InputENGSynonyms.Text = string.Join(", ", word.Homonyms);
                }
            }
            else
            {
                selectedWord = ENGWords.SelectedItems.OfType<string>().FirstOrDefault();
                if (selectedWord != null)
                {
                    var word = WordsDictionary.storage.FirstOrDefault(x => x.eng == selectedWord);
                    if (word != null)
                    {
                        InputUKR.Text = word.ukr;
                        InputENG.Text = word.eng;
                        InputUKRSynonyms.Text = string.Join(", ", word.Synonyms);
                        InputENGSynonyms.Text = string.Join(", ", word.Homonyms);
                    }
                }
                else
                {
                    InputUKR.Clear();
                    InputENG.Clear();
                    InputUKRSynonyms.Clear();
                    InputENGSynonyms.Clear();
                }
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }
        private void DeleteWordFromDictionary_Click(object sender, RoutedEventArgs e)
        {
            var selectedWord = UKRWords.SelectedItem as string;
            if (selectedWord != null)
            {
                var wordToDelete = WordsDictionary.storage.FirstOrDefault(x => x.ukr == selectedWord);
                if (wordToDelete != null)
                {
                    WordsDictionary.storage.Remove(wordToDelete);
                    ShowWords();
                    WordsDictionary.SaveData();
                    UKRWords.SelectedItem = null;
                    ENGWords.SelectedItem = null;
                }
            }
            else
            {
                selectedWord = ENGWords.SelectedItem as string;
                if (selectedWord != null)
                {
                    var wordToDelete = WordsDictionary.storage.FirstOrDefault(x => x.eng == selectedWord);
                    if (wordToDelete != null)
                    {
                        WordsDictionary.storage.Remove(wordToDelete);
                        ShowWords();
                        WordsDictionary.SaveData();
                        UKRWords.SelectedItem = null;
                        ENGWords.SelectedItem = null;
                    }
                }
            }
        }

    }
}