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

namespace TestingTranslate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
