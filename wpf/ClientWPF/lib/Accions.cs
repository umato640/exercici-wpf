using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace SpotifyWPF.lib
{

    public class Request 
    {
        public int col { get; set; }
        public int row { get; set; }
    }

    public class Response
    {
        public string content { get; set; }
    }

    public class ActionsHandler
    {
        private NetworkStream stream;

        /// <summary>
        /// Inicialitza una nova instància de la classe ActionsHandler amb el NetworkStream i LlistatsWindow especificats.
        /// </summary>
        /// <param name="networkStream">El NetworkStream a utilitzar.</param>
        /// <param name="llistats">El LlistatsWindow a utilitzar.</param>
        public ActionsHandler(NetworkStream networkStream)
        {
            this.stream = networkStream;
        }

        /// <summary>
        /// Maneja la cadena JSON entrant i mostra el contingut
        /// </summary>
        /// <param name="json">La cadena JSON a processar.</param>
        public void handle(string json)
        {
            var res = this.parseResponse(json);
            Console.WriteLine(res.content);
        }

        /// <summary>
        /// Genera una sol·licitud, la serialitza a JSON, afegeix un delimitador especial i l'envia a través del NetworkStream.
        /// </summary>
        /// <param name="llista">El valor per a la propietat 'llista' a la sol·licitud.</param>
        public void validar(int col, int row)
        {
            var req = new Request()
            {
                col = col,
                row = row
            };
            var str = JsonConvert.SerializeObject(req) + Client.END;
            this.sendData(this.stream, str);
        }

        /// <summary>
        /// Envia el missatge especificat a través del NetworkStream.
        /// </summary>
        /// <param name="networkStream">El NetworkStream a utilitzar.</param>
        /// <param name="message">El missatge a enviar.</param>
        private void sendData(NetworkStream networkStream, string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                networkStream.Write(data, 0, data.Length);
                //Console.WriteLine($"Enviat: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en enviar dades: {ex.Message}");
            }
        }

        /// <summary>
        /// Analitza la cadena JSON entrant en un objecte Response.
        /// </summary>
        /// <param name="res">La cadena JSON a analitzar.</param>
        /// <returns>Un objecte Response que representa el JSON analitzat.</returns>
        private Response parseResponse(string res)
        {
            return JsonConvert.DeserializeObject<Response>(res);
        }
    }

}
