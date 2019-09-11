using System;
using System.IO;
using System.Net.Sockets;

namespace MathDateClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World it is client!");
            Console.ReadLine();
            TcpClient clientSocket = new TcpClient("localhost", 3001);

            Stream ns = clientSocket.GetStream();  //provides a Stream
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            string message = Console.ReadLine();
            while (!string.IsNullOrEmpty(message))
            {
                sw.WriteLine(message);
                string serverAnswer = sr.ReadLine();
                while (serverAnswer != "_")
                {
                    Console.WriteLine("Server: " + serverAnswer);
                    serverAnswer = sr.ReadLine();
                    
                }
                
                message = Console.ReadLine();
            }
            

            ns.Close();

            clientSocket.Close();
        }
    }
}
