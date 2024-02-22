using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace SpotifyWPF.lib
{
    public class Config
    {
        public string ip { get; set; }
        public int port { get; set; }
    }

    public class Client
    {
        public static string END = "--END--";
        private const int BufferSize = 1024;
        public ActionsHandler actions;
        public Config config;

        /// <summary>
        /// Inicialitza una nova instància de la classe Client amb la finestra de llistats especificada.
        /// </summary>
        /// <param name="llistats">La finestra de llistats a utilitzar.</param>
        public Client()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory() + "/../../../", "config.json");
            string jsonContent = File.ReadAllText(filePath);
            config = JsonConvert.DeserializeObject<Config>(jsonContent);
            Console.WriteLine("Config loaded " + config.ip + ":" + config.port);
        }

        /// <summary>
        /// 
        /// Estableix una connexió amb el servidor utilitzant la IP i el port especificats.
        /// </summary>
        /// <param name="ip">La IP del servidor.</param>
        /// <param name="port">El port del servidor.</param>
        public void connect()
        {
            try
            {
                TcpClient client = new TcpClient(config.ip, config.port);

                NetworkStream networkStream = client.GetStream();
                this.actions = new ActionsHandler(networkStream);

                Thread receiveThread = new Thread(() => ReceiveData(networkStream));
                receiveThread.Start();

                // Console.ReadLine(); // Manté l'aplicació de consola en execució
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Rep les dades del servidor a través del NetworkStream.
        /// </summary>
        /// <param name="networkStream">El NetworkStream a utilitzar.</param>
        private void ReceiveData(NetworkStream networkStream)
        {
            try
            {
                StringBuilder receivedData = new StringBuilder();
                byte[] buffer = new byte[BufferSize];
                int bytesRead;

                while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    receivedData.Append(receivedMessage);
                    int index = receivedMessage.IndexOf(Client.END);

                    if (index != -1)
                    {
                        index = receivedData.Length - receivedMessage.Length + index;
                        string json = receivedData.ToString().Substring(0, index);
                        actions.handle(json);
                        receivedData.Clear();
                    };
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en rebre dades: {ex.Message}");
            }
        }
    }
}
