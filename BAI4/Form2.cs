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
    public partial class Form2 : Form
    {
        public Form2()
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

                SqlDataAdapter sa = new SqlDataAdapter("select * from Table_KHACHHANG", conn);
                DataTable dt = new DataTable();
                sa.Fill(dt);
                dGV_dsNV.DataSource = dt;
                bingding();

                if (dt.Rows.Count > 0)
                {
                    var lastRow = dt.Rows[dt.Rows.Count - 1];
                    string lastMaNVStr = lastRow["maKH"].ToString();
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
            txtMaKH.DataBindings.Clear();
            txtMaKH.DataBindings.Add("Text", dGV_dsNV.DataSource, "maKH");
            txtTenKH.DataBindings.Clear();
            txtTenKH.DataBindings.Add("Text", dGV_dsNV.DataSource, "tenKH");
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

                SqlDataAdapter saGioiTinh = new SqlDataAdapter("SELECT DISTINCT gioiTinh FROM Table_KHACHHANG", conn);
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
            txtMaKH.Enabled = false;
            txtTenKH.Enabled = true;
            txtNgaySinh.Enabled = true;
            cbbGioiTinh.Enabled = true;
            txtDiaChi.Enabled = true;
            txtDienThoai.Enabled = true;

            btnLuu.Visible = true;
            btnHuy.Visible = true;

            btnThem.Visible = false;
            btnSua.Visible = false;
            btnXoa.Visible = false;


        }

        private void AnThongTin()
        {
            txtMaKH.Enabled = false;
            txtTenKH.Enabled = false;
            txtNgaySinh.Enabled = false;
            cbbGioiTinh.Enabled = false;
            txtDiaChi.Enabled = false;
            txtDienThoai.Enabled = false;

            btnLuu.Visible = false;
            btnHuy.Visible = false;

            btnThem.Visible = true;
            btnSua.Visible = true;
            btnXoa.Visible = true;
        }

        private void xoaText()
        {
            txtTenKH.Clear();
            cbbGioiTinh.SelectedIndex = -1;
            txtDiaChi.Clear();
            txtDienThoai.Clear();

            lastMaNV++;
            txtMaKH.Text = "KH" + lastMaNV.ToString();
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
                string query = "INSERT INTO Table_KHACHHANG (maKH, tenKH, ngaySinh, gioiTinh, dienThoai, diaChi) " +
                               "VALUES ('" + txtMaKH.Text + "', N'" + txtTenKH.Text + "', '" + ngaySinh + "', '" + cbbGioiTinh.SelectedValue + "', '" + txtDienThoai.Text + "', N'" + txtDiaChi.Text + "')";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm mới thành công!");
                getData();
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
                string query = "UPDATE Table_KHACHHANG SET tenKH = N'" + txtTenKH.Text + "', " +
                               "ngaySinh = '" + ngaySinh + "', " +
                               "gioiTinh = '" + cbbGioiTinh.SelectedValue + "', " +
                               "dienThoai = '" + txtDienThoai.Text + "', " +
                               "diaChi = N'" + txtDiaChi.Text + "' " +
                               "WHERE maKH = '" + txtMaKH.Text + "'";

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

                string query = "DELETE FROM Table_KHACHHANG WHERE maKH = '" + txtMaKH.Text + "'";

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

        private void Form2_Load(object sender, EventArgs e)
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
