using System;
using System.Collections.Generic;
using System.Text;

using System.Net;      
using System.Net.Sockets;

namespace OnlineChatClient
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            int port = 8005;
            string ip = "127.0.0.1";
            string message = "a";

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Socket Ready");

            socket.Connect(ipPoint);

            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);

            data = new byte[256]; // буфер для ответа
            StringBuilder builder = new StringBuilder();
            int bytes = 0; // количество полученных байт

            do
            {
                bytes = socket.Receive(data, data.Length, 0);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (socket.Available > 0);
            Console.WriteLine("ответ сервера: " + builder.ToString());

            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            Console.WriteLine("end!");
            string m = Console.ReadLine();
        }
    }
}
