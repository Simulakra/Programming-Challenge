using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

namespace Static_Website_Generator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //btn_generate_and_open_Click(null, null);
        }

        public Variables site_variables = new Variables();

        private void btn_generate_and_open_Click(object sender, EventArgs e)
        {
            //first variables
            string startup_path = Application.StartupPath;
            get_variables();

            //prepare main files
            if(Directory.Exists(startup_path + @"\unzip_source"))
                Directory.Delete(startup_path + @"\unzip_source", true);
            ZipFile.ExtractToDirectory(startup_path + @"\breezycv.zip", startup_path + @"\unzip_source");
            if (Directory.Exists(startup_path + @"\generate"))
                Directory.Delete(startup_path + @"\generate", true);
            string folder_name = (site_variables.Theme == Variables.Themes.Light) ? "lightfw" : "darkfw";
            Directory.Move(startup_path + @"\unzip_source\breezycv\" + folder_name, startup_path + @"\generate");
            Directory.Delete(startup_path + @"\unzip_source", true);

            #region change variables in html file
            //get default site
            string full_HTML = File.ReadAllText(startup_path + @"\generate\index.html");

            //home page
            full_HTML = full_HTML.Replace("<title>BreezyCV - Resume / CV / vCard Template</title>", "<title>" + site_variables.SiteName + "</title>");
            full_HTML = full_HTML.Replace("Alex Smith", site_variables.SiteName);
            #endregion

            //save last HTML file
            File.Delete(startup_path + @"\generate\index.html");
            File.WriteAllText(startup_path + @"\generate\index.html", full_HTML);

            //all done :)
            MessageBox.Show("All site generation done","Done");
        }

        private void get_variables()
        {
            site_variables.Theme = radioButton1.Checked ? Variables.Themes.Light : Variables.Themes.Dark;
            site_variables.SiteName = textBox1.Text;
        }
    }
}
