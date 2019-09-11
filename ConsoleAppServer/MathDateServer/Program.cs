using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MathDateServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 6789;
            int clientNr = 0;

            Console.WriteLine("Hello Echo Server!");

            IPAddress ip = GetIp();
            TcpListener ServerListener = StartServer(ip, port);

            do
            {
                TcpClient ClientConnection = GetConnectionSocket(ServerListener, ref clientNr);
                //ReadWriteStream(ClientConnection);
                Task.Run(() => ReadWriteStream(ClientConnection, ref clientNr));

            } while (clientNr != 0);

            StopServer(ServerListener);
        }

        private static void StopServer(TcpListener serverListener)
        {
            serverListener.Stop();
            Console.WriteLine("listener stopped");
        }

        private static TcpClient GetConnectionSocket(TcpListener serverListener, ref int clientNr)
        {

            TcpClient connectionSocket = serverListener.AcceptTcpClient();
            clientNr++;
            //Socket connectionSocket = serverSocket.AcceptSocket();
            Console.WriteLine("Client " + clientNr + " connected");
            return connectionSocket;
        }

        private static void ReadWriteStream(TcpClient connectionSocket, ref int clientNr)
        {
            Stream ns = connectionSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            string message = sr.ReadLine();
            Thread.Sleep(1000);
            string answer = "";
            while (message != null && message != "")
            {
                Console.WriteLine("Client: " + clientNr + " " + message);
                string[] messageArray = message.Split(' ');
                for (int i = 0; i<messageArray.Length; i++)
                {
                    answer = messageArray[i].ToUpper();
                    sw.WriteLine(answer);
                    Thread.Sleep(1000);
                }
                sw.WriteLine("_");
                message = sr.ReadLine();
                //Thread.Sleep(5000);
            }

            Console.WriteLine("Empty message detected");
            ns.Close();
            connectionSocket.Close();
            clientNr--;
            Console.WriteLine("connection socket " + clientNr + " closed");

        }

        private static TcpListener StartServer(IPAddress ip, int port)
        {
            TcpListener serverSocket = new TcpListener(ip, port);
            serverSocket.Start();

            Console.WriteLine("server started waiting for connection!");
            Console.WriteLine("Ip: " + ip);
            Console.WriteLine("Port: " + port);

            return serverSocket;
        }

        private static IPAddress GetIp()
        {
            string name = "google.com";
            IPAddress[] addrs = Dns.GetHostEntry(name).AddressList;
            Console.WriteLine("Google IP returned by GetHostEntry" + addrs[0]);

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Console.WriteLine("Local host IP:" + ip);
            return ip;
        }

    }
}

