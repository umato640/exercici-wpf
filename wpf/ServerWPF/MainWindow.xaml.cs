using Newtonsoft.Json;
using ServerWPF.Controllers;
using ServerWPF.lib;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerWPF
{
    /// <summary>
    /// Classe de configuració per al servidor.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Obté o estableix la adreça IP del servidor.
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// Obté o estableix el port del servidor.
        /// </summary>
        public int port { get; set; }
    }

    /// <summary>
    /// Classe per a la lògica de la interacció de la finestra principal.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Classe per a missatges de text.
        /// </summary>
        class TextMessage
        {
            /// <summary>
            /// Obté o estableix el text del missatge.
            /// </summary>
            public string text { get; set; }
        }

        private ObservableCollection<TextMessage> messages;
        private Server socket;
        private Config config;
        private readonly string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory() + "/../../../", "config.json");

        /// <summary>
        /// Constructor de la finestra principal.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            string jsonContent = File.ReadAllText(filePath);
            config = JsonConvert.DeserializeObject<Config>(jsonContent);
            this.messages = new ObservableCollection<TextMessage>();
            this.listMsg.ItemsSource = messages;

            taulell.build();

            this.socket = new Server(this);
            this.socket.start(config.ip, config.port);
        }

        /// <summary>
        /// Valida si la columna i la fila són vàlides.
        /// </summary>
        /// <param name="col">La columna a validar.</param>
        /// <param name="row">La fila a validar.</param>
        /// <returns>True si la columna i la fila són vàlides; fals en cas contrari.</returns>
        public bool validar(int col, int row)
        {
            return taulell.check(col, row);
        }

        /// <summary>
        /// Afegeix un missatge a la col·lecció de missatges.
        /// </summary>
        /// <param name="text">El text del missatge a afegir.</param>
        public void add(String text)
        {
            Application.Current.Dispatcher.Invoke(() => {
                this.messages.Add(new TextMessage() { text = text });
            });
        }

        /// <summary>
        /// Gestiona l'esdeveniment del botó d'aturada.
        /// </summary>
        /// <param name="sender">L'objecte que ha generat l'esdeveniment.</param>
        /// <param name="e">Les dades de l'esdeveniment.</param>
        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            socket.stop();
            MessageBox.Show("Stopped!");
        }

        /// <summary>
        /// Gestiona l'esdeveniment del botó d'inici.
        /// </summary>
        /// <param name="sender">L'objecte que ha generat l'esdeveniment.</param>
        /// <param name="e">Les dades de l'esdeveniment.</param>
        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            string address = AddressElement.Text.Trim();
            string portText = PortElement.Text.Trim();

            if (!int.TryParse(portText, out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("Si us plau, introduïu un port vàlid.");
                return;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Si us plau, introduïu una adreça vàlida.");
                return;
            }

            socket.stop();
            socket.start(address, port);
            MessageBox.Show("Iniciat!");
        }

        /// <summary>
        /// Gestiona l'esdeveniment del botó d'actualització.
        /// </summary>
        /// <param name="sender">L'objecte que ha generat l'esdeveniment.</param>
        /// <param name="e">Les dades de l'esdeveniment.</param>
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            string rowsText = RowsElement.Text.Trim();
            string colsText = ColsElement.Text.Trim();

            if (!int.TryParse(rowsText, out int rows) || rows <= 0)
            {
                MessageBox.Show("Si us plau, introduïu un número vàlid de files.");
                return;
            }

            if (!int.TryParse(colsText, out int cols) || cols <= 0)
            {
                MessageBox.Show("Si us plau, introduïu un número vàlid de columnes.");
                return;
            }

            taulell.InitTable(rows, cols);
        }
    }
}