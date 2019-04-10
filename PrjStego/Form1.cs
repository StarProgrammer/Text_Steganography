using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace PrjStego
{
    public partial class Form1 : Form
    {
        Microsoft.Office.Interop.Word.ApplicationClass App;
        Microsoft.Office.Interop.Word.Document doc;
        string DocContent;
        object Obj = System.Reflection.Missing.Value;
        object ObjSave = false;
        bool save;
        Steganography s = new Steganography();
       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
              openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "ٌWord Document(*.DOC;*.DOCX)|*.DOC;*.DOCX|Text Files(*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                #region txtFile
                if (openFileDialog1.FilterIndex == 2)
                {
                    DocContent = System.IO.File.ReadAllText(openFileDialog1.FileName, Encoding.UTF8);
                    richTextBox1.Text = DocContent;
                } 
                #endregion
                else
                {
                    #region WordFile
                    App = new Microsoft.Office.Interop.Word.ApplicationClass();
                    object Obj = System.Reflection.Missing.Value;
                    object ObjName = openFileDialog1.FileName;
                    object ObjROnly = false;
                    
                    //object ObjEncode = (object)Encoding.UTF8;
                    object ObjVisible = false;
                   
                    doc = App.Documents.Open(ref ObjName, ref Obj, ref ObjROnly, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref ObjVisible, ref Obj, ref Obj, ref Obj, ref Obj);
                    doc.Activate();
                    //doc1 = App.ActiveDocument;
                    DocContent = doc.Content.Text;

                    //doc.ActiveWindow.Selection.WholeStory();
                    //doc.ActiveWindow.Selection.Copy();
                    //IDataObject data = Clipboard.GetDataObject();
                    //DocContent = data.GetData(DataFormats.UnicodeText).ToString();

                    //for(int i=0;i<doc.ActiveWindow.Selection.
                    richTextBox1.Text = DocContent;
                    //((Microsoft.Office.Interop.Word._Document)doc).Close(ref Obj, ref Obj, ref Obj);
                    //((Microsoft.Office.Interop.Word._Application)App).Quit(ref Obj, ref Obj, ref Obj);
                    //((Microsoft.Office.Interop.Word._Document)doc).Close(ref Obj, ref Obj, ref Obj);
                    //App.Quit(ref Obj, ref Obj, ref Obj);

                    #endregion
                }
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void BtnBin_Click(object sender, EventArgs e)
        {

            txtBinMsg.Text = s.ProduceBinCode(txtMsg.Text.Trim());
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            //richTextBox2.Text = "ب" + "\u200d" + "\u200d" + "ی";
            saveFileDialog1.Filter = "Word Document(*.docx)|*.docx|Word 97-2003 Document(*.doc)|*.doc";
            saveFileDialog1.FileName = "Stego.docx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                object filename = (object)saveFileDialog1.FileName;
                //Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                //wordApp.Visible = true;
             
                //Microsoft.Office.Interop.Word.Document wordDoc = wordApp.Documents.Add(ref objMissing, ref objMissing, ref objMissing, ref objMissing);
                //wordDoc.Paragraphs.LineSpacing = 8;
                //Microsoft.Office.Interop.Word.Paragraph wordParagraph = wordDoc.Paragraphs.Add(ref objMissing);
                doc.Content.Text = richTextBox2.Text;
                doc.SaveAs(ref filename, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj);
            //doc.Close(ref Obj, ref Obj, ref Obj);
                save = true;
                ((Microsoft.Office.Interop.Word._Document)doc).Close(ref Obj, ref Obj, ref Obj);
                //App.Quit(ref ObjSave, ref Obj, ref Obj);
            }
        }

        private void BtnHide_Click(object sender, EventArgs e)
        {
            //richTextBox2.Text="ق"+"\u200c"+"\u200c"+"\u200c"+"ب";



            richTextBox2.Text = s.SetStego(txtBinMsg.Text, DocContent);
            txtkey.Text = s.SetKey();

            txtCoverlen.Text = DocContent.Length.ToString();
            txtMsglen.Text = (Convert.ToInt32(txtkey.Text) / 3).ToString();
            txtpercent.Text = s.GivePercent();
        
            if (s.wrong)
                MessageBox.Show("طول رمز بیشتر از حد مجاز است", "خطا");
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!save)
                ((Microsoft.Office.Interop.Word._Document)doc).Close(ref Obj, ref Obj, ref Obj);
            App.Quit(ref ObjSave, ref Obj, ref Obj);
                
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ((Microsoft.Office.Interop.Word._Document)doc).Close(ref Obj, ref Obj, ref Obj);
            textBox1.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }

      
    }
}