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
    public partial class 添加用户 : Form
    {
        public 添加用户()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string bh = this.txt_yhbh.Text;
            string name = this.txt_yhname.Text;
            string dizhi = this.txt_yh_dizhi.Text;

            string leixing = "";
            string hour = "";
            string choujin = "";

            string yuexin = "";

            string jibengongzi = "";
            string xiaoshoue = "";

            if (rb_daixin.Checked)
            {
                leixing = "带薪雇员";
                jibengongzi = this.txt_basesalary.Text;
                xiaoshoue = this.txt_sales.Text;
            }
            else if (rb_zhongdian.Checked) {
                leixing = "钟点工";
                hour = this.txt_hour.Text;
                choujin = this.txt_choujin.Text;
            }
            else if (rb_yuexin.Checked) {
                leixing = "月薪雇员";
                yuexin = this.txt_salary.Text;
            }

            string conn = "server=.;database=ZGGZ;integrated security=true";
            SqlConnection myconn = new SqlConnection(conn);
            myconn.Open();
            string mysql = "insert into YH(编号,姓名,地址,类型,酬金,小时数,工资数,基本工资,销售额) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ";
            string nsql = string.Format(mysql ,bh,name,dizhi,leixing,choujin,hour,yuexin,jibengongzi,xiaoshoue );
            SqlCommand com2 = new SqlCommand();
            com2.CommandText = nsql;//commandtext属性                 
            com2.Connection = myconn;//connection属性                   
            com2.ExecuteNonQuery();
            myconn.Close();
            MessageBox.Show("添加成功！");
            
            
            this.Close();        


        }
    }
}
