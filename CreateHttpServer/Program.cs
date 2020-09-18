using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CreateHttpServer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            const string NewLine = "\r\n";
            TcpListener listener = new TcpListener(IPAddress.Loopback, 1234);
            listener.Start();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1000000];
                int length = stream.Read(buffer, 0, buffer.Length);
                string requestString = Encoding.UTF8.GetString(buffer, 0, length);


                 Console.WriteLine(requestString);

                string html = $"<h1> Hello from the a other side {DateTime.Now} </h1>"+
                $"<form method=post><input name = username /><input name = password />" +
                $"<input type = submit /></form>";

                string response = "HTTP/1.1 200 Ok" + NewLine +
                                  "Server: CeciServer 2020" + NewLine +
                                  "Content-Type: text/html; charset=utf-8" + NewLine +
                                 //"Content-Disposition: attachment; filename=ceci.txt" + NewLine +
                                  "Content-Length: " + html.Length + NewLine + NewLine + html + NewLine + NewLine;

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes);
                stream.Close();

            }
        }
    }
}
