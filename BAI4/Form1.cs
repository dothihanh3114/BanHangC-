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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data Source=HANH\\HANH;Initial Catalog=QLBH_C#;Integrated Security=True");
        int lastMaNV = 0;
        private bool kt = false;

        private void getData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlDataAdapter sa = new SqlDataAdapter("select * from Table_NHANVIEN", conn);
                DataTable dt = new DataTable();
                sa.Fill(dt);
                dGV_dsNV.DataSource = dt;
                bingding();

                if (dt.Rows.Count > 0)
                {
                    var lastRow = dt.Rows[dt.Rows.Count - 1];
                    string lastMaNVStr = lastRow["maNV"].ToString();
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
            txtMaNV.DataBindings.Add("Text", dGV_dsNV.DataSource, "maNV");
            txtTenNV.DataBindings.Clear();
            txtTenNV.DataBindings.Add("Text", dGV_dsNV.DataSource, "tenNV");
            txtNgaySinh.DataBindings.Clear();
            txtNgaySinh.DataBindings.Add("Text", dGV_dsNV.DataSource, "ngaySinh");
            txtDiaChi.DataBindings.Clear();
            txtDiaChi.DataBindings.Add("Text", dGV_dsNV.DataSource, "diaChi");
            txtDienThoai.DataBindings.Clear();
            txtDienThoai.DataBindings.Add("Text", dGV_dsNV.DataSource, "dienThoai");
            cbbGioiTinh.DataBindings.Clear();
            cbbGioiTinh.DataBindings.Add("Text", dGV_dsNV.DataSource, "gioiTinh");
        }

        private void LoadGioiTinhData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlDataAdapter saGioiTinh = new SqlDataAdapter("SELECT DISTINCT gioiTinh FROM Table_NHANVIEN", conn);
                DataTable dtGioiTinh = new DataTable();
                saGioiTinh.Fill(dtGioiTinh);

                cbbGioiTinh.DataSource = dtGioiTinh;
                cbbGioiTinh.DisplayMember = "gioiTinh";
                cbbGioiTinh.ValueMember = "gioiTinh"; 
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

        private void xoaText() {
            txtTenNV.Clear();
            cbbGioiTinh.SelectedIndex = -1;
            txtDiaChi.Clear();
            txtDienThoai.Clear();

            lastMaNV++;
            txtMaNV.Text = "NV" + lastMaNV.ToString();
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
                string query = "INSERT INTO Table_NHANVIEN (maNV, tenNV, ngaySinh, gioiTinh, dienThoai, diaChi) " +
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
                string query = "UPDATE Table_NHANVIEN SET tenNV = N'" + txtTenNV.Text + "', " +
                               "ngaySinh = '" + ngaySinh + "', " +
                               "gioiTinh = '" + cbbGioiTinh.SelectedValue + "', " +
                               "dienThoai = '" + txtDienThoai.Text + "', " +
                               "diaChi = N'" + txtDiaChi.Text + "' " +
                               "WHERE maNV = '" + txtMaNV.Text + "'";

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

                string query = "DELETE FROM Table_NHANVIEN WHERE maNV = '" + txtMaNV.Text + "'";

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


        private void Form1_Load(object sender, EventArgs e)
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
