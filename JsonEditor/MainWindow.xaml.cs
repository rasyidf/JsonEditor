using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace JsonEditor
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
         
        private async  void BOpen_Click(object sender, RoutedEventArgs e)
        {
            var od = new OpenFileDialog();
            if (od.ShowDialog() == true)
            {

                using StreamReader file = File.OpenText(od.FileName);
                
                using JsonTextReader reader = new JsonTextReader(file);

                JObject o2 = (JObject)JToken.ReadFrom(reader);

                tvRoots.SetObjectAsJson(o2);
                file.DiscardBufferedData();
                file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                var fileText = await file.ReadToEndAsync();

                tSource.Text = fileText.Replace("\t","    ");

            }
        }
    }
}
