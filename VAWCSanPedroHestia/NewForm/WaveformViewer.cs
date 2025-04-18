using System;
using System.Drawing;
using NAudio.Wave;
using System.Windows.Forms;

namespace VAWCSanPedroHestia.NewForm
{
    public class WaveformViewer : Control
    {
        private float[] audioData;
        private Pen foregroundPen = new Pen(Color.Blue, 1);
        private float currentPosition;

        public WaveformViewer()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Size = new Size(465, 138);
        }

        public void LoadAudio(string fileName)
        {
            try
            {
                using (var audioFileReader = new AudioFileReader(fileName))
                {
                    audioData = new float[audioFileReader.Length];
                    audioFileReader.Read(audioData, 0, (int)audioFileReader.Length);
                }
                this.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading audio: {ex.Message}");
            }
        }

        public float CurrentPosition
        {
            get => currentPosition;
            set
            {
                currentPosition = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (audioData == null || audioData.Length == 0)
                return;

            var g = e.Graphics;
            int width = this.Width;
            int height = this.Height;
            int halfHeight = height / 2;

            g.DrawLine(Pens.LightGray, 0, halfHeight, width, halfHeight);

            int samplesPerPixel = Math.Max(1, audioData.Length / width);

            for (int x = 0; x < width; x++)
            {
                int startSample = x * samplesPerPixel;
                if (startSample >= audioData.Length) break;

                float max = 0, min = 0;
                for (int n = 0; n < samplesPerPixel && (startSample + n) < audioData.Length; n++)
                {
                    float sample = audioData[startSample + n];
                    max = Math.Max(max, sample);
                    min = Math.Min(min, sample);
                }

                int pixelMax = halfHeight - (int)(max * halfHeight);
                int pixelMin = halfHeight - (int)(min * halfHeight);
                g.DrawLine(foregroundPen, x, pixelMax, x, pixelMin);
            }

            // Draw position indicator
            if (currentPosition > 0)
            {
                int posX = (int)(currentPosition * width);
                g.DrawLine(new Pen(Color.Red, 2), posX, 0, posX, height);
            }
        }
    }
}