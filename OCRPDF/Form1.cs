using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
//using from cmd:
//1.tesseract OCR
//https://github.com/tesseract-ocr
//2. image magick
//https://www.imagemagick.org/script/binary-releases.php
//3. ghost script (must have for image magick)
//https://ghostscript.com/download/gsdnld.html

namespace OCRPDF
{

    public partial class frmMain : Form
    {
        public frmMain()
        {
            //Delete temp files
            var di = new DirectoryInfo(@"temp\");
            foreach (var file in di.GetFiles())
                file.Delete();
            InitializeComponent();
        }

        #region Common Functions

        private static void RunCmd(string command)
        {
            //http://stackoverflow.com/questions/1469764/run-command-prompt-commands
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + command
            };
            var process = new System.Diagnostics.Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();
            process.Close();
            process.Dispose();
        }

        private void SetPictureBox(string picture)
        {
            var i = Image.FromFile(picture);
            i.Save(@"temp\temp.png");
            if (i.Width < i.Height)
            {
                pbOriginal.Height = 250;
                pbOriginal.Width = 250 * i.Width / i.Height;
            }
            else
            {
                pbOriginal.Width = 250;
                pbOriginal.Height = 250 * i.Height / i.Width;
            }
            i.Dispose();
            using (var stream = new FileStream(@"temp\temp.png", FileMode.Open, FileAccess.Read))
                pbOriginal.Image = Image.FromStream(stream);
            _cropRectangle = new Rectangle(0, 0, 0, 0);
            _cropStart = new Point(0, 0);
            PaintCropped(@"temp\temp.png");
            pbOriginal.Invalidate();
        }

        private void PaintCropped(string picture)
        {
            var i = Image.FromFile(picture);
            if (picture != @"temp\temp2.png")
                i.Save(@"temp\temp2.png");
            if (i.Width < i.Height)
            {
                pbCrop.Height = 500;
                pbCrop.Width = 500 * i.Width / i.Height;
            }
            else
            {
                pbCrop.Width = 500;
                pbCrop.Height = 500 * i.Height / i.Width;
            }
            i.Dispose();
            using (var stream = new FileStream(@"temp\temp2.png", FileMode.Open, FileAccess.Read))
                pbCrop.Image = Image.FromStream(stream);
            _cropRectangle2 = new Rectangle(0, 0, 0, 0);
            _cropStart2 = new Point(0, 0);
            pbCrop.Invalidate();
            txtExtracted.Text = "";
            if (autoCalculate.Checked)
                btnCalculate_Click();
        }

        #endregion

