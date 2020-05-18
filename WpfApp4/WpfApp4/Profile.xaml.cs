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
	/// Interaction logic for Profile.xaml
	/// </summary>
	public partial class Profile : UserControl
	{
		public event RoutedEventHandler goToUploadImg;
		public event RoutedEventHandler goToMainPage;
		public event RoutedEventHandler goToHomePage;

		public Profile()
		{
			InitializeComponent();
		}

		private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void ImageUploadButtom_Click(object sender, RoutedEventArgs e)
		{
			this.goToUploadImg(sender, e);
		}

		private void LogOutButtom_Click(object sender, RoutedEventArgs e)
		{
			Client.Logout();

			this.goToMainPage(sender, e);
		}

		private void HomePageButton_Click(object sender, RoutedEventArgs e)
		{
			this.goToHomePage(sender, e);
		}
	}
}
