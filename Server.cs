// A C# Program for Server 
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Server
    {
        // Main Method 
        static void Main(string[] args)
        {
            ExecuteServer();
        }
        public static void ExecuteServer()
        {
            // Establish the local endpoint for the socket. Dns.GetHostName returns the name of the host running the application. 
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            // Creation TCP/IP Socket using  
            // Socket Class Costructor 
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // Using Bind() method we associate a 
                // network address to the Server Socket 
                // All client that will connect to this  
                // Server Socket must know this network 
                // Address 
                listener.Bind(localEndPoint);

                // Using Listen() method we create  
                // the Client list that will want 
                // to connect to Server 
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");

                    // Suspend while waiting for 
                    // incoming connection Using  
                    // Accept() method the server  
                    // will accept connection of client 
                    Socket clientSocket = listener.Accept();

                    // Data buffer 
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text received -> {0} ", data);

                    byte[] message = Encoding.ASCII.GetBytes("Test Server");

                    // Send a message to Client using Send() method 
                    clientSocket.Send(message);

                    // ======================================================<SELF-ESTABLISHED>======================================================
                    while (clientSocket.Connected)
                    {
                        string user_input = Console.ReadLine();
                        if (user_input == "q")
                        {
                            break;
                        }
                        message = Encoding.ASCII.GetBytes(user_input);
                        clientSocket.Send(message);
                        //"If no data is available for reading, the Receive method will block until data is available."
                        byte[] messageReceived = new byte[1024];
                        if (messageReceived.Length > 0)
                        {
                            Console.WriteLine("Waiting for reply from client...");
                            int byteRecv = clientSocket.Receive(messageReceived);
                            Console.WriteLine("Message from client -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));
                        }
                    }
                    Console.WriteLine("Ending Connection");
                    // ======================================================<SELF-ESTABLISHED>======================================================

                    // Close client Socket using the Close() method. After closing, we can use the closed Socket for a new Client Connection 
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
