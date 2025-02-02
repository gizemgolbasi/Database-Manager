using DatabaseManager.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DatabaseManager
{
    public partial class DatabaseListForm : Form
    {
        string connectStr = "";
        public DatabaseListForm(string connectionString)//connectionstring parametresi form1de bağlan butonunun click olayınca veriliyor burada da kullanılıyor
        {

            InitializeComponent();

            lstDbName.DrawMode = DrawMode.OwnerDrawFixed;
            lstDbName.ItemHeight = 25; // Satır yüksekliği (satır aralığını artırmak için)
            lstDbName.DrawItem += new DrawItemEventHandler(lstDbName_DrawItem);

            lstDbField.DrawMode = DrawMode.OwnerDrawFixed;
            lstDbField.ItemHeight = 25; // Satır yüksekliği (satır aralığını artırmak için)
            lstDbField.DrawItem += new DrawItemEventHandler(lstField_DrawItem);
            //            SELECT TABLE_NAME
            //FROM INFORMATION_SCHEMA.TABLES
            //WHERE TABLE_TYPE = 'BASE TABLE';

            DataTable dt = null;
            string sqlStr = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_TYPE = 'BASE TABLE' Order By TABLE_NAME ASC";
            connectStr = connectionString;//diğer metotlarda da connection string kullanabilmesi için methot dışına string olarak connection string tanımlandı ve bu gelen parametreyle eşitlendi.

            using (MSSQLHelper db = new MSSQLHelper(connectionString))
            {
                db.ExecuteReader(ref dt, sqlStr);
            }
            if (dt != null)
            {

                foreach (DataRow row in dt.Rows)
                {

                    lstDbName.Items.Add(row["TABLE_NAME"].ToString());

                }
            }


        }
        private void lstDbName_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Arka planı boyama
            e.DrawBackground();

            // Yazı rengini ayarla
            Brush textBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Brushes.White : Brushes.Black;

            // Metni çiz
            e.Graphics.DrawString(lstDbName.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds);

            // Çerçeve çiz (isteğe bağlı)
            e.DrawFocusRectangle();
        }
        private void lstField_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Arka planı boyama
            e.DrawBackground();

            // Yazı rengini ayarla
            Brush textBrush = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Brushes.White : Brushes.Black;

            // Metni çiz
            e.Graphics.DrawString(lstDbField.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds);

            // Çerçeve çiz (isteğe bağlı)
            e.DrawFocusRectangle();
        }
        private void lstDbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = lstDbName.SelectedItem.ToString();
            DataTable td = null;
            string sqlStr = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{selectedItem}'";
            using (MSSQLHelper db = new MSSQLHelper(connectStr))
            {
                db.ExecuteReader(ref td, sqlStr);
            }

            lstDbField.Items.Clear();
            foreach (DataRow row in td.Rows)
            {
                lstDbField.Items.Add(row["COLUMN_NAME"].ToString());
            }
            lblfieldName.Text = "Database Name:" + selectedItem + " Field Name";
            grpDescriptionKaydet.Visible = false;
        }

        private void lstDbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescription.Text = "";
            string selectedDbName = lstDbName.SelectedItem.ToString();
            string selectedField = lstDbField.SelectedItem.ToString();
            grpDescriptionKaydet.Visible = true;
            DataTable dt = null;
            string sqlStr = $"SELECT S.name as SchemaName,c.name ColumnName, O.name AS TableName, ep.name ExtendedProperty, ep.value AS ExtendedPropertyValue  FROM sys.extended_properties EP INNER JOIN sys.all_objects O ON ep.major_id = O.object_id INNER JOIN sys.schemas S on O.schema_id = S.schema_id INNER JOIN sys.columns AS c ON ep.major_id = c.object_id AND ep.minor_id = c.column_id WHERE TableName= '{selectedDbName}' AND ColumnName = '{selectedField}'";
            using (MSSQLHelper db = new MSSQLHelper(connectStr))
            {
                db.ExecuteReader(ref dt, sqlStr);
            }
            if (dt == null)
            {

            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    txtDescription.Text = row["ExtendedPropertyValue"].ToString();
                }


            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                //if (description == 0) {add} else {update}

                if (string.IsNullOrEmpty(txtDescription.Text))
                {
                    MessageBox.Show("Description alanı boş geçilemez");
                }
                else
                {
                    DataTable dt = null;

                    string selectedDbName = lstDbName.SelectedItem.ToString();
                    string selectedField = lstDbField.SelectedItem.ToString();
                    string sqlStr = $"SELECT S.name as SchemaName,c.name ColumnName, O.name AS TableName, ep.name ExtendedProperty, ep.value AS ExtendedPropertyValue  FROM sys.extended_properties EP INNER JOIN sys.all_objects O ON ep.major_id = O.object_id INNER JOIN sys.schemas S on O.schema_id = S.schema_id INNER JOIN sys.columns AS c ON ep.major_id = c.object_id AND ep.minor_id = c.column_id WHERE TableName= '{selectedDbName}' AND ColumnName = '{selectedField}'";
                    using (MSSQLHelper db = new MSSQLHelper(connectStr))
                    {
                        db.ExecuteReader(ref dt, sqlStr);

                        if (dt.Rows.Count == 0)
                        {
                            string sqlStt = $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{selectedDbName}'";
                            DataTable tableInfoDt = null;
                            db.ExecuteReader(ref tableInfoDt, sqlStt);
                            string table_Schema = "";
                            foreach (DataRow row in tableInfoDt.Rows)
                            {
                                table_Schema = row["TABLE_SCHEMA"].ToString();
                            }



                            if (string.IsNullOrEmpty(table_Schema))
                            {

                            }
                            else
                            {
                                string insertSp = $"EXEC sp_addextendedproperty 'MS_Description', N'{txtDescription.Text}', N'schema', N'{table_Schema}', N'table', N'{selectedDbName}', N'column',N'{selectedField}'";
                                db.ExecuteNonQuery(insertSp);
                                MessageBox.Show("Description Eklem İşlemi Başırılı Bir Şekilde Gerçekleştirilmiştir");

                                //olduğu durumda ekleme  sp'si çalışacak
                            }
                        }
                        else
                        {
                            string table_SchemaName = "";
                            foreach (DataRow row in dt.Rows)
                            {
                                table_SchemaName = row["SchemaName"].ToString();
                            }
                            //update sp'si çalışacak

                            if (string.IsNullOrEmpty(table_SchemaName))
                            {

                            }
                            else
                            {
                                string updateSp = $"EXEC sp_updateextendedproperty @name = N'MS_Description',  @value = N'{txtDescription.Text}', @level0type = N'SCHEMA', @level0name = N'{table_SchemaName}',@level1type = N'TABLE', @level1name = N'{selectedDbName}',@level2type = N'COLUMN', @level2name = N'{selectedField}'";
                                db.ExecuteNonQuery(updateSp);
                                MessageBox.Show("Description Güncelleme İşlemi Başarıyla Gerçekleştirilmiştir");
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Meydana Geldi Tekrardan Deneyiniz");
            }

        }

        private void btnPdfKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = null;
                List<DbDescription> DbDescriptionList = new List<DbDescription>();
                string sqlStr = $"SELECT S.name as SchemaName,c.name ColumnName, O.name AS TableName, ep.name ExtendedProperty, ep.value AS ExtendedPropertyValue  FROM sys.extended_properties EP INNER JOIN sys.all_objects O ON ep.major_id = O.object_id INNER JOIN sys.schemas S on O.schema_id = S.schema_id INNER JOIN sys.columns AS c ON ep.major_id = c.object_id AND ep.minor_id = c.column_id";
                using (MSSQLHelper db = new MSSQLHelper(connectStr))
                {
                    db.ExecuteReader(ref dt, sqlStr);
                }
                if (dt == null)
                {

                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DbDescription description = new DbDescription();
                        description.SchemaName = row["SchemaName"].ToString();
                        description.ColumnName = row["ColumnName"].ToString();
                        description.TableName = row["TableName"].ToString();
                        //description.ExtendedProperty = row["ExtendedProperty"].ToString();
                        description.ExtendedPropertyValue = row["ExtendedPropertyValue"].ToString();
                        DbDescriptionList.Add(description);
                    }
                }
                //string filePath = @"C:\Dosyalar\veriler.pdf";
                //using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                //{
                //    // PDF belge oluştur
                //    Document document = new Document();
                //    PdfWriter writer = PdfWriter.GetInstance(document, fs);

                //    // Belgeyi aç
                //    document.Open();

                //    // Listedeki her bir öğeyi PDF'ye ekleme
                //    foreach (var data in DbDescriptionList)
                //    {
                //        string paragraf = $"{data.SchemaName} {data.TableName} {data.ColumnName} {data.ExtendedPropertyValue}";

                //        // Yeni bir paragraf olarak her veriyi ekler
                //        document.Add(new Paragraph(paragraf));
                //    }

                //    // Belgeyi kapatma
                //    document.Close();
                //    writer.Close();

                //}


                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Dosyası|*.pdf";  // Sadece PDF dosyalarını kaydetmeyi sağlar
                saveFileDialog.Title = "PDF Dosyasını Kaydet";
                saveFileDialog.FileName = "DosyaAdi";  // Varsayılan dosya adı

                // Eğer kullanıcı bir dosya adı ve konum seçerse
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Dosya yolunu alıyoruz
                        string filePath = saveFileDialog.FileName;

                        // PDF dosyasını oluşturma işlemi
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();

                            // PDF içeriği ekleme (örnek)
                            foreach (var data in DbDescriptionList)
                            {
                                //string paragraf = $" Şema Adı: {data.SchemaName} Tablo Adı: {data.TableName} Tablo Sütun Adı: {data.ColumnName} Açıklama: {data.ExtendedPropertyValue}";

                                //// Yeni bir paragraf olarak her veriyi ekler
                                //pdfDoc.Add(new Paragraph(paragraf));
                                // iTextSharp.text.Font sınıfını kullanarak bir yazı tipi tanımlayın
                                string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

                                // BaseFont oluşturun
                                BaseFont bfArialUniCode = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                                // "Şema Adı" için farklı bir font ve renk ayarlayın
                                iTextSharp.text.Font schemaFont = new iTextSharp.text.Font(bfArialUniCode, 13, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                                // Diğer metin için varsayılan bir font ayarlayın
                                iTextSharp.text.Font defaultFont = new iTextSharp.text.Font(bfArialUniCode, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                                // Paragrafı parçalara ayırarak oluşturun
                                Paragraph paragraph = new Paragraph();

                                // "Şema Adı" kısmını kırmızı ve kalın yapıyoruz
                                paragraph.Add(new Chunk("Şema Adı: ", schemaFont));
                                // Diğer metin parçalarını varsayılan font ile ekliyoruz
                                paragraph.Add(new Chunk($"{data.SchemaName}"));
                                paragraph.Add(new Chunk(",Tablo Adı: ", schemaFont));
                                paragraph.Add(new Chunk($"{data.TableName}"));
                                paragraph.Add(new Chunk(",Tablo Sütun Adı: ", schemaFont));
                                paragraph.Add(new Chunk($"{data.ColumnName}"));
                                paragraph.Add(new Chunk(",Açıklama: ", schemaFont));
                                paragraph.Add(new Chunk($"{data.ExtendedPropertyValue}"));
                                paragraph.Add(new Chunk($"\n "));

                                //paragraph.Add(new Chunk($"{data.TableName}, Tablo Sütun Adı: ", defaultFont));
                                //paragraph.Add(new Chunk($"{data.ColumnName}, Açıklama: ", defaultFont));
                                //paragraph.Add(new Chunk($"{data.ExtendedPropertyValue}", defaultFont));
                                // Paragrafı PDF'e ekleyin
                                pdfDoc.Add(paragraph);
                            }


                            //pdfDoc.Add(new Paragraph("Bu bir örnek PDF dosyasıdır."));
                            pdfDoc.Close();
                            MessageBox.Show("Pdf Başarılı Bir Şekilde Oluşturulmuştur");
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = filePath,
                                UseShellExecute = true // Varsayılan PDF görüntüleyicisini kullan
                            });
                        }

                        //MessageBox.Show("PDF başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }




            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
                string error1 = ex.StackTrace.ToString();
                MessageBox.Show("Hata Meydana Geldi Lütfen Tekrar Deneyiniz");
            }
        }
    }

    //private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (listBox1.SelectedItem != null)
    //    {
    //        string selectedItem = listBox1.SelectedItem.ToString();

    //        // Seçilen öğeyi ikinci ListBox'a ekle
    //        listBox2.Items.Add(selectedItem);

    //        // Seçilen öğeyi ilk ListBox'tan çıkar (opsiyonel)
    //        listBox1.Items.Remove(selectedItem);
    //    }
    //}




}

