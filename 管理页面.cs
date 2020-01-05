using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace WindowsFormsApplication2
{
    public partial class 管理页面 : Form
    {
        static string ConStr = "initial catalog=ZGGZ;data source=.;Integrated Security=True;";
        //private SqlParameter[] paras;
        public 管理页面()
        {
            InitializeComponent();
        }
        private void Form2_load(object sender, EventArgs e)
        {
             RefreshDataGridView();//刷新DATAGRIDVIEW
             RefreshDataGridView();
             //GetSelectInfo();
             
        }
        
        private ArrayList ExecuteReArrList(String CommandText)
        {
            ArrayList arr = new ArrayList();
             SqlConnection  conn = new SqlConnection (ConStr);
            SqlCommand cmd = new SqlCommand(CommandText, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader ();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        arr.Add(reader[i]);
                }
                //RefreshDataGridView();

            }
            catch (SqlException err)
            {
                MessageBox.Show(err.ToString());
            }
            finally
            {
                conn.Close();
            }
            return arr ;

        }
        private int Execute(String CommandText)
        {
            int c = -1;
            SqlConnection conn = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand(CommandText, conn);

            try
            {
                conn.Open();
                c=cmd.ExecuteNonQuery();
                //RefreshDataGridView();

            }
            catch (SqlException err)
            {
                MessageBox.Show(err.ToString());
            }
            finally
            {
                conn.Close();
            }
            return c;
        }
        //*****************************************************************************
        private int ExecuteProc(String Proc,SqlParameter[] paras)
        {
            int c = -1;
            SqlConnection conn = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand(Proc , conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter para in paras)
            {
                cmd.Parameters.Add(para);
            }
            try
            {
                conn.Open();
                c = cmd.ExecuteNonQuery();
                //RefreshDataGridView();

            }
            catch (SqlException err)
            {
                MessageBox.Show(err.ToString());
            }
            finally
            {
                conn.Close();
            }
            return c;
        }
//************************************************************************************************************
        private DataTable ExecuteReTable(String CommandText)
        {
            SqlConnection conn = new SqlConnection(ConStr);
            SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, conn);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                Adapter.Fill(ds);

            }
            catch (SqlException err)
            {
                MessageBox.Show(err.ToString());
            }
            finally
            {
                conn.Close();
            }
            return ds.Tables[0].Copy();
        }
        //*********************************************************************************************
        private void GetSelectInfo()
        {
            int ID;
            int SelectRow = dataGridView1.CurrentCell.RowIndex;
            ID = int.Parse(textBox8.Text.Trim());
            String CommandText = "SELECT * FROM XX_T WHERE ID=" + ID;
            ArrayList arr = new ArrayList();
            arr = ExecuteReArrList(CommandText);
            textBox8.Text = arr[0].ToString();
            textBox9.Text = arr[1].ToString();
            textBox10.Text = arr[2].ToString();
            textBox11.Text = arr[3].ToString();
            textBox12.Text = arr[4].ToString();
            textBox13.Text = arr[5].ToString();
            textBox14.Text = arr[6].ToString();
        }
      //*********************************************************************************************************** 
        static public object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
                return obj;
        }
       //*************************************************
        private void Insert()//添加
        {
            
            int c = 0;
            c = int.Parse(ExecuteReArrList("SELECT COUNT(ID)FROM XX_T WHERE ID=" + textBox1.Text.Trim())[0].ToString());
            
            if (textBox2.Text == "")
            {
                MessageBox.Show("名字不能为空，添加失败！");
            }
            else
                if (textBox3.Text == "")
            {
                MessageBox.Show("岗位不能为空，添加失败！");
            }
            else
                    if (textBox5.Text == "")
            {
                MessageBox.Show("年龄不能为空，添加失败！");
            }
            else
                        if (textBox6.Text == "")
            {
                MessageBox.Show("性别不能为空，添加失败！");
            }
            else
                            if (c > 0)
            {
                MessageBox.Show("编号已存在!");
            }
            else
            {
                SqlParameter[] paras =
                {  
               
                new SqlParameter ("@ID",Convert .ToInt32 (textBox1 .Text .Trim ())),
                new SqlParameter ("@Name", textBox2.Text .Trim ()),
                new SqlParameter ("@Post", textBox3 .Text .Trim ()),
                new SqlParameter ("@Wages",Convert .ToInt32 (textBox4 .Text .Trim ())),
                new SqlParameter ("@Age",Convert .ToInt32 (textBox5.Text .Trim ())),
                new SqlParameter ("@Sex",textBox6 .Text .Trim ()),
                new SqlParameter ("@Link",Convert .ToInt32 (textBox7 .Text .Trim ())),
                };
                c = ExecuteProc("proc_T_insert", paras);
                if (c > 0)
                {
                    MessageBox.Show("添加成功！");
                    RefreshDataGridView();
                }
                else
                {
                    MessageBox.Show("添加失败！");
                }
            }

        }
        private void RefreshDataGridView()
        {
            String CommandText = "SELECT *FROM XX_T";
            DataTable dt = new DataTable();
            dt = ExecuteReTable(CommandText);
            dt.TableName = "T";
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "T";
        }
        private void del()//删除
        {
            int row = dataGridView1.CurrentCell.RowIndex;
            int ID = int.Parse(dataGridView1.Rows[row].Cells[0].Value.ToString());
            string ConnandText = "DELETE FROM XX_T WHERE ID=" + ID ;
            SqlConnection conn = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand(ConnandText, conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                RefreshDataGridView();
            }
            catch (SqlException err)
            {
                MessageBox.Show(err.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        private void UPDATE()//修改
        {

            int c = 0;
            c = int.Parse(ExecuteReArrList("SELECT COUNT(ID)FROM XX_T WHERE ID=" + textBox8.Text.Trim())[0].ToString());
            if (c ==0)
            {
                MessageBox.Show("编号不存在。");
            }
            else
            {
                string Name;
                string Post;
                string Wages;
                string Age;
                string Sex;
                string Link;
                string day;
                string ID = textBox8.Text.Trim();//职工号
                if (textBox9.Text.Length <= 0)
                    Name = "";
                else
                    Name = "Name='" + textBox9.Text.Trim() + "',";//名字
                if (textBox10.Text.Length <= 0)
                    Post = "";
                else
                {
                    Post = "Post='" + textBox10.Text.Trim() + "',";//岗位
                }
                if (textBox11.Text.Length <= 0)
                    Wages = "";
                else
                    Wages = "Wages='" + textBox11.Text.Trim() + "',";//工资
                if (textBox12.Text.Length <= 0)
                    Age = "";
                else
                    Age = "Age='" + textBox12.Text.Trim() + "',";//年龄
                if (textBox13.Text.Length <= 0)
                    Sex = "";
                else
                    Sex = "Sex='" + textBox13.Text.Trim() + "',";//性别
                if (textBox14.Text.Length <= 0)
                    Link = "";
                else
                    Link = "Link='" + textBox14.Text.Trim() + "',";//联系
                if (textBox17.Text.Length<=0)
                {
                    day = "";
                }
                else
                {
                    day = "day='" + textBox17.Text.Trim() + "',";
                }
                string strTest = Name + Post + Wages + Age + Sex + Link+day;
                strTest = strTest.Substring(0, strTest.LastIndexOf(','));

                String CommandText = "UPDATE XX_T SET " + strTest + "where ID=" + textBox8.Text.Trim() + ";";
                //MessageBox.Show(textBox10.Text);
                //MessageBox.Show(CommandText);
                Execute(CommandText);
                RefreshDataGridView();
                //GetSelectInfo();*/
            }
        }
        private void Search1()
        {
            string CommandText="SELECT* FROM XX_T WHERE "+comboBox1.Text .Trim ()+"='"+textBox15 .Text .Trim()+"'";
            DataTable DT = new DataTable();
            DT = ExecuteReTable (CommandText);
            DT.TableName = "X";
            DataSet ds = new DataSet();
            ds.Tables.Add(DT);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "X";
        }
       
      //*************************************************************
        private void Search2()
        {
            string CommandText = "SELECT* FROM GZ_B WHERE ID IN (select ID from XX_T WHERE " + comboBox2.Text.Trim() + "='" + textBox16.Text.Trim() + "')";
            DataTable DT = new DataTable();
            //MessageBox.Show(CommandText);
            DT = ExecuteReTable(CommandText);
            DT.TableName = "S";
            DataSet ds = new DataSet();
            ds.Tables.Add(DT);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "S";
        }
        // *******************************************************************
        //private void Search3()
        //{
        //    string CommandText = "SELECT Name,Wages FROM XX_T WHERE Wages >= '" + textBox17.Text.Trim() + "' and " + "Wages <= '" +   textBox18.Text.Trim ()+ "'";
        //    DataTable DT = new DataTable();
        //    //MessageBox.Show(CommandText);
        //    DT = ExecuteReTable(CommandText);
        //    DT.TableName = "S2";
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(DT);
        //    dataGridView1.DataSource = ds;
        //    dataGridView1.DataMember = "S2";
        //}
        // *******************************************************************
        private void UP()
        {
            String CommandText = "SELECT *FROM XX_T ORDER BY cast(ID as int)";
            DataTable dt = new DataTable();
            dt = ExecuteReTable(CommandText);
            dt.TableName = "T";
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "T";

        }
        //*********************************************************
        private void button1_Click(object sender, EventArgs e)
        {

            RefreshDataGridView();     
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Insert();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除此个成员的信息?", "删除",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) 
            {
                del();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确认修改信息?", "修改",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                UPDATE();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Search1();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Search2();
        }

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    Search3();
        //}


        private void button9_Click(object sender, EventArgs e)
        {
            float a, b, c, d, h, f, g;
            a = float.Parse(textBox19.Text);
            b = float.Parse(textBox20.Text);
            c = float.Parse(textBox21.Text);
            d = float.Parse(textBox22.Text);
            h = float.Parse(textBox23.Text);
            f = float.Parse(textBox24.Text);
            g = a - b * c + d + h - f;
            textBox25.Text = g.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UP();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["UserName"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["UserSex"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["UserPhone"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["UserBrith"].Value.ToString();
            textBox5.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["UserEmail"].Value.ToString();
        }
        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}    
       
          
