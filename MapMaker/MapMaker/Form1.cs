using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapMaker
{
    public partial class Form1 : Form
    {
        int mapSize = 30;
        int numberOfRooms = 5;

        char[,] map;        
        Vector2[] points; //class declared at bottom of this file! 


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            map = new char[mapSize, mapSize];
            points = new Vector2[numberOfRooms];

            initMap();
            generateRoom();
            displayMap();
        }
        void initMap()
        {   
            mapSize = trackBar1.Value;
            numberOfRooms = trackBar2.Value;

            map = new char[mapSize, mapSize];
            points = new Vector2[numberOfRooms];


            for (int row = 0; row < mapSize; row++)
            {
                for (int col = 0; col < mapSize; col++)
                {
                    map[row, col] = '~';
                }
            }
            displayMap();
        }
        void displayMap()
        {
            textBox1.Clear();
            string buffer = "";
            for (int row = 0; row < mapSize; row++)
            {
                for (int col = 0; col < mapSize; col++)
                {
                    buffer+=(map[row, col].ToString());

                }
                buffer+=("\r\n");
            }
            textBox1.Text = buffer;
            Refresh();
        }

        void generateRoom()
        {
            Random rnd = new Random();
            int roomSize = rnd.Next(5, 8);
            int roomRow = rnd.Next(0, mapSize-roomSize-1);
            int roomCol = rnd.Next(0, mapSize - roomSize - 1);

            for (int row = roomRow; row < roomRow + roomSize; row++)
            {
                for (int col = roomCol; col < roomCol + roomSize; col++)
                {
                    if (row==roomRow||row==roomRow+roomSize-1||col==roomCol||col==roomCol+roomSize-1) 
                    {
                        map[row, col] = 'W';
                    }
                    else
                    {
                        map[row, col] = ' ';
                    }
                }
            }
        }

        void generateRoom2()
        {
            Random rnd = new Random();
            
            for (int i = 0; i < numberOfRooms; i++)
            {
                int roomSize = rnd.Next(3, 7);
                int roomRow = rnd.Next(2, mapSize - roomSize-1);
                int roomCol = rnd.Next(2, mapSize - roomSize-1);

                points[i] = new Vector2();
                points[i].y = roomCol + (roomSize / 2);
                points[i].x = roomRow + (roomSize / 2);

                for (int row = roomRow; row < roomRow + roomSize; row++)
                {
                    for (int col = roomCol; col < roomCol + roomSize; col++)
                    {                       
                       map[row,col] = ' ';                       
                    }
                }

                map[points[i].x, points[i].y] = 'X';
               
            }


            //add corridors

            //DrawLine(1,1,trackBar1.Value,trackBar2.Value);


            /*
            for (int row = 1; row < 20 - 1; row++)
            {
                for (int col = 1; col < 20 - 1; col++)
                {
                    if (map[row, col] == 'W')
                    {
                        int door = rnd.Next(1, 10);
                        if(door==1)
                        {
                            map[row, col] = 'D';
                        }
                    }
                }
            }
            */
            displayMap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initMap();
            displayMap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            initMap();
            generateRoom2();
            displayMap();
            
        }

        void bresenham(int x1, int y1, int x2, int y2)
        {
            int m_new = 2 * (y2 - y1);
            int slope_error_new = m_new - (x2 - x1);
            for (int x = x1, y = y1; x <= x2; x++)
            {
                textBox2.AppendText(x + ":" + y + " \r\n");
                map[y, x] = 'C';
                // Add slope to increment angle formed
                slope_error_new += m_new;

                // Slope error reached limit, time to
                // increment y and update slope error.
                if (slope_error_new >= 0)
                {
                    y++;
                    slope_error_new -= 2 * (x2 - x1);
                }
            }
        }

        void plotLine(int x0, int y0, int x1, int y1)
        {
            int dx, dy, p, x, y;

            dx = x1 - x0;
            dy = y1 - y0;

            x = x0;
            y = y0;

            p = 2 * dy - dx;

            while (x < x1)
            {

                textBox2.AppendText(x + ":" + y + " \r\n");

                if (p >= 0)
                {
                    map[y, x]='P';
                    y = y + 1;
                    p = p + 2 * dy - 2 * dx;
                }
                else
                {
                    map[y, x] = 'P';
                    p = p + 2 * dy;
                }
                x = x + 1;
            }
        }

        private void DrawLine(int x0, int y0, int x1, int y1)
        {
            int xInitial = x0, yInitial = y0, xFinal = x1- 0, yFinal = y1- 0;
            int dx = xFinal - xInitial, dy = yFinal - yInitial, steps, k, xf, yf;
            float xIncrement, yIncrement, x = xInitial, y = yInitial;

            if (Math.Abs(dx) > Math.Abs(dy))
                steps = Math.Abs(dx);
            else
                steps = Math.Abs(dy);

            xIncrement = dx / (float)steps;
            yIncrement = dy / (float)steps;

            for (k = 0; k < steps; k++)
            {
                x += xIncrement;
                xf = (int)x;
                y += yIncrement;
                yf = (int)y;

                map[xf, yf]='P';
            }
        }

        void manhattenPaths(int x0, int y0, int x1, int y1)
        {
            bool atGoal = false;
            while (atGoal == false)
            {
                if (x0 < x1)
                {
                    x0++;
                    map[x0, y0] = 'P';
                }
                if (x0 > x1)
                {
                    x0--;
                    map[x0, y0] = 'P';
                }

                if (y0 < y1)
                {
                    y0++;
                    map[x0, y0] = 'P';
                }
                if (y0 > y1)
                {
                    y0--;
                    map[x0, y0] = 'P';
                }

                if (x0 == x1 && y0 == y1) {atGoal = true; }
            }
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black, 1);

            SolidBrush greyBrush = new SolidBrush(Color.Gray);
            SolidBrush brownBrush = new SolidBrush(Color.Brown);
            SolidBrush greenBrush = new SolidBrush(Color.GreenYellow);
            SolidBrush reddyBrush = new SolidBrush(Color.SaddleBrown);
            SolidBrush blueBrush = new SolidBrush(Color.DarkSlateGray );


            for (int row = 0; row < mapSize; row++)
            {
                for (int col = 0; col < mapSize; col++)
                {
                    if (map[row, col] == '~') { e.Graphics.FillRectangle(greenBrush, new Rectangle(col * 10, row * 10, 10,  10)); }
                    if (map[row, col] == ' ') { e.Graphics.FillRectangle(greyBrush, new Rectangle(col * 10, row * 10, 10, 10)); }
                    if (map[row, col] == 'W') { e.Graphics.FillRectangle(brownBrush, new Rectangle(col * 10, row * 10, 10, 10)); }
                    if (map[row, col] == 'D') { e.Graphics.FillRectangle(reddyBrush, new Rectangle(col * 10, row * 10, 10, 10)); }
                    if (map[row, col] == 'P') { e.Graphics.FillRectangle(blueBrush, new Rectangle(col * 10, row * 10, 10, 10)); }

                    e.Graphics.DrawRectangle(blackPen, new Rectangle(col*10, row*10, 10, 10));
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            initMap();
            generateRoom2();
            displayMap();
            Refresh();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numberOfRooms - 1; i++)
            {
                DrawLine(points[i].x, points[i].y, points[i + 1].x, points[i + 1].y);
                //manhattenPaths(points[i].x, points[i].y, points[i + 1].x, points[i + 1].y);
            }
            displayMap();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //add walls by checking neighbours
            for (int row = 1; row < mapSize - 1; row++)
            {
                for (int col = 1; col < mapSize - 1; col++)
                {
                    if (map[row, col] == '~')
                    {
                        if (map[row - 1, col] == ' ') { map[row, col] = 'W'; }
                        if (map[row + 1, col] == ' ') { map[row, col] = 'W'; }
                        if (map[row, col - 1] == ' ') { map[row, col] = 'W'; }
                        if (map[row, col + 1] == ' ') { map[row, col] = 'W'; }
                    }
                    if (map[row, col] == 'P')
                    {
                        if (map[row - 1, col] == '~') { map[row-1, col] = 'W'; }
                        if (map[row + 1, col] == '~') { map[row+1, col] = 'W'; }
                        if (map[row, col - 1] == '~') { map[row, col-1l] = 'W'; }
                        if (map[row, col + 1] == '~') { map[row, col+1] = 'W'; }
                    }
                }
            }
            displayMap();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numberOfRooms - 1; i++)
            {
                //DrawLine(points[i].x, points[i].y, points[i + 1].x, points[i + 1].y);
                manhattenPaths(points[i].x, points[i].y, points[i + 1].x, points[i + 1].y);
            }
            displayMap();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black, 1);

            SolidBrush greyBrush = new SolidBrush(Color.Gray);
            SolidBrush lightBrush = new SolidBrush(Color.LightGray);
            SolidBrush slateBrush = new SolidBrush(Color.DarkSlateBlue);


            for (int row = 0; row < mapSize; row++)
            {
                for (int col = 0; col < mapSize; col++)
                {
                    char t = map[row, col];
                    int tileSize = 10;

                    switch(t)
                    {
                        case '~':
                            e.Graphics.FillRectangle(greyBrush, new Rectangle(col * tileSize, row * tileSize, tileSize, tileSize));
                            break;

                        case ' ':
                        case 'P':
                            e.Graphics.FillRectangle(lightBrush, new Rectangle(col * tileSize, row * tileSize, tileSize, tileSize));
                            break;

                        case 'W':
                            e.Graphics.FillRectangle(slateBrush, new Rectangle(col * tileSize, row * tileSize, tileSize, tileSize));
                            break;

                        default:
                            break;
                    }

                    e.Graphics.DrawRectangle(blackPen, new Rectangle(col * 10, row * 10, 10, 10));
                }
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = "MapSize: " + trackBar1.Value.ToString();
            mapSize = trackBar1.Value;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = "Number of Rooms: " + trackBar2.Value.ToString();
            numberOfRooms = trackBar2.Value;
        }
    }


    class Vector2
    {
        public int x;
        public int y;

    }
}
