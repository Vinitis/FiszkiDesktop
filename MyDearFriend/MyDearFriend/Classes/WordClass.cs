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

    class WordClass {

       

        //Define getter and setter properties
        public int WordID { get; set; }
        public string Word { get; set; }
        public string Translate { get; set; }

        //DB connecting string
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        //Selecting data from DB
        public DataTable Select() {
            SqlConnection conn = new SqlConnection(myconnstring);
            DataTable dt = new DataTable();
            try {
                string sql = "SELECT * FROM WT_table";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally {
                conn.Close();
            }
            return dt;
        }

        //Insert data into DB
        public bool Insert (WordClass c) {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstring);
            try {
                string sql = "INSERT INTO WT_table (Word, Translate) VALUES (@Word, @Translate)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (String.IsNullOrEmpty(Word)) {
                    cmd.Parameters.AddWithValue("@Word", DBNull.Value);
                }
                else cmd.Parameters.AddWithValue("@Word", c.Word);
                if (String.IsNullOrEmpty(Translate)) {
                    cmd.Parameters.AddWithValue("@Translate", DBNull.Value);
                }
                else cmd.Parameters.AddWithValue("@Translate", c.Translate);
                if (cmd.ExecuteNonQuery() > 0) {
                    MessageBox.Show("Inserted");
                }
                conn.Open();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally {
                conn.Close();
            }
            
            return isSuccess;
        }

        //Update data in DB
        public bool Update(WordClass c) {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstring);
            try {
                string sql = "UPDATE WT_table SET Word=@Word, Translate=@Translate WHERE WordID=@WordID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (String.IsNullOrEmpty(Word)) {
                    cmd.Parameters.AddWithValue("Word", DBNull.Value);
                } else cmd.Parameters.AddWithValue("Word", c.Word);
                if (String.IsNullOrEmpty(Translate)) {
                    cmd.Parameters.AddWithValue("Translate", DBNull.Value);
                } else cmd.Parameters.AddWithValue("Translate", c.Translate);
                if (cmd.ExecuteNonQuery() > 0) {
                    MessageBox.Show("Inserted");
                }
                conn.Open();
                
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally {
                conn.Close();
            }
            return isSuccess;
        }

        //Delete data from DB
        public bool Delete(WordClass c) {
            bool isSucccess = false;
            SqlConnection conn = new SqlConnection(myconnstring);
            try {
                string sql = "DELETE FROM WT_table WHERE WordID=@WordID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@WordID", c.WordID);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0) {
                    MessageBox.Show("Inserted");
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally {
                conn.Close();
            }
            return isSucccess;
        }  
    }
}

