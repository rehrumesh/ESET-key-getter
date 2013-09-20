using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsetKeys
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lbl1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                lbl1.Text = "Please wait. Data is fetching from the server";
                gv.DataSource = null;
                var source = new BindingSource();
                KeyList kl = getKeys();
                source.DataSource = kl.Kies;
                gv.DataSource = source;
                gv.Columns[0].Width = 150;
                lbl1.Text = "";
            }
            catch (Exception ex) {
                MessageBox.Show("Error Occured.Please try again later. And make sure to check your internet connection.");
                lbl1.Text = "Connection error.";
            }
        }

        private KeyList getKeys()
        {
            WebClient wc = new WebClient();
            string httpRawData = wc.DownloadString(new Uri("http://anonymouse.org/cgi-bin/anon-www.cgi/http://www.nod325.com", UriKind.Absolute));
            string[] words = Regex.Split(httpRawData, "<p>Username:");

            KeyList kl = new KeyList();
            
            for (int i = 1; i < words.Length;i++ )
            {
                extractKeys(words[i], ref kl);
            }
            
            return kl;
        }

        private void extractKeys(string s,ref KeyList tmp) {
            string[] out1 = Regex.Split(s,"<br />");
            string Username = out1[0];
            string[] out2 = Regex.Split(out1[1],"</p>");
            string pass = Regex.Split((Regex.Split(out2[0], "\r\n"))[1], "Password:")[1];

            KeyPair k = new KeyPair();
            k.Username = Username;
            k.Password = pass;

            tmp.Kies.Add(k);

        }
    }

    class KeyPair {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    class KeyList {
        public List<KeyPair> Kies { get; set; }

        public KeyList() {
            Kies = new List<KeyPair>();
        }
    }
}
