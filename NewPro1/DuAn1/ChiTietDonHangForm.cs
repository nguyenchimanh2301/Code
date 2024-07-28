using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAn1
{
    public partial class ChiTietDonHangForm : Form
    {
        string data;
        Pro1QuanLiDienThoaiFinalContext db = new Pro1QuanLiDienThoaiFinalContext();
        public ChiTietDonHangForm(string id)
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            data = id;
        }

        private void ChiTietDonHangForm_Load(object sender, EventArgs e)
        {
            var order = db.Orders.FirstOrDefault(x => x.Idorder == data);
            var orderdetail = db.OrderDetails.FirstOrDefault(x => x.Idorder == data);
            var customer =  db.Customers.FirstOrDefault(x => x.Idcustomer == order.Idcustomer);
           
            
            
            
            label2.Text = data;
            label18.Text = order.TotalAmount.ToString();
            label15.Text = order.EstablishedDate.ToString();
            label7.Text = order.Idcustomer.ToString();
            label9.Text = customer.CustomerName;
            label11.Text = customer.PhoneNumber;
            label13.Text = customer.CustomerAddress;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.CurrentCell = null;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
