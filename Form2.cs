using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Черкасов
{
    public partial class Form2 : Form
    {
        private void Get_Table(string table_name, int num_dG)
        {
            Form1 f1 = new Form1();
            string CommandText = "SELECT * FROM ";
            CommandText += table_name;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, f1.ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, table_name);
            if (num_dG == 1) dataGridView1.DataSource = ds.Tables[table_name].DefaultView;
            if (num_dG == 2) dataGridView2.DataSource = ds.Tables[table_name].DefaultView;
            if (num_dG == 3) dataGridView3.DataSource = ds.Tables[table_name].DefaultView;
            if (num_dG == 4) dataGridView4.DataSource = ds.Tables[table_name].DefaultView;
            if (num_dG == 5) dataGridView5.DataSource = ds.Tables[table_name].DefaultView;
        }
        public Form2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Get_Table("Маршрут", 1); // заповнюємо таблицю "Маршрут"
            Get_Table("Білет", 2); // заповнюємо таблицю "Білет"
            Get_Table("Водій", 3);
            Get_Table("Диспетчер", 5);
            Get_Table("Автобус", 4);
            textBox1.Text = "0";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int row;
            // Беремо дані з комірок таблиці "Маршрут"
            row = dataGridView1.CurrentCell.RowIndex;
            label2.Text = "Маршрут - " + "№" + Convert.ToString(dataGridView1[1, row].Value) +
                "/" + Convert.ToString(dataGridView1[2, row].Value) +
                "/" + Convert.ToString(dataGridView1[3, row].Value) +
                "/" + Convert.ToString(dataGridView1[4, row].Value) +
                "/" + Convert.ToString(dataGridView1[7, row].Value) +
               " - " + Convert.ToString(dataGridView1[8, row].Value);
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int row;
            // так само беремо дані з таблиці "Білет"
            row = dataGridView2.CurrentCell.RowIndex;
            label3.Text = "Білет: місце №" + Convert.ToString(dataGridView2[1, row].Value) +
              " / " + Convert.ToString(dataGridView2[2, row].Value) +
              " / " + Convert.ToString(dataGridView2[3, row].Value) +
              " / " + Convert.ToString(dataGridView2[4, row].Value) +
              " / " + Convert.ToString(dataGridView2[5, row].Value) +
              " / " + Convert.ToString(dataGridView2[6, row].Value);
        }

        private void dataGridView3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int row;
            // Дані про водія
            row = dataGridView3.CurrentCell.RowIndex;
            label4.Text = "Водій: " + Convert.ToString(dataGridView3[1, row].Value);
        }

        private void dataGridView4_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int row;
            // Інформація про автобус
            row = dataGridView4.CurrentCell.RowIndex;
            label5.Text = "Автобус: №" + Convert.ToString(dataGridView4[1, row].Value) +
              " / " + Convert.ToString(dataGridView4[2, row].Value) +
              " / " + Convert.ToString(dataGridView4[3, row].Value) +
              " / " + Convert.ToString(dataGridView4[4, row].Value);
        }

        private void dataGridView5_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int row;
            // Інформація про диспетчера
            row = dataGridView5.CurrentCell.RowIndex;
            label6.Text = "Диспетчер: " + Convert.ToString(dataGridView5[1, row].Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Фільтр в таблиці "Маршрут"
            string CommandText = "SELECT * FROM [Маршрут]";
            // формуємо змінну CommandText
            if (textBox2.Text == "")
                CommandText = "SELECT * FROM [Маршрут]";
            else
            if (comboBox1.SelectedIndex == 0) // № перевезення
                CommandText = "SELECT * FROM [Маршрут] WHERE [Номер маршруту] = '" + textBox2.Text + "'"; // працює
            else
            if (comboBox1.SelectedIndex == 1) //
                CommandText = "SELECT * FROM [Маршрут] WHERE [Пункт призначення] LIKE '" + textBox2.Text + "%'";
            else
            if (comboBox1.SelectedIndex == 2) // Район
                CommandText = "SELECT * FROM [Маршрут] WHERE Район LIKE '" + textBox2.Text + "%'";
            else
            if (comboBox1.SelectedIndex == 3) // Область
                CommandText = "SELECT * FROM [Маршрут] WHERE Область LIKE '" + textBox2.Text + "%'";

            Form1 f = new Form1();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, f.ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "[Маршрут]");
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Фільтр в таблиці "Білет"
            string CommandText = "SELECT * FROM [Білет]";

            // формуємо змінну CommandText
            if (textBox3.Text == "")
                CommandText = "SELECT * FROM [Білет]";
            else
            if (comboBox2.SelectedIndex == 0) // № перевезення
                CommandText = "SELECT * FROM [Білет] WHERE [Місце] = " + textBox3.Text; // працює
            else
            if (comboBox2.SelectedIndex == 1) //
                CommandText = "SELECT * FROM [Білет] WHERE [П_І_Б пасажира] LIKE '" + textBox3.Text + "%'";

            Form1 f = new Form1();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, f.ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "[Маршрут]");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
        }
    }
}
