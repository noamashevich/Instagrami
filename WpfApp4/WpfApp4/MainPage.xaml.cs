using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {


        public event RoutedEventHandler goToSigTUp;

        public MainPage()
        {
            InitializeComponent();
        }

        

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
			// Establish the remote endpoint for the socket.  
			// This example uses port 7000 on the local computer.  
			// IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // ipHostInfo.AddressList[0];
			IPEndPoint serverAddress = new IPEndPoint(ipAddress, 7019);

			SynchronousSocketClient client = new SynchronousSocketClient(serverAddress);

			// client.Send("WOW", MessageType.SignIn);

			// string message = client.Recieve();
		}

        private void SingUpButton_Click(object sender, RoutedEventArgs e)
        {
           // if (goToSigTUp != null)
            {
                goToSigTUp(this, new RoutedEventArgs());
            }
        }
    }
}
