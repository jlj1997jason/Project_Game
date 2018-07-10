using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
namespace MODE
{
    public class Board
    {


        //EX.叫用方式 GFX 名稱 = New GFX(Graphics 名稱)
       // Form1 f = new Form1();
        public static Graphics gObject;
        public static int gap = 0;//間距
        public static int r;//偏移
        public static int count = 0;
        
       

        //建構式:若無設立的話,則預設無引數建構式
        public Board(Graphics g,int Rows,int Size,int slant)//物件,行,最大,偏移
        {
            gObject = g;
            r = slant;
            drawBoard(Rows,Size);//預設值:15,560
        }
        public void drawB(Point P)  //draw Black
        {
            if (Form1.rockInt == 3)
            {
                gObject.FillEllipse(new SolidBrush(Color.Blue), drawPoint(P.X, P.Y, 32, gap));
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("格式工廠下棋.wav");
                if (Form1.music == true)
                {
                    player.Play();
                }

            }
            else
            {
                gObject.FillEllipse(new SolidBrush(Color.Red), drawPoint(P.X, P.Y, 32, gap));
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("格式工廠下棋.wav");
                if (Form1.music == true)
                {
                    player.Play();
                }

            }
            
           
            // count++;
            //if (count > 5) { gObject.FillEllipse(new SolidBrush(Color.Pink), drawPoint(P.X, P.Y, 30, gap)); }
            //if (count > 10) { gObject.FillEllipse(new SolidBrush(Color.PowderBlue), drawPoint(P.X, P.Y, 30, gap)); }

        }

        public void drawW(Point P)  //darw White
        {           
            if (Form1.rockInt == 3)
            {
                gObject.FillEllipse(new SolidBrush(Color.Green), drawPoint(P.X, P.Y, 32, gap));
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("格式工廠下棋.wav");
                if (Form1.music == true)
                {
                    player.Play();
                }

            }
            else
            {
                gObject.FillEllipse(new SolidBrush(Color.DarkSlateBlue), drawPoint(P.X, P.Y, 32, gap));
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("格式工廠下棋.wav");
                if (Form1.music == true)
                {
                    player.Play();
                }

            }
            //gObject.FillEllipse(new SolidBrush(Color.White), drawPoint(P.X, P.Y, 30, gap));
            //count++;
            //if (count > 5) { gObject.FillEllipse(new SolidBrush(Color.LightGreen), drawPoint(P.X, P.Y, 30, gap)); }
            //if (count > 10) { gObject.FillEllipse(new SolidBrush(Color.SeaGreen), drawPoint(P.X, P.Y, 30, gap)); }
        }

        public void drawBoard(int row,int max)//row:行,max:終點位置,r:偏移位置
        {
            row--;            
            gap = max / row;//間隔
            max += r;
            //標示點
            SolidBrush B = new SolidBrush(Color.Brown);
            Rectangle[] rect = new Rectangle[] { drawPoint(3,3,8,gap) , drawPoint(3,11,8,gap), drawPoint(7,7,8,gap) , drawPoint(11,3,8,gap), drawPoint(11,11,8,gap)};
            gObject.FillRectangles(B, rect);

            Pen P_line = new Pen(Color.Brown, 2);
            for (int i = 0; i < row; i++)
            {
                int sp = gap * i + r;//間距大小
                gObject.DrawLine(P_line, new Point(sp, r), new Point(sp, max));
                gObject.DrawLine(P_line, new Point(r, sp), new Point(max, sp));
            }
            gObject.DrawLine(P_line, new Point(max, r), new Point(max, max));
            gObject.DrawLine(P_line, new Point(r, max), new Point(max, max));
        }
        

        public Rectangle drawPoint(int x,int y,int s_max,int gap_)//[直角]坐標,範圍,間隔
        {
            int s_min = s_max / 2;//範圍
            Rectangle rec = new Rectangle(gap_ * x - s_min + r, gap_ * y - s_min + r, s_max, s_max);
            return rec;
        }


    }
}
