using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
	/// Interaction logic for ImageUpload.xaml
	/// </summary>
	public partial class ImageUpload : UserControl
	{
		public ImageUpload()
		{
			InitializeComponent();
		}

		/**
		 * Opens a dialoge box to choose an image file.
		 * Also changes the image on the page to present the chosen image.
		 */
		private void chooseImageButton_Click(object sender, RoutedEventArgs e)
		{
			string imageContent;

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "Image files (*.jpg)|*.jpg|(*.png)|*.png|All Files (*.*)|*.*";
			openFileDialog.RestoreDirectory = true;

			if (!(bool)openFileDialog.ShowDialog())
				return;

			BitmapImage bitmap = new BitmapImage();
			bitmap.BeginInit();
			bitmap.UriSource = new Uri(openFileDialog.FileName);
			bitmap.EndInit();
			ImageViewer.Source = bitmap;
			uploadButton.IsEnabled = true;
		}

		private void uploadButton_Click(object sender, RoutedEventArgs e)
		{
			byte[] imageContent;
			string imagePath = ImageViewer.Source.ToString();

			BitmapImage bitmap = new BitmapImage();
			bitmap.BeginInit();
			bitmap.UriSource = new Uri(imagePath);
			bitmap.EndInit();

			BmpBitmapEncoder encoder = new BmpBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(bitmap));

			using (MemoryStream ms = new MemoryStream())
			{
				encoder.Save(ms);
				imageContent = ms.ToArray();
			}

			string imageDesc = imageDescription.Text;
		}
	}
}
