using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1.DTO
{
    public class BillInfoDTO
    {
        public BillInfoDTO(int id, int billID, int foodID, int count)
        {
            this.ID = id;
            this.BillId = billID;
            this.FoodID = foodID;
            this.Count = count;
        }

        public BillInfoDTO(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillId = (int)row["idbill"];
            this.FoodID = (int)row["idfood"];
            this.Count = (int)row["count"];
        }

        public List<BillInfoDTO> GetListBillInfo(int id)
        {
            List<BillInfoDTO> listBillInfo = new List<BillInfoDTO>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill = " + id);

            foreach (DataRow item in data.Rows)
            {
                BillInfoDTO info = new BillInfoDTO(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo", new object[] { idBill, idFood, count });
        }

        private int iD;
        private int billId;
        private int foodID;
        private int count;

        public int ID { get => iD; set => iD = value; }
        public int FoodID { get => foodID; set => foodID = value; }
        public int BillId { get => billId; set => billId = value; }
        public int Count { get => count; set => count = value; }
    }
}
