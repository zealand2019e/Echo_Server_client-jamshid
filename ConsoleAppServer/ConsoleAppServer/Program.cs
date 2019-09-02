using System;
using System.IO;
using System.Net.Sockets;

namespace ConsoleAppServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");
            TcpListener serverSocket = new TcpListener(6789);
            serverSocket.Start();

            Console.WriteLine("server started witing for connection!");

            TcpClient connectionSocket = serverSocket.AcceptTcpClient();
            //Socket connectionSocket = serverSocket.AcceptSocket();
            Console.WriteLine("Server activated");

            Stream ns = connectionSocket.GetStream();
            // Stream ns = new NetworkStream(connectionSocket);

            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            string message = sr.ReadLine();
            string answer = "";
            while (message != null && message != "")
            {
                Console.WriteLine("Client: " + message);
                answer = message.ToUpper();
                sw.WriteLine(answer);
                message = sr.ReadLine();
            }

            ns.Close();
            connectionSocket.Close();
            serverSocket.Stop();
        }
    }
}
