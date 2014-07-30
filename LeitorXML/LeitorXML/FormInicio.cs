using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LeitorXML
{
    public partial class FormInicio : Form
    {
        public static string folderPath = "";
        public static int numeroclaseI = 0;
        public string combo = "";
        public string data = "";
        public FormInicio()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pathRaiz_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            caminho.Text = fbd.SelectedPath;
            
            //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderPath = caminho.Text;
            if(folderPath.Equals(""))
            {
                System.Windows.Forms.MessageBox.Show("Preencha o caminho da pasta ", "Message");
                return;
            }
            combo = classe.Text;
            if(combo.Equals(""))
            {
                System.Windows.Forms.MessageBox.Show("Preencha a classe ", "Message");
                return;
            }

            data = dateTimePicker1.Text;
            if (data.Equals(""))
            {
                System.Windows.Forms.MessageBox.Show("Preencha a classe ", "Message");
                return;
            }

            string[] files = Directory.GetFiles(folderPath);
            foreach (var item in files)
            {
                GetFile(item);
            }
            GetXML(folderPath + "/extrat");
            caminho.Text = numeroclaseI.ToString();
        }

        private void GetXML(string path) 
        {
            string[] files = Directory.GetFiles(path);
            foreach (var item in files)
            {
                ProcessFile(item);
            }
        }

        private void GetFile(string path) 
        {
            
                if (File.Exists(path))
                {
                    if (Path.GetExtension(path).Equals(".zip") || Path.GetExtension(path).Equals(".gz"))
                    {
                        if (!Directory.Exists(folderPath + "/extrat"))
                        {
                            Directory.CreateDirectory(folderPath + "/extrat");
                        }
                    }
                    Decompress(new FileInfo(path));
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
        }

        // Process all files in the directory passed in, recurse on any directories  
        // that are found, and process the files they contain. 
        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here. 
        public void ProcessFile(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList list = doc.GetElementsByTagName("evento");
            XmlNodeList listAbril = new EmptyNodeList();
            foreach (XmlElement evento in list)
            {
                
                XmlNodeList nodeList = evento.ChildNodes;
                foreach (XmlNode item in nodeList)
                {
                    if (item.Name.Equals("dtOcorrencia")) 
                    {
                        DateTime date = DateTime.Parse(data);
                        DateTime dataOcorrencia = DateTime.Parse(item.InnerText);
                        if(dataOcorrencia.Month == date.Month)
                        {
                            if(dataOcorrencia.Day == date.Day)
                            {
                                listAbril = nodeList;
                            }
                        }
                    }
                   
                }
                
                //numeroclaseI = 0;
                foreach (XmlNode item in listAbril)
                {
                    
                    if (item.Name.Equals("classVeiculo"))
                    {
                        if (item.InnerText.Substring(0, 1).Equals(combo))
                        {
                            Console.WriteLine(item.InnerText +"QTD :"+ numeroclaseI);
                            numeroclaseI++;
                        }
                    }
                    
                }
            } 
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = fileToDecompress.Name;

                using (FileStream decompressedFileStream = File.Create(folderPath + "/extrat/"+newFileName.Replace(".gz", ".xml")))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }      


    }
}
