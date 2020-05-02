using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp4
{
	public class Client
	{
		private static Client client = null;

		private SynchronousSocketClient clientSocket;
		private string username = "";

		public string Username
		{
			get { return this.username; }
		}

		/**
		 * Singleton method.
		 * Used to prevent code duplication.
		 * 
		 * @return Client Returns the same instance of the client.
		 */
		public static Client GetClient()
		{
			if (Client.client == null)
			{
				Client.client = new Client("127.0.0.1", 7019);
			}

			return Client.client;
		}

		/**
		 * The constructor of the client.
		 * 
		 * @param ip The ip of the server
		 * @param port The port to connect to server in.
		 */
		public Client(string ip, int port)
		{
			IPAddress ipAddress = IPAddress.Parse(ip);
			IPEndPoint serverAddress = new IPEndPoint(ipAddress, port);

			clientSocket = new SynchronousSocketClient(serverAddress);
		}

		public string SignUp(string username, string password)
		{
			clientSocket.Send(username + ", " + password, MessageType.SignUp);

			Tuple<Header, string> reply = clientSocket.Recieve();

			Header header = reply.Item1;

			if (header.type == MessageType.SignUpSuccess)
				return "SignUp was successful";
			else if (header.type == MessageType.SignUpExists)
				return "SignUp failed - user already exists";
			else
				return "Got invalid message from server";
		}

		public string SignIn(string username, string password)
		{
			clientSocket.Send(username + ", " + password, MessageType.SignIn);

			Tuple<Header, string> reply = clientSocket.Recieve();

			Header header = reply.Item1;

			if (header.type == MessageType.SignInSuccess)
			{
				this.username = username;
				return "SignIn was successful";
			}
			else if (header.type == MessageType.SignInFail)
				return "SignIn failed - wrong username or password";
			else
				return "Got invalid message from server";
		}

		/**
		 * Uploads an image to the server.
		 * 
		 * @param description  The description of the image
		 * @param imageContent The bytes that represent the image to be uploaded
		 * 
		 * @return string The string with the result of the image uploading
		 */
		public string ImageUpload(string description, byte[] imageContent)
		{
			// Convert username and description to byte arrays.
			byte[] usernameBytes = ASCIIEncoding.ASCII.GetBytes(client.username);
			byte[] descriptionBytes = ASCIIEncoding.ASCII.GetBytes(description);

			// Get all of the fields' sizes and pad them to a length of 10 with zeros.
			string usernameSize = usernameBytes.Length.ToString().PadLeft(10, '0');
			string descriptionSize = descriptionBytes.Length.ToString().PadLeft(10, '0');
			string imgContentSize = imageContent.Length.ToString().PadLeft(10, '0');

			// Convert all of the sizes to a bytearray
			byte[] sizesBytes = ASCIIEncoding.ASCII.GetBytes(usernameSize + descriptionSize + imgContentSize);

			// Concat all of the message's parts
			byte[] message = sizesBytes.Concat(usernameBytes).Concat(descriptionBytes).Concat(imageContent).ToArray();

			// Send the message
			clientSocket.Send(message, MessageType.ImageUpload);

			// Get a reply and parse it
			Tuple<Header, string> reply = clientSocket.Recieve();

			Header header = reply.Item1;

			if (header.type == MessageType.ImgUploadSuccess)
				return "Image uploaded successfully";
			else if (header.type == MessageType.ImgUploadFail ||
					 header.type == MessageType.NoSuchUser ||
					 header.type == MessageType.InvalMsgFmt)
				return "Image upload failed!";
			else
				return "Got invalid message from server";
		}


		public Tuple<string, List<InstagramiImage>> RequestHomePage(int start, int amount)
		{
			string request = this.username + ", " + start.ToString() + ", " + amount.ToString();

			clientSocket.Send(request, MessageType.RequestMainPage);

			Tuple<Header, byte[]> reply = clientSocket.RecieveBytes();

			Header header = reply.Item1;
			byte[] message = reply.Item2;

			if (header.type != MessageType.ResponseMainPage)
				return new Tuple<string, List<InstagramiImage>>("Failed to get a response", null);

			List<InstagramiImage> images = new List<InstagramiImage>();

			while (message != null && message.Length > 0)
			{
				string sizes = Encoding.ASCII.GetString(message, 0, 30);
				
				int usernameSize = int.Parse(sizes.Substring(0, 10));
				int imageSize = int.Parse(sizes.Substring(10, 10));
				int textSize = int.Parse(sizes.Substring(20, 10));

				message = message.Skip(30).ToArray();

				string username = Encoding.ASCII.GetString(message, 0, usernameSize);
				byte[] image = message.Skip(usernameSize).Take(imageSize).ToArray();
				string text = Encoding.ASCII.GetString(message, usernameSize + imageSize, textSize);

				/*
				using (System.IO.MemoryStream ms = new System.IO.MemoryStream(image))
				{
					BitmapImage imageBitMap = new BitmapImage();

					imageBitMap.BeginInit();
					imageBitMap.StreamSource = ms;
					imageBitMap.EndInit();

					images.Add(new InstagramiImage(username + ": " + text, imageBitMap));
				}
				*/

				string imagePath = @"C:\Users\user\" + start.ToString() + ".bmp";

				File.WriteAllBytes(imagePath, image);

				BitmapImage bmapImage = new BitmapImage();

				bmapImage.BeginInit();
				bmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bmapImage.UriSource = new Uri(imagePath);
				bmapImage.EndInit();


				images.Add(new InstagramiImage(username + ": " + text, bmapImage));

				File.Delete(imagePath);

				message = message.Skip(usernameSize + imageSize + textSize).ToArray();

				start++;
			}

			return new Tuple<string, List<InstagramiImage>>("Got images from server", images);
		}
	}
}
