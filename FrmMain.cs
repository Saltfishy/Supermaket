using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supermaket
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        string constr = "Data Source=.;Initial Catalog=SuperMark;User ID=admin;Password=lxh6823313";
        private void FrmMain_Load(object sender, EventArgs e)
        {
            rbMetalsCard.Checked = true;
            if (rbMetalsCard.Checked==true)
            {
                txtScore.Text = 500.ToString();
            }
            BindCbo(cboSates);
            BindDgv(dgvShow);
        }

        private void BindDgv(DataGridView dgvShow)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter adapter = new SqlDataAdapter("select u.Id,u.CustomerId,u.CustomerPassword,u.CustomerType,u.Score,s.StatesName from UsersInfo as u ,States as s where s.Id=u.StatesId", con);
            adapter.Fill(ds,"show");
            dgvShow.DataSource = ds.Tables[0];
        }

        private void rbMetalsCard_Click(object sender, EventArgs e)
        {
            txtScore.Text = 500.ToString();
        }

        private void rbPlatinumCard_Click(object sender, EventArgs e)
        {
            txtScore.Text = 2000.ToString();
        }

        private void rbDiamondCard_Click(object sender, EventArgs e)
        {
            txtScore.Text = 5000.ToString();
        }


        public bool chekinput(TextBox box)
        {
            if (!string.IsNullOrEmpty(box.Text.Trim()))
            {
                
                return false;
            }
            box.Focus();
            return true;
        }

        public void BindCbo(ComboBox box)
        {
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter adapter = new SqlDataAdapter("select * from dbo.States",con);
            adapter.Fill(ds,"States");
            box.DataSource = ds.Tables[0];
            box.DisplayMember = "StatesName";
            box.ValueMember = "Id";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string type = null;
            if (rbDiamondCard.Checked==true)
            {
                type = rbDiamondCard.Text;
            }
            if (rbMetalsCard.Checked==true)
            {
                type = rbMetalsCard.Text;
            }
            if (rbPlatinumCard.Checked==true)
            {
                type = rbPlatinumCard.Text;
            }
            if (chekinput(txtUser) || chekinput(txtPwd))
            {
                MessageBox.Show("账户密码都不能为空！");
            }
            else
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand cmd = new SqlCommand(string.Format("insert into UsersInfo(CustomerId,CustomerPassword,CustomerType,Score,StatesId) Values('{0}','{1}','{2}','{3}','{4}')",txtUser.Text.Trim(),txtPwd.Text.Trim(),type,txtScore.Text,cboSates.SelectedValue), con);
                int isok=cmd.ExecuteNonQuery();
                if (isok>0)
                {
                    MessageBox.Show("添加成功！");
                    FrmMain_Load(sender,e);
                }
            }
        }

        private void dgvShow_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvShow.SelectedRows[0].Cells[3].Value.ToString()==rbMetalsCard.Text)
            {
                rbMetalsCard.Checked = true;
            }else if (dgvShow.SelectedRows[0].Cells[3].Value.ToString() == rbPlatinumCard.Text)
            {
                rbPlatinumCard.Checked = true;
            }
            else if (dgvShow.SelectedRows[0].Cells[3].Value.ToString() == rbDiamondCard.Text)
            {
                rbDiamondCard.Checked = true;
            }
            txtPwd.Text = dgvShow.SelectedRows[0].Cells[2].Value.ToString();
            txtUser.Text= dgvShow.SelectedRows[0].Cells[1].Value.ToString();
            txtScore.Text = dgvShow.SelectedRows[0].Cells[4].Value.ToString();
            cboSates.Text = dgvShow.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand(string.Format("delete UsersInfo where Id={0}",dgvShow.SelectedRows[0].Cells[0].Value), con);
            int isok = cmd.ExecuteNonQuery();
            if (isok > 0)
            {
                MessageBox.Show("删除成功！");
                FrmMain_Load(sender, e);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
