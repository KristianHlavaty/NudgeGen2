using System.Text;
using System.Windows.Forms;

namespace nugget2
{
    public partial class Form1 : Form
    {
        private List<definition> defLine = new List<definition>();
        public Form1()
        {
            InitializeComponent();
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

        private void button2_Click(object sender, EventArgs e)
        {
            ProcessBitmapAndGenerateFile();
        }
        private void ProcessBitmapAndGenerateFile()
        {
            using (Bitmap bitmap = (Bitmap)Image.FromFile("C:\\Users\\Kryx\\Desktop\\eu4\\coding\\provinces.bmp"))
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color pixelColor = bitmap.GetPixel(x, y);
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
            /* X � Maps 1:1.i.e.In CS4 the X co-ordinate will match the co-ordinate you put in the positions.tt
             Y � From the width of your map, take the position within your image editor and subtract it from the width(i.e. 2048 � 755). */

            using (StreamWriter writer = new StreamWriter("C:\\Users\\Kryx\\Desktop\\eu4\\coding\\positions.txt"))
            {
                foreach (definition def in defLine)
                {
                    int defX = def.pixelX.Count == 0 ? 0 : Convert.ToInt32(def.pixelX.Average());
                    int defY = def.pixelY.Count == 0 ? 0 : Convert.ToInt32(def.pixelY.Average());
                    int defYPos = 2048 - defY;
                    writer.WriteLine($"#{def.provinceName}");
                    writer.WriteLine($"{def.provinceID}={{");
                    writer.WriteLine($"\tposition={{\n\t\t {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 {defX}.000 {defYPos}.000 0.000 0.000 \n\t}}");
                    writer.WriteLine("\trotation={\n\t\t0.000 0.000 0.000 0.000 0.000 0.000 0.000 \n\t}");
                    writer.WriteLine("\theight={\n\t\t0.000 0.000 1.000 0.000 0.000 0.000 0.000 \n\t}");
                    writer.WriteLine("}");
                }
            }
            Console.WriteLine("Done");
        }
    }
}
