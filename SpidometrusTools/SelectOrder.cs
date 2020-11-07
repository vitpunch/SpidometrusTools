using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SpidometrusTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ordersTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ordersTable.AllowUserToAddRows = false;
            var data = new PrepareDateForSelectOrderTable();

            ordersTable.DataSource = data.SelectDataFromDB();
            //ordersTable.Columns[0].Visible = false; //скрываем колонку с id заказа
        }

        private void ordersTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            label2.Text = ordersTable.SelectedRows[0].Cells[0].Value.ToString();
        }
    }
}
