using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TestingTranslate
{
    public partial class TestMe : Window
    {
        private List<Word> testWords; // Список слов для тестирования
        private int currentQuestionIndex; // Индекс текущего вопроса
        private int correctAnswerCount; // Количество правильных ответов

        public TestMe()
        {
            InitializeComponent();
            InitializeTest();
            ShowCurrentQuestion();
            UpdateQuestionCount();
            UpdateAnsweredCount();
        }

        private void InitializeTest()
        {
            // Проверяем, есть ли в словаре достаточное количество слов для теста
            if (WordsDictionary.storage.Count < 5)
            {
                MessageBox.Show("Not enough words in the dictionary to start the test.");
                Close();
                return;
            }

            // Получаем случайные слова для теста
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
            }
            else
            {
                WordsBox.Items.Clear();
                WordsBox.Items.Add("Test is completed.");
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
                bool isCorrect = currentWord.eng.Equals(inputText, StringComparison.OrdinalIgnoreCase);

                // Проверяем также синонимы на правильность ответа
                if (!isCorrect)
                {
                    isCorrect = currentWord.Synonyms.Any(synonym => synonym.Equals(inputText, StringComparison.OrdinalIgnoreCase));
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
                WordsBox.Items.Clear();
                WordsBox.Items.Add(currentWord.eng);
            }
        }
    }
}
