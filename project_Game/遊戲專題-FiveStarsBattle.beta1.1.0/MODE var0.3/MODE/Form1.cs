using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MODE
{
    public partial class Form1 : Form
    {
        Panel Panel1;
        public int Rows = 15;//每行大小
        public int size = 560;//棋盤大小
        public int slant = 50;//調整
        int[,] board = new int[15, 15];

        bool gameover = false;
        bool turn = true;//true 表示黑贏, false表示白贏
        int mode = 1;//PvP or PvPC
        public static bool music = true;//是否music
        public static int rockInt = 1;//是否rock
        public static bool lv = true;//PC Lv
        public static bool rpg = false;//rpg
        public static int rpgCount = 0;//rpg關卡
        public Board Engine;
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(400, 0);
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(900, 800);
            Panel1 = new Panel();

            //System.Media.SoundPlayer player = new System.Media.SoundPlayer("1.wav");//放背景音樂
            // player.PlayLooping();
            Panel1.Location = new Point(0, 110);
            Panel1.Size = this.Size;
            Panel1.BorderStyle = BorderStyle.None;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.BackColor = Color.FromArgb(0, 0, 0);//SandyBrown,213,176,146
            Panel1.BackColor = Color.FromArgb(10, this.BackColor);//把panel變成透明的
            Panel1.BackgroundImage = Properties.Resources.海邊的城堡 ;//panel背景



            this.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.Panel1.MouseDown += new MouseEventHandler(this.panel1_MouseDown);
            this.Controls.Add(Panel1);
            this.Text = "Five Stars Battle!";
            //this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            MessageBox.Show("Welcome to Five Stars Battle\n\nPress PC Level to change dificulty\n\nPress PvP or PvPc to start\n\nPress Atmospere to change your feeling");
        }

        private void panel1_Paint(object sender, EventArgs e)//使用panel1.Refresh()來觸發
        {
            gameover = false;
            turn = true;
            board = new int[15, 15];
            Graphics pg = Panel1.CreateGraphics();
            Engine = new Board(pg, Rows, size, slant);
        }

        private void panel1_MouseDown(object sender, EventArgs e)//下棋
        {

            Point mouse = Cursor.Position;
            mouse = Panel1.PointToClient(mouse);
            if (mouse.X < slant - Board.gap / 2 || mouse.X > size + slant + Board.gap / 2 ||
                mouse.Y < slant - Board.gap / 2 || mouse.Y > size + slant + Board.gap / 2) { return; }//是否出界

            Game(location(mouse));

            if (mode == 1 && turn == false)
            {
                Game(AI1(board));
            }
            //MessageBox.Show(mouse.ToString()+"\r\n"+location(mouse).ToString());//顯示位置
        }

        private void Game(Point P)
        {
            if (gameover)
            {
                //MessageBox.Show("Press Restart to play again.");
                return;
            }
            if (board[P.X, P.Y] != 0)
            {
                return;
            }
            if (turn)
            {
                Engine.drawB(P);
                board[P.X, P.Y] = 1;
            }
            else
            {
                Engine.drawW(P);
                board[P.X, P.Y] = 2;
            }
            if (cheakwin(P))
            {
                if (turn)
                {
                    if (rockInt == 3)
                    {
                        MessageBox.Show("藍營了...");
                        Panel1.BackgroundImage = Properties.Resources.不分;

                    }
                    else if (rpg == true && rpgCount == 1)
                    {

                        MessageBox.Show("太棒了");
                        Panel1.Refresh();
                        Panel1.BackgroundImage = Properties.Resources.結婚1;
                        rpgCount++;
                    }
                    else if (rpg == true && rpgCount == 2)
                    {

                        MessageBox.Show("好好享受結婚生活吧^_^");
                        Panel1.Refresh();
                        Panel1.BackgroundImage = Properties.Resources.結婚1;

                    }
                    else if (rpg == true && rpgCount == 0)
                    {
                        MessageBox.Show("你贏惹!!");
                        Panel1.Refresh();
                        
                        MessageBox.Show("接下來\n試著贏得公主的心吧!!");
                        Panel1.BackgroundImage = Properties.Resources.公主1;
                        rpgCount++;
                    }

                    else
                    {
                        MessageBox.Show("Nice!  Red   Win!");

                    }


                }
                else
                {
                    if (rockInt == 3)
                    {
                        MessageBox.Show("綠營了...");
                        Panel1.BackgroundImage = Properties.Resources.不分;

                    }
                    else if (rpg == true && rpgCount == 1)
                    {
                        MessageBox.Show("你輸惹...");
                        Panel1.Refresh();
                        MessageBox.Show("生命會為自己找到出路...");
                        MessageBox.Show("如果沒有強者能夠保護我，就自己變強吧!");
                        Panel1.BackgroundImage = Properties.Resources.公主2;
                        rpgCount++;
                    }
                    else if (rpg == true && rpgCount == 2)
                    {

                        MessageBox.Show("追尋強大的公主吧...");
                        Panel1.Refresh();
                        Panel1.BackgroundImage = Properties.Resources.公主2;

                    }
                    else if (rpg == true && rpgCount == 0)
                    {
                        MessageBox.Show("你輸惹...");
                        Panel1.Refresh();

                        MessageBox.Show("還好老天爺再給你一次機會!\n在蘋果棋魔王逼迫公主嫁給他之前再挑戰一次!");
                        Panel1.BackgroundImage = Properties.Resources.魔王2;
                        rpgCount++;

                    }
                    else
                    {
                        MessageBox.Show("Wow!   Purple   Win!");

                    }

                }
                gameover = true;
            }
            turn = !turn;
        }

        AI Ai = new AI();
        private Point AI1(int[,] B)//格式[橫,直] 0=空 1=黑 2=白
        {
            int[,] old = B;
            Ai.board = B;//匯入當前棋盤
            Ai.computer();
            return new Point(Ai.loc.X, Ai.loc.Y);
        }

        private bool cheakwin(Point P)
        {
            int t;
            if (turn) t = 1;
            else t = 2;

            int count = 0;
            for (int k = 0; k < 4; k++)
            {
                count = 0;
                for (int i = -4; i < 5; i++)
                {
                    switch (k)
                    {
                        case 0:// / 上斜
                            int sx = P.X - i; int sy = P.Y + i;
                            if (sx < 0 || sx > 14 || sy < 0 || sy > 14) continue;
                            if (board[sx, sy] == t) count++;
                            else count = 0;
                            break;
                        case 1:// \ 下斜
                            int s1 = P.X + i;
                            int s2 = P.Y + i;
                            if (s1 < 0 || s1 > 14 || s2 < 0 || s2 > 14) continue;
                            if (board[s1, s2] == t) count++;
                            else count = 0;
                            break;
                        case 2:// - 水平
                            int x = P.X + i;
                            if (x < 0 || x > 14) continue;
                            if (board[x, P.Y] == t) count++;
                            else count = 0;
                            break;
                        case 3:// | 垂直
                            int y = P.Y + i;
                            if (y < 0 || y > 14) continue;
                            if (board[P.X, y] == t) count++;
                            else count = 0;
                            break;
                    }
                    if (count == 5) return true;
                }
            }
            return false;
        }

        private Point location(Point loc)
        {
            int x = 0;
            int y = 0;
            int xp = 0;
            int yp = 0;
            int gap = Board.gap;//gap為間距

            for (int i = 0; i < Rows; i++)
            {
                if ((i * gap - gap / 2) + slant - 1 < loc.X && loc.X < (i * gap + gap / 2) + slant)
                {
                    x = i;
                    xp = i * gap;
                }
                if ((i * gap - gap / 2) + slant - 1 < loc.Y && loc.Y < (i * gap + gap / 2) + slant)
                {
                    y = i;
                    yp = i * gap;
                }
            }
            return new Point(x, y);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // if (MessageBox.Show("Player VS AI", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{   

            if (rpg == true)
            {
                mode = 1;
            }
            else
            {
                button2.Text = "PvPC  Start";
                button1.Text = "Player1 VS Player2";
                button1.ForeColor = Color.DeepSkyBlue;
                button2.ForeColor = Color.Chartreuse;



                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }

                Panel1.Refresh();
                mode = 1;
            }
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rpg == true)
            {
                mode = 1;
            }
            else
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }
                button1.Text = "PvP  Start";
                button2.Text = "Player    VS      PC     ";
                button1.ForeColor = Color.Chartreuse;
                button2.ForeColor = Color.DeepSkyBlue;
                Panel1.Refresh();
                mode = 0;
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
            if (music == true)
            {
                player.Play();
            }
            Panel1.Refresh();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (rockInt == 1)
            {
                rockInt++;
                button5.Text = "Universe";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }
                //this.BackColor = Color.FromArgb(0, 70, 255);//SandyBrown,213,176,146
                //this.BackColor = Color.FromArgb(0, 0, 0, 0);//SandyBrown,213,176,146
                Panel1.BackgroundImage = Properties.Resources.宇宙;//panel背景
                                                                 // System.Media.SoundPlayer player = new System.Media.SoundPlayer("2.wav");//放背景音樂
                                                                 // player.PlayLooping();
                                                                 // this.label1

            }
            else if (rockInt == 0)
            {
                rockInt++;
                rpgCount = 0;
                button5.Text = "Saint-Michel";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }
                //this.BackColor = Color.FromArgb(213, 176, 146);
                Panel1.BackgroundImage = Properties.Resources.海邊的城堡;//panel背景
                                                                    // System.Media.SoundPlayer player = new System.Media.SoundPlayer("1.wav");//放背景音樂
                                                                    // player.PlayLooping();
            }
            else if (rockInt == 2)
            {
                rockInt++;
                rpgCount = 0;
                button5.Text = "Ko-P";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }
                //this.BackColor = Color.FromArgb(213, 176, 146);
                Panel1.BackgroundImage = Properties.Resources.tpi;//panel背景
                                                                  // System.Media.SoundPlayer player = new System.Media.SoundPlayer("1.wav");//放背景音樂
                                                                  // player.PlayLooping();
            }
            else if (rockInt == 3)
            {
                rockInt = 0;
                rpgCount = 0;
                button5.Text = "Rurikoin";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }
                //this.BackColor = Color.FromArgb(213, 176, 146);
                Panel1.BackgroundImage = Properties.Resources.琉璃光院1;//panel背景
                                                                    // System.Media.SoundPlayer player = new System.Media.SoundPlayer("1.wav");//放背景音樂
                                                                    // player.PlayLooping();
            }
        }



        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (music)
            {
                //System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                //if (music == true)
                //{
                //    player.Play();
                //}
                music = false;
                button6.Text = "Audio clips Off";
                button6.ForeColor = Color.DimGray;
            }
            else
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                player.Play();

                music = true;
                button6.Text = "Audio clips On";
                button6.ForeColor = Color.DeepSkyBlue;
            }

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
            if (music == true)
            {
                player.Play();
            }
            if (lv)
            {

                button3.Text = "PC   Level  Hard";
                lv = false;
            }
            else
            {
                button3.Text = "PC   Level  Easy";
                lv = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            rpgCount = 0;
            if (rpg == false)
            {
                rockInt = 10;//讓人不能換版面
                if (mode == 1)
                {
                    Panel1.Refresh();
                    mode = 1;
                }
                else
                {
                    Panel1.Refresh();
                    mode = 0;
                }

                rpg = true;
                button7.BackColor = Color.White;
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }
                Panel1.BackgroundImage = Properties.Resources.魔王1;
                MessageBox.Show("五子棋公主被蘋果棋魔王抓走了!!");
                MessageBox.Show("勇敢的人啊!\n你能運用聰明才智打敗蘋果棋魔王，\n救出可愛的公主嗎!?!?\nOnce in your lifetime, to be a hero or nobody.");
            }
            else
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer("wate.wav");
                if (music == true)
                {
                    player.Play();
                }
                Panel1.BackgroundImage = Properties.Resources.海邊的城堡;
                rpg = false;
                button7.BackColor = Color.Black;
                rockInt = 1;
            }

        }

        private void backgroundWorker11_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker12_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
