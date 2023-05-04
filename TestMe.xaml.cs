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
    /// Логика взаимодействия для TestMe.xaml
    /// </summary>
    public partial class TestMe : Window
    {
        public TestMe()
        {
            InitializeComponent();
            ShowRandomWord();
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            TestMe t = new TestMe();
            t.Show();
            this.Close();
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }

        
        private Random random = new Random();

        private void ShowRandomWord()
        {
            if (WordsDictionary.storage.Count == 0)
            {
                WordsBox.Items.Clear();
                WordsBox.Items.Add("There is no words in dictionary, please add some words to it.");
                selectedWord = null;
            }
            else
            {
                
                int index = random.Next(WordsDictionary.storage.Count);

                
                Word word = WordsDictionary.storage[index];

                
                WordsBox.Items.Clear();
                WordsBox.Items.Add(word.ukr);

                
                selectedWord = word;
            }
        }

        private Word selectedWord;

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string inputText = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Please enter a non-empty word before confirming!");
                return;
            }

            if (selectedWord != null)
            {
                bool isCorrect = selectedWord.eng.Equals(inputText.Trim());

                MessageBox.Show(isCorrect ? "Correct!" : "Incorrect!");
            }
        }
    }
}
