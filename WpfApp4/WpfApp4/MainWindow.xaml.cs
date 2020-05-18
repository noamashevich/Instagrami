using System;
using System.Collections.Generic;
using System.Drawing;
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
		HomePage homePage;
		ImageUpload imgUploadPage;
		Profile profilePage;

		public MainWindow()
		{
			InitializeComponent();
			this.mainPage = new MainPage();
			this.signInPage = new SingInPage();
			this.homePage = new HomePage();
			this.imgUploadPage = new ImageUpload();
			this.profilePage = new Profile();

			this.mainPage.goToSigTUp += MainPage_goToSigTUp;
			this.mainPage.goToHomePage += MainPage_goToHomePage;

			this.homePage.goToUploadImg += MainPage_goToUploadImg;
			this.homePage.goToProfilePage += MainPage_goToProfilePage;
			this.homePage.goToMainPage += MainPage_goToMainPage;

			this.profilePage.goToUploadImg += MainPage_goToUploadImg;
			this.profilePage.goToHomePage += MainPage_goToHomePage;
			this.profilePage.goToMainPage += MainPage_goToMainPage;
			

			this.canvas1.Children.Add(mainPage);
		}

		private void MainPage_goToSigTUp(object sender, RoutedEventArgs e)
		{
			this.canvas1.Children.Clear();
			this.canvas1.Children.Add(signInPage);
		}

		private void MainPage_goToHomePage(object sender, RoutedEventArgs e)
		{
			this.canvas1.Children.Clear();
			this.canvas1.Children.Add(homePage);

			Client client = Client.GetClient();
			Tuple<string, List<InstagramiImage>> response = client.RequestHomePage(0, 10);

			if (response.Item2 != null)
			{
				this.homePage.ImagesList.ItemsSource = new List<InstagramiImage>(response.Item2);
			}
		}

		private void MainPage_goToUploadImg(object sender, RoutedEventArgs e)
		{
			this.canvas1.Children.Clear();
			this.canvas1.Children.Add(imgUploadPage);
		}

		private void MainPage_goToProfilePage(object sender, RoutedEventArgs e)
		{
			this.canvas1.Children.Clear();
			this.canvas1.Children.Add(profilePage);

			Client client = Client.GetClient();

			this.profilePage.usernameLabel.Content = client.Username;

			Tuple<string, List<InstagramiImage>> response = client.RequestProfile(0, 10);

			if (response.Item2 != null)
			{
				this.profilePage.ImagesList.ItemsSource = new List<InstagramiImage>(response.Item2);
			}
		}

		private void MainPage_goToMainPage(object sender, RoutedEventArgs e)
		{
			this.canvas1.Children.Clear();
			this.canvas1.Children.Add(mainPage);
		}
	}
}
