﻿using BUS.Services;
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
    public partial class ProductDeltailForm : Form
    {
        ProductDetailBUS productDetailBUS = new ProductDetailBUS();
        ProductBUS productBUS = new ProductBUS();
        CpuBUS cpuBUS = new CpuBUS();
        ProductColorBUS colorBUS = new ProductColorBUS();
        PromotionBUS promotionBUS = new PromotionBUS();
        AccountBUS accountBUS = new AccountBUS();
        FormMenu menu= new FormMenu();
        public ProductDeltailForm()
        {
            InitializeComponent();
        }
        public bool CheckCbb(params ComboBox[] cbbs)
        {
            foreach (ComboBox cbb in cbbs)
            {
                if (cbb.SelectedIndex == -1)
                {
                    return false;
                }
            }
            return true;
        }
        public void LoadDataGridView()
        {
            dgvProductDetail.Columns.Add("IDproduct", "ID product");
            dgvProductDetail.Columns.Add("IdproductDetails", "Id productDetails");
            dgvProductDetail.Columns.Add("Idcolor", "Id color");
            dgvProductDetail.Columns.Add("Storage", "Storage");
            dgvProductDetail.Columns.Add("Price", "Price");
            dgvProductDetail.Columns.Add("Idpromotion", "Id promotion");
            dgvProductDetail.Columns.Add("WarrantyPeriod", "Warranty period");
            dgvProductDetail.Columns.Add("Inventory", "Inventory");
            dgvProductDetail.Columns.Add("Idaccount", "Idaccount");
        }
        public void ShowOnDataGridView(List<ProductDetail> productDetails)
        {
            dgvProductDetail.Rows.Clear();
            foreach (ProductDetail productDetail in productDetails)
            {
                dgvProductDetail.Rows.Add(productDetail.Idproduct, productDetail.IdproductDetails, productDetail.Idcolor, productDetail.Storage, productDetail.Price, productDetail.Idpromotion, productDetail.WarrantyPeriod, productDetail.Inventory, productDetail.Idaccount);
            }
        }
        public bool CheckProductDetailIfExists(string idProductDetail)
        {
            if (productDetailBUS.GetProductDetailByID(idProductDetail) != null)
                return true;
            return false;
        }
        public bool CheckNullInProductDetail(params string[] strings)
        {
            foreach (string s in strings)
            {
                if (string.IsNullOrEmpty(s))
                    return true;
            }
            return false;
        }
        public string CheckIsNumberProductDetail(string storage, string price, string warrantyPeriod, string inventory)
        {
            if (!int.TryParse(storage, out _))
                return "Storage phải là số nguyên";
            else if (!decimal.TryParse(price, out _))
                return "Price phải là số";
            else if (!int.TryParse(warrantyPeriod, out _))
                return "Warranty period phải là số nguyên";
            else if (!int.TryParse(inventory, out _))
                return "Inventory phải là số nguyên";
            return null;
        }
        public void LoadCbbWhenDropDown(ComboBox currentCbb, List<string> originList)
        {
            currentCbb.Items.Clear();
            var text = currentCbb.Text;
            foreach (var item in originList.Where(c => c.Contains(text)))
            {
                currentCbb.Items.Add(item);
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (CheckCbb(cbbIdProduct, cbbIdColor, cbbIdCpu))
            {
                var idProduct = cbbIdProduct.SelectedItem.ToString();
                var idCpu = cbbIdCpu.SelectedItem.ToString();
                var idProductDetail = txtIdProductDetail.Text;
                var idColor = cbbIdColor.SelectedItem.ToString();
                var storage = txtStorage.Text;
                var price = txtPrice.Text;
                string idPromotion;
                if (cbbIdPromotion.SelectedIndex == -1)
                    idPromotion = null;
                else
                    idPromotion = cbbIdPromotion.SelectedItem.ToString();
                var warrantyPeriod = txtWarrantyPeriod.Text;
                var inventory = txtInventory.Text;
                var imeiNumber = txtImeiNumber.Text;
                var idAccount = txtIdAccount.Text;
                if (!CheckNullInProductDetail(idProduct, idCpu, idProductDetail, idColor, storage, price, warrantyPeriod, inventory, imeiNumber, idAccount))
                {
                    if (CheckProductDetailIfExists(cbbIdProduct.SelectedItem.ToString()) == false)
                    {
                        var check = CheckIsNumberProductDetail(storage, price, warrantyPeriod, inventory);
                        if (check == null)
                        {
                            if (productDetailBUS.AddNewProductDetail(idProduct, idProductDetail, idColor, int.Parse(storage), decimal.Parse(price), idPromotion, int.Parse(warrantyPeriod), int.Parse(inventory), idAccount))
                                MessageBox.Show("Thêm thành công");
                            else
                                MessageBox.Show("Thêm thất bại");
                        }
                        else
                            MessageBox.Show(check);
                    }
                    else
                        MessageBox.Show("Product detail này đã tồn tại");
                }
                else
                    MessageBox.Show("Nhập đầy đủ thông tin trước khi thêm");
            }
            else
                MessageBox.Show("Chọn lại giá trị từ các combo box");
            ShowOnDataGridView(productDetailBUS.GetAllProductDetail());
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (CheckCbb(cbbIdProduct, cbbIdColor, cbbIdCpu))
            {
                var idProduct = cbbIdProduct.Text;
                var idCpu = cbbIdCpu.Text;
                var idProductDetail = txtIdProductDetail.Text;
                var idColor = cbbIdColor.Text;
                var storage = txtStorage.Text;
                var price = txtPrice.Text;
                string idPromotion;
                if (cbbIdPromotion.SelectedIndex == -1)
                    idPromotion = null;
                else
                    idPromotion = cbbIdPromotion.SelectedItem.ToString();
                var warrantyPeriod = txtWarrantyPeriod.Text;
                var inventory = txtInventory.Text;
                var imeiNumber = txtImeiNumber.Text;
                var idAccount = txtIdAccount.Text;
                if (!CheckNullInProductDetail(idProduct, idCpu, idProductDetail, idColor, storage, price, warrantyPeriod, inventory, imeiNumber, idAccount))
                {
                    if (CheckProductDetailIfExists(cbbIdProduct.SelectedItem.ToString()) == false)
                    {
                        var check = CheckIsNumberProductDetail(storage, price, warrantyPeriod, inventory);
                        if (check == null)
                        {
                            if (productDetailBUS.UpdateProductDetail(idProduct, idProductDetail, idColor, int.Parse(storage), decimal.Parse(price), idPromotion, int.Parse(warrantyPeriod), int.Parse(inventory), idAccount))
                                MessageBox.Show("Sửa thành công");
                            else
                                MessageBox.Show("Sửa thất bại");
                        }
                        else
                            MessageBox.Show(check);
                    }
                    else
                        MessageBox.Show("Product detail này đã tồn tại");
                }
                else
                    MessageBox.Show("Nhập đầy đủ thông tin trước khi sửa");
            }
            else
                MessageBox.Show("Chọn lại giá trị từ các combo box");
            ShowOnDataGridView(productDetailBUS.GetAllProductDetail());
        }

        private void cbbIdProduct_DropDown(object sender, EventArgs e)
        {
            LoadCbbWhenDropDown(cbbIdProduct, productBUS.GetAllIDProduct());
        }

        private void cbbIdCpu_DropDown(object sender, EventArgs e)
        {
            LoadCbbWhenDropDown(cbbIdCpu, cpuBUS.GetAllIDCpu());
        }

        private void cbbIdColor_DropDown(object sender, EventArgs e)
        {
            LoadCbbWhenDropDown(cbbIdColor, colorBUS.GetAllIdColor());
        }

        private void cbbIdPromotion_DropDown(object sender, EventArgs e)
        {
            LoadCbbWhenDropDown(cbbIdPromotion, promotionBUS.GetAllIdPromotion());
        }
        private void ProductDeltailForm_Load(object sender, EventArgs e)
        {
            txtIdAccount.Text = menu.IdAccountMenu;
            LoadDataGridView();
            ShowOnDataGridView(productDetailBUS.GetAllProductDetail());
        }

        private void vbButton1_Click(object sender, EventArgs e)
        {

        }
        private void cbbIdCpu_Leave(object sender, EventArgs e)
        {
            if (cbbIdCpu.SelectedItem != null)
            {
                var id = cbbIdCpu.SelectedItem.ToString();
                var cpu = cpuBUS.GetCpuById(id);
                if (cpu != null)
                {
                    txtManufacturer.Text = cpu.Manufacturer;
                    txtNameCpu.Text = cpu.NameCpu;
                }
            }
        }
        private void cbbIdColor_Leave(object sender, EventArgs e)
        {
            if (cbbIdColor.SelectedItem != null)
            {
                var color = colorBUS.GetProductColorById(cbbIdColor.SelectedItem.ToString());
                if (color != null)
                {
                    txtColorName.Text = color.ColorName;
                }
            }
        }

        private void cbbIdPromotion_Leave(object sender, EventArgs e)
        {
            if (cbbIdPromotion.SelectedItem != null)
            {
                var promotion = promotionBUS.GetPromotionById(cbbIdPromotion.SelectedItem.ToString());
                if (promotion != null)
                {
                    txtPromotionName.Text = promotion.PromotionName;
                    txtDiscount.Text=promotion.Discount.ToString();
                    dtpStartDate.Value=promotion.StartTime;
                    if (promotion.EndTime == null)
                        dtpEndDate.Visible = false;
                    else
                        dtpEndDate.Value = (DateTime)promotion.EndTime;
                }
            }
        }
    }
}
