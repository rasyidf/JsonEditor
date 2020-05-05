using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace JsonViewer
{
    public partial class form : Form
    {
        public form()
        {
            InitializeComponent();
        }

        private void btnDeserialize_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "")
            {
                Deserialize();
            }
            else
            {
                MessageBox.Show("Please enter some JSON data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void BuildTree(Dictionary<string, object> dictionary, TreeNode node)
        {
            foreach (KeyValuePair<string, object> item in dictionary)
            {
                TreeNode parentNode = new TreeNode(item.Key);
                node.Nodes.Add(parentNode);

                try
                {
                    dictionary = (Dictionary<string, object>)item.Value;
                    BuildTree(dictionary, parentNode);
                }
                catch (InvalidCastException)
                {
                    try
                    {
                        ArrayList list = (ArrayList)item.Value;
                        foreach (string value in list)
                        {
                            TreeNode finalNode = new TreeNode(value)
                            {
                                ForeColor = Color.Blue
                            };
                            parentNode.Nodes.Add(finalNode);
                        }

                    }
                    catch (InvalidCastException)
                    {
                        TreeNode finalNode = new TreeNode(item.Value.ToString())
                        {
                            ForeColor = Color.Blue
                        };
                        parentNode.Nodes.Add(finalNode);
                    }
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            if (ofDialog.ShowDialog() == DialogResult.OK)
            { 
                try
                {
                    using (StreamReader fileReader = new StreamReader(ofDialog.FileName))
                    {
                        txtInput.Text = fileReader.ReadToEnd();
                        Deserialize();
                    }
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("Unable to open the file. " + ioEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Deserialize()
        {
            jsonExplorer.Nodes.Clear();
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                Dictionary<string, object> dic = js.Deserialize<Dictionary<string, object>>(txtInput.Text);

                TreeNode rootNode = new TreeNode("Root");
                jsonExplorer.Nodes.Add(rootNode);
                BuildTree(dic, rootNode);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("JSON data is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