        #region Buttons

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            //Ask for file
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "All files (*.*)|*.*|pdf files (*.pdf)|*.pdf|png files (*.png)|*.png",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            var myStream = openFileDialog1.OpenFile();
            //Delete temp files
            var di = new DirectoryInfo(@"temp\");
            foreach (var file in di.GetFiles())
                file.Delete();
            using (myStream)
            {
                //Disable interaction while processing
                btnCalculate.Enabled = false;
                btnOpenFile.Enabled = false;
                btnPage.Enabled = false;
                //Pdf to Png
                if(openFileDialog1.FileName.Contains(".pdf"))
                    RunCmd($@"magick -density 300 ""{openFileDialog1.FileName}"" ""{Directory.GetCurrentDirectory()}\temp\pdf.png""");
                else
                {
                    var i = Image.FromFile(openFileDialog1.FileName);
                    i.Save(@"temp\pdf.png");
                    i.Dispose();
                }
                //Set pictureboxes
                SetPictureBox(@"temp\" + new DirectoryInfo(@"temp\").GetFiles("pdf*.png")[0]);
                //Reset extarcted text
                txtExtracted.Text = "";
                //Set paths
                txtPath.Text = openFileDialog1.SafeFileName;
                //get text
                if(autoCalculate.Checked)
                    btnCalculate_Click();
                //Reset interaction
                btnPage.Enabled = true;
                btnCalculate.Enabled = true;
                btnOpenFile.Enabled = true;
            }
        }

        private void btnCalculate_Click(object sender = null, EventArgs e = null)
        {
            //Dont calculate if no file selected
            if (txtPath.Text == "") return;
            //Disable interaction while processing
            btnCalculate.Enabled = false;
            btnOpenFile.Enabled = false;
            btnPage.Enabled = false;
            //OCR
            RunCmd("tesseract " + Directory.GetCurrentDirectory() + @"\temp\temp2.png " + 
                    Directory.GetCurrentDirectory() + @"\temp\out ");
            //Grab text, insert correct endlines, remove extra ones
            txtExtracted.Text = File.ReadAllText(Directory.GetCurrentDirectory()
                                + @"\temp\out.txt", Encoding.UTF8).Replace("\n", "\r\n")
                                .TrimEnd('\r', '\n');
            //Reset interaction
            btnPage.Enabled = true;
            btnCalculate.Enabled = true;
            btnOpenFile.Enabled = true;
        }

        private int _current;
        private void btnPage_Click(object sender, EventArgs e)
        {
            //Set pictureboxes
            _current++;
            try
            {
                SetPictureBox(@"temp\" + new DirectoryInfo(@"temp\").GetFiles("pdf*.png")[_current]);
            }
            catch (Exception exception)
            {
                _current = 0;
                try
                {
                    SetPictureBox(@"temp\" + new DirectoryInfo(@"temp\").GetFiles("pdf*.png")[_current]);
                }
                catch (Exception exception2)
                {
                    pbCrop.Image = null;
                    pbCrop.Width = 500;
                    pbCrop.Height = 500;
                    pbOriginal.Image = null;
                    pbOriginal.Width = 250;
                    pbOriginal.Height = 250;
                    txtExtracted.Text = "";
                    txtPath.Text = "";
                }
            }
        }

        #endregion

        #region pbOriginal Events

        private Rectangle _cropRectangle;
        private Point _cropStart;
        private bool _isDragging;
        private bool _moved;

        private void pbOriginal_MouseDown(object sender, MouseEventArgs e)
        {
            //if n ofile is selected, leave
            if (txtPath.Text == "") return;
            //Reset moved variable
            _moved = false;
            if (e.Button == MouseButtons.Left)
            {
                //if left click, prepare to drag
                _cropRectangle = new Rectangle(e.X, e.Y, 0, 0);
                _cropStart = new Point(e.X, e.Y);
                _isDragging = true;
            }
            else
            {
                //if right click, reset crop
                _cropRectangle = new Rectangle(0, 0, 0, 0);
                _cropStart = new Point(0, 0);
                pbOriginal.Invalidate();
                PaintCropped(@"temp\temp.png");
            }
        }

        private void pbOriginal_MouseUp(object sender, MouseEventArgs e)
        {
            //exit if no file or right click
            if (e.Button != MouseButtons.Left || txtPath.Text == "") return;
            if (_moved)
            {
                try
                {
                    //Done selecting crop rectangle, copy to new bitmap
                    var bmpImage = new Bitmap(@"temp\temp.png");
                    var croppedImage = bmpImage.Clone(
                        new Rectangle(_cropRectangle.X * bmpImage.Width / pbOriginal.Width,
                                        _cropRectangle.Y * bmpImage.Height / pbOriginal.Height,
                                        _cropRectangle.Width * bmpImage.Width / pbOriginal.Width,
                                        _cropRectangle.Height * bmpImage.Height / pbOriginal.Height),
                                        bmpImage.PixelFormat);
                    bmpImage.Dispose();
                    //save to new temp and paint in picture box
                    croppedImage.Save(@"temp\temp2.png");
                    croppedImage.Dispose();
                    PaintCropped(@"temp\temp2.png");
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                //if it didnt move, reset selection
                _cropRectangle = new Rectangle(0, 0, 0, 0);
                _cropStart = new Point(0, 0);
                pbOriginal.Invalidate();
                PaintCropped(@"temp\temp.png");
            }
            //Reset variables
            _moved = false;
            _isDragging = false;
        }

        private void pbOriginal_MouseMove(object sender, MouseEventArgs e)
        {
            //if they didnt left click before, leave
            if (!_isDragging) return;
            //tell methods mouse dragged
            _moved = true;
            //Make sure the selection is limited to the controlbox
            var x = e.X > pbOriginal.Width ? pbOriginal.Width - 4 : e.X;
            var y = e.Y > pbOriginal.Height ? pbOriginal.Height - 4 : e.Y;
            x = x >= 2 ? x : 2;
            y = y >= 2 ? y : 2;
            //if it didnt move a considerable amount, move it one pixel
            var w = Math.Abs(x - _cropStart.X) == 0? 2: Math.Abs(x - _cropStart.X);
            var h = Math.Abs(y - _cropStart.Y) == 0? 2: Math.Abs(y - _cropStart.Y);
            //Create the new crop rectangle
            _cropRectangle = new Rectangle(Math.Min(_cropStart.X, x),
                                           Math.Min(_cropStart.Y, y), w, h);
            //redraw
            pbOriginal.Invalidate();
        }

        private void pbOriginal_Paint(object sender, PaintEventArgs e)
        {
            //Paint selection rectangle
            e.Graphics.DrawRectangle(Pens.Red, _cropRectangle);
        }

        #endregion

        #region pbCrop Events

        private Rectangle _cropRectangle2;
        private Point _cropStart2;
        private bool _isDragging2;
        private bool _moved2;

        private void pbCrop_MouseDown(object sender, MouseEventArgs e)
        {
            //if n ofile is selected, leave
            if (txtPath.Text == "") return;
            //Reset moved variable
            _moved2 = false;
            if (e.Button == MouseButtons.Left)
            {
                //if left click, prepare to drag
                _cropRectangle2 = new Rectangle(e.X, e.Y, 0, 0);
                _cropStart2 = new Point(e.X, e.Y);
                _isDragging2 = true;
            }
            else
            {
                //if right click, reset crop
                PaintCropped(@"temp\temp2.png");
            }
        }

        private void pbCrop_MouseUp(object sender, MouseEventArgs e)
        {
            //exit if no file or right click
            if (e.Button != MouseButtons.Left || txtPath.Text == "") return;
            if (_moved2)
            {
                try
                {
                    //Done selecting crop rectangle, copy to new bitmap
                    var bmpImage = new Bitmap(@"temp\temp2.png");
                    var croppedImage = bmpImage.Clone(
                        new Rectangle(_cropRectangle2.X * bmpImage.Width / pbCrop.Width,
                                        _cropRectangle2.Y * bmpImage.Height / pbCrop.Height,
                                        _cropRectangle2.Width * bmpImage.Width / pbCrop.Width,
                                        _cropRectangle2.Height * bmpImage.Height / pbCrop.Height),
                                        bmpImage.PixelFormat);
                    bmpImage.Dispose();
                    //save to new temp and paint in picture box
                    croppedImage.Save(@"temp\temp2.png");
                    croppedImage.Dispose();
                    PaintCropped(@"temp\temp2.png");
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                //if it didnt move, reset selection
                PaintCropped(@"temp\temp2.png");
            }
            //Reset variables
            _moved2 = false;
            _isDragging2 = false;
        }

        private void pbCrop_MouseMove(object sender, MouseEventArgs e)
        {
            //if they didnt left click before, leave
            if (!_isDragging2) return;
            //tell methods mouse dragged
            _moved2 = true;
            //Make sure the selection is limited to the controlbox
            var x = e.X > pbCrop.Width ? pbCrop.Width - 4 : e.X;
            var y = e.Y > pbCrop.Height ? pbCrop.Height - 4 : e.Y;
            x = x >= 2 ? x : 2;
            y = y >= 2 ? y : 2;
            //if it didnt move a considerable amount, move it one pixel
            var w = Math.Abs(x - _cropStart2.X) == 0 ? 2 : Math.Abs(x - _cropStart2.X);
            var h = Math.Abs(y - _cropStart2.Y) == 0 ? 2 : Math.Abs(y - _cropStart2.Y);
            //Create the new crop rectangle
            _cropRectangle2 = new Rectangle(Math.Min(_cropStart2.X, x),
                                            Math.Min(_cropStart2.Y, y), w, h);
            //redraw
            pbCrop.Invalidate();
        }

        private void pbCrop_Paint(object sender, PaintEventArgs e)
        {
            //Paint selection rectangle
            e.Graphics.DrawRectangle(Pens.Red, _cropRectangle2);
            //reset selection
            _cropRectangle = new Rectangle(0, 0, 0, 0);
            _cropStart = new Point(0, 0);
            pbOriginal.Invalidate();
        }

        #endregion

    }

}
