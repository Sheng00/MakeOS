using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MakeOSLib;

namespace MakeOS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        string topText = "org 0x7c00";
        string end512 = "times 510 - ($-$$) db 0" + Environment.NewLine + "dw 0xAA55";
        string eofText = "";
        public bool bootSectored = false;

        string outCode = "";

        private string printfFunc = "";
        private string cursorFunc = "";
        private string waitFunc = "";
        private string cursorPrintFunc = "";

        string variables = "";
        int varCount = 0;

        public bool good;

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = textBox2.Text;
            good = true;
            logBox.Text = "";
            if (!Directory.Exists(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}");
                logBox.Text += "Created Folder " + fileName + Environment.NewLine;
            }
            Process.Start(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\");

            List<string> lins = new List<string>();
                foreach (string line in codeBox.Lines)
                {
                    string currentLine = line;
                    while (currentLine.StartsWith(" ") || currentLine.StartsWith("   "))
                    {
                        currentLine = currentLine.Remove(0, 1);
                    }
                    lins.Add(currentLine);
                }

            logBox.Text += "Removed un-needed spaces" + Environment.NewLine;


            File.WriteAllText(Directory.GetCurrentDirectory() + $@"\BACKUP\Backup of {fileName}.txt", codeBox.Text);
            logBox.Text += "Created Backup File" + Environment.NewLine;

            outCodeBox.Text = "";
                codeBox.Text = "";
                foreach (string lin in lins)
                {
                    codeBox.Text += lin + Environment.NewLine;
                }
            Build bld = new Build();
            if (bld.errors != "")
            {
                MessageBox.Show(bld.errors);
            }
            outCode = bld.ParseInput(lins.ToArray());
            
            outCodeBox.Text = outCode;
            
            if (good)
                {

                while(File.Exists(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\{fileName}.img"))
                {
                    File.Delete(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\{fileName}.img");
                    logBox.Text += "Deleted old OS image file" + Environment.NewLine;
                }

                File.WriteAllText(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\{fileName}.asm", outCode);
                logBox.Text += "Built ASM file" + Environment.NewLine;
                //Assemble OS
                File.WriteAllText(Directory.GetCurrentDirectory() + $@"\makefile.bat", $@"Fasm ""BUILD\{fileName}\{fileName}.asm"" ""BUILD\{fileName}\{fileName}.bin""" + Environment.NewLine + "pause");
                Process.Start(Directory.GetCurrentDirectory() + $@"\makefile.bat");
                logBox.Text += "Created BIN file" + Environment.NewLine;


                //Make IMG
                File.WriteAllText(Directory.GetCurrentDirectory() + $@"\build.bat", $@"copy /b ""BUILD\{fileName}\{fileName}.bin"" ""BUILD\{fileName}\{fileName}.img""");
                Process.Start(Directory.GetCurrentDirectory() + $@"\build.bat");
                Thread.Sleep(1000);
                Process.Start(Directory.GetCurrentDirectory() + $@"\build.bat");

                logBox.Text += "Created IMG file" + Environment.NewLine;

                logBox.Text += "Openned Operating System Folder..." + Environment.NewLine;

            }
            good = false;
            File.WriteAllText(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\log.txt", logBox.Text);
            logBox.Text += "Saved Log File" + Environment.NewLine;
            logBox.Text += "Finished . . ." + Environment.NewLine;



        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            logBox.ReadOnly = true;
            outCodeBox.ReadOnly = true;
           }

        private void button2_Click(object sender, EventArgs e)
        {
            string ascii = textBox1.Text;
            char[] carray = ascii.ToCharArray();
            string hexValue = "";
            foreach(char letter in carray)
            {
                int value = Convert.ToInt32(letter);
                hexValue += String.Format("{0:X}", value);

            }

            textBox1.Text = hexValue;


        }

        private void codeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            codeBox.WordWrap = false;
            int cursorpos = codeBox.SelectionStart;
            int Row = codeBox.GetLineFromCharIndex(cursorpos);
            label1.Text = $"Line {Row}";

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = textBox2.Text;
            if (!Directory.Exists(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}");
            }

            File.WriteAllText(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\{fileName}.asm", codeBox.Text);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fINDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("By creating a New file, you will exit the current, are you sure?", "Are you sure?", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                codeBox.Text = "";
                outCodeBox.Text = "";
                logBox.Text = "";
                textBox2.Text = "My Operating System";
                textBox1.Text = "";
                MessageBox.Show("Successfully created new file!");
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            if(opf.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = Path.GetFileNameWithoutExtension(opf.FileName);
                codeBox.Text = File.ReadAllText(opf.FileName);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = textBox2.Text;
            good = true;
            logBox.Text = "";
            if (!Directory.Exists(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}");
                logBox.Text += "Created Folder " + fileName + Environment.NewLine;
            }
            Process.Start(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\");

            List<string> lins = new List<string>();
            foreach (string line in codeBox.Lines)
            {
                string currentLine = line;
                while (currentLine.StartsWith(" ") || currentLine.StartsWith("   "))
                {
                    currentLine = currentLine.Remove(0, 1);
                }
                lins.Add(currentLine);
            }

            logBox.Text += "Removed un-needed spaces" + Environment.NewLine;


            File.WriteAllText(Directory.GetCurrentDirectory() + $@"\BACKUP\Backup of {fileName}.txt", codeBox.Text);
            logBox.Text += "Created Backup File" + Environment.NewLine;

            outCodeBox.Text = "";
            codeBox.Text = "";
            foreach (string lin in lins)
            {
                codeBox.Text += lin + Environment.NewLine;
            }
            Build bld = new Build();
            outCode = bld.ParseInput(lins.ToArray());


            if (good)
            {

                while (File.Exists(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\{fileName}.img"))
                {
                    File.Delete(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\{fileName}.img");
                    logBox.Text += "Deleted old OS image file" + Environment.NewLine;
                }

                File.WriteAllText(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\{fileName}.asm", outCode);
                logBox.Text += "Built ASM file" + Environment.NewLine;
                //Assemble OS
                File.WriteAllText(Directory.GetCurrentDirectory() + $@"\makefile.bat", $@"Fasm ""BUILD\{fileName}\{fileName}.asm"" ""BUILD\{fileName}\{fileName}.bin""" + Environment.NewLine + "pause");
                Process.Start(Directory.GetCurrentDirectory() + $@"\makefile.bat");
                logBox.Text += "Created BIN file" + Environment.NewLine;


                //Make IMG
                File.WriteAllText(Directory.GetCurrentDirectory() + $@"\build.bat", $@"copy /b ""BUILD\{fileName}\{fileName}.bin"" ""BUILD\{fileName}\{fileName}.img""");
                Process.Start(Directory.GetCurrentDirectory() + $@"\build.bat");
                Thread.Sleep(1000);
                Process.Start(Directory.GetCurrentDirectory() + $@"\build.bat");

                logBox.Text += "Created IMG file" + Environment.NewLine;

                logBox.Text += "Openned Operating System Folder..." + Environment.NewLine;

            }
            good = false;
            File.WriteAllText(Directory.GetCurrentDirectory() + $@"\BUILD\{fileName}\log.txt", logBox.Text);
            logBox.Text += "Saved Log File" + Environment.NewLine;
            logBox.Text += "Finished . . ." + Environment.NewLine;


        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.Redo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(codeBox.SelectedText);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.Paste();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            codeBox.Cut();
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            codeBox.SelectAll();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
