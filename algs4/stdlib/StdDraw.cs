using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace algs4.stdlib
{
    public static class StdDraw
    {
        #region Pre-defined colors

        public static readonly Color Black = Color.Black;
        public static readonly Color Blue = Color.Blue;
        public static readonly Color Cyan = Color.Cyan;
        public static readonly Color DarkGray = Color.DarkGray;
        public static readonly Color Gray = Color.Gray;
        public static readonly Color Green = Color.Green;
        public static readonly Color LightGray = Color.LightGray;
        public static readonly Color Magenta = Color.Magenta;
        public static readonly Color Orange = Color.Orange;
        public static readonly Color Pink = Color.Pink;
        public static readonly Color Red = Color.Red;
        public static readonly Color White = Color.White;
        public static readonly Color Yellow = Color.Yellow;

        #endregion

        /// <summary>
        /// Shade of blue used in Introduction to Programming in Java.
        /// It is Pantone 300U. The RGB values are approximately (9, 90, 166).
        /// </summary>
        public static readonly Color BookBlue = Color.FromArgb(9, 90, 166);

        public static readonly Color BookLightBlue = Color.FromArgb(103, 198, 243);

        /// <summary>
        /// Shade of red used in Algorithms 4th edition.
        /// It is Pantone 1805U. The RGB values are approximately (150, 35, 31).
        /// </summary>
        public static readonly Color BookRed = Color.FromArgb(150, 35, 31);

        /// <summary>
        /// Default pen color
        /// </summary>
        private static readonly Color DefaultPenColor = Black;

        /// <summary>
        /// Default clear color
        /// </summary>
        private static readonly Color DefaultClearColor = White;

        /// <summary>
        /// Current pen color
        /// </summary>
        private static Color _penColor;

        /// <summary>
        /// Default canvas size is DefaultSize-by-DefaultSize
        /// </summary>
        private const int DefaultSize = 512;

        private static int _width = DefaultSize;
        private static int _height = DefaultSize;

        /// <summary>
        /// Default pen radius
        /// </summary>
        private const float DefaultPenRadius = 0.002f;

        /// <summary>
        /// Current pen radius
        /// </summary>
        private static float _penRadius;

        /// <summary>
        /// Show we draw immediately or wait until next show?
        /// </summary>
        private static bool _defer;

        /// <summary>
        /// Boundary of drawing canvas, 0.1% border
        /// </summary>
        private const float Border = 0.001f;

        private const float DefaultXmin = 0.0f;
        private const float DefaultXmax = 1.0f;
        private const float DefaultYmin = 0.0f;
        private const float DefaultYmax = 1.0f;
        private static float _xmin, _ymin, _xmax, _ymax;

        /// <summary>
        /// For mouse synchronization
        /// </summary>
        private static readonly object MouseLock = new object();

        /// <summary>
        /// For keyboard synchronization
        /// </summary>
        private static readonly object KeyLock = new object();

        /// <summary>
        /// Default font
        /// </summary>
        private static readonly Font DefaultFont = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular);

        /// <summary>
        /// Current font
        /// </summary>
        private static Font _font;

        /// <summary>
        /// Double buffered graphics
        /// </summary>
        private static Bitmap _offscreenImage, _onscreenImage;

        private static Graphics _offscreen, _onscreen;

        /// <summary>
        /// The frame for drawing to the screen
        /// </summary>
        private static Form _frame;

        /// <summary>
        /// Mouse state pressed
        /// </summary>
        private static bool _mousePressed;

        /// <summary>
        /// Mouse state x
        /// </summary>
        private static double _mouseX;

        /// <summary>
        /// Mouse state y
        /// </summary>
        private static double _mouseY;

        /// <summary>
        /// Queue of typed key characters
        /// </summary>
        private static readonly List<char> KeysTyped = new List<char>();

        /// <summary>
        /// Set of key codes currently pressed down
        /// </summary>
        private static readonly SortedSet<int> KeysDown = new SortedSet<int>();

        /// <summary>
        /// Static initializer
        /// </summary>
        static StdDraw()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Init();
        }

        /// <summary>
        /// Set the window size to the default size 512-by-512 pixels.
        /// This method must be called before any other commands.
        /// </summary>
        public static void SetCanvasSize()
        {
            SetCanvasSize(DefaultSize, DefaultSize);
        }

        /// <summary>
        /// Set the window size to w-by-h pixels.
        /// This method must be called before any other commands.
        /// </summary>
        /// <param name="w">the width as a number of pixels</param>
        /// <param name="h">the height as a number of pixels</param>
        public static void SetCanvasSize(int w, int h)
        {
            if (w < 1 || h < 1)
            {
                throw new ArgumentException("width and height must be positive");
            }
            _width = w;
            _height = h;
            Init();
        }

        /// <summary>
        /// Init
        /// </summary>
        private static void Init()
        {
            if (_frame != null)
            {
                _frame.Visible = false;
            }
            _frame = new Form { AutoSize = true };
            _offscreenImage = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
            _onscreenImage = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);

            _offscreen = Graphics.FromImage(_offscreenImage);
            _onscreen = Graphics.FromImage(_onscreenImage);
            SetXscale();
            SetYscale();

            _offscreen.Clear(DefaultClearColor);
            SetPenColor();
            SetPenRadius();
            SetFont();
            Clear();

            _offscreen.TextRenderingHint = TextRenderingHint.AntiAlias;

            // frame stuff
            Image icon = _onscreenImage;
            PictureBox draw = new PictureBox { Image = icon, AutoSize = true };

            draw.MouseDown += MouseDown;
            draw.MouseUp += MouseUp;

            draw.DragOver += MouseDragged;
            draw.MouseMove += MouseMoved;

            _frame.Controls.Add(draw);
            _frame.KeyPress += KeyPressed;
            _frame.KeyDown += KeyDown;
            _frame.KeyUp += KeyUp;
            _frame.FormBorderStyle = FormBorderStyle.Fixed3D;
            _frame.Text = "Standard Draw";
            _frame.Menu = CreateMenuBar();

            new Thread(() => Application.Run(_frame)).Start();
            _frame.Focus();
        }

        /// <summary>
        /// Create the menu bar (changed to private)
        /// </summary>
        /// <returns></returns>
        private static MainMenu CreateMenuBar()
        {
            MainMenu menuBar = new MainMenu();

            MenuItem menu = new MenuItem("File");
            menuBar.MenuItems.Add(menu);

            MenuItem menuItem1 = new MenuItem("Save...");
            menuItem1.Click += SaveMenuItemClicked;
            menuItem1.Shortcut = Shortcut.CtrlS;

            menu.MenuItems.Add(menuItem1);
            return menuBar;
        }

        #region User and screen coordinate systems

        /// <summary>
        /// Set the x-scale to be the default (between 0.0 and 1.0).
        /// </summary>
        public static void SetXscale()
        {
            SetXscale(DefaultXmin, DefaultXmax);
        }

        /// <summary>
        /// Set the y-scale to be the default (between 0.0 and 1.0).
        /// </summary>
        public static void SetYscale()
        {
            SetYscale(DefaultYmin, DefaultYmax);
        }

        /// <summary>
        /// Set the x-scale
        /// </summary>
        /// <param name="min">the minimum value of the x-scale</param>
        /// <param name="max">the maximum value of the x-scale</param>
        public static void SetXscale(float min, float max)
        {
            float size = max - min;
            _xmin = min - Border * size;
            _xmax = max + Border * size;
        }

        /// <summary>
        /// Set the y-scale
        /// </summary>
        /// <param name="min">the minimum value of the y-scale</param>
        /// <param name="max">the maximum value of the y-scale</param>
        public static void SetYscale(float min, float max)
        {
            float size = max - min;
            _ymin = min - Border * size;
            _ymax = max + Border * size;
        }

        /// <summary>
        /// Set the x-scale and y-scale
        /// </summary>
        /// <param name="min">the minimum value of the x- and y-scales</param>
        /// <param name="max">the maximum value of the x- and y-scales</param>
        public static void SetScale(float min, float max)
        {
            float size = max - min;
            _xmin = min - Border * size;
            _xmax = max + Border * size;
            _ymin = min - Border * size;
            _ymax = max + Border * size;
        }

        #endregion

        #region Helper functions that scale from user coordinates to screen coordinates and back

        private static float ScaleX(float x)
        {
            return _width * (x - _xmin) / (_xmax - _xmin);
        }

        private static float ScaleY(float y)
        {
            return _height * (_ymax - y) / (_ymax - _ymin);
        }

        private static float FactorX(float w)
        {
            return w * _width / Math.Abs(_xmax - _xmin);
        }

        private static float FactorY(float h)
        {
            return h * _height / Math.Abs(_ymax - _ymin);
        }

        private static float UserX(float x)
        {
            return _xmin + x * (_xmax - _xmin) / _width;
        }

        private static float UserY(float y)
        {
            return _ymax - y * (_ymax - _ymin) / _height;
        }

        #endregion

        /// <summary>
        /// Clear the screen to the default color (white).
        /// </summary>
        public static void Clear()
        {
            Clear(DefaultClearColor);
        }

        /// <summary>
        /// Clear the screen to the given color.
        /// </summary>
        /// <param name="color">the Color to make the background</param>
        public static void Clear(Color color)
        {
            _offscreen.Clear(color);
            Draw();
        }

        /// <summary>
        /// Get the current pen radius.
        /// </summary>
        /// <returns></returns>
        public static double GetPenRadius()
        {
            return _penRadius;
        }

        /// <summary>
        /// Set the pen size to the default (.002).
        /// </summary>
        public static void SetPenRadius()
        {
            SetPenRadius(DefaultPenRadius);
        }

        /// <summary>
        /// Set the radius of the pen to the given size.
        /// </summary>
        /// <param name="r">the radius of the pen</param>
        public static void SetPenRadius(float r)
        {
            if (r < 0)
            {
                throw new ArgumentException("pen radius must be nonnegative");
            }
            _penRadius = r;
        }

        /// <summary>
        /// Get the current pen color.
        /// </summary>
        /// <returns></returns>
        public static Color GetPenColor()
        {
            return _penColor;
        }

        /// <summary>
        /// Set the pen color to the default color (black).
        /// </summary>
        public static void SetPenColor()
        {
            SetPenColor(DefaultPenColor);
        }

        /// <summary>
        /// Set the pen color to the given color. The available pen colors are
        /// BLACK, BLUE, CYAN, DARK_GRAY, GRAY, GREEN, LIGHT_GRAY, MAGENTA,
        /// ORANGE, PINK, RED, WHITE, and YELLOW.
        /// </summary>
        /// <param name="color">the Color to make the pen</param>
        public static void SetPenColor(Color color)
        {
            _penColor = color;
        }

        /// <summary>
        /// Set the pen color to the given RGB color.
        /// </summary>
        /// <param name="red">the amount of red (between 0 and 255)</param>
        /// <param name="green">the amount of green (between 0 and 255)</param>
        /// <param name="blue">the amount of blue (between 0 and 255)</param>
        public static void SetPenColor(int red, int green, int blue)
        {
            if (red < 0 || red >= 256)
            {
                throw new ArgumentException("amount of red must be between 0 and 255");
            }
            if (green < 0 || green >= 256)
            {
                throw new ArgumentException("amount of green must be between 0 and 255");
            }
            if (blue < 0 || blue >= 256)
            {
                throw new ArgumentException("amount of blue must be between 0 and 255");
            }
            SetPenColor(Color.FromArgb(red, green, blue));
        }

        /// <summary>
        /// Get the current font.
        /// </summary>
        /// <returns></returns>
        public static Font GetFont()
        {
            return _font;
        }

        /// <summary>
        /// Set the font to the default font (sans serif, 16 point).
        /// </summary>
        public static void SetFont()
        {
            SetFont(DefaultFont);
        }

        /// <summary>
        /// Set the font to the given value.
        /// </summary>
        /// <param name="f">the font to make text</param>
        public static void SetFont(Font f)
        {
            _font = f;
        }

        #region Drawing geometric shapes.

        /// <summary>
        /// Draw a line from (x0, y0) to (x1, y1).
        /// </summary>
        /// <param name="x0">the x-coordinate of the starting point</param>
        /// <param name="y0">the y-coordinate of the starting point</param>
        /// <param name="x1">the x-coordinate of the destination point</param>
        /// <param name="y1">the y-coordinate of the destination point</param>
        public static void Line(float x0, float y0, float x1, float y1)
        {
            _offscreen.DrawLine(new Pen(_penColor, _penRadius), ScaleX(x0), ScaleY(y0), ScaleX(x1), ScaleY(y1));
            Draw();
        }

        /// <summary>
        /// Draw one pixel at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the pixel</param>
        /// <param name="y">the y-coordinate of the pixel</param>
        private static void Pixel(float x, float y)
        {
            _offscreen.FillRectangle(new SolidBrush(_penColor), (int)Math.Round(ScaleX(x)), (int)Math.Round(ScaleY(y)), 1, 1);
            Draw();
        }

        /// <summary>
        /// Draw a point at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the point</param>
        /// <param name="y">the y-coordinate of the point</param>
        public static void Point(float x, float y)
        {
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float r = _penRadius;
            float scaledPenRadius = r * DefaultSize;

            if (scaledPenRadius <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.FillEllipse(new SolidBrush(_penColor), xs - scaledPenRadius / 2, ys - scaledPenRadius / 2, scaledPenRadius, scaledPenRadius);
            }
            Draw();
        }

        /// <summary>
        /// Draw a circle of radius r, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the circle</param>
        /// <param name="y">the y-coordinate of the center of the circle</param>
        /// <param name="r">the radius of the circle</param>
        public static void Circle(float x, float y, float r)
        {
            if (r < 0)
            {
                throw new ArgumentException("circle radius must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * r);
            float hs = FactorY(2 * r);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.DrawEllipse(new Pen(_penColor, _penRadius), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw filled circle of radius r, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the circle</param>
        /// <param name="y">the y-coordinate of the center of the circle</param>
        /// <param name="r">the radius of the circle</param>
        public static void FilledCircle(float x, float y, float r)
        {
            if (r < 0)
            {
                throw new ArgumentException("circle radius must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * r);
            float hs = FactorY(2 * r);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.FillEllipse(new SolidBrush(_penColor), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw an ellipse with given semimajor and semiminor axes, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the ellipse</param>
        /// <param name="y">the y-coordinate of the center of the ellipse</param>
        /// <param name="semiMajorAxis">is the semimajor axis of the ellipse</param>
        /// <param name="semiMinorAxis">is the semiminor axis of the ellipse</param>
        public static void Ellipse(float x, float y, float semiMajorAxis, float semiMinorAxis)
        {
            if (semiMajorAxis < 0)
            {
                throw new ArgumentException("ellipse semimajor axis must be nonnegative");
            }
            if (semiMinorAxis < 0)
            {
                throw new ArgumentException("ellipse semiminor axis must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * semiMajorAxis);
            float hs = FactorY(2 * semiMinorAxis);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.DrawEllipse(new Pen(_penColor, _penRadius), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw an ellipse with given semimajor and semiminor axes, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the ellipse</param>
        /// <param name="y">the y-coordinate of the center of the ellipse</param>
        /// <param name="semiMajorAxis">is the semimajor axis of the ellipse</param>
        /// <param name="semiMinorAxis">is the semiminor axis of the ellipse</param>
        public static void FilledEllipse(float x, float y, float semiMajorAxis, float semiMinorAxis)
        {
            if (semiMajorAxis < 0)
            {
                throw new ArgumentException("ellipse semimajor axis must be nonnegative");
            }
            if (semiMinorAxis < 0)
            {
                throw new ArgumentException("ellipse semiminor axis must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * semiMajorAxis);
            float hs = FactorY(2 * semiMinorAxis);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.FillEllipse(new SolidBrush(_penColor), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw an arc of radius r, centered on (x, y), from angle1 to angle2 (in degrees).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the circle</param>
        /// <param name="y">the y-coordinate of the center of the circle</param>
        /// <param name="r">the radius of the circle</param>
        /// <param name="angle1">the starting angle. 0 would mean an arc beginning at 3 o'clock.</param>
        /// <param name="angle2">the angle at the end of the arc. For example, if you want a 90 degree arc, then angle2 should be angle1 + 90.</param>
        public static void Arc(float x, float y, float r, float angle1, float angle2)
        {
            if (r < 0)
            {
                throw new ArgumentException("arc radius must be nonnegative");
            }
            while (angle2 < angle1)
            {
                angle2 += 360;
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * r);
            float hs = FactorY(2 * r);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.DrawArc(new Pen(_penColor, _penRadius), xs - ws / 2, ys - hs / 2, ws, hs, angle1, angle2 - angle1);
            }
            Draw();
        }

        /// <summary>
        /// Draw a square of side length 2r, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the square</param>
        /// <param name="y">the y-coordinate of the center of the square</param>
        /// <param name="r">radius is half the length of any side of the square</param>
        public static void Square(float x, float y, float r)
        {
            if (r < 0)
            {
                throw new ArgumentException("square side length must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * r);
            float hs = FactorY(2 * r);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.DrawRectangle(new Pen(_penColor, _penRadius), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw a filled square of side length 2r, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the square</param>
        /// <param name="y">the y-coordinate of the center of the square</param>
        /// <param name="r">radius is half the length of any side of the square</param>
        public static void FilledSquare(float x, float y, float r)
        {
            if (r < 0)
            {
                throw new ArgumentException("square side length must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * r);
            float hs = FactorY(2 * r);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.FillRectangle(new SolidBrush(_penColor), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw a rectangle of given half width and half height, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the rectangle</param>
        /// <param name="y">the y-coordinate of the center of the rectangle</param>
        /// <param name="halfWidth">is half the width of the rectangle</param>
        /// <param name="halfHeight">is half the height of the rectangle</param>
        public static void Rectangle(float x, float y, float halfWidth, float halfHeight)
        {
            if (halfWidth < 0)
            {
                throw new ArgumentException("half width must be nonnegative");
            }
            if (halfHeight < 0)
            {
                throw new ArgumentException("half height must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * halfWidth);
            float hs = FactorY(2 * halfHeight);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.DrawRectangle(new Pen(_penColor, _penRadius), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw a filled rectangle of given half width and half height, centered on (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the center of the rectangle</param>
        /// <param name="y">the y-coordinate of the center of the rectangle</param>
        /// <param name="halfWidth">is half the width of the rectangle</param>
        /// <param name="halfHeight">is half the height of the rectangle</param>
        public static void FilledRectangle(float x, float y, float halfWidth, float halfHeight)
        {
            if (halfWidth < 0)
            {
                throw new ArgumentException("half width must be nonnegative");
            }
            if (halfHeight < 0)
            {
                throw new ArgumentException("half height must be nonnegative");
            }
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            float ws = FactorX(2 * halfWidth);
            float hs = FactorY(2 * halfHeight);
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.FillRectangle(new SolidBrush(_penColor), xs - ws / 2, ys - hs / 2, ws, hs);
            }
            Draw();
        }

        /// <summary>
        /// Draw a polygon with the given (x[i], y[i]) coordinates.
        /// </summary>
        /// <param name="x">an array of all the x-coordindates of the polygon</param>
        /// <param name="y">an array of all the y-coordindates of the polygon</param>
        public static void Polygon(float[] x, float[] y)
        {
            int n = x.Length;
            GraphicsPath path = new GraphicsPath();

            for (int i = 0; i < n; i++)
            {
                path.AddLine(ScaleX(x[i]), ScaleY(y[i]), ScaleX(x[(i + 1) % n]), ScaleY(y[(i + 1) % n]));
            }

            _offscreen.DrawPath(new Pen(_penColor, _penRadius), path);
            Draw();
        }

        /// <summary>
        /// Draw a filled polygon with the given (x[i], y[i]) coordinates.
        /// </summary>
        /// <param name="x">an array of all the x-coordindates of the polygon</param>
        /// <param name="y">an array of all the y-coordindates of the polygon</param>
        public static void FilledPolygon(float[] x, float[] y)
        {
            int n = x.Length;
            GraphicsPath path = new GraphicsPath();

            for (int i = 0; i < n; i++)
            {
                path.AddLine(ScaleX(x[i]), ScaleY(y[i]), ScaleX(x[(i + 1) % n]), ScaleY(y[(i + 1) % n]));
            }

            _offscreen.FillPath(new SolidBrush(_penColor), path);
            Draw();
        }

        #endregion

        #region Drawing images.

        /// <summary>
        /// Get an image from the given filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static Image GetImage(string filename)
        {
            // to read from file
            Bitmap icon = null;
            try
            {
                icon = new Bitmap(filename, true);
            }
            catch (ArgumentException)
            {
                Stream stream = WebRequest.Create(filename).GetResponse().GetResponseStream();
                if (stream != null)
                {
                    icon = new Bitmap(stream);
                }
            }

            return icon;
        }

        /// <summary>
        /// Draw picture (gif, jpg, or png) centered on (x, y).
        /// </summary>
        /// <param name="x">the center x-coordinate of the image</param>
        /// <param name="y">the center y-coordinate of the image</param>
        /// <param name="s">the name of the image/picture, e.g., "ball.gif"</param>
        public static void Picture(float x, float y, string s)
        {
            Image image = GetImage(s);
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            int ws = image.Width;
            int hs = image.Height;
            if (ws < 0 || hs < 0)
            {
                throw new ArgumentException("image " + s + " is corrupt");
            }

            _offscreen.DrawImage(image, (int)Math.Round(xs - ws / 2.0), (int)Math.Round(ys - hs / 2.0));
            Draw();
        }

        /// <summary>
        /// Draw picture (gif, jpg, or png) centered on (x, y).
        /// rotated given number of degrees
        /// </summary>
        /// <param name="x">the center x-coordinate of the image</param>
        /// <param name="y">the center y-coordinate of the image</param>
        /// <param name="s">the name of the image/picture, e.g., "ball.gif"</param>
        /// <param name="degree">is the number of degrees to rotate counterclockwise</param>
        public static void Picture(float x, float y, string s, float degree)
        {
            Image image = GetImage(s);
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            int ws = image.Width;
            int hs = image.Height;
            if (ws < 0 || hs < 0)
            {
                throw new ArgumentException("image " + s + " is corrupt");
            }

            _offscreen.TranslateTransform(-xs, -ys);
            _offscreen.RotateTransform(-degree);
            _offscreen.DrawImage(image, (int)Math.Round(xs - ws / 2.0), (int)Math.Round(ys - hs / 2.0));
            _offscreen.RotateTransform(degree);
            _offscreen.TranslateTransform(xs, ys);
            Draw();
        }

        /// <summary>
        /// Draw picture (gif, jpg, or png) centered on (x, y), rescaled to w-by-h.
        /// </summary>
        /// <param name="x">the center x-coordinate of the image</param>
        /// <param name="y">the center y-coordinate of the image</param>
        /// <param name="s">the name of the image/picture, e.g., "ball.gif"</param>
        /// <param name="w">the width of the image</param>
        /// <param name="h">the height of the image</param>
        public static void Picture(float x, float y, string s, float w, float h)
        {
            Image image = GetImage(s);
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            if (w < 0)
            {
                throw new ArgumentException("width is negative: " + w);
            }
            if (h < 0)
            {
                throw new ArgumentException("height is negative: " + h);
            }
            float ws = FactorX(w);
            float hs = FactorY(h);
            if (ws < 0 || hs < 0)
            {
                throw new ArgumentException("image " + s + " is corrupt");
            }
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.DrawImage(image, (int)Math.Round(xs - ws / 2.0), (int)Math.Round(ys - hs / 2.0), (int)Math.Round(ws), (int)Math.Round(hs));
            }

            Draw();
        }

        /// <summary>
        /// Draw picture (gif, jpg, or png) centered on (x, y), rotated
        /// given number of degrees, rescaled to w-by-h.
        /// </summary>
        /// <param name="x">the center x-coordinate of the image</param>
        /// <param name="y">the center y-coordinate of the image</param>
        /// <param name="s">the name of the image/picture, e.g., "ball.gif"</param>
        /// <param name="w">the width of the image</param>
        /// <param name="h">the height of the image</param>
        /// <param name="degree">is the number of degrees to rotate counterclockwise</param>
        public static void Picture(float x, float y, string s, float w, float h, float degree)
        {
            Image image = GetImage(s);
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            if (w < 0)
            {
                throw new ArgumentException("width is negative: " + w);
            }
            if (h < 0)
            {
                throw new ArgumentException("height is negative: " + h);
            }
            float ws = FactorX(w);
            float hs = FactorY(h);
            if (ws < 0 || hs < 0)
            {
                throw new ArgumentException("image " + s + " is corrupt");
            }
            if (ws <= 1 && hs <= 1)
            {
                Pixel(x, y);
            }
            else
            {
                _offscreen.TranslateTransform(-xs, -ys);
                _offscreen.RotateTransform(-degree);
                _offscreen.DrawImage(image, (int)Math.Round(xs - ws / 2.0), (int)Math.Round(ys - hs / 2.0), (int)Math.Round(ws), (int)Math.Round(hs));
                _offscreen.RotateTransform(degree);
                _offscreen.TranslateTransform(xs, ys);
            }

            Draw();
        }

        #endregion

        #region Drawing text.

        /// <summary>
        /// Write the given text string in the current font, centered on (x, y).
        /// </summary>
        /// <param name="x">the center x-coordinate of the text</param>
        /// <param name="y">the center y-coordinate of the text</param>
        /// <param name="s">the text</param>
        public static void Text(float x, float y, string s)
        {
            float xs = ScaleX(x);
            float ys = ScaleY(y);

            SizeF stringSize = _offscreen.MeasureString(s, _font);
            float ws = stringSize.Width;
            float hs = stringSize.Height;

            _offscreen.DrawString(s, _font, new SolidBrush(_penColor), xs - ws / 2.0f, ys - hs / 2.0f);
            Draw();
        }

        /// <summary>
        /// Write the given text string in the current font, centered on (x, y) and
        /// rotated by the specified number of degrees  
        /// </summary>
        /// <param name="x">the center x-coordinate of the text</param>
        /// <param name="y">the center y-coordinate of the text</param>
        /// <param name="s">the text</param>
        /// <param name="degrees">is the number of degrees to rotate counterclockwise</param>
        public static void Text(float x, float y, string s, float degrees)
        {
            float xs = ScaleX(x);
            float ys = ScaleY(y);
            _offscreen.TranslateTransform(-xs, -ys);
            _offscreen.RotateTransform(-degrees);
            Text(x, y, s);
            _offscreen.RotateTransform(degrees);
            _offscreen.TranslateTransform(xs, ys);
        }

        /// <summary>
        /// Write the given text string in the current font, left-aligned at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the text</param>
        /// <param name="y">the y-coordinate of the text</param>
        /// <param name="s">the text</param>
        public static void TextLeft(float x, float y, string s)
        {
            float xs = ScaleX(x);
            float ys = ScaleY(y);

            SizeF stringSize = _offscreen.MeasureString(s, _font);
            float hs = stringSize.Height;

            _offscreen.DrawString(s, _font, new SolidBrush(_penColor), xs, ys - hs / 2.0f);
            Draw();
        }

        /// <summary>
        /// Write the given text string in the current font, right-aligned at (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate of the text</param>
        /// <param name="y">the y-coordinate of the text</param>
        /// <param name="s">the text</param>
        public static void TextRight(float x, float y, string s)
        {
            float xs = ScaleX(x);
            float ys = ScaleY(y);

            SizeF stringSize = _offscreen.MeasureString(s, _font);
            float ws = stringSize.Width;
            float hs = stringSize.Height;

            _offscreen.DrawString(s, _font, new SolidBrush(_penColor), xs - ws, ys - hs / 2.0f);
            Draw();
        }

        #endregion

        /// <summary>
        /// Display on screen, pause for t milliseconds, and turn on
        /// animation mode: subsequent calls to
        /// drawing methods such as line(), circle(), and square()
        /// will not be displayed on screen until the next call to show().
        /// This is useful for producing animations (clear the screen, draw a bunch of shapes,
        /// display on screen for a fixed amount of time, and repeat). It also speeds up
        /// drawing a huge number of shapes (call show(0) to defer drawing
        /// on screen, draw the shapes, and call show(0) to display them all
        /// on screen at once).
        /// </summary>
        /// <param name="t">number of milliseconds</param>
        public static void Show(int t)
        {
            _defer = false;
            Draw();
            Thread.Sleep(t);
            _defer = true;
        }

        /// <summary>
        /// Display on-screen and turn off animation mode:
        /// subsequent calls to
        /// drawing methods such as line(), circle(), and square()
        /// will be displayed on screen when called. This is the default.
        /// </summary>
        public static void Show()
        {
            _defer = false;
            Draw();
        }

        /// <summary>
        /// Draw onscreen if defer is false
        /// </summary>
        private static void Draw()
        {
            if (_defer)
            {
                return;
            }

            _onscreen.DrawImage(_offscreenImage, 0, 0);
        }

        #region Save drawing to a file.

        /// <summary>
        /// Save onscreen image to file - suffix must be png, jpg, or gif.
        /// </summary>
        /// <param name="filename">the name of the file with one of the required suffixes</param>
        public static void Save(string filename)
        {
            string suffix = filename.Substring(filename.LastIndexOf('.') + 1);

            // png files
            if (suffix.ToLower().Equals("png"))
            {
                try
                {
                    _onscreenImage.Save(filename, ImageFormat.Png);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            else if (suffix.ToLower().Equals("jpg"))
            {
                try
                {
                    _onscreenImage.Save(filename, ImageFormat.Jpeg);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            else
            {
                Console.WriteLine("Invalid image file type: " + suffix);
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SaveMenuItemClicked(object sender, EventArgs e)
        {
            FileDialog chooser = new SaveFileDialog
            {
                Title = "Use a .png or .jpg extension"
            };

            Thread t = new Thread(() => chooser.ShowDialog());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            string filename = chooser.FileName;
            if (filename != null)
            {
                Save(filename);
            }
        }

        #endregion

        #region Mouse interactions.

        /// <summary>
        /// Is the mouse being pressed?
        /// </summary>
        /// <returns>true or false</returns>
        public static bool MouseDown()
        {
            lock (MouseLock)
            {
                return _mousePressed;
            }
        }

        /// <summary>
        /// What is the x-coordinate of the mouse?
        /// </summary>
        /// <returns>the value of the x-coordinate of the mouse</returns>
        public static double MouseX()
        {
            lock (MouseLock)
            {
                return _mouseX;
            }
        }

        /// <summary>
        /// What is the y-coordinate of the mouse?
        /// </summary>
        /// <returns>the value of the y-coordinate of the mouse</returns>
        public static double MouseY()
        {
            lock (MouseLock)
            {
                return _mouseY;
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MouseDown(object sender, MouseEventArgs e)
        {
            lock (MouseLock)
            {
                _mouseX = UserX(e.X);
                _mouseY = UserY(e.Y);
                _mousePressed = true;
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MouseUp(object sender, MouseEventArgs e)
        {
            lock (MouseLock)
            {
                _mousePressed = false;
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MouseDragged(object sender, DragEventArgs e)
        {
            lock (MouseLock)
            {
                _mouseX = UserX(e.X);
                _mouseY = UserY(e.Y);
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MouseMoved(object sender, MouseEventArgs e)
        {
            lock (MouseLock)
            {
                _mouseX = UserX(e.X);
                _mouseY = UserY(e.Y);
            }
        }

        #endregion

        #region Keyboard interactions.

        /// <summary>
        /// Has the user typed a key?
        /// </summary>
        /// <returns>true if the user has typed a key, false otherwise</returns>
        public static bool HasNextKeyTyped()
        {
            lock (KeyLock)
            {
                return KeysTyped.Count != 0;
            }
        }

        /// <summary>
        /// What is the next key that was typed by the user? This method returns
        /// a Unicode character corresponding to the key typed (such as 'a' or 'A').
        /// It cannot identify action keys (such as F1
        /// and arrow keys) or modifier keys (such as control).
        /// </summary>
        /// <returns>the next Unicode key typed</returns>
        public static char NextKeyTyped()
        {
            lock (KeyLock)
            {
                char value = KeysTyped[KeysTyped.Count - 1];
                KeysTyped.RemoveAt(KeysTyped.Count - 1);
                return value;
            }
        }

        /// <summary>
        /// Is the keycode currently being pressed? This method takes as an argument
        /// the keycode (corresponding to a physical key). It can handle action keys
        /// (such as F1 and arrow keys) and modifier keys (such as shift and control).
        /// </summary>
        /// <param name="keycode">true if keycode is currently being pressed, false otherwise</param>
        /// <returns></returns>
        public static bool IsKeyPressed(int keycode)
        {
            lock (KeyLock)
            {
                return KeysDown.Contains(keycode);
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        private static void KeyPressed(object sender, KeyPressEventArgs e)
        {
            lock (KeyLock)
            {
                KeysTyped.Insert(0, e.KeyChar);
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        private static void KeyDown(object sender, KeyEventArgs e)
        {
            lock (KeyLock)
            {
                KeysDown.Add(Convert.ToChar(e.KeyValue));
            }
        }

        /// <summary>
        /// This method cannot be called directly.
        /// </summary>
        private static void KeyUp(object sender, KeyEventArgs e)
        {
            lock (KeyLock)
            {
                KeysDown.Remove(Convert.ToChar(e.KeyValue));
            }
        }

        #endregion

        /// <summary>
        /// Test client.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Square(.2f, .8f, .1f);
            FilledSquare(.8f, .8f, .2f);
            Circle(.8f, .2f, .2f);

            SetPenColor(BookRed);
            SetPenRadius(0.02f);
            Arc(.8f, .2f, .1f, 200, 90);

            // draw a blue diamond
            SetPenRadius();
            SetPenColor(BookBlue);
            float[] x = { .1f, .2f, .3f, .2f };
            float[] y = { .2f, .3f, .2f, .1f };
            FilledPolygon(x, y);

            // text
            SetPenColor(Black);
            Text(0.2f, 0.5f, "black text");
            SetPenColor(White);
            Text(0.8f, 0.8f, "white text");
        }
    }
}