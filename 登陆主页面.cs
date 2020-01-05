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

namespace WindowsFormsApplication2
{
    public partial class 登陆主页面 : Form
    {
         static string Constr = "initial catalog=ZGGZ;data source=.;Integrated Security=True;";
        public 登陆主页面()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string commandtext = "select *from ZC_B where 用户名='" + textBox1.Text.Trim() + "'and 密码='" + textBox2.Text.Trim() + "'";
            SqlConnection conn = new SqlConnection(Constr);//数据库连接
            SqlCommand cmd = new SqlCommand(commandtext, conn);//写一个命令，运用数据库方法
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();//数据阅读，返回多少行有影响的代码
                if (!reader.Read())
                {
                    MessageBox.Show("用户名密码不正确，请从新输入！");
                }
                else
                {
                 this.Hide();
                管理页面 mainForm = new 管理页面();
                mainForm.Show();
                }
            }
                catch (SqlException err)
                {
                    MessageBox .Show (err.ToString ());
                }
                finally 
               {
                conn.Close ();
               }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            注册页面   form2= new 注册页面  ();
            form2.Show();
        }

        private void 登陆主页面_Load(object sender, EventArgs e)
        {

        }                
    }
}
