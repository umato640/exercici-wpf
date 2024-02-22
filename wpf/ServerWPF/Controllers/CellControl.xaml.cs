using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerWPF.Controllers
{
    /// <summary>
    /// Lógica de interacción para CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {

        public Casella casella;

        public CellControl(Casella casella)
        {
            InitializeComponent();
            this.casella = casella;
            this.update();
        }

        public void update() {
            this.border.Background = casella.color;
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            casella.valor = !casella.valor;
            casella.color = casella.valor ? Brushes.Green : Brushes.Gray;
            this.update();
        }
    }
}
