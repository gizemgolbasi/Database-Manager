using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DatabaseManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtDbAd.Text) /*|| string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Text)*/)
            {
                MessageBox.Show("Hata Meydana Geldi, Lütfen Tekrardan Kontrol Edin.");

            }
            else
            {
                try
                {
                    //string connectionadress = "Server=" + txtId.Text + ";Database=" + txtDbAd.Text + ";User Id=" + txtUser.Text + ";Password=" + txtPass.Text + ";";
                    string connectionadress_ = $"Server={txtId.Text};Database={txtDbAd.Text};User Id={txtUser.Text};Password={txtPass.Text};";
                    //string connectionadress_ = $"Data Source={txtId.Text};Initial Catalog={txtDbAd.Text};Integrated Security=True";

                    //string connectionadress__ = String.Format("Server='{0}';Database='{1}';User Id='{2}';Password='{3}'",txtId.Text,txtDbAd.Text,txtUser.Text,txtPass.Text);
                    SqlConnection connection = new SqlConnection(connectionadress_);
                    Cursor.Current = Cursors.WaitCursor;
                    btnTest.Enabled = false;
                    connection.Open();
                    btnTest.Enabled = true;
                    connection.Close();
                    MessageBox.Show("Veritabanı Bağlantısı Başarılı");
                    btnBaglan.Enabled = true;

                }
                catch (Exception ex)
                {
                    btnTest.Enabled = true;
                    MessageBox.Show("Veritabanı Bağlantısı Sırasında Hata Meydana Geldi Lütfen Kontrol Edip Tekrardan Bağlanın", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Form modalForm = new Form();
                    // Özel bir buton ekleyelim
                    Button closeButton = new Button();
                    closeButton.Location = new Point(100, 50);
                    modalForm.Controls.Add(closeButton);

                }

            }



        }

        private void btnBaglan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtDbAd.Text) /*|| string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Text)*/)
            {
                MessageBox.Show("Hata Meydana Geldi, Lütfen Tekrardan Kontrol Edin.");

            }
            else
            {
                try
                {
                    //string connectionadress = "Server=" + txtId.Text + ";Database=" + txtDbAd.Text + ";User Id=" + txtUser.Text + ";Password=" + txtPass.Text + ";";
                    string connectionadress_ = $"Server={txtId.Text};Database={txtDbAd.Text};User Id={txtUser.Text};Password={txtPass.Text};";
                    //string connectionadress_ = $"Data Source={txtId.Text};Initial Catalog={txtDbAd.Text};Integrated Security=True";

                    //string connectionadress__ = String.Format("Server='{0}';Database='{1}';User Id='{2}';Password='{3}'",txtId.Text,txtDbAd.Text,txtUser.Text,txtPass.Text);
                    SqlConnection connection = new SqlConnection(connectionadress_);
                    btnBaglan.Enabled = false;
                    Cursor.Current = Cursors.WaitCursor;
                    connection.Open();
                    btnBaglan.Enabled = true;
                    DatabaseListForm dbf = new DatabaseListForm(connectionadress_);
                    dbf.Show();
                    this.Hide();
                    connection.Close();

                }
                catch (Exception ex)
                {
                    btnBaglan.Enabled = true;
                    MessageBox.Show("Veritabanı Bağlantısı Sırasında Hata Meydana Geldi Lütfen Kontrol Edip Tekrardan Bağlanın", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Form modalForm = new Form();
                    // Özel bir buton ekleyelim
                    Button closeButton = new Button();
                    closeButton.Text = "Kapat";
                    closeButton.Location = new Point(100, 50);
                    modalForm.Controls.Add(closeButton);
                }



            }
        }
    }
}
