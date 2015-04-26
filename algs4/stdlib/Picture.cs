using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace algs4.stdlib
{
    public sealed class Picture
    {
        /// <summary>
        /// The rasterized image
        /// </summary>
        private readonly Bitmap _image;

        /// <summary>
        /// On-screen view
        /// </summary>
        private Form _frame;

        /// <summary>
        /// Name of file
        /// </summary>
        private string _filename;

        /// <summary>
        /// Location of origin
        /// </summary>
        private bool _isOriginUpperLeft = true;

        /// <summary>
        /// Width
        /// </summary>
        private readonly int _width;

        /// <summary>
        ///Height
        /// </summary>
        private readonly int _height;

        /// <summary>
        /// Initializes a blank width-by-height picture, with width columns
        /// and height rows, where each pixel is black.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Picture(int width, int height)
        {
            if (width < 0)
            {
                throw new ArgumentException("width must be nonnegative");
            }
            if (height < 0)
            {
                throw new ArgumentException("height must be nonnegative");
            }
            _width = width;
            _height = height;
            _image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            // set to TYPE_INT_ARGB to support transparency
            _filename = width + "-by-" + height;
        }

        /// <summary>
        /// Initializes a new picture that is a deep copy of picture.
        /// </summary>
        /// <param name="picture"></param>
        public Picture(Picture picture)
        {
            _width = picture.Width();
            _height = picture.Height();
            _image = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
            _filename = picture._filename;
            for (int col = 0; col < Width(); col++)
            {
                for (int row = 0; row < Height(); row++)
                {
                    _image.SetPixel(col, row, picture.Get(col, row));
                }
            }
        }

        /// <summary>
        /// Initializes a picture by reading in a .png, .gif, or .jpg from
        /// the given filename or URL name.
        /// </summary>
        /// <param name="filename"></param>
        public Picture(string filename)
        {
            _filename = filename;
            try
            {
                _image = new Bitmap(filename, true);
            }
            catch (ArgumentException)
            {
                Stream stream = WebRequest.Create(filename).GetResponse().GetResponseStream();
                if (stream != null)
                {
                    _image = new Bitmap(stream);
                }
            }
            _width = _image.Width;
            _height = _image.Height;
        }

        /// <summary>
        /// Initializes a picture by reading in a .png, .gif, or .jpg from a File.
        /// </summary>
        /// <param name="file"></param>
        public Picture(FileStream file)
        {
            _image = new Bitmap(file);
            _width = _image.Width;
            _height = _image.Height;
            _filename = file.Name;
        }

        /// <summary>
        /// Returns a PictureBox containing this picture, for embedding in a Form or other GUI widget.
        /// </summary>
        /// <returns>the PictureBox</returns>
        public PictureBox GetPictureBox()
        {
            // no image available
            if (_image == null)
            {
                return null;
            }

            return new PictureBox { Image = _image, AutoSize = true };
        }

        /// <summary>
        /// Sets the origin to be the upper left pixel. This is the default.
        /// </summary>
        public void SetOriginUpperLeft()
        {
            _isOriginUpperLeft = true;
        }

        /// <summary>
        /// Sets the origin to be the lower left pixel.
        /// </summary>
        public void SetOriginLowerLeft()
        {
            _isOriginUpperLeft = false;
        }

        /// <summary>
        /// Displays the picture in a window on the screen.
        /// </summary>
        public void Show()
        {
            // create the GUI for viewing the image if needed
            if (_frame == null)
            {
                _frame = new Form { AutoSize = true };

                MainMenu menuBar = new MainMenu();

                MenuItem menu = new MenuItem("File");
                menuBar.MenuItems.Add(menu);

                MenuItem menuItem1 = new MenuItem("Save...");
                menuItem1.Click += SaveMenuItemClicked;
                menuItem1.Shortcut = Shortcut.CtrlS;

                menu.MenuItems.Add(menuItem1);
                _frame.Menu = menuBar;

                _frame.Controls.Add(GetPictureBox());
                _frame.Text = _filename;
                _frame.FormBorderStyle = FormBorderStyle.Fixed3D;

                new Thread(() => Application.Run(_frame)).Start();
                _frame.Focus();
            }
        }

        /// <summary>
        /// Returns the height of the picture.
        /// </summary>
        /// <returns>the height of the picture (in pixels)</returns>
        public int Height()
        {
            return _height;
        }

        /// <summary>
        /// Returns the width of the picture.
        /// </summary>
        /// <returns>the width of the picture (in pixels)</returns>
        public int Width()
        {
            return _width;
        }

        /// <summary>
        /// Returns the color of pixel (col, row).
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns>the color of pixel (col, row)</returns>
        public Color Get(int col, int row)
        {
            if (col < 0 || col >= Width())
            {
                throw new IndexOutOfRangeException("col must be between 0 and " + (Width() - 1));
            }
            if (row < 0 || row >= Height())
            {
                throw new IndexOutOfRangeException("row must be between 0 and " + (Height() - 1));
            }
            if (_isOriginUpperLeft)
            {
                return _image.GetPixel(col, row);
            }
            return _image.GetPixel(col, _height - row - 1);
        }

        /// <summary>
        /// Sets the color of pixel (col, row) to given color.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="color"></param>
        public void Set(int col, int row, Color color)
        {
            if (col < 0 || col >= Width())
            {
                throw new IndexOutOfRangeException("col must be between 0 and " + (Width() - 1));
            }
            if (row < 0 || row >= Height())
            {
                throw new IndexOutOfRangeException("row must be between 0 and " + (Height() - 1));
            }
            if (color == null)
            {
                throw new NullReferenceException("can't set Color to null");
            }
            if (_isOriginUpperLeft)
            {

            }
            else
            {
                _image.SetPixel(col, _height - row - 1, color);
            }
        }

        /// <summary>
        /// Is this Picture equal to obj?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if this picture is the same dimension as obj and if all pixels have the same color</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }
            Picture that = (Picture)obj;
            if (Width() != that.Width())
            {
                return false;
            }
            if (Height() != that.Height())
            {
                return false;
            }
            for (int col = 0; col < Width(); col++)
            {
                for (int row = 0; row < Height(); row++)
                {
                    if (!Get(col, row).Equals(that.Get(col, row)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return _image.GetHashCode();
        }

        /// <summary>
        /// Saves the picture to a file in a standard image format.
        /// The filetype must be .png or .jpg.
        /// </summary>
        /// <param name="name"></param>
        public void Save(string name)
        {
            Save(new FileStream(name, FileMode.Create));
        }

        /// <summary>
        /// Saves the picture to a file in a standard image format.
        /// </summary>
        /// <param name="file"></param>
        public void Save(FileStream file)
        {
            _filename = file.Name;
            if (_frame != null)
            {
                _frame.Text = _filename;
            }
            string suffix = _filename.Substring(_filename.LastIndexOf('.') + 1);
            suffix = suffix.ToLower();

            if (suffix.Equals("jpg") || suffix.Equals("png"))
            {
                _image.Save(file, suffix == "jpg" ? ImageFormat.Jpeg : ImageFormat.Png);
            }
            else
            {
                Console.WriteLine("Error: filename must end in .jpg or .png");
            }
        }

        /// <summary>
        /// Opens a save dialog box when the user selects "Save As" from the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SaveMenuItemClicked(object sender, EventArgs e)
        {
            FileDialog chooser = new SaveFileDialog
            {
                Title = "Use a .png or .jpg extension"
            };

            Thread t = new Thread(() => chooser.ShowDialog());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            if (chooser.FileName != null)
            {
                Save(chooser.FileName);
            }
        }

        /// <summary>
        /// Tests this Picture data type. Reads a picture specified by the command-line argument,
        /// and shows it in a window on the screen.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Picture picture = new Picture(args[0]);
            Console.WriteLine("{0}-by-{1}", picture.Width(), picture.Height());
            picture.Show();
        }
    }
}
