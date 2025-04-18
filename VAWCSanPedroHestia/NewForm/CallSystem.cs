using System;
using System.Drawing;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using NAudio.Wave;

namespace VAWCSanPedroHestia.NewForm
{
    public partial class CallSystem : Form
    {
        private GMapControl gMapControl1;
        private WaveformViewer waveformViewer;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private Timer playbackTimer;

        public CallSystem()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Initialize Map
            gMapControl1 = new GMapControl
            {
                Dock = DockStyle.Fill,
                MapProvider = OpenStreetMapProvider.Instance,
                Position = new PointLatLng(7.823382471929188, 123.44301037440286),
                MinZoom = 2,
                MaxZoom = 18,
                Zoom = 15,
                MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter,
                CanDragMap = true,
                DragButton = MouseButtons.Left
            };
            panel2.Controls.Add(gMapControl1);

            // Initialize Waveform Viewer
            waveformViewer = new WaveformViewer();
            panel7.Controls.Add(waveformViewer);

            // Add control buttons
            AddControlButton("Load Audio", 10, BtnLoad_Click);
            AddControlButton("Play", 100, BtnPlay_Click);
            AddControlButton("Stop", 190, BtnStop_Click);

            // Initialize playback timer
            playbackTimer = new Timer { Interval = 50 };
            playbackTimer.Tick += PlaybackTimer_Tick;
            playbackTimer.Start();
        }

        private void AddControlButton(string text, int x, EventHandler handler)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(80, 25),
                Location = new Point(x, 110)
            };
            btn.Click += handler;
            panel7.Controls.Add(btn);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Audio Files|*.mp3;*.wav;*.aiff";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    waveformViewer.LoadAudio(openFileDialog.FileName);

                    // Initialize playback
                    outputDevice?.Dispose();
                    audioFile?.Dispose();

                    audioFile = new AudioFileReader(openFileDialog.FileName);
                    outputDevice = new WaveOutEvent();
                    outputDevice.Init(audioFile);
                }
            }
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            outputDevice?.Play();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            outputDevice?.Stop();
            if (audioFile != null)
            {
                audioFile.CurrentTime = TimeSpan.Zero;
                waveformViewer.CurrentPosition = 0;
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (audioFile != null && audioFile.TotalTime.TotalSeconds > 0)
            {
                waveformViewer.CurrentPosition = (float)(audioFile.CurrentTime.TotalSeconds / audioFile.TotalTime.TotalSeconds);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            outputDevice?.Dispose();
            audioFile?.Dispose();
            playbackTimer?.Dispose();
        }
    }
}