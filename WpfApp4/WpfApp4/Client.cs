using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4
{
	class Client
	{
		private static Client client = null;
		public SynchronousSocketClient clientSocket;

		/**
		 * Singleton method.
		 * Used to prevent code duplication.
		 * 
		 * @return Client Returns the same instance of the client.
		 */
		public static Client GetClient()
		{
			if (client == null)
			{
				Client client = new Client("127.0.0.1", 7019);
			}

			return client;
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

		public string ImageUpload(string username, string description, byte[] imageContent)
		{
		}
	}
}
