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
using System.Net;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for SingInPage.xaml
    /// </summary>
    public partial class SingInPage : UserControl
    {
        public SingInPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			string username = UserNameNewAccount.Text;
			string password = PasswortNewAccount.Text;

			IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			IPEndPoint serverAddress = new IPEndPoint(ipAddress, 7019);

			SynchronousSocketClient client = new SynchronousSocketClient(serverAddress);

			client.Send(username + ", " + password, MessageType.SignUp);

			Tuple<Header, string> reply = client.Recieve();

			Header header = reply.Item1;

			if (header.type == MessageType.SignUpSuccess)
				SignUpStatus.Content = "SignUp was successful";
			else if (header.type == MessageType.SignUpExists)
				SignUpStatus.Content = "SignUp failed - user already exists";
			else
				SignUpStatus.Content = "Got invalid message from server";
		}
    }
}
