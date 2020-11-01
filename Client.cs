// A C# program for Client 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        // Main Method 
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        // ExecuteClient() Method 
        static void ExecuteClient()
        {
            try
            {
                // Establish the remote endpoint for the socket. This example uses port 11111 on the local computer. 
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

                // Creation TCP/IP Socket using Socket Class Costructor.
                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    // Connect Socket to the remote endpoint using method Connect() 
                    sender.Connect(localEndPoint);

                    // We print EndPoint information that we are connected 
                    Console.WriteLine("Socket connected to -> {0} ", sender.RemoteEndPoint.ToString());

                    // Creation of messagge that we will send to Server 
                    byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>");
                    // int byteSent = sender.Send(messageSent);
                    sender.Send(messageSent);

                    // Data buffer 
                    // Note to self: Length of text.
                    byte[] messageReceived = new byte[1024];

                    // We receive the message using the method Receive(). This method returns number of bytes received, that we'll use to convert them to string 
                    // Note to self: Mutiple receive methods must be placed here to recieve further messages from the server.
                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));

                    // ======================================================<SELF-ESTABLISHED>======================================================
                    while (messageReceived.Length > 0)
                    {
                        Console.WriteLine("Waiting for reply from server...");
                        //"If no data is available for reading, the Receive method will block until data is available."
                        byteRecv = sender.Receive(messageReceived);
                        Console.WriteLine("Message from Server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));

                        string user_input = Console.ReadLine();
                        if (user_input == "q")
                        {
                            break;
                        }
                        byte[]  message = Encoding.ASCII.GetBytes(user_input);
                        sender.Send(message);
                    }
                    // ======================================================<SELF-ESTABLISHED>======================================================

                    // Close Socket using the method Close() 
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

                // Manage of Socket's Exceptions 
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
    }
}
