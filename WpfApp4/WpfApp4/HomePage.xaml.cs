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
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : UserControl
	{
		public event RoutedEventHandler goToUploadImg;

		public HomePage()
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
	}
}
