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
            full_HTML = full_HTML.Replace("BreezyCV - Resume / CV / vCard Template", site_variables.SiteName);
            full_HTML = full_HTML.Replace("<meta name=\"author\" content=\"lmpixels\" />", "<meta name=\"author\" content=\""+ site_variables.SiteName + "\" />");
            full_HTML = full_HTML.Replace("Alex Smith", site_variables.SiteName);
            full_HTML = full_HTML.Replace("<span class=\"link - text\">Home</span>", "<span class=\"link - text\">"+site_variables.PageTitle_Home+"</span>");
            full_HTML = full_HTML.Replace("<a href=\"#\" target=\"_blank\" class=\"btn btn-primary\">Download CV</a>",
                "<a href=\"" + site_variables.DownloadLink + "\" target=\"_blank\" class=\"btn btn-primary\">" + site_variables.DownloadButton+"</a>");

            full_HTML = full_HTML.Replace("<li><a href=\"#\" target=\"_blank\"><i class=\"fab fa-linkedin-in\"></i></a></li>", "");
            full_HTML = full_HTML.Replace("<li><a href=\"#\" target=\"_blank\"><i class=\"fab fa-facebook-f\"></i></a></li>", "");
            full_HTML = full_HTML.Replace("<li><a href=\"#\" target=\"_blank\"><i class=\"fab fa-twitter\"></i></a></li>", "#link_here#");

            string social_links = "";
            foreach (var item in site_variables.Socials)
            {
                social_links += "<li><a href=\"" + item.Value + "\" target=\"_blank\"><i class=\"fab " +
                    Variables.SocialMediaIcons[item.Key] + "\"></i></a></li>";
            }
            full_HTML = full_HTML.Replace("#link_here#", social_links);

            if (site_variables.Titles.Count > 0)
            {
                //??????????????????????????
                string title_sets = "";
                foreach (var item in site_variables.Titles)
                {
                    title_sets += "<div class=\"item\"><div class=\"sp-subtitle\">"+item+"</div></div>";
                }
                full_HTML = full_HTML.Replace("<div class=\"item\">\n" +
                    "<div class=\"sp-subtitle\">Web Designer</div>\n" +
                    "</div>", "");
                full_HTML = full_HTML.Replace("<div class=\"item\">\n" +
                    "<div class=\"sp-subtitle\">Frontend-developer</div>\n" +
                    "</div>", "#title_set#");
                full_HTML = full_HTML.Replace("Web Designer", site_variables.Titles[0]);
                full_HTML = full_HTML.Replace("#title_set#", title_sets);

            }

            #endregion

            //save last HTML file
            File.Delete(startup_path + @"\generate\index.html");
            File.WriteAllText(startup_path + @"\generate\index.html", full_HTML);

            //all done :)
            MessageBox.Show("All site generation done","Done");
            System.Diagnostics.Process.Start(startup_path + @"\generate\index.html");
        }

        private void get_variables()
        {
            site_variables.Theme = radioButton1.Checked ? Variables.Themes.Light : Variables.Themes.Dark;
            site_variables.SiteName = textBox1.Text;
            site_variables.PageTitle_Home = textBox6.Text;
            site_variables.DownloadButton = textBox3.Text;
            site_variables.DownloadLink = textBox4.Text;

            site_variables.Titles = new List<string>();
            foreach (string item in listBox1.Items)
                site_variables.Titles.Add(item);

            site_variables.Socials = new Dictionary<Variables.SocialMedias, string>();
            foreach (ListViewItem item in listView1.Items)
                site_variables.Socials.Add( (Variables.SocialMedias)int.Parse(item.Tag.ToString()), item.SubItems[0].Text );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox2.Text);
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1) listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
    }
}
