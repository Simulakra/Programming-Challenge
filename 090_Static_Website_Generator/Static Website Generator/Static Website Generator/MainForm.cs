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
using HtmlAgilityPack;

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
        string startup_path;

        private void Generate_Site()
        {
            //first variables
            startup_path = Application.StartupPath;
            get_variables();

            //prepare main files
            if (Directory.Exists(startup_path + @"\unzip_source"))
                Directory.Delete(startup_path + @"\unzip_source", true);
            ZipFile.ExtractToDirectory(startup_path + @"\breezycv.zip", startup_path + @"\unzip_source");
            if (Directory.Exists(startup_path + @"\generate"))
                Directory.Delete(startup_path + @"\generate", true);
            string folder_name = (site_variables.Theme == Variables.Themes.Light) ? "lightfw" : "darkfw";
            Directory.Move(startup_path + @"\unzip_source\breezycv\" + folder_name, startup_path + @"\generate");
            Directory.Delete(startup_path + @"\unzip_source", true);

            #region change variables in html file
            //get default site
            HtmlAgilityPack.HtmlDocument html_index = new HtmlAgilityPack.HtmlDocument();
            html_index.Load(startup_path + @"\generate\index.html");

            //home page
            html_index.DocumentNode.SelectSingleNode("/html/head/title").InnerHtml = site_variables.SiteName;
            html_index.DocumentNode.SelectSingleNode("/html/head/meta[4]").SetAttributeValue("content", site_variables.SiteName);//description meta
            html_index.DocumentNode.SelectSingleNode("/html/head/meta[6]").SetAttributeValue("content", site_variables.SiteName);//author meta
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[1]/div[2]/h2").InnerHtml = site_variables.SiteName;
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/ul/li[1]/a/span[2]").InnerHtml = site_variables.PageTitle_Home;
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/ul/li[2]/a/span[2]").InnerHtml = site_variables.PageTitle_About;
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/ul/li[3]/a/span[2]").InnerHtml = site_variables.PageTitle_Resume;
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/ul/li[4]/a/span[2]").InnerHtml = site_variables.PageTitle_Portfolio;
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/ul/li[5]/a/span[2]").InnerHtml = site_variables.PageTitle_Contact;
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[3]/a").InnerHtml = site_variables.DownloadButton;
            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[3]/a").SetAttributeValue("href", site_variables.DownloadFileName.Substring(1));

            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[2]/ul").InnerHtml = "";
            foreach (var item in site_variables.Socials)
            {
                html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[2]/ul").InnerHtml +=
                    "<li><a href=\"" + item[1] + "\" target=\"_blank\"><i class=\"fab " + item[0] + "\"></i></a></li>";
            }

            html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[1]/div[2]/h4").InnerHtml = "";
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[1]/div[1]/div/div/div/div").InnerHtml = "";
            if (site_variables.Titles.Count > 0)
            {
                html_index.DocumentNode.SelectSingleNode("//*[@id=\"site_header\"]/div[1]/div[2]/h4").InnerHtml = site_variables.Titles[0];
                foreach (var item in site_variables.Titles)
                {
                    html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[1]/div[1]/div/div/div/div").InnerHtml +=
                        "<div class=\"item\"><div class=\"sp-subtitle\">" + item + "</div></div>";
                }
            }

            //contact info
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[2]/div[2]/div[1]/div[2]/div/ul/li[4]/span[2]/a").InnerHtml = site_variables.Email;
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[2]/div[2]/div[1]/div[2]/div/ul/li[4]/span[2]/a").SetAttributeValue("href", "mailto:"+site_variables.Email);
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[2]/div[2]/div[1]/div[2]/div/ul/li[5]/span[2]").InnerHtml = site_variables.Phone;
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[1]/h4").InnerHtml = site_variables.Location;
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[2]/h4").InnerHtml = site_variables.Phone;
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[3]/h4/a").InnerHtml = site_variables.Email;
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[3]/h4/a").SetAttributeValue("href", "mailto:"+site_variables.Email);
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[4]/h4").InnerHtml = site_variables.Freelance;
            if (!site_variables.isFreelance) html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[4]").Remove();
            if (!site_variables.isEmail) html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[3]").Remove();
            if (!site_variables.isPhone) html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[2]").Remove();
            if (!site_variables.isPhone) html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[2]/div[2]/div[1]/div[2]/div/ul/li[5]").Remove();
            if (!site_variables.isEmail) html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[2]/div[2]/div[1]/div[2]/div/ul/li[4]").Remove();
            if (!site_variables.isLocation) html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[6]/div[2]/div/div[1]/div[1]").Remove();


            //delete blog page and nav button
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/header/ul/li[5]").Remove();
            html_index.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[3]/div/section[5]").Remove();

            #endregion

            //save last HTML file
            html_index.Save(startup_path + @"\generate\index.html");
            if (site_variables.DownloadLink != "") File.Copy(site_variables.DownloadLink, startup_path + site_variables.DownloadFileName);

            //all done :)
            MessageBox.Show("All site generation done", "Done");
        }

        private void btn_generate_and_open_Click(object sender, EventArgs e)
        {
            startup_path = Application.StartupPath;
            Generate_Site();
            System.Diagnostics.Process.Start(startup_path + @"\generate\index.html");
        }

        private void btn_generate_and_open_folder_Click(object sender, EventArgs e)
        {
            startup_path = Application.StartupPath;
            Generate_Site();
            System.Diagnostics.Process.Start(startup_path + @"\generate\");
        }

        private void btn_just_generate_Click(object sender, EventArgs e)
        {
            Generate_Site();
        }

        private void get_variables()
        {
            site_variables.Theme = radioButton1.Checked ? Variables.Themes.Light : Variables.Themes.Dark;
            site_variables.SiteName = textBox1.Text;
            site_variables.PageTitle_Home = textBox6.Text;
            site_variables.PageTitle_About = textBox5.Text;
            site_variables.PageTitle_Resume = textBox13.Text;
            site_variables.PageTitle_Portfolio = "Portfolio";
            site_variables.PageTitle_Contact = textBox24.Text;
            site_variables.DownloadButton = textBox3.Text;
            site_variables.DownloadLink = textBox4.Text;
            if (site_variables.DownloadLink == "") site_variables.DownloadFileName = "##";
            else site_variables.DownloadFileName = textBox4.Text.Substring(textBox4.Text.LastIndexOf('\\'));

            site_variables.Titles = new List<string>();
            foreach (string item in listBox1.Items)
                site_variables.Titles.Add(item);

            site_variables.Socials = new List<string[]>();
            foreach (ListViewItem item in listView1.Items)
                site_variables.Socials.Add( new string[]{
                Variables.SocialMediaIcons[(Variables.SocialMedias)int.Parse(item.Tag.ToString())], item.SubItems[1].Text } );

            site_variables.Location = textBox23.Text;
            site_variables.Phone = textBox22.Text;
            site_variables.Email = textBox21.Text;
            site_variables.Freelance = textBox20.Text;
            site_variables.Map_Location = textBox25.Text;

            site_variables.isLocation = checkBox13.Checked;
            site_variables.isPhone = checkBox12.Checked;
            site_variables.isEmail = checkBox11.Checked;
            site_variables.isFreelance = checkBox10.Checked;
            site_variables.isMap_Location = checkBox14.Checked;
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0) listView1.SelectedItems[0].Remove();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dia = new Dialog.AddSocial();
            if (dia.ShowDialog() == DialogResult.OK) {
                var temp = new ListViewItem();
#pragma warning disable CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
                temp.Text = dia.SocialMedia.ToString();
#pragma warning restore CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
                temp.Tag = (int)dia.SocialMedia;
                temp.SubItems.Add(dia.Link);
                listView1.Items.Add(temp);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                textBox4.Text = ofd.FileName;
            else textBox4.Text = "";
        }
    }
}
