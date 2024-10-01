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
using System.Data.SqlClient;

namespace BAI4
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        //SqlConnection conn = new SqlConnection("");

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void btnTKe_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = txtTuNgay.Value;
            DateTime denNgay = txtDenNgay.Value;

            string connectionString = "Data Source=HANH\\HANH;Initial Catalog=QLBH_C#;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                SELECT HD.maHD, HD.ngayLap, KH.tenKH, NV.tenNV, SUM(CT.donGia * CT.soLuong) AS tongTien
                FROM tbl_HOADON HD
                JOIN tbl_CHITIETHOADON CT ON HD.maHD = CT.maHD
                JOIN Table_KHACHHANG KH ON HD.maKH = KH.maKH
                JOIN Table_NHANVIEN NV ON HD.maNV = NV.maNV
                WHERE HD.ngayLap BETWEEN @TuNgay AND @DenNgay
                GROUP BY HD.maHD, HD.ngayLap, KH.tenKH, NV.tenNV;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dt.Columns.Add("STT", typeof(int));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["STT"] = i + 1; // STT bắt đầu từ 1
                }

                dGV_dsHD.DataSource = dt;

                decimal tongCong = 0;
                foreach (DataRow row in dt.Rows)
                {
                    tongCong += Convert.ToDecimal(row["tongTien"]);
                }

                txtTongCong.Text = tongCong.ToString("N0"); // Format theo kiểu số
            }
        }


    }
}
