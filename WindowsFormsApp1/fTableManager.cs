using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.DTO;
using MenuDTO = WindowsFormsApp1.DTO.MenuDTO;

namespace WindowsFormsApp1
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();

            LoadTable();
            LoadCategory();
        }

        #region Method
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<TableDTO> tableList = TableDAO.Instance.LoadTableList();

            foreach(TableDTO item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWitdth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.LightBlue;
                        break;
                    default:
                        btn.BackColor = Color.PaleVioletRed;
                        break;
                }
                flpTable.Controls.Add(btn);

            }
        }
        void LoadCategory()
        {
            List<CategoryDTO> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<FoodDTO> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void ShowBill(int id)
        {
            CultureInfo culture = new CultureInfo("vi-VN");
            lsvBill.Items.Clear();
            List<MenuDTO> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (MenuDTO item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("c", culture));
                lsvItem.SubItems.Add(item.TotalPrice.ToString("c", culture));
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            
            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }

        #endregion

        #region Event
        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }
        void btn_Click(object sender, EventArgs e)
        {
            int tableID =((sender as Button).Tag as TableDTO).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.ShowDialog();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            CategoryDTO selected = cb.SelectedItem as CategoryDTO;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            TableDTO table = lsvBill.Tag as TableDTO;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as FoodDTO).ID;
            int count = (int)nmFoodCount.Value;

            if(idBill == -1)
            {
                // Tạo một hóa đơn mới
                BillDAO.Instance.InsertBill(table.ID);

                // Lấy idBill của hóa đơn mới được tạo
                int newBillID = BillDAO.Instance.GetMaxIDBill();

                // Chèn dữ liệu vào bảng BillInfo
                BillInfoDAO.Instance.InsertBillInfo(newBillID, foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);
            LoadTable();

        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            TableDTO table = lsvBill.Tag as TableDTO;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;
           
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = (totalPrice *1000) - ((totalPrice *1000) * discount) /100 ;

            if (idBill != 1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.Checkout(idBill, discount,(float)finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();

                }
            }
        }

        #endregion
    }
}
