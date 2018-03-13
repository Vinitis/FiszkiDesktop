using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDearFriend.Classes {

    class DBClass {

        

        public ListView listViewAll = new ListView();
        TextBox textBoxWord = new TextBox();
        TextBox textBoxTranslate = new TextBox();
        public int WordID { get; set; }
        public string Word { get; set; }
        public string Translate { get; set; }

        static string myconnstring = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        public void populateLV(String id, String name, String position)
        {
            String[] row = { id, name, position };

            listViewAll.Items.Add(new ListViewItem(row));
        }

        //INSERTING
        public void Add(String Word, String Translate)
        {
            //SQL STMT
            String sql = "INSERT INTO WT_table(Word,Translate) VALUES(@Word,@Translate)";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Word", Word);
            cmd.Parameters.AddWithValue("@Translate", Translate);

            try {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0) {
                    MessageBox.Show("Inserted");
                }
                conn.Close();

                Retrieve();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        //SELECT
        public void Retrieve()
        {
            listViewAll.Items.Clear();

            //SQL STMT
            String sql = "SELECT * FROM WT_table";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            //OPEN CON,RETRIEVE,FILL LISTVIEW
            try {
                conn.Open();

                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                //LOOP THRU DT
                foreach (DataRow row in dt.Rows) {
                    populateLV(row[0].ToString(), row[1].ToString(), row[2].ToString());
                }

                conn.Close();

                //CLEAR DT
                dt.Rows.Clear();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        //UPDATE
        public void Update(int id, String newWord, String newTranslate)
        {
            //SQL STMT
            string sql = "UPDATE Wt_table SET name='" + newWord + "',position='" + newTranslate + "' WHERE id=" + id + "";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);


            //OPEN CON,UPDATE,RETRIEVE LISTVIEW
            try {
                conn.Open();

                
                adapter.UpdateCommand = conn.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0) {
                    textBoxWord.Text = "";
                    textBoxTranslate.Text = "";
                    MessageBox.Show("Successfully Updated");
                }

                conn.Close();

                //REFRESH
                Retrieve();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }

        //DELETE
        public void Delete(int id)
        {
            //SQL STMT
            String sql = "DELETE FROM Wt_table WHERE id=" + id + "";
            SqlConnection conn = new SqlConnection(myconnstring);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            //'OPEN CON,EXECUTE DELETE,CLOSE CON
            try {
                conn.Open();

                adapter.DeleteCommand = conn.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;

                //PROMT FOR CONFIRMATION
                if (MessageBox.Show("Sure ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                    if (cmd.ExecuteNonQuery() > 0) {
                        textBoxWord.Text = "";
                        textBoxTranslate.Text = "";
                        MessageBox.Show("Successfully deleted");
                    }
                }

                conn.Close();

                //REFRESH
                Retrieve();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }

        
    }
}
