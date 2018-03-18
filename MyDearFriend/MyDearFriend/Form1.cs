using MyDearFriend.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDearFriend
{
    public partial class Form1 : Form
    {
        public Form1() {

            InitializeComponent();

            listViewAll.Columns.Add("Słowo", 230);
            listViewAll.Columns.Add("Tłumaczenie", 230);

            listViewAll.View = View.Details;
            listViewAll.FullRowSelect = true;
        }

        public bool selected = false;
        public int pushed = 1;

        public void PopulateLV(String id, String Word, String Translate)
        {
            String[] row = {id, Word, Translate};

            listViewAll.Items.Add(new ListViewItem(row));
        }

        static string myconnstring = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        //Insert data into DB
        private void Add(String Word, String Translate)
        {
            
            String sql = "INSERT INTO WTK_table(Word,Translate) VALUES(@WORD,@TRANSLATE)";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            
            cmd.Parameters.AddWithValue("@WORD", Word);   
            cmd.Parameters.AddWithValue("@TRANSLATE", Translate);

            try {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0) {
                    MessageBox.Show("Dodano!");
                }
                conn.Close();

                Retrieve();
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        //Selecting data from DB
        private void Retrieve()
        {
            listViewAll.Items.Clear();

            String sql = "SELECT * FROM WTK_table";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try {
                conn.Open();

                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows) {
                    PopulateLV(row[1].ToString(), row[2].ToString(), row[0].ToString());
                }

                conn.Close();
                dt.Rows.Clear();
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        //Update data in DB
        private void Update(int id, String newWord, String newTranslate)
        {
            string sql = "UPDATE WTK_table SET Word='" + newWord + "',Translate='" + newTranslate + "' WHERE id=" + id + "";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try {
                conn.Open();
                adapter.UpdateCommand = conn.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0) {
                    textBoxWordUp.Text = "";
                    textBoxTranslateUp.Text = "";
                    MessageBox.Show("Edytowano!");
                }

                conn.Close();

                //Refresh
                Retrieve();
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }

        //Delete data from DB
        private void Delete(int id)
        {

            String sql = "DELETE FROM WTK_table WHERE id=" + id + "";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            try {
                conn.Open();
                adapter.DeleteCommand = conn.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;

                if (MessageBox.Show("Jesteś pewien, że chcesz usunąć ten element?", "USUŃ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                    if (cmd.ExecuteNonQuery() > 0) {
                        textBoxWord.Text = "";
                        textBoxTranslate.Text = "";
                        MessageBox.Show("Usunięto!");
                    }
                }

                conn.Close();

                //Refresh
                Retrieve();
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }

        /*
        public void IfPushed () {

            if (pushed % 2 == 0) {
                buttonChange.Text = listViewAll.SelectedItems[0].SubItems[0].Text;
            } else buttonChange.Text = listViewAll.SelectedItems[0].SubItems[1].Text;

        }  */


        private void listViewAll_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("List view");
            textBoxWordUp.Text = listViewAll.SelectedItems[0].SubItems[0].Text;
            textBoxTranslateUp.Text = listViewAll.SelectedItems[0].SubItems[1].Text;           
            //buttonChange.Text = listViewAll.SelectedItems[0].SubItems[0].Text;
            selected = true;

        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {
            Debug.WriteLine("Add");
            Add(textBoxWord.Text, textBoxTranslate.Text);
            textBoxWord.Text = "";
            textBoxTranslate.Text = "";     
        }

        private void buttonRet_Click_1(object sender, EventArgs e)
        {
            Debug.WriteLine("Ret");
            Retrieve();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Update");

            if (selected) {
                int id = Convert.ToInt16(listViewAll.SelectedItems[0].SubItems[2].Text);
                String newWord = textBoxWordUp.Text;
                String newTranslate = textBoxTranslateUp.Text;

                Update(id, newWord, newTranslate);
            } else {
                MessageBox.Show("Musisz najpierw wybrać słowo!");
            }
   
        }

        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            Debug.WriteLine("Delete");

            if (selected) {
                int id = Convert.ToInt16(listViewAll.SelectedItems[0].SubItems[2].Text);
                Delete(id);
                textBoxWord.Text = "";
                textBoxTranslate.Text = "";
                selected = false;
            }
            else {
                MessageBox.Show("Musisz najpierw wybrać słowo!");
            }
    
        }

        /*
        private void buttonClear_Click_1(object sender, EventArgs e)
        {
            Debug.WriteLine("Clear");
            listViewAll.Items.Clear();
            textBoxWord.Text = "";
            textBoxTranslate.Text = "";
        }
        */

        private void Form1_Load(object sender, EventArgs e)
        {
            Retrieve();
        }

        //This feature is no longer avible
        private void buttonChange_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Klikam!");

            pushed++;
            if (selected) {
               // IfPushed();
            } else {
                MessageBox.Show("Wybierz słowo!");
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();

            if (selectedItem == "Od A do Z") {
                listViewAll.Sorting = System.Windows.Forms.SortOrder.Ascending;
            }
            else if (selectedItem == "Od Z do A") {
                listViewAll.Sorting = System.Windows.Forms.SortOrder.Descending;
            }
            else
                listViewAll.Sorting = System.Windows.Forms.SortOrder.None;

        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = textBoxSearch.Text;

            SqlConnection conn = new SqlConnection(myconnstring);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM WTK_table WHERE id LIKE '%"+keywords+"%' OR Word LIKE '%"+keywords+"%' OR Translate LIKE '%"+keywords+"%'", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (String.IsNullOrEmpty(keywords)) {
                Retrieve();
            }
            else {
                listViewAll.Items.Clear();
                foreach (DataRow row in dt.Rows) {
                    PopulateLV(row[1].ToString(), row[2].ToString(), row[0].ToString());
                }

            }
        }

     

        /*
        private void button3_Click(object sender, EventArgs e)
        {            
            c.Word = textBoxWord.Text;
            c.Translate = textBoxTranslate.Text;

            
            bool success = c.Insert(c);
            if (success == true) {
                //Successfully Inserted
                MessageBox.Show("New Contact Successfully Inserted");     
            }
            else {
                //FAiled to Add Contact
                MessageBox.Show("Failed to add New Contact. Try Again.");
            }

            DataTable dt = c.Select();             
        }
         */
    }
}
