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
    public partial class Form2 : Form
    {
        string DocContent;
        Steganalyse s = new Steganalyse();
        object ObjSave = false;

        public Form2()
        {
            InitializeComponent();
        }

        private void BtnOpen_Click(object sender, EventArgs e)
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
                    Microsoft.Office.Interop.Word.ApplicationClass App = new Microsoft.Office.Interop.Word.ApplicationClass();
                    object Obj = System.Reflection.Missing.Value;
                    object ObjName = openFileDialog1.FileName;
                    object ObjROnly = false;
                    object ObjVisible = false;
                    Microsoft.Office.Interop.Word.Document doc = App.Documents.Open(ref ObjName, ref Obj, ref ObjROnly, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref Obj, ref ObjVisible, ref Obj, ref Obj, ref Obj, ref Obj);
                    doc.Activate();
                    DocContent = doc.Content.Text;
                    richTextBox1.Text = DocContent;
                    doc.Close(ref Obj, ref Obj, ref Obj);
                    App.Quit(ref ObjSave, ref Obj, ref Obj);
                    //((Microsoft.Office.Interop.Word._Document)doc).Close(ref Obj, ref Obj, ref Obj);
                    //((Microsoft.Office.Interop.Word._Application)App).Quit(ref Obj, ref Obj, ref Obj);
                    #endregion
                }
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           txtbinmsg.Text= s.GiveBinMsg(txtkey.Text.Trim(), DocContent);
           txtmsg.Text = s.GiveMsg(txtbinmsg.Text);
        }
    }
}
