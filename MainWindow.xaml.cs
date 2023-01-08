using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using SharpCompress;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace really_funny_thing
{
    public partial class MainWindow : Window
    {
        private string getPath()
        {
            string path = File.ReadAllText("path.txt");
            if (!path.EndsWith("PAYDAY 2"))
            {
                MessageBox.Show("Please type in the path of the payday 2 directory in this file:", "Setup");
                Process.Start("path.txt");
                MessageBox.Show("Press ok after saving the file", "Setup");
                return File.ReadAllText("path.txt");
            } else
            {
                return path;
            }
        }

        public MainWindow()
        {
            string pd2p = getPath();
            InitializeComponent();
            Thread.Sleep(1500);
            payday_path.Text = pd2p;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = amongus.Text;
            var path = payday_path.Text;
            var modLink = link.Text;
            if (modLink == string.Empty)
            {
                MessageBox.Show("Please add a mod link");
                return;
            } else
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c node . " + modLink;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory() + "\\js";
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                var ddl = File.ReadAllText(Directory.GetCurrentDirectory() + "\\js\\link.txt");
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");
                    client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    client.Headers.Add("Accept-Language", "en-US,en;q=0.8");

                    using (Stream stream = client.OpenRead(ddl))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string contentDisposition = client.ResponseHeaders["Content-Disposition"];
                            string fileName = Regex.Match(contentDisposition, "filename=\"(.+?)\"").Groups[1].Value;
                            if (selectedItem != "mod_overrides")
                            {
                                var downloadPath = $"{path}\\{selectedItem}\\";
                                File.WriteAllBytes($"{path}\\{selectedItem}\\{fileName}", client.DownloadData(ddl));
                                Thread.Sleep(5000);
                                if (fileName.EndsWith(".zip"))
                                {
                                    using (var archive = ZipArchive.Open($"{downloadPath}{fileName}"))
                                    {
                                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                                        {
                                            entry.WriteToDirectory(downloadPath, new ExtractionOptions()
                                            {
                                                ExtractFullPath = true,
                                                Overwrite = true
                                            });
                                        }
                                    }
                                    Thread.Sleep(500);
                                    File.Delete($"{downloadPath}{fileName}");
                                }
                                if (fileName.EndsWith(".rar"))
                                {
                                    using (var archive = RarArchive.Open($"{downloadPath}{fileName}"))
                                    {
                                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                                        {
                                            entry.WriteToDirectory(downloadPath, new ExtractionOptions()
                                            {
                                                ExtractFullPath = true,
                                                Overwrite = true
                                            });
                                        }
                                    }
                                    Thread.Sleep(500);
                                    File.Delete($"{downloadPath}{fileName}");
                                }
                            } else
                            {
                                var downloadPath = $"{path}\\assets\\{selectedItem}\\";
                                //MessageBox.Show($"{path}\\assets\\{selectedItem}\\{fileName}");
                                File.WriteAllBytes($"{path}\\assets\\{selectedItem}\\{fileName}", client.DownloadData(ddl));
                                Thread.Sleep(5000);
                                if (fileName.EndsWith(".zip"))
                                {
                                    using (var archive = ZipArchive.Open($"{downloadPath}{fileName}"))
                                    {
                                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                                        {
                                            entry.WriteToDirectory(downloadPath, new ExtractionOptions()
                                            {
                                                ExtractFullPath = true,
                                                Overwrite = true
                                            });
                                        }
                                    }
                                    Thread.Sleep(500);
                                    File.Delete($"{downloadPath}{fileName}");
                                }
                                if (fileName.EndsWith(".rar"))
                                {
                                    using (var archive = RarArchive.Open($"{downloadPath}{fileName}"))
                                    {
                                        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                                        {
                                            entry.WriteToDirectory(downloadPath, new ExtractionOptions()
                                            {
                                                ExtractFullPath = true,
                                                Overwrite = true
                                            });
                                        }
                                    }
                                    Thread.Sleep(500);
                                    File.Delete($"{downloadPath}{fileName}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}