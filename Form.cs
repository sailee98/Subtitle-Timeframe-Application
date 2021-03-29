using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Globalization;

namespace TimeFrameAlert
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lblFail.Visible = false;
            lblSuccess.Visible = false;
            txtNum.Text = "10";
        }

        public string Formattime(string t)
        {
            
            string[] a = t.Split('.');
            // DateTime date = new DateTime();
            double sec = Convert.ToDouble(a[0]);
            // date.AddSeconds(sec);
            TimeSpan time = TimeSpan.FromSeconds(sec);
            string result = time.ToString(@"hh\:mm\:ss", DateTimeFormatInfo.InvariantInfo);
            string result1 = result + "," + a[1];
            return result1;

        }


        private void btngetText_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Text Files(*.txt;*.json) | *.txt;*.json|All Files(*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName.ToString();
                    txtJsonFileName.Text = path;
                }
                string st = File.ReadAllText(txtJsonFileName.Text);

                dynamic jsonObject = JObject.Parse(st);
                dynamic obj11 = jsonObject["results"]["items"];
               
                JObject my_obj = JsonConvert.DeserializeObject<JObject>(st);

                StringBuilder sb = new StringBuilder();
                string start_time = "";
                string end_time = "";
                string sentence = "";
                int n = 1;
                int t = 1;
                if (txtNum.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please input break line after");
                }
                int wtb = 10;



                int c = obj11.Count;
                for (int i = 0; i < c; i++)
                {
                    if (jsonObject.results.items[i].type == "pronunciation")
                    {
                        if (start_time == "")
                        start_time = jsonObject.results.items[i].start_time;
                        end_time = jsonObject.results.items[i].end_time;
                        sentence += jsonObject.results.items[i].alternatives[0].content + " ";
                        t++;
                    }
                    else if (jsonObject.results.items[i].type == "punctuation" && jsonObject.results.items[i].alternatives[0].content == ".")
                    {
                        double var1 = Convert.ToDouble(end_time);
                        string fend_time = Math.Round(var1, 2).ToString("00.00");
                       
                        if (start_time == "")
                        {

                           // string txt4 = sb.Append(Formattime("0.0") + " undefined " + " -- >  " + Formattime(fend_time)).ToString();
                            MessageBox.Show("Undefined on line " + n);
                            continue;

                        }
                        else
                        {
                            string txt2 = sb.Append(n).ToString();
                            string txt3 = sb.Append(Environment.NewLine).ToString();
                            string txt5 = sb.Append(Formattime(start_time) + " --> " + Formattime(fend_time)).ToString();
                        }
                        string txt6 = sb.Append(Environment.NewLine).ToString();
                        string txt7 = sb.Append(sentence).ToString();
                        string txt8 = sb.Append(Environment.NewLine).ToString();
                        string txt9 = sb.Append(Environment.NewLine).ToString();
                        string wholetxt = string.Format("{0}", txt9);
                       // txtOutputFileName.Text = Path.GetFileName(openFD.FileName);
                       // File.WriteAllText(@"D:\outputfile.txt", wholetxt);
                        File.WriteAllText(@txtOutputFileName.Text, wholetxt);
                        sentence = "";
                        start_time = "";
                        n++;
                        t = 1;
                    }
                    if (t > wtb)
                    {
                        double var1 = Convert.ToDouble(end_time);

                        string fend_time = Math.Round(var1, 2).ToString("00.00");
                        string txt4 = sb.Append(n).ToString();
                        string txt5 = sb.Append(Environment.NewLine).ToString();
                        string txt6 = sb.Append(Formattime(start_time) + " --> " + Formattime(fend_time)).ToString();
                        string txt7 = sb.Append(Environment.NewLine).ToString();
                        string txt8 = sb.Append(sentence).ToString();
                        string txt9 = sb.Append(Environment.NewLine).ToString();
                        string txt10 = sb.Append(Environment.NewLine).ToString();
                        string wholetxt = string.Format("{0} ", txt10);
                        File.WriteAllText(@txtOutputFileName.Text, wholetxt);
                       // File.WriteAllText(@"D:\outputfile.txt", wholetxt);

                        sentence = "";
                        start_time = "";
                        n++;
                        t = 1;
                    }
                }
                lblSuccess.Visible= true;
            }


            catch (Exception er)
            {
                Console.WriteLine("File could not be read");
                lblFail.Visible = true;
                Console.WriteLine(er.Message);
            }
           
        }

        private void btnBrwse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFD = new CommonOpenFileDialog();
            openFD.IsFolderPicker = true;
            if (openFD.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtJsonFileName.Text = Path.GetFileName(openFD.FileName);
                MessageBox.Show("You selected: " + openFD.FileName);

            }
        }

   }
}

