using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerWPF.lib
{
    class AccionsHandler
    {
        public MainWindow window;

        class Request
        {
            public int row { get; set; }
            public int col { get; set; }
        }

        class Response
        {
            public string content { get; set; }
        }

        /// <summary>
        /// Inicialitza una nova instància de la classe AccionsHandler, utilitza
        /// la MainWindow per mostrar les accións del socket
        /// </summary>
        public AccionsHandler(MainWindow window)
        {
            this.window = window;
        }

        /// <summary>
        /// Maneixa la sol·licitud rebuda i retorna una resposta.
        /// </summary>
        /// <param name="json">La sol·licitud en format JSON.</param>
        /// <param name="conn">El client TCP connectat.</param>
        /// <returns>La resposta a la sol·licitud.</returns>
        public async Task<string> handle(string json)
        {
            Request req = parseRequest(json);
            window.add("Row: " + req.row.ToString() + " - Col: " + req.col.ToString());

            bool valid = await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                return window.validar(req.col, req.row);
            });
            //window.show(valid.ToString());

            return generateResponse(valid ? "tocat" : "aigua");
        }

        private string generateResponse(string content)
        {
            var res = new Response()
            {
                content = content,
            };

            return JsonConvert.SerializeObject(res);
        }

        /// <summary>
        /// Analiza una solicitud JSON y la convierte en un objeto de tipo Request.
        /// </summary>
        /// <param name="json">La cadena JSON que representa la solicitud.</param>
        /// <returns>El objeto Request resultante.</returns>
        private Request parseRequest(string json)
        {
            return JsonConvert.DeserializeObject<Request>(json);
        }

    }
}
