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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=HANH\\HANH;Initial Catalog=QLBH_C#;Integrated Security=True");
        int lastMaNV = 0;
        private bool kt = false;

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void getData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlDataAdapter sa = new SqlDataAdapter("select * from tbl_HANGHOA", conn);
                DataTable dt = new DataTable();
                sa.Fill(dt);
                dGV_dsNV.DataSource = dt;
                bingding();

                if (dt.Rows.Count > 0)
                {
                    var lastRow = dt.Rows[dt.Rows.Count - 1];
                    string lastMaNVStr = lastRow["maHang"].ToString();
                    lastMaNV = int.Parse(lastMaNVStr.Substring(2));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void bingding()
        {
            txtMaNV.DataBindings.Clear();
            txtMaNV.DataBindings.Add("Text", dGV_dsNV.DataSource, "maHang");
            txtTenNV.DataBindings.Clear();
            txtTenNV.DataBindings.Add("Text", dGV_dsNV.DataSource, "tenHang");
            txtNgaySinh.DataBindings.Clear();
            txtNgaySinh.DataBindings.Add("Text", dGV_dsNV.DataSource, "ngayNhap");
            txtDiaChi.DataBindings.Clear();
            txtDiaChi.DataBindings.Add("Text", dGV_dsNV.DataSource, "moTa");
            txtDienThoai.DataBindings.Clear();
            txtDienThoai.DataBindings.Add("Text", dGV_dsNV.DataSource, "donGia");
            cbbGioiTinh.DataBindings.Clear();
            cbbGioiTinh.DataBindings.Add("Text", dGV_dsNV.DataSource, "donVT");
        }

        private void LoadGioiTinhData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlDataAdapter saGioiTinh = new SqlDataAdapter("SELECT DISTINCT donVT FROM tbl_HANGHOA", conn);
                DataTable dtGioiTinh = new DataTable();
                saGioiTinh.Fill(dtGioiTinh);

                cbbGioiTinh.DataSource = dtGioiTinh;
                cbbGioiTinh.DisplayMember = "donVT";
                cbbGioiTinh.ValueMember = "donVT";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu giới tính: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void HienThongTin()
        {
            txtMaNV.Enabled = false;
            txtTenNV.Enabled = true;
            txtNgaySinh.Enabled = true;
            cbbGioiTinh.Enabled = true;
            txtDiaChi.Enabled = true;
            txtDienThoai.Enabled = true;

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;


        }

        private void AnThongTin()
        {
            txtMaNV.Enabled = false;
            txtTenNV.Enabled = false;
            txtNgaySinh.Enabled = false;
            cbbGioiTinh.Enabled = false;
            txtDiaChi.Enabled = false;
            txtDienThoai.Enabled = false;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;

            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void xoaText()
        {
            txtTenNV.Clear();
            cbbGioiTinh.SelectedIndex = -1;
            txtDiaChi.Clear();
            txtDienThoai.Clear();

            lastMaNV++;
            txtMaNV.Text = "MH" + lastMaNV.ToString();
        }

        private void them()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string ngaySinh = DateTime.Parse(txtNgaySinh.Text).ToString("yyyy-MM-dd");
                string query = "INSERT INTO tbl_HANGHOA (maHang, tenHang, ngayNhap, donVT, donGia, moTa) " +
                               "VALUES ('" + txtMaNV.Text + "', N'" + txtTenNV.Text + "', '" + ngaySinh + "', '" + cbbGioiTinh.SelectedValue + "', '" + txtDienThoai.Text + "', N'" + txtDiaChi.Text + "')";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm mới thành công!");
                getData(); // Refresh lại dữ liệu sau khi thêm mới
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }




        private void sua()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string ngaySinh = DateTime.Parse(txtNgaySinh.Text).ToString("yyyy-MM-dd");
                string query = "UPDATE tbl_HANGHOA SET tenHang = N'" + txtTenNV.Text + "', " +
                               "ngayNhap = '" + ngaySinh + "', " +
                               "donVT = '" + cbbGioiTinh.SelectedValue + "', " +
                               "donGia = '" + txtDienThoai.Text + "', " +
                               "moTa = N'" + txtDiaChi.Text + "' " +
                               "WHERE maHang = '" + txtMaNV.Text + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công!");
                getData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void xoa()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = "DELETE FROM tbl_HANGHOA WHERE maHang = '" + txtMaNV.Text + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công!");
                getData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            AnThongTin();
            LoadGioiTinhData();
            getData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            HienThongTin();
            xoaText();
            kt = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            HienThongTin();
            kt = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (kt)
            {
                them();
            }
            else
            {
                sua();
            }
            AnThongTin();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            AnThongTin();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            xoa();
        }
    }
}
