using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WpfApp4
{
	public enum MessageType
	{
		SignIn,
		SignUp,
		ImageUpload,
		ImageDownload,
		MainPage
	}
	public class SynchronousSocketClient
	{
		private Socket sender;

		public SynchronousSocketClient(IPEndPoint serverAddress)
		{
			this.sender = new Socket(serverAddress.Address.AddressFamily,
									   SocketType.Stream, ProtocolType.Tcp);

			// Connect the socket to the remote endpoint. Catch any errors.  
			this.sender.Connect(serverAddress);

			Console.WriteLine("Socket connected to {0}",
							  this.sender.RemoteEndPoint.ToString());
		}

		/**
		 * The method sends a message to the server
		 * 
		 * @param message The message to send
		 * 
		 * @return bool Returns true if the message was sent successfuly, false otherwise.
		 */
		public bool Send(string message, MessageType messageType)
		{
			Header header = new Header((uint)message.Length, 0, messageType);

			MemoryStream ms = new MemoryStream();

			byte[] headerBytes = header.Serialize();
			ms.Write(headerBytes, 0, headerBytes.Length);

			// Encode the data string into a byte array.  
			byte[] msg = Encoding.ASCII.GetBytes(message);
			ms.Write(msg, 0, msg.Length);

			// Send the data through the socket.  
			int bytesSent = this.sender.Send(ms.ToArray());

			return (bytesSent == ms.Length);
		}

		/**
		 * The method recieves a message from the server
		 * 
		 * @return string The message recieved from the server
		 */
		public string Recieve()
		{
			byte[] headerBytes = new byte[Header.HEADER_LENGTH];
			int bytesRec = sender.Receive(headerBytes);

			Header header = new Header(headerBytes);

			byte[] msgBytes = new byte[header.length];
			bytesRec = sender.Receive(msgBytes);

			return Encoding.ASCII.GetString(msgBytes, 0, bytesRec);
		}
	}

	public class Header
	{
		public UInt32 length;
		public UInt32 id;
		public MessageType type;

		public static readonly int HEADER_LENGTH = 12;

		/**
		 * Serializes the header to bytes
		 * 
		 * @
		 */
		public byte[] Serialize()
		{
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);

			bw.Write(this.length);
			bw.Write(this.id);
			bw.Write((Int32)this.type);

			return ms.ToArray();
		}

		public Header(UInt32 length, UInt32 id, MessageType type)
		{
			this.length = length;
			this.id = id;
			this.type = type;
		}

		public Header(byte[] data)
		{
			MemoryStream stream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(stream);

			this.length = br.ReadUInt32();
			this.id = br.ReadUInt32();
			this.type = (MessageType)br.ReadInt32();
		}
	}
}
