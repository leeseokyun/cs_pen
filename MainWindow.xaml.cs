using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using Windows.UI;
using Microsoft.UI.Xaml.Controls.Primitives;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace c_pen
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainWindow : Window
    {
        private bool flag;
        private float px, py;
        private float mySize;
        private Color myCol;
        private List<float> vx = new List<float>();
        private List<float> vy = new List<float>();
        private List<Color> col = new List<Color>();
        private List<float> size = new List<float>();

        public MainWindow()
        {
            this.InitializeComponent();
            flag = false;
            px = 100;
            py = 100;
            mySize = 16;
            myCol = Colors.Green;
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            int n = vx.Count;
            if (n <= 2) return;

            for (int i = 1; i < n; i++)
            {
                if (vx[i] == 0.0 && vy[i] == 0.0)
                {
                    i++;
                    continue;
                }
                args.DrawingSession.DrawLine(vx[i - 1], vy[i - 1], vx[i], vy[i], col[i], size[i]);
                args.DrawingSession.FillCircle(vx[i - 1], vy[i - 1], size[i] / 2, col[i]);
                args.DrawingSession.FillCircle(vx[i], vy[i], size[i] / 2, col[i]);
            }
        }

        private void CanvasControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            flag = true;
        }

        private void CanvasControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var canvas = sender as CanvasControl;
            Point point = e.GetCurrentPoint(canvas).Position;
            px = (float)point.X;
            py = (float)point.Y;
            if (flag)
            {
                vx.Add(px);
                vy.Add(py);
                col.Add(myCol);
                size.Add(mySize);
                canvas.Invalidate();
            }
        }

        private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            myCol = args.NewColor;
        }

        private void CanvasControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            flag = false;
            px = py = 0.0f;
            vx.Add(px);
            vy.Add(py);
            col.Add(myCol);
            size.Add(mySize);
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            mySize = (float)e.NewValue;
        }

        private void myWrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("C:/Users/user/Desktop/visual/pen_folder/my.txt"))
                {
                    sw.WriteLine(vx.Count);
                    for (int i = 0; i < vx.Count; i++)
                    {
                        sw.WriteLine($"{vx[i]} {vy[i]} {col[i].A} {col[i].B} {col[i].G} {col[i].R} {size[i]}");
                    }
                }
                // MessageBox.Show("The file was saved", "Success");
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"Error saving file: {ex.Message}", "Error");
            }
        }

        private void myRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader("C:/Users/user/Desktop/visual/pen_folder/my.txt"))
                {
                    vx.Clear();
                    vy.Clear();
                    col.Clear();
                    size.Clear();

                    int num = int.Parse(sr.ReadLine());
                    for (int i = 0; i < num; i++)
                    {
                        string[] parts = sr.ReadLine().Split();
                        vx.Add(float.Parse(parts[0]));
                        vy.Add(float.Parse(parts[1]));
                        col.Add(Color.FromArgb(byte.Parse(parts[2]), byte.Parse(parts[5]), byte.Parse(parts[4]), byte.Parse(parts[3])));
                        size.Add(float.Parse(parts[6]));
                    }
                }
                CanvasControl_PointerReleased(null, null);
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"Error reading file: {ex.Message}", "Error");
            }
        }

        private void CanvasControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var canvas = sender as CanvasControl;
            canvas.Invalidate();
        }

        private void myClear_Click(object sender, RoutedEventArgs e)
        {
            vx.Clear();
            vy.Clear();
            col.Clear();
            size.Clear();
            flag = false;
            px = 100;
            py = 100;
            mySize = 16;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            vx.Clear();
            vy.Clear();
            size.Clear();
            col.Clear();
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Save
            try
            {
                using (StreamWriter sw = new StreamWriter("C:/Users/user/Desktop/visual/pen_folder/1.txt"))
                {
                    for (int i = 0; i < vx.Count; i++)
                    {
                        sw.WriteLine($"{vx[i]} {vy[i]} {col[i].R} {col[i].G} {col[i].B} {col[i].A} {size[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"Error saving file: {ex.Message}", "Error");
            }
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            // Load
            try
            {
                using (StreamReader sr = new StreamReader("C:/Users/user/Desktop/visual/pen_folder/1.txt"))
                {
                    vx.Clear();
                    vy.Clear();
                    size.Clear();
                    col.Clear();

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split();
                        vx.Add(float.Parse(parts[0]));
                        vy.Add(float.Parse(parts[1]));
                        col.Add(Color.FromArgb(byte.Parse(parts[5]), byte.Parse(parts[2]), byte.Parse(parts[3]), byte.Parse(parts[4])));
                        size.Add(float.Parse(parts[6]));
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"Error reading file: {ex.Message}", "Error");
            }
        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}