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
    public partial class CreateOrderForm : Form
    {
        public CreateOrderForm()
        {
            InitializeComponent();
        }
        Pro1QuanLiDienThoaiFinalContext db = new Pro1QuanLiDienThoaiFinalContext();
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btninfoOrder_Click(object sender, EventArgs e)
        {
            ChiTietDonHangForm a = new ChiTietDonHangForm(txtId.Text);
            a.ShowDialog();
        }
        /// <summary>
        /// Nút Tạo đơn hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTaoDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Hãy nhập mã đơn hàng!");
                return;
            }

            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Hãy nhập số lượng!");
                return;
            }
            var dupplicate = db.Orders.FirstOrDefault(x => x.Idorder == txtId.Text);
            if (dupplicate != null)
            {
                MessageBox.Show("Mã đơn hàng đã tồn tại!");
                return;
            }
            try
            {

                Order order = new Order();
                order.Idorder = txtId.Text;
                order.Idaccount = "acc1";
                order.EstablishedDate = dateTimePicker1.Value;
                order.Idcustomer = cbcus.SelectedValue.ToString();
                order.TotalAmount = decimal.Parse(label13.Text);
                order.Idvoucher = comboBox3.SelectedValue.ToString();
                OrderDetail detail = new OrderDetail();
                detail.IdproductDetails = comboBox1.SelectedValue.ToString();
                detail.Idorder = order.Idorder;
                detail.Quantity = int.Parse(textBox4.Text);
                db.Orders.Add(order);
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                MessageBox.Show("Tạo đơn hàng thành công!");
                LoadDataGridView();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Load dữ liệu  ra combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadCombobox()
        {
            cbcus.DataSource = db.Customers.ToList();
            cbcus.DisplayMember = "Idcustomer";
            cbcus.ValueMember = "Idcustomer";

            comboBox1.DataSource = db.ProductDetails.ToList();
            comboBox1.ValueMember = "IdproductDetails";
            comboBox1.DisplayMember = "IdproductDetails";


            comboBox3.DataSource = db.Vouchers.ToList();
            comboBox3.ValueMember = "Idvoucher";
            comboBox3.DisplayMember = "VoucherName";
        }
        /// <summary>
        /// Load dữ liệu  ra vew
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoadDataGridView()
        {
            var data = db.Orders.Select(x => new
            {
                x.Idorder,
                x.Idcustomer,
                x.EstablishedDate,
                x.Idvoucher,
                x.TotalAmount
            }).ToList();
            dataGridView1.DataSource = data;
        }
       
        private void CreateOrderForm_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.MultiSelect = false;
            LoadCombobox();
            LoadDataGridView();
            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu nhấp vào tiêu đề cột thì không làm gì cả
            if (e.RowIndex < 0)
                return;

            // Lấy dữ liệu của dòng được nhấp
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            txtId.Text = row.Cells["Idorder"].Value.ToString();
        }
        /// <summary>
        /// Hiển thị tiền khi nhập số vào ô
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                var product = db.ProductDetails.FirstOrDefault(x => x.IdproductDetails == comboBox1.SelectedValue.ToString());
                label13.Text = (product.Price * int.Parse(textBox4.Text)).ToString();
            }
            else
            {
                return;
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        /// <summary>
        /// Chức năng hủy đơn hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHuyDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Hãy chọn đơn hàng!");
                return;
            }
            // Hiển thị MessageBox với câu hỏi Yes/No
            DialogResult result = MessageBox.Show($"Bạn muốn hủy đơn hàng này ?",
                                                  "Confirmation",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            // Xử lý phản hồi của người dùng
            if (result == DialogResult.Yes)
            {
                ///Xóa cả chi tiết đơn hàng và cả đơn hàng
                var orderdetail = db.OrderDetails.FirstOrDefault(x => x.Idorder == txtId.Text);
                var order = db.Orders.FirstOrDefault(x => x.Idorder == txtId.Text);
                db.OrderDetails.Remove(orderdetail);
                db.Orders.Remove(order);
                db.SaveChanges();
                MessageBox.Show("Hủy thành công!");
                LoadDataGridView();
            }
        }
        /// <summary>
        /// Chọn mã khách hàng để lấy ra các thông tin còn lại
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbcus_SelectedIndexChanged(object sender, EventArgs e)
        {
            var customer = db.Customers.FirstOrDefault(x => x.Idcustomer == cbcus.SelectedValue.ToString());
            if (customer != null)
            {
                txtName.Text = customer.CustomerName;
                textBox1.Text = customer.CustomerAddress;
                textBox2.Text = customer.PhoneNumber;
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            txtId.Text = textBox4.Text = textBox1.Text = textBox2.Text = txtName.Text = null;
        }
    }
}
