using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Черкасов
{
    public partial class Form1 : Form
    {
        public string ConnectionString = "provider=Microsoft.Jet.OLEDB.4.0;Data Source=02_02_00_009_Baza_ua.mdb";
        private int act_table = 1;
        public Form1()
        {
            InitializeComponent();

        }
           
       




        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
         string CommandText = "SELECT " +
         "[Перевезення].[Номер], " +
         "[Маршрут].[Номер маршруту], " +
         "[Маршрут].[Пункт призначення], " +
         "[Маршрут].[Час відправлення], " +
         "[Маршрут].[Час прибуття], " +
         "[Білет].[Місце], " +
         "[Білет].[П_І_Б пасажира], " +
         "[Білет].[Вартість], " +
         "[Водій].[П_І_Б] " +
       "FROM " +
         "[Перевезення], " +
         "[Маршрут], " +
         "[Білет], " +
         "[Водій] " +
       "WHERE " +
         "([Перевезення].[ID_Marshrut]=[Маршрут].[ID_Marshrut]) AND " +
         "([Перевезення].[ID_Bilet] = [Білет].[ID_Bilet]) AND " +
         "([Перевезення].[ID_Vodij] = [Водій].[ID_Vodij]) ";

            if (textBox1.Text != "")  // якщо набрано текст у полі фільтру
            {
                if (comboBox1.SelectedIndex == 0) // № перевезення
                    CommandText = CommandText + " AND ([Перевезення].[Номер] = '" + textBox1.Text + "')";
                if (comboBox1.SelectedIndex == 1) // № маршруту
                    CommandText = CommandText + " AND (Маршрут.[Номер маршруту] = '" + textBox1.Text + "') ";
                if (comboBox1.SelectedIndex == 2) // Пункт призначення
                    CommandText = CommandText + " AND (Маршрут.[Пункт призначення] LIKE '" + textBox1.Text + "%') ";
                if (comboBox1.SelectedIndex == 3) // Пасажир
                    CommandText = CommandText + " AND (Білет.[П_І_Б пасажира] LIKE '" + textBox1.Text + "%') ";
                if (comboBox1.SelectedIndex == 4) // Водій
                    CommandText = CommandText + " AND (Водій.П_І_Б LIKE '" + textBox1.Text + "%') ";
            }

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "[Перевезення]");
            dataGridView1.DataSource = ds.Tables["[Перевезення]"].DefaultView;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            button1_Click(sender, e);
            Get_Bilets();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string CommandText;
            string num_per, ID_M, ID_B, ID_D, ID_A, ID_V;
            int row;

            Form2 f = new Form2(); // створили нову форму
            if (f.ShowDialog() == DialogResult.OK)
            {
                // додаємо дані
                // Номер перевезення
                if (f.textBox1.Text == "") num_per = "0";
                else num_per = f.textBox1.Text;

                // Додаємо ID_Marshrut
                row = f.dataGridView1.CurrentCell.RowIndex; // взяли рядок з dataGridView1
                ID_M = Convert.ToString(f.dataGridView1[0, row].Value);
                // Додаємо ID_Bilet
                row = f.dataGridView2.CurrentCell.RowIndex; // взяли рядок з dataGridView2
                ID_B = Convert.ToString(f.dataGridView2[0, row].Value);
                // Додаємо ID_Dispetcher
                row = f.dataGridView3.CurrentCell.RowIndex; // взяли рядок з dataGridView3
                ID_D = Convert.ToString(f.dataGridView3[0, row].Value);

                // Додаємо ID_Avtobus
                row = f.dataGridView4.CurrentCell.RowIndex; // взяли рядок з dataGridView4
                ID_A = Convert.ToString(f.dataGridView4[0, row].Value);
                // Додаємо ID_Vodij
                row = f.dataGridView5.CurrentCell.RowIndex; // взяли рядок з dataGridView5
                ID_V = Convert.ToString(f.dataGridView5[0, row].Value);
                // формуємо CommandText
                CommandText = "INSERT INTO [Перевезення] (Номер, ID_Marshrut, ID_Bilet, ID_Dispetcher, ID_Avtobus, ID_Vodij) " + "VALUES (" + num_per + ", " + ID_M + ", " + ID_B + ", " + ID_D + ", " + ID_A + ", " + ID_V + ")";

                // виконуємо SQL-команду
                My_Execute_Non_Query(CommandText);
                // перемалювати dataGridView1
                button1_Click(sender, e);
             }   
        }
        public void My_Execute_Non_Query(string CommandText)
        {
            OleDbConnection conn = new OleDbConnection(ConnectionString);
            conn.Open();
            OleDbCommand myCommand = conn.CreateCommand();
            myCommand.CommandText = CommandText;
            myCommand.ExecuteNonQuery();
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            if (f.ShowDialog() == DialogResult.OK)
            {
                int index, index_old;
                string ID;
                string CommandText = "DELETE FROM ";
                index = dataGridView1.CurrentRow.Index; // № по порядку в таблиці представлення
                index_old = index;
                ID = Convert.ToString(dataGridView1[0, index].Value); // ID подаємо в запит як рядок

                // Формуємо рядок CommandText
                CommandText = "DELETE FROM [Перевезення] WHERE [Перевезення].[Номер] = '" + ID + "'";

                // виконуємо SQL-запит
                My_Execute_Non_Query(CommandText);

                // перемальовування dbGridView1
                button1_Click(sender, e);
                if (index_old >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1[0, index_old].Selected = true;
                }
            }
        }
        private void Get_Bilets()  // читає усі поля з таблиці "Білет"
        {
            string CommandText = "SELECT ID_Bilet, Місце, Вартість, [Час купівлі], [П_І_Б пасажира], Паспорт, Пільги FROM [Білет]";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);

            // створюємо об'єкт DataSet
            DataSet ds = new DataSet();
            // заповнюємо dataGridView1 даними із таблиці "Білет" бази даних
            dataAdapter.Fill(ds, "[Білет]");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false; // Ховаємо поле ID_Bilets
        }

        // Клік на кнопці "Білети" з групи "Перегляд"
        private void button4_Click(object sender, EventArgs e)
        {
            Get_Bilets();
            act_table = 1;
        }
        private void Get_Marshruts()  // читає усі поля з таблиці "Маршрут"
        {
            string CommandText = "SELECT * FROM [Маршрут]";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();  // створюємо об'єкт DataSet
            dataAdapter.Fill(ds, "[Маршрут]");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Get_Marshruts();
            act_table = 2;
        }
        private void Get_Avtobus()  // читає усі поля з таблиці "Автобус"
        {
            string CommandText = "SELECT * FROM Автобус";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();  // створюємо об'єкт DataSet
            dataAdapter.Fill(ds, "Автобус"); // заповнюємо набір даних даними з таблиці "Автобус"
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false; // сховати нульовий стовпець (поле ID_Avtobus)
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Get_Avtobus();
            act_table = 3;
        }

        private void Get_Vodij()  // читає усі поля з таблиці "Водій"
        {
            string CommandText = "SELECT * FROM Водій";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Водій");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Get_Vodij();
            act_table = 4;
        }
        // заповнює dataGridView2 даними з таблиці "Диспетчер"
        private void Get_Dispetcher()
        {
            string CommandText = "SELECT * FROM [Диспетчер]";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(CommandText, ConnectionString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "Диспетчер");
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            dataGridView2.Columns[0].Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Get_Dispetcher();
            act_table = 5;
        }
        // додавання Білету через Connection і запит ExecuteNonQuery()
        private void Add_Bilet(string misce, string vartist, DateTime chas, string name, string passport, bool pilga)
        {
            string CommandText;
            string s_chas, s_pilga;
            string s_v;

            s_chas = Convert.ToString(chas);
            s_pilga = Convert.ToString(pilga);
            s_v = vartist.Replace(',', '.');

            CommandText = "INSERT INTO [Білет] (Місце, Вартість, [Час купівлі], [П_І_Б пасажира], Паспорт, Пільги) " + "VALUES ('" + misce + "', " + s_v + ", '" + s_chas + "', '"
          + name + "', '" + passport + "', " + pilga + ")";

            My_Execute_Non_Query(CommandText);
        }
        private void Add_Marshrut(string num_marsh, string punkt, string rajon, string oblast, double vidstan, double vaha, DateTime chas_vidpr, DateTime chas_prub)
        {
            string CommandText;
            string s_vidpr, s_prub;
            string s_vaha, s_vidst;

            s_vidpr = Convert.ToString(chas_vidpr);
            s_prub = Convert.ToString(chas_prub);
            s_vaha = Convert.ToString(vaha);
            s_vaha = s_vaha.Replace(',', '.');
            s_vidst = Convert.ToString(vidstan);
            s_vidst = s_vidst.Replace(',', '.');

            CommandText = "INSERT INTO [Маршрут] ([Номер маршруту], [Пункт призначення], Район, Область, Відстань, Вага, [Час відправлення], [Час прибуття])"
                                                      + " VALUES ('" + num_marsh + "', '" + punkt + "', '" + rajon + "', '" + oblast + "', "
                                                      + s_vidst + ", " + s_vaha + ", '" + s_vidpr + "', '" + s_prub + "')";

            My_Execute_Non_Query(CommandText);
        }
        void Add_Avtobus(string num, string model, string znak, string k_misc)
        {
            string CommandText;
            CommandText = "INSERT INTO [Автобус] ([Номер], [Модель], [Номерний знак], [Кількість місць])"
                                                      + " VALUES ('" + num + "', '" + model + "', '" + znak + "', " + k_misc + ")";
            My_Execute_Non_Query(CommandText);
        }
        void Add_Vodij(string p_i_b, string d_nar, string passport) // додати водія
        {
            string CommandText;
            CommandText = "INSERT INTO [Водій] ([П_І_Б], [Дата народження], [Паспорт])"
                                                      + " VALUES ('" + p_i_b + "', '" + d_nar + "', '" + passport + "')";
            My_Execute_Non_Query(CommandText);
        }
        void Add_Dispetcher(string p_i_b, string d_nar, string adresa) // додати водія
        {
            string CommandText;
            CommandText = "INSERT INTO [Диспетчер] ([П_І_Б], [Дата народження], [Адреса])"
                                                      + " VALUES ('" + p_i_b + "', '" + d_nar + "', '" + adresa + "')";
            My_Execute_Non_Query(CommandText);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (act_table == 1) // обробляємо таблицю білети
            {
                Form4 f = new Form4();

                if (f.ShowDialog() == DialogResult.OK)
                {
                    // додаємо дані в таблицю "Білети"
                    Add_Bilet(f.textBox1.Text, f.textBox2.Text, Convert.ToDateTime(f.textBox3.Text),
                              f.textBox4.Text, f.textBox5.Text, f.checkBox1.Checked);
                    Get_Bilets();
                }
            }
            else
  if (act_table == 2) // обробляємо таблицю "Маршрут"
            {
                Form5 f = new Form5();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // додаємо дані в таблицю "Маршрут"
                    Add_Marshrut(f.textBox1.Text, f.textBox2.Text, f.textBox3.Text, f.textBox4.Text,
                                Convert.ToDouble(f.textBox5.Text), Convert.ToDouble(f.textBox6.Text),
                                 f.dateTimePicker1.Value, f.dateTimePicker2.Value);
                    Get_Marshruts();
                }
            }
            else
  if (act_table == 3) // обробляємо таблицю "Автобус"
            {
                Form6 f = new Form6();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // додаємо дані в таблицю "Автобус"
                    Add_Avtobus(f.textBox1.Text, f.textBox2.Text, f.textBox3.Text, f.textBox4.Text);
                    Get_Avtobus();
                }
            }
            else
  if (act_table == 4) // обробляємо таблицю "Водій"
            {
                Form7 f = new Form7();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // додаємо дані в таблицю "Водій"
                    Add_Vodij(f.textBox1.Text, Convert.ToString(f.dateTimePicker1.Value), f.textBox2.Text);
                    Get_Vodij();
                }
            }
            else
  if (act_table == 5) // обробляємо таблицю "Диспетчер"
            {
                Form8 f = new Form8();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // додаємо дані в таблицю "Водій"
                    Add_Dispetcher(f.textBox1.Text, Convert.ToString(f.dateTimePicker1.Value), f.textBox2.Text);
                    Get_Dispetcher();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();

            if (f.ShowDialog() == DialogResult.OK)
            {
                int index, index_old;
                string ID;
                string CommandText = "DELETE FROM ";

                index = dataGridView2.CurrentRow.Index; // № по порядку в таблиці представлення
                index_old = index;
                ID = Convert.ToString(dataGridView2[0, index].Value); // ID подаємо в запит як рядок

                // Формуємо рядок CommandText
                if (act_table == 1) // обробляємо таблицю "Білет"
                    CommandText = "DELETE FROM Білет WHERE Білет.ID_Bilet = " + ID;
                if (act_table == 2) // обробляємо таблицю "Маршрут"
                    CommandText = "DELETE FROM Маршрут WHERE Маршрут.ID_Marshrut = " + ID;
                if (act_table == 3) // обробляємо таблицю "Автобус"
                    CommandText = "DELETE FROM Автобус WHERE Автобус.ID_Avtobus = " + ID;
                if (act_table == 4) // обробляємо таблицю "Водій"
                    CommandText = "DELETE FROM Водій WHERE Водій.ID_Vodij = " + ID;
                if (act_table == 5) // обробляємо таблицю "Диспетчер"
                    CommandText = "DELETE FROM Диспетчер WHERE Диспетчер.ID_Dispetcher = " + ID;

                // виконуємо SQL-запит
                My_Execute_Non_Query(CommandText);

                // перемальовування dbGridView2
                if (act_table == 1) Get_Bilets();
                else
                if (act_table == 2) Get_Marshruts();
                else
                if (act_table == 3) Get_Avtobus();
                else
                if (act_table == 4) Get_Vodij();
                else
                if (act_table == 5) Get_Dispetcher();

                if (index_old >= 0)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2[0, index_old].Selected = true;
                }
            }
        }
    }

}
