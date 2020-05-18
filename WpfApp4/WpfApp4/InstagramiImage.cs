using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp4
{
	public class InstagramiImage
	{
		public InstagramiImage(string Title, BitmapImage ImageData, string imageID, string likes)
		{
			this.Title = Title;
			this.ImageData = ImageData;
			this.ImageID = imageID;
			this.Likes = likes;
		}

		private string _Title;
		public string Title
		{
			get { return this._Title; }
			set { this._Title = value; }
		}

		private BitmapImage _ImageData;
		public BitmapImage ImageData
		{
			get { return this._ImageData; }
			set { this._ImageData = value; }
		}

		private string imageID;
		public string ImageID
		{
			get { return this.imageID; }
			set { this.imageID = value; }
		}

		private string likes;
		public string Likes
		{
			get { return this.likes; }
			set { this.likes = value; }
		}
	}
}
