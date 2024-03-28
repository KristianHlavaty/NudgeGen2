using System.Text;
using System.Windows.Forms;
//for the background worker
using System.ComponentModel;

namespace NudgeGen2
{
    public partial class Form1 : Form
    {
        private List<definition> defLine = new List<definition>();
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

            openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog2.Filter = "Bitmap files (*.bmp)|*.bmp";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // get the definition.csv file
            string fileName = openFileDialog1.FileName;
            textBox1.Text = fileName;
            defLine.Clear();
            LoadDefinitionsFromFile();
        }

        private void LoadDefinitionsFromFile()
        {
            using (StreamReader reader = new StreamReader(textBox1.Text, CodePagesEncodingProvider.Instance.GetEncoding(1250)))
            {
                // line to skip header
                string? line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    definition def = definition.loadFromLine(line);
                    defLine.Add(def);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            // get the provinces.bmp file
            string fileName = openFileDialog2.FileName;
            textBox2.Text = fileName;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            // init the progress bar
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            // Run the process in the background
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessBitmapAndGenerateFile();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 0;
            MessageBox.Show("Processing completed.");
        }
        private void ProcessBitmapAndGenerateFile()
        {
            int bitmapHeight = 2048; // height of the bitmap 2048 is default

            using (Bitmap bitmap = (Bitmap)Image.FromFile(textBox2.Text))
            {
                int width = bitmap.Width;
                int height = bitmap.Height;

                bitmapHeight = bitmap.Height;
                // for reporting progress
                int totalPixels = width * height;
                int pixelsProcesssed = 0;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color pixelColor = bitmap.GetPixel(x, y);
                        // for reporting progress
                        pixelsProcesssed++;
                        int progressPercentage = (int)((double)pixelsProcesssed / totalPixels * 100);
                        backgroundWorker1.ReportProgress(progressPercentage);
                        foreach (definition def in defLine)
                        {
                            if (def.red == pixelColor.R && def.green == pixelColor.G && def.blue == pixelColor.B)
                            {
                                def.pixelX.Add(x);
                                def.pixelY.Add(y);
                                break;
                            }
                        }
                    }
                }
            }
            /* X – Maps 1:1.i.e.In CS4 the X co-ordinate will match the co-ordinate you put in the positions.tt
             Y – From the height of your map, take the position within your image editor and subtract it from the Height(i.e. 2048 – 755). */

            using (StreamWriter writer = new StreamWriter(".\\positions.txt"))
            {
                foreach (definition def in defLine)
                {
                    int defX = def.pixelX.Count == 0 ? 0 : Convert.ToInt32(def.pixelX.Average());
                    int defY = def.pixelY.Count == 0 ? 0 : Convert.ToInt32(def.pixelY.Average());
                    int defYPos = bitmapHeight - defY;
                    writer.WriteLine($"#{def.provinceName}");
                    writer.WriteLine($"{def.provinceID}={{");
                    writer.WriteLine($"\tposition={{\n\t\t {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 0.000 0.000 \n\t}}");
                    writer.WriteLine("\trotation={\n\t\t0.000 0.000 0.000 0.000 0.000 0.000 0.000 \n\t}");
                    writer.WriteLine("\theight={\n\t\t0.000 0.000 1.000 0.000 0.000 0.000 0.000 \n\t}");
                    writer.WriteLine("}");

                    //report progress
                    backgroundWorker1.ReportProgress(0, def.provinceName);
                }
            }
            Console.WriteLine("Done");
        }

    }
}
