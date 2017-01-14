using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThunderMoveCompleteFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //取得Setting的預設值
            textBox1.Text = (Properties.Settings.Default["DownloadPath"]??"").ToString();
            textBox2.Text = (Properties.Settings.Default["CompletePath"]??"").ToString();
            numSize.Value =int.Parse((Properties.Settings.Default["Size"] ?? "0").ToString());
        }
        //下載目錄Brows按鈕
        private void btnBrows1_Click(object sender, EventArgs e)
        {
            textBox1.Text = GetBrowsePatch();
            Properties.Settings.Default["DownloadPath"] = textBox1.Text;
            Properties.Settings.Default.Save();
        }
        //已完成目錄Browse按鈕
        private void btnBrows2_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetBrowsePatch();
            Properties.Settings.Default["CompletePath"] = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        public string GetBrowsePatch() {
            string folderPath = "";
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
            }
            return folderPath;
        }

        //按下啟動鈕
        private void btnStart_Click(object sender, EventArgs e)
        {
            //儲存檔案大小的值
            Properties.Settings.Default["Size"] = numSize.Value;
            Properties.Settings.Default.Save();


            if (!Directory.Exists(textBox1.Text)|| !Directory.Exists(textBox2.Text))
            {
                MessageBox.Show("請選擇正確的下載路徑和已完成路徑!!");
                return;
            }
                DirectoryInfo di = new DirectoryInfo(textBox1.Text);
                //取得下載資料夾裡所有的資料夾
                DirectoryInfo[] Directorys = di.GetDirectories();

                //迴圈跑所有的子資料夾
                foreach (DirectoryInfo element in Directorys)
                {
                    //取得子資料夾裡所有的檔
                    var files = element.GetFiles("*", SearchOption.AllDirectories);
                    var fileSize = numSize.Value * 1024 * 1024;//MB轉成bytes
                    //如果資料夾裡有大於指定MB的檔 且都沒有.xltd副檔名 就搬移資料夾
                    if (files.Where(x => x.Length > fileSize).Any(x => x.Extension != ".xltd"))
                    {
                        //將資料夾從原本路徑搬到已完成資料夾
                        Directory.Move(element.FullName, textBox2.Text+"\\"+element.Name);
                    }
                }
            
        }
    }
}
