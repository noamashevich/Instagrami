using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public event RoutedEventHandler goToSigTUp;
		public event RoutedEventHandler goToHomePage;

		public MainPage()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
			Client client = Client.GetClient();

			SignInStateLabel.Content = client.SignIn(UserName.Text, Password.Password);

            Password.Password = "";

            if (SignInStateLabel.Content == "SignIn was successful")
            {
                SignInStateLabel.Content = "";
                UserName.Text = "";
                this.goToHomePage(sender, e);
            }
		}

        private void SingUpButton_Click(object sender, RoutedEventArgs e)
        {
            goToSigTUp(this, new RoutedEventArgs());
        }

		private void HomePageButton_Click(object sender, RoutedEventArgs e)
		{
			goToHomePage(this, new RoutedEventArgs());
		}
	}
}
