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

		public MainWindow()
		{
			InitializeComponent();
			this.mainPage = new MainPage();
			this.signInPage = new SingInPage();
			this.homePage = new HomePage();
			this.imgUploadPage = new ImageUpload();

			this.mainPage.goToSigTUp += MainPage_goToSigTUp;
			this.mainPage.goToHomePage += MainPage_goToHomePage;
			this.homePage.goToUploadImg += MainPage_goToUploadImg;

			this.canvas1.Children.Add(mainPage);
		}

		private void MainPage_goToSigTUp(object sender, RoutedEventArgs e)
		{
			
			this.canvas1.Children.Remove(mainPage);
			this.canvas1.Children.Add(signInPage);
		}

		private void MainPage_goToHomePage(object sender, RoutedEventArgs e)
		{
			this.canvas1.Children.Remove(mainPage);
			this.canvas1.Children.Add(homePage);

			this.homePage.ImagesList.ItemsSource = new InstagramiImage[]
			{
				new InstagramiImage{Title="Image1", ImageData=new BitmapImage(new Uri("C:\\Users\\user\\Pictures\\regex.PNG"))},
				new InstagramiImage{Title="Image2", ImageData=new BitmapImage(new Uri("C:\\Users\\user\\Pictures\\Capture.PNG"))}
			};
		}

		private void MainPage_goToUploadImg(object sender, RoutedEventArgs e)
		{
			this.canvas1.Children.Remove(homePage);
			this.canvas1.Children.Add(imgUploadPage);
		}
	}
}
