using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAWCSanPedroHestia.NewForm
{
    public partial class DataDashboardDesign: UserControl
    {
        public DataDashboardDesign()
        {
            InitializeComponent();
            SetupTableLayout();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SetupTableLayout()
        {
            // Clear any existing controls
            tableLayoutPanel1.Controls.Clear();

            // Configure table layout
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel1.BackColor = Color.Black; // For the grid lines

            // Define headers
            string[] headers = {
        "NATURE OF CASE", "Total No. of Victims", "CSWDO", "PNP", "Court",
        "Hospital", "OTHERS (NBI/PAO),", "Total No.BPO's Issued", ""
    };

            // Add header row
            for (int col = 0; col < headers.Length; col++)
            {
                Label header = new Label
                {
                    Text = headers[col],
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    BackColor = Color.LightGray,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(0)
                };
                tableLayoutPanel1.Controls.Add(header, col, 0);
            }

            // Define rows (exactly matching your image)
            string[,] rows = new string[,]
            {
        { "RA 9262", "", "", "", "", "", "", "" },
        { "Physical", "", "", "", "", "", "", ""},
        { "Sexual", "", "", "", "", "", "", ""},
        { "Psychological", "", "", "", "", "", "", "",},
        { "Economic", "", "", "", "", "", "", ""},
        { "RA 8353", "", "", "", "", "", "", "",},
        { "RA 9208/10364", "", "", "", "", "", "", ""},
        { "RA 7877", "", "", "", "", "", "", "" }
            };

            // Add data rows
            for (int row = 0; row < rows.GetLength(0); row++)
            {
                for (int col = 0; col < rows.GetLength(1); col++)
                {
                    Label cell = new Label
                    {
                        Text = rows[row, col],
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        BackColor = Color.White,
                        Margin = new Padding(0)
                    };

                    // Left-align the first column
                    if (col == 0)
                    {
                        cell.TextAlign = ContentAlignment.MiddleLeft;
                        cell.Padding = new Padding(10, 0, 0, 0);
                    }

                    tableLayoutPanel1.Controls.Add(cell, col, row + 1);
                }
            }

            // Adjust column styles if needed
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < headers.Length; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }
        }

    }
}
