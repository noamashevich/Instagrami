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

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainPage mainPage;
        SingInPage signInPage;
        public MainWindow()
        {
            InitializeComponent();
            mainPage = new MainPage();
            mainPage.goToSigTUp += MainPage_goToSigTUp;
            this.canvas1.Children.Add(mainPage);
            
        }

        private void MainPage_goToSigTUp(object sender, RoutedEventArgs e)
        {
            this.signInPage = new SingInPage();
            this.canvas1.Children.Remove(mainPage);
            this.canvas1.Children.Add(signInPage);
        }
    }
}
