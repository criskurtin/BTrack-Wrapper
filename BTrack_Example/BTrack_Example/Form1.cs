using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NAudio.Wave;
using NAudio.Dsp;
using BWrapper;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace BTrack_Example
{
    public partial class Form1 : Form
    {
        ////
        // Variables and Objects
        ////
        WaveInEvent waveIn = new WaveInEvent();                                 // NAudio WaveIn event - used for real-time input sampling 
        int samplerate = 44100;                                                 // samplerate to which input will be set
        int channels = 2;                                                       // number of input channels (mono/stereo)
        private Random rnd = new Random();                                      // random number generator - used to create variations in duration of 'takes'
        BTrackWrapper btw = new BTrackWrapper(512, 1024);                       // main object of interest - BTrack Wrapper object for interfacing the BTrack library
        bool nodevices = false;
        int beatsdetected = 0;                                                  // counter for detected beats
        BiQuadFilter lpf = BiQuadFilter.LowPassFilter(44100.0f, 130.0f, 30.0f); // experimental filter (trying to improve beat tracker's response by cutting higher frequencies)
        int[] beatarray = { 15, 23, 31 };                                       // part of randomizer
        int beatskip;

        public Form1()
        {
            InitializeComponent();
            ////
            // Initialize Audio
            ////
            int devcount = WaveIn.DeviceCount;
            if (devcount > 0)
            {
                for (int i = 0; i < devcount; i++)
                {
                    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                    AudioDevices.Items.Add("Device " + i + ": " + deviceInfo.ProductName + ", " + deviceInfo.Channels + " channels");
                }
                AudioDevices.SelectedIndex = 0;
                nodevices = false;
            }
            else
            {
                nodevices = true;
            }

            beatskip = rnd.Next(0, 2);                                          // part of randomizer

            buttonStop.Enabled = false;
            buttonStart.Enabled = true;
            buttonReset.Enabled = false;

            chart.Series.Add("wave");
            chart.Series["wave"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart.Series["wave"].ChartArea = "ChartArea1";
            chart.Series["wave"].Color = Color.Green;
            chart.Series["wave"].IsVisibleInLegend = false;
            chart.BackColor = SystemColors.ControlDarkDark;
            chart.ChartAreas["ChartArea1"].BackColor = Color.Black;
            chart.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisX.MinorGrid.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisY.MinorGrid.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisY.LabelStyle.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisX.MajorTickMark.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisY.MajorTickMark.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisX.MinorTickMark.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisY.MinorTickMark.Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisX.LineWidth = 0;
            chart.ChartAreas["ChartArea1"].AxisY.LineWidth = 0;
            chart.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            chart.ChartAreas["ChartArea1"].AxisY.IsMarginVisible = false;
            chart.Legends[0].Enabled = false;
            chart.ChartAreas["ChartArea1"].AxisY.Minimum = -1;
            chart.ChartAreas["ChartArea1"].AxisY.Maximum = 1;
            chart.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart.ChartAreas["ChartArea1"].AxisX.Maximum = 256;

        }

        // Processing Wave In data
        private void OnDataAvailable(object sender, WaveInEventArgs args)
        {
            double sum = 0;
            List<double> frame = new List<double>();
            // interpret as 16 bit audio... Fill the list with samples (LRLRLRLR...)
            for (int index = 0; index < 2048; index += 2)
            {
                short sample = (short)((args.Buffer[index + 1] << 8) | args.Buffer[index + 0]);
                // to floating point
                var sample32 = sample / 32768.0;
                sum += (sample32 * sample32);
                //sample32 = lpf.Transform((float)sample32);
                //sample32 = lpf.Transform(sample32);
                if (index % 2 == 0)
                {
                    frame.Add(sample32);
                }
                if (index == 0) { 
                    var point = sample32;
                    chart.Invoke(new Action(() => { chart.Series["wave"].Points.Add(point); }));
                }

            }

            if (chart.Series["wave"].Points.Count > 256) {
                chart.Invoke(new Action(() => { chart.Series["wave"].Points.RemoveAt(0); }));

            }

            // Convert list to array
            var framearray = frame.ToArray();

            // some manual garbage collection
            GCHandle handle = GCHandle.Alloc(framearray, GCHandleType.Pinned);
            IntPtr framePtr = handle.AddrOfPinnedObject();

            // call ProcessAudioFrame - uses IntPtr to access the sample array
            btw.processAudioFrameWrapper(framePtr);
            framearray = null;
            framePtr = (IntPtr)null;
            frame.Clear();

            // calculate the power of audio signal
            double rms = Math.Sqrt(sum / (2048));
            var decibel = 20 * Math.Log10(rms);
            labelDBs.Invoke(new Action(() => { labelDBs.Text = "Power: " + ((int)decibel).ToString() + " dB"; }));

            // What to do if beat is detected
            if (btw.beatDueInCurrentFrameWrapper())
            {
                chart.Invoke(new Action(() => { chart.Series["wave"].Points.Last().BorderWidth = 5; chart.Series["wave"].Points.Last().Color = Color.Red; }));
                labelTempo.Invoke(new Action(() => { labelTempo.Text = "Tempo: " + Math.Round(btw.getCurrentTempoEstimate(), 1).ToString(); }));
                labelBPM.Invoke(new Action(() => { labelBPM.BackColor = Color.Red; }));

                if ((beatsdetected == beatarray[beatskip] || beatsdetected >= 31) && decibel > -60.0) //&& beatsdetected % 2 == 1
                {
                    var prevcam = labelBPM.Text;
                    var nextcam = "";
                    do
                    {
                        nextcam = rnd.Next(1, 4).ToString();
                    } while (nextcam == prevcam);
                    labelBPM.Invoke(new Action(() => { labelBPM.Text = nextcam; }));
                    //labelDBs.Invoke(new Action(() => { labelDBs.Text = btw.getCurrentTempoEstimate().ToString(); }));
                    beatskip = rnd.Next(0, 2);
                    beatsdetected = 0;
                }
                else
                {
                    beatsdetected += 1;
                }
                labelBeatCount.Invoke(new Action(() => { labelBeatCount.Text = "Beats Detected: " + beatsdetected.ToString() + " of " + beatarray[beatskip].ToString(); }));
            }
            else
            {
                labelBPM.Invoke(new Action(() => { labelBPM.BackColor = Color.Green; }));
            }
            handle.Free();
        }

        private void startAudio()
        {
            // run audio recording
            waveIn.DeviceNumber = AudioDevices.SelectedIndex;
            waveIn.BufferMilliseconds = 24;
            waveIn.WaveFormat = new WaveFormat(samplerate, channels);
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
            Trace.WriteLine("Recording - Sample rate: " + waveIn.WaveFormat.SampleRate.ToString() + "; Bit depth: " + waveIn.WaveFormat.BitsPerSample.ToString() + "; Channels: " + waveIn.WaveFormat.Channels.ToString());
        }

        private void stopAudio()
        {
            // stop audio recording
            waveIn.RecordingStopped += WaveIn_RecordingDisconnected;
            waveIn.StopRecording();
        }

        private void WaveIn_RecordingDisconnected(object sender, StoppedEventArgs e)
        {
            waveIn.DataAvailable -= OnDataAvailable;
            waveIn.Dispose();
            Trace.WriteLine("Wave In device is now halted.");
            waveIn.RecordingStopped -= WaveIn_RecordingDisconnected;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            startAudio();
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            buttonReset.Enabled = true;
            AudioDevices.Enabled = false;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stopAudio();
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            buttonReset.Enabled = false;
            AudioDevices.Enabled = true;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            beatsdetected = 0;
        }
    }
}
