using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerWPF.lib
{
    class Server
    {
        public TcpListener server;
        private AccionsHandler accions;
        public static string END = "--END--";

        /// <summary>
        /// Inicialitza una nova instància de la classe Server amb la IP i el port especificats.
        /// </summary>
        /// <param name="ip">La IP en què el servidor escoltarà les connexions.</param>
        /// <param name="port">El port en què el servidor escoltarà les connexions.</param>
        public Server(MainWindow window)
        {
            accions = new AccionsHandler(window);
        }

        /// <summary>
        /// Inicia el servidor i espera connexions.
        /// </summary>
        public async Task start(string ip, int port)
        {
            server = null;

            try
            {
                IPAddress ipAddr = IPAddress.Parse(ip);
                server = new TcpListener(ipAddr, port);
                this.accions.window.add("Server started!");
                server.Start();

                while (true)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client));
                }
            }
            catch (Exception e)
            {
                // accions.window.show(e.Message);
            }
            finally
            {
                server?.Stop();
            }
        }

        /// <summary>
        /// Gestiona la comunicació amb un client mitjançant un stream de xarxa.
        /// </summary>
        /// <param name="client">El client que s'ha connectat.</param>
        async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            this.accions.window.add("Client connected!");
            try
            {
                byte[] buffer = new byte[1024];
                StringBuilder receivedData = new StringBuilder();

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    receivedData.Append(data);
                    string rd = receivedData.ToString();
                    int index = rd.IndexOf(END);

                    if (index != -1)
                    {
                        string tmp = data.Substring(index + END.Length, rd.Length - index - END.Length);
                        receivedData.Clear();
                        if (tmp.Length > 0) receivedData.Append(tmp);

                        string json = rd.Substring(0, index);
                        string response = await accions.handle(json);

                        if (response != null)
                        {
                            byte[] reply = Encoding.UTF8.GetBytes(response + END);
                            await stream.WriteAsync(reply, 0, reply.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }

        public void stop() {
            //this.server.Server.Close();
            this.server.Stop();
            this.accions.window.add("Server closed!");
        }
    }
}
