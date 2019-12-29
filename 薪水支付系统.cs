using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication2
{
    public partial class 薪水支付系统 : Form
    {
        static string Constr = "initial catalog=ZGGZ;data source=.;Integrated Security=True;";

     

        public 薪水支付系统()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            添加用户 tjyh = new 添加用户();
            tjyh.ShowDialog();
        }

        private void 薪水支付系统_Load(object sender, EventArgs e)
        {
            LoadNames();
        }

        public  void LoadNames()
        {
            string commandtext = "select * from YH ";
            SqlConnection conn = new SqlConnection(Constr);//数据库连接
            SqlCommand cmd = new SqlCommand(commandtext, conn);//写一个命令，运用数据库方法
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();//数据阅读，返回多少行有影响的代码
                StringBuilder sb = new StringBuilder();
                while (reader.Read())
                {
                    sb.Append(reader.GetString(1)+"\r\n");
                }
                this.textBox1.Text = sb.ToString();
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

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = this.textBox1.SelectedText;
            if (string.IsNullOrEmpty(txt))
            {
                MessageBox.Show("请从左侧选择用户信息！");
                return;
            }

            string commandtext = "select * from YH where 姓名='"+txt.Trim()+"'";
            SqlConnection conn = new SqlConnection(Constr);//数据库连接
            SqlCommand cmd = new SqlCommand(commandtext, conn);//写一个命令，运用数据库方法
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();//数据阅读，返回多少行有影响的代码

                string xingming = "";
                string leixing = "";
                string yingfagongzi = "";

                StringBuilder sb = new StringBuilder();

                while (reader.Read())
                {
                    xingming = reader.GetString(1);
                    leixing = reader.GetString(3);
                    if (leixing == "钟点工")
                    {
                        int choujin = reader.GetInt32(4);
                        int hour = reader.GetInt32(5);
                        yingfagongzi = (choujin * hour).ToString();
                    }
                    else if (leixing == "带薪雇员")
                    {
                        int jibengongzi  = reader.GetInt32(7);
                        int xiaoshoue = reader.GetInt32(8);
                        yingfagongzi = (jibengongzi + xiaoshoue*0.05).ToString();
                    }
                    else if (leixing == "月薪雇员")
                    {
                        int salary = reader.GetInt32(6);
                        yingfagongzi = salary.ToString();
                    }
                }

                sb.Append("姓名：");
                sb.Append(xingming); sb.Append("\r\n\r\n");
                
                sb.Append("类型：");
                sb.Append(leixing); sb.Append("\r\n\r\n");

                sb.Append("应发工资：");
                sb.Append(yingfagongzi); sb.Append("\r\n");

                this.textBox2.Text = sb.ToString();
                
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

        private void button3_Click(object sender, EventArgs e)
        {
            LoadNames();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
