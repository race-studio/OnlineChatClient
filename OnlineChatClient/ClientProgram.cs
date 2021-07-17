using System;
using System.Collections.Generic;
using System.Text;

using System.Net;      
using System.Net.Sockets;
using System.Threading;

namespace OnlineChatClient
{
    class ClientProgram
    {
        const int port = 8005;
        const string ip = "127.0.0.1";
        private static ClientTreading listenThreadObj;

        static void Main(string[] args)
        {
            
           
            Console.Write("Введите свое имя:");
            string userName = Console.ReadLine();
            TcpClient client = null;

            client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();

            Console.Write(userName + ": ");


            listenThreadObj = new ClientTreading( stream );

            Thread listenThread = new Thread(new ThreadStart(listenThreadObj.ListenForMessages) );
            listenThread.Start();

            while (true)
            {
                // ввод сообщения
                string message = Console.ReadLine();

                if (message.Equals( "exit" ) )
                {
                    System.Environment.Exit(0);
                    break;
                }

                message = String.Format("{0}: {1}", userName, message);
                // преобразуем сообщение в массив байтов
                byte[] data = Encoding.Unicode.GetBytes(message);
                // отправка сообщения
                stream.Write(data, 0, data.Length);
            }           
        }
    }

    class ClientTreading
    {
        public NetworkStream stream;
        public ClientTreading(NetworkStream clientStream)
        {
            this.stream = clientStream;
        }
        public void ListenForMessages()
        {
            while ( true )
            {
                Thread.Sleep(60);

                if (stream.CanRead == false )
                {
                    break;
                }

                byte[] data = new byte[64]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт
                do
                {
                    if (stream.CanRead == true && stream != null)
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                }
                while (stream.DataAvailable);

                string message = builder.ToString();
                Console.WriteLine("Сервер: {0}", message);
            }
        }
    }
}
