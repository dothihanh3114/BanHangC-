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

namespace BAI4
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data Source=HANH\\HANH;Initial Catalog=QLBH_C#;Integrated Security=True");

        private void LoadKhachHang()
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM Table_KHACHHANG";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbbKH.DataSource = dt;
                cbbKH.DisplayMember = "tenKH"; 
                cbbKH.ValueMember = "maKH"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu khách hàng: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadNhanVien()
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM Table_NHANVIEN";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbbNhanVien.DataSource = dt;
                cbbNhanVien.DisplayMember = "tenNV"; 
                cbbNhanVien.ValueMember = "maNV"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadHangHoa()
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM tbl_HANGHOA";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbbTenHang.DataSource = dt;
                cbbTenHang.DisplayMember = "tenHang";
                cbbTenHang.ValueMember = "maHang"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu hàng hóa: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void UpdateTongTien()
        {
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dGV_cthd.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    tongTien += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
                }
            }

            txtTongTien.Text = tongTien.ToString();
        }


        private void UpdateSTT()
        {
            for (int i = 0; i < dGV_cthd.Rows.Count; i++)
            {
                dGV_cthd.Rows[i].Cells["STT"].Value = i + 1; // Cập nhật cột STT với giá trị i+1
            }
        }

        private void UpdateTienThua()
        {
            try
            {
                if (decimal.TryParse(txtKhachDua.Text, out decimal tienKhachDua) && decimal.TryParse(txtTongTien.Text, out decimal tongTien))
                {
                    decimal tienThua = tienKhachDua - tongTien;
                    txtTienThua.Text = tienThua.ToString();
                }
                else
                {
                    txtTienThua.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính tiền thừa: " + ex.Message);
            }
        }

        private void cbbTenHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbTenHang.SelectedValue != null)
            {
                string maHang = cbbTenHang.SelectedValue.ToString();

                try
                {
                    conn.Open();
                    string query = "SELECT donGia, donVT FROM tbl_HANGHOA WHERE maHang = @maHang";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maHang", maHang);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtDonGia.Text = reader["donGia"].ToString();
                        txtDVT.Text = reader["donVT"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu đơn giá và đơn vị tính: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void tangma()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=HANH\\HANH;Initial Catalog=QLBH_C#;Integrated Security=True"))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT MAX(maHD) FROM tbl_HOADON", conn);
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        string ma = result.ToString();
                        int indexStart = ma.IndexOfAny("0123456789".ToCharArray());
                        if (indexStart != -1)
                        {
                            string numberPart = ma.Substring(indexStart);
                            int index = Convert.ToInt32(numberPart) + 1;
                            txtMaHD.Text = "HD" + index;
                        }
                        else
                        {
                            txtMaHD.Text = "HD1";
                        }
                    }
                    else
                    {
                        txtMaHD.Text = "HD1";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tăng mã nhân viên: " + ex.Message);
            }
        }




        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void Form4_Load(object sender, EventArgs e)
        {
            txtMaHD.Enabled = false;
            LoadKhachHang(); 
            LoadNhanVien();
            LoadHangHoa();
            tangma();
            cbbTenHang.SelectedIndexChanged += new EventHandler(cbbTenHang_SelectedIndexChanged);
        }

        private void danhSáchNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }

        private void danhSáchKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f1 = new Form2();
            f1.ShowDialog();
        }

        private void danhSáchHàngHóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f1 = new Form3();
            f1.ShowDialog();
        }

        private void btnThemHang_Click(object sender, EventArgs e)
        {
            try
            {
                string maHang = cbbTenHang.SelectedValue.ToString(); 
                string tenHang = cbbTenHang.Text;
                int soLuong = int.Parse(txtSL.Text);
                decimal donGia = decimal.Parse(txtDonGia.Text); 
                decimal thanhTien = soLuong * donGia;

                dGV_cthd.Rows.Add("",maHang, tenHang, soLuong, donGia, thanhTien);
                UpdateSTT();
                UpdateTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hàng: " + ex.Message);
            }
        }

        private void btnBotHang_Click(object sender, EventArgs e)
        {
            try
            {
                if (dGV_cthd.SelectedRows.Count > 0)
                {
                    dGV_cthd.Rows.RemoveAt(dGV_cthd.SelectedRows[0].Index);

                    UpdateSTT();
                    UpdateTongTien();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn hàng muốn bớt.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bớt hàng: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {

                conn.Open();
                string maHD = txtMaHD.Text;
                string maKH = cbbKH.SelectedValue.ToString();
                string maNV = cbbNhanVien.SelectedValue.ToString();
                DateTime ngayLap = DateTime.Parse(txtNgayLap.Text);

                string queryHD = "INSERT INTO tbl_HOADON (maHD, maKH, maNV, ngayLap) VALUES (@maHD, @maKH, @maNV, @ngayLap)";
                SqlCommand cmdHD = new SqlCommand(queryHD, conn);
                cmdHD.Parameters.AddWithValue("@maHD", maHD);
                cmdHD.Parameters.AddWithValue("@maKH", maKH);
                cmdHD.Parameters.AddWithValue("@maNV", maNV);
                cmdHD.Parameters.AddWithValue("@ngayLap", ngayLap);
                cmdHD.ExecuteNonQuery();

                foreach (DataGridViewRow row in dGV_cthd.Rows)
                {
                    if (row.Cells["maHang"].Value != null)
                    {
                        string maHang = row.Cells["maHang"].Value.ToString();
                        int soLuong = int.Parse(row.Cells["soLuong"].Value.ToString());
                        decimal donGia = decimal.Parse(row.Cells["donGia"].Value.ToString());

                        string queryCheckExist = "SELECT COUNT(*) FROM tbl_CHITIETHOADON WHERE maHD = @maHD AND maHang = @maHang";
                        SqlCommand cmdCheckExist = new SqlCommand(queryCheckExist, conn);
                        cmdCheckExist.Parameters.AddWithValue("@maHD", maHD);
                        cmdCheckExist.Parameters.AddWithValue("@maHang", maHang);

                        int exists = (int)cmdCheckExist.ExecuteScalar();

                        if (exists == 0)
                        {
                            string queryCTHD = "INSERT INTO tbl_CHITIETHOADON (maHD, maHang, soLuong, donGia) VALUES (@maHD, @maHang, @soLuong, @donGia)";
                            SqlCommand cmdCTHD = new SqlCommand(queryCTHD, conn);
                            cmdCTHD.Parameters.AddWithValue("@maHD", maHD);
                            cmdCTHD.Parameters.AddWithValue("@maHang", maHang);
                            cmdCTHD.Parameters.AddWithValue("@soLuong", soLuong);
                            cmdCTHD.Parameters.AddWithValue("@donGia", donGia);
                            cmdCTHD.ExecuteNonQuery();
                        }
                    }
                }


                MessageBox.Show("Thanh toán thành công!");
                xoaText();
                tangma();
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void xoaText()
        {
            dGV_cthd.Rows.Clear();
            txtTongTien.Clear();
            txtKhachDua.Clear();
            txtTienThua.Clear();
            txtDonGia.Clear();
            txtDVT.Clear();
        }

        private void txtKhachDua_TextChanged(object sender, EventArgs e)
        {
            UpdateTienThua();
        }

        private void tHỐNGKÊToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 f1 = new Form5();
            f1.ShowDialog();
        }
    }
}
