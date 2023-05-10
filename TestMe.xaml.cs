using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TestingTranslate
{
    public partial class TestMe : Window
    {
        private List<Word> testWords;
        private int currentQuestionIndex;
        private int correctAnswerCount;

        public TestMe()
        {
            InitializeComponent();
            InitializeTest();
            ShowCurrentQuestion();
            UpdateQuestionCount();
            UpdateAnsweredCount();
            LanguageTextBlock.DataContext = this; // Устанавливаем контекст данных для LanguageTextBlock
        }

        public string CurrentLanguage
        {
            get { return LanguageTextBlock.Text; }
            set { LanguageTextBlock.Text = value; }
        }

        private void InitializeTest()
        {
            if (WordsDictionary.storage.Count < 5)
            {
                MessageBox.Show("Not enough words in the dictionary to start the test.");
                Close();
                return;
            }

            testWords = WordsDictionary.storage.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            currentQuestionIndex = 0;
            correctAnswerCount = 0;
        }

        private void ShowCurrentQuestion()
        {
            if (currentQuestionIndex >= 0 && currentQuestionIndex < testWords.Count)
            {
                Word currentWord = testWords[currentQuestionIndex];
                WordsBox.Items.Clear();
                WordsBox.Items.Add(currentWord.ukr);
                CurrentLanguage = "Ukrainian"; // Устанавливаем текущий язык в "Ukrainian"
            }
            else
            {
                WordsBox.Items.Clear();
                WordsBox.Items.Add("Test is completed.");
                CurrentLanguage = "Completed"; // Устанавливаем текущий язык в "Completed"
            }
        }

        private void UpdateQuestionCount()
        {
            int questionCount = testWords.Count;
            int currentQuestionNumber = currentQuestionIndex + 1;

            QuestionCountTextBlock.Text = $"Question {currentQuestionNumber} of {questionCount}";
        }

        private void UpdateAnsweredCount()
        {
            AnsweredCountTextBlock.Text = $"Answered: {correctAnswerCount} out of {currentQuestionIndex}";
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeTest();
            ShowCurrentQuestion();
            UpdateQuestionCount();
            UpdateAnsweredCount();
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            WordsDictionary.SaveData();
            Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string inputText = InputTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Please enter a non-empty word before confirming!");
                return;
            }

            if (currentQuestionIndex >= 0 && currentQuestionIndex < testWords.Count)
            {
                Word currentWord = testWords[currentQuestionIndex];
                bool isCorrect = false;

                if (CurrentLanguage == "Ukrainian")
                {
                    isCorrect = currentWord.eng.Equals(inputText, StringComparison.OrdinalIgnoreCase);

                    if (!isCorrect)
                    {
                        isCorrect = currentWord.AlternateEngTranslations.Any(translation => translation.Equals(inputText, StringComparison.OrdinalIgnoreCase));
                    }

                    if (!isCorrect)
                    {
                        isCorrect = currentWord.SynonymsEng.Any(synonym => synonym.Equals(inputText, StringComparison.OrdinalIgnoreCase));
                    }
                }
                else if (CurrentLanguage == "English")
                {
                    isCorrect = currentWord.ukr.Equals(inputText, StringComparison.OrdinalIgnoreCase);

                    if (!isCorrect)
                    {
                        isCorrect = currentWord.AlternateUkrTranslations.Any(translation => translation.Equals(inputText, StringComparison.OrdinalIgnoreCase));
                    }

                    if (!isCorrect)
                    {
                        isCorrect = currentWord.SynonymsUkr.Any(synonym => synonym.Equals(inputText, StringComparison.OrdinalIgnoreCase));
                    }
                }

                if (isCorrect)
                {
                    correctAnswerCount++;
                }

                currentQuestionIndex++;
                ShowCurrentQuestion();
                UpdateQuestionCount();
                UpdateAnsweredCount();
            }
        }



        private void ToggleLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex >= 0 && currentQuestionIndex < testWords.Count)
            {
                Word currentWord = testWords[currentQuestionIndex];
                if (CurrentLanguage == "Ukrainian")
                {
                    WordsBox.Items.Clear();
                    WordsBox.Items.Add(currentWord.eng);
                    CurrentLanguage = "English"; // Устанавливаем текущий язык в "English"
                }
                else if (CurrentLanguage == "English")
                {
                    WordsBox.Items.Clear();
                    WordsBox.Items.Add(currentWord.ukr);
                    CurrentLanguage = "Ukrainian"; // Устанавливаем текущий язык в "Ukrainian"
                }
            }
        }
    }
}
