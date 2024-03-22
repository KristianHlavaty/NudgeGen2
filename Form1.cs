using System.Text;
using System.Windows.Forms;

namespace nugget2
{
    public partial class Form1 : Form
    {
        //TODO change to windows aplication in settings after testing
        public Form1()
        {
            InitializeComponent();
            List<definition> defLine = new List<definition>();
            using (StreamReader reader = new StreamReader("C:\\Users\\Kryx\\Desktop\\eu4\\coding\\definition.csv", CodePagesEncodingProvider.Instance.GetEncoding(1250)))
            {
                // line to skip header
                string? line = reader.ReadLine();
                
                while ((line = reader.ReadLine()) != null)
                {
                    definition def = definition.loadFromLine(line);
                    defLine.Add(def);
                }
            }

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
           /* X – Maps 1:1.i.e.In CS4 the X co-ordinate will match the co-ordinate you put in the positions.tt
            Y – From the width of your map, take the position within your image editor and subtract it from the width(i.e. 2048 – 755). */
           //test

            Console.WriteLine("Done");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
            button1.Text = "Hello World!";
        }
    }
}
