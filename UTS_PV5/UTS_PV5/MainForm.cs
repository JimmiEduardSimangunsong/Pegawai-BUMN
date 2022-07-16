/*
 * Created by SharpDevelop.
 * User: User
 * Date: 24/05/2022
 * Time: 11:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;



namespace UTS_PV5
{
	
	public partial class MainForm : Form
	{
		MySqlConnection co = new MySqlConnection("Server=localhost; Database=bumn; Uid=root");
		string connectionSQL = "Server=localhost; Database=bumn; Uid=root";
		Bitmap img;
		string pathing;
		string SourceFilePath;
		string inputid;
		int intinputid;
		
 		MySqlCommand mycommand = new MySqlCommand();
		MySqlDataAdapter myadapter = new MySqlDataAdapter();
		String id,nama,jabatan,notelp,alamat;
		Boolean status = false;
		
		
		
		
//		public void ReadData(){
//		 try{
//		 mycommand.Connection = co;
//		 myadapter.SelectCommand = mycommand;
//		 mycommand.CommandText = "select * FROM tbl_pegawai";
//		 DataSet ds = new DataSet();
//		 
//		 if (myadapter.Fill(ds,"tbl_pegawai")>0){
//		 dataGridView1.DataSource = ds;
//		 dataGridView1.DataMember = "tbl_pegawai";
//		 dataGridView1.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
//		 }
//		 }
//		 catch (Exception ex){
//		 MessageBox.Show(ex.ToString());
//		 }
//		}
		
		
		//insert data
		public void InsertData(){
		 try{
				mycommand.Parameters.Clear();
				
		 mycommand.Connection=co;
		 mycommand.CommandText="insert into tbl_pegawai values('"+txtid.Text+"','"+txtnama.Text+"','"+cbxjabatan.Text+"','"+txtnotelp.Text+"','"+txtalamat.Text+"',?Picture)";
		 myadapter.SelectCommand= mycommand;
		 System.IO.FileStream fs = new System.IO.FileStream(pathing, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				Byte[] b = new byte[fs.Length];
				fs.Read(b, 0, b.Length);
				fs.Close();
				MySqlParameter P = new MySqlParameter("?Picture", MySqlDbType.LongBlob, b.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, b);
				mycommand.Parameters.Add(P);
		 if (mycommand.ExecuteNonQuery()==1){
		 MessageBox.Show("Data berhasil dimasukan","Informasi",MessageBoxButtons.OK,MessageBoxIcon.Information);
		 }ReadData();
				
		 }
		 catch(Exception ex){
		 MessageBox.Show(ex.ToString());
		 
		 }
			status = false;
		}
		
		//update data
		public void UpdateData(){
		 try{
				mycommand.Parameters.Clear();
				
		 mycommand.Connection=co;
		 mycommand.CommandText = "update tbl_pegawai set id_pegawai='"+txtid.Text+"',nama_pegawai='"+txtnama.Text+"',jabatan ='"+cbxjabatan.Text+"',no_telp='"+txtnotelp.Text+"',alamat='"+txtalamat.Text+"',data_file=?gambar where id_pegawai ='"+txtid.Text+"'";
		 myadapter.SelectCommand = mycommand;
		 System.IO.FileStream fs = new System.IO.FileStream(pathing, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				Byte[] b = new byte[fs.Length];
				fs.Read(b, 0, b.Length);
				fs.Close();
				MySqlParameter P = new MySqlParameter("?gambar", MySqlDbType.LongBlob, b.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, b);
				mycommand.Parameters.Add(P);
		 if (mycommand.ExecuteNonQuery()==1){
		 MessageBox.Show("Data berhasil diupdate","Informasi",MessageBoxButtons.OK,MessageBoxIcon.Information);
		 }ReadData();
		 }
		 catch(Exception ex){
		 MessageBox.Show(ex.ToString());
		 }
			status = false;
		}
		
		
		//delete data
		public void DeleteData(){
		 try{
			mycommand.Connection=co;
		 mycommand.CommandText="delete from tbl_pegawai where id_pegawai='"+txtid.Text+"'";
		 myadapter.SelectCommand = mycommand;
		 if (mycommand.ExecuteNonQuery()==1){
		 MessageBox.Show("Data berhasil dihapus","Informasi",MessageBoxButtons.OK,MessageBoxIcon.Information);
		 ReadData();
		 }
		 }
		 catch(Exception ex)
		 {
		 MessageBox.Show(ex.ToString());
		 } 
		}
		
		

		public MainForm()
		{
			InitializeComponent();
			co.Open();
 			ReadData();
		}
		void Button2Click(object sender, EventArgs e)
		{
			if (status){
				UpdateData();
				
			} else {
				InsertData();
			}
		}
		void Button4Click(object sender, EventArgs e)
		{	
			status = true;
			tabControl1.SelectedTab = tabPage1;
			
		}
		void Button5Click(object sender, EventArgs e)
		{
			DeleteData();

		}
		void DataGridView1CellClick(object sender, DataGridViewCellEventArgs e)
		{
			txtid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
			txtnama.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
			cbxjabatan.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
			txtnotelp.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
			txtalamat.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
			var data = (Byte[])(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
			var stream = new MemoryStream(data);
			pictureBox1.Image= Image.FromStream(stream);
	
		}
		void Button1Click(object sender, EventArgs e)
		{
			txtid.Text = "";
			txtnama.Text = "";
			cbxjabatan.Text = "";
			txtnotelp.Text = "";
			txtalamat.Text = "";
			pictureBox1.Image = null;			
		}
		void Button3Click(object sender, EventArgs e)
		{
			if (DialogResult.OK ==openFileDialog1.ShowDialog())
			{
				img = new Bitmap(openFileDialog1.FileName);
				pictureBox1.Image=img;
				pathing=openFileDialog1.FileName.ToString();
			}
		}
		
		
		void Button7Click(object sender, EventArgs e)
		{
			try{
				MySqlConnection koneksi = new MySqlConnection(connectionSQL);
				koneksi.Open();
				MySqlDataAdapter dataAdapter = new MySqlDataAdapter(new MySqlCommand("Select data_file FROM tbl_pegawai WHERE id_pegawai ="+textBox1.Text, koneksi));
				DataSet dataSet = new DataSet();
				dataAdapter.Fill(dataSet);
				
				if (dataSet.Tables[0].Rows.Count == 1){
					Byte[] data = new Byte[0];
					data = (Byte[])(dataSet.Tables[0].Rows[0]["data_file"]);
					MemoryStream mem = new MemoryStream(data);
					pictureBox2.Image= Image.FromStream(mem);
				}
				koneksi.Close();
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}
		void Button8Click(object sender, EventArgs e)
		{
			DialogResult dr = saveFileDialog1.ShowDialog();
			if (dr == DialogResult.OK){
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Filter = "Images|.png;.bmp;*.jpg";
				ImageFormat format = ImageFormat.Png;
				if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK){
					string ext = System.IO.Path.GetExtension(sfd.FileName);
					switch (ext){
						case ".jpg":
							format = ImageFormat.Jpeg;
							break;
						case ".bmp":
							format = ImageFormat.Bmp;
							break;
					}
					pictureBox2.Image.Save(sfd.FileName, format);
				}
			}
		}
		
		
		private int incrementValueId(){
			int temp = 0;
			try{
				MySqlConnection db = new MySqlConnection(connectionSQL);
				db.Open();
				MySqlCommand dbcmd = db.CreateCommand();
				string sql = "select max(ID) from tbl_pegawai";
				dbcmd.CommandText = sql;
				MySqlDataReader reader = dbcmd.ExecuteReader();
				while (reader.Read()){
					if(!reader.IsDBNull(0)){
						return Convert.ToInt16(reader.GetString(0))+1;
					}
				}
				return 0;
			}
			catch (MySqlException error){
				MessageBox.Show(error.ToString());
				return 0;
			}
		}
		
		public void ReadData(){
			MySqlConnection koneksi = new MySqlConnection(connectionSQL);
			koneksi.Open();
			DataTable dt = new DataTable();
			MySqlDataAdapter da = new MySqlDataAdapter("Select* from tbl_pegawai", koneksi);
			da.Fill(dt);
			dataGridView1.DataSource = dt.DefaultView;
			koneksi.Close();
		}
		void Label8Click(object sender, EventArgs e)
		{
	
		}
		void MainFormLoad(object sender, EventArgs e)
		{
	
		}
		
		
		
	
		
	}
}
