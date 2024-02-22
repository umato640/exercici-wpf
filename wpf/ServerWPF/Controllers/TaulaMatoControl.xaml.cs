using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ServerWPF.Controllers
{
    // Represents a single cell in the table/grid
    public class Casella : INotifyPropertyChanged
    {
        public bool valor { get; set; } = false; // Represents the value of the cell
        public Brush _color { get; set; } = Brushes.Gray;

        public Brush color
        {
            set
            {
                _color = value;
                OnPropertyChanged(nameof(color));
            }
            get { return _color; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class TaulaMatoControl : UserControl
    {
        public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register("ColumnsProperty", typeof(int), typeof(TaulaMatoControl), new PropertyMetadata(null));

        public int ColumnsProprety
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty RowsProperty =
        DependencyProperty.Register("RowsProperty", typeof(int), typeof(TaulaMatoControl), new PropertyMetadata(null));

        private int rows;
        private int cols;

        public int RowsProprety
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        private CellControl[,] caselles; // 2D array to hold TextBox controls

        public TaulaMatoControl()
        {
            InitializeComponent();
        }

        public void build() {
            InitTable(RowsProprety, ColumnsProprety); // Example: Initialize table with 3 rows and 3 columns
        }

        public void InitTable(int Rows, int Cols)
        {
            this.rows = Rows;
            this.cols = Cols;
            // Clear any existing definition or children
            mainGrid.ColumnDefinitions.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.Children.Clear();
            caselles = new CellControl[Cols, Rows];

            // Define columns 
            for (int c = 0; c < Cols; c++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Define rows
            for (int r = 0; r < Rows; r++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Draw grid 
            for (int c = 0; c < Cols; c++)
            {
                for (int r = 0; r < Rows; r++)
                {

                    Casella casella = new Casella();
                    CellControl m;
                    m = new CellControl(casella);
                    caselles[c, r] = m;

                    Grid.SetColumn(m, c);
                    Grid.SetRow(m, r);
                    mainGrid.Children.Add(m);

                }
            }

            Random random = new Random();
            int randomRow = random.Next(0, Rows);
            int randomCol = random.Next(0, Cols);
            var _casella = caselles[randomCol, randomRow];
            _casella.casella.valor = true;
            _casella.casella.color = Brushes.Green;
            _casella.update();
        }

        public bool check(int col, int row) {
            //MessageBox.Show(caselles.Length.ToString());
            if (rows > row && cols > col) {
                CellControl cell = caselles[col, row];
                if (cell.casella.valor)
                {
                    cell.casella.color = Brushes.Red;
                    cell.update();
                    return true;
                }
                else
                {
                    cell.casella.color = Brushes.Yellow;
                    cell.update();
                    return false;
                }
            }
            return false;
        }

    }

}
