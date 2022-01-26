using System;
using System.Drawing;
using System.Windows.Forms;
using FArr = System.Func<System.Drawing.Bitmap, decimal, System.Drawing.Bitmap>;



namespace Life



{
    public partial class MAIN : Form
    {
        private FArr[] xint = new FArr[2];

        private  Bitmap frf1(Bitmap bmp , decimal dcm)
        {
            return new Bitmap(2,3);
        }

        private  Bitmap frf2(Bitmap bmp, decimal dcm)
        {
            return new Bitmap(3, 2);
        }


        private Graphics graphics;
        int resolution;
        bool gact = false;
        GameEngine gameEngine;

        public MAIN()
        {
            xint[0] = frf1;
            xint[1] = frf2;
            InitializeComponent();
        }

        private void StartGame()
        {

            b_start.Enabled = false;
            nudDensity.Enabled = false;
            nudResolution.Enabled = false;
            b_stop.Enabled = true;

            resolution = (int)nudResolution.Value;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            gameEngine = new GameEngine
            (
                cols: pictureBox1.Width / resolution,
                rows: pictureBox1.Height / resolution,
                density: (int)(nudDensity.Minimum + nudDensity.Maximum - nudDensity.Value)
            ); ;

            if (radioButton1.Checked) timer1.Start();
            else b_step.Enabled = true;
            gact = true;

            Text = $"Generation: {gameEngine.CGNumber}";
        }

        private void StopGame()
        {
            timer1.Stop();
            gact = false;
            b_start.Enabled = true;
            nudDensity.Enabled = true;
            nudResolution.Enabled = true;
            b_stop.Enabled = false;
        }

        private void DrawNextGen()
        {
            var field = gameEngine.GetCGen();
            SuspendLayout();
            graphics.Clear(Color.Black);
            for (int x = 0; x < field.GetLength(0); x++)
                for (int y = 0; y < field.GetLength(1); y++)
                    if (field[x, y]) graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);

            ResumeLayout(true);
            pictureBox1.Refresh();
            
            this.Text = $"Generation: {gameEngine.CGNumber}";
            gameEngine.NextGen();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!gameEngine.EndOfDays)
                DrawNextGen();

            else { StopGame(); MessageBox.Show("Sorry! Game over!"); }
                    
                 
        }

        private void b_start_Click(object sender, EventArgs e)
        {
            StartGame();
        }
        

        private void b_stop_Click(object sender, EventArgs e)
        {
            
            StopGame();
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!gact) return;
            var x = e.Location.X / resolution;
            var y = e.Location.Y / resolution;

            if (e.Button == MouseButtons.Left)
                gameEngine.AddCell(x, y);

            if (e.Button == MouseButtons.Right)
            {
                gameEngine.RemoveCell(x, y);
            }
            if (!timer1.Enabled) DrawNextGen();
        }
        

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && timer1.Enabled) 
            
                timer1.Stop();

            else if(gact) timer1.Start();

            b_step.Enabled = radioButton2.Checked && gact;
        }

       private void b_step_Click(object sender, EventArgs e)
        {
            DrawNextGen();
        }

        
    }
}
