using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Static_Website_Generator.Dialog
{
    public partial class AddSocial : Form
    {
        public AddSocial()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Link = textBox1.Text;
            //this.SocialMedia = Variables.SocialMediaIcons.Keys.ElementAt(comboBox1.SelectedIndex);
            this.SocialMedia = (Variables.SocialMedias)comboBox1.SelectedIndex;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string Link;
        public Variables.SocialMedias SocialMedia;
        
        private void AddSocial_Load(object sender, EventArgs e)
        {
            foreach (var item in Variables.SocialMediaIcons)
            {
                comboBox1.Items.Add(item.Key.ToString());
            }
            comboBox1.SelectedIndex = 0;
        }
    }
}
