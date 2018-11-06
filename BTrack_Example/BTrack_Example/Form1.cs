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
            buttonStop.Enabled = false;
            buttonStart.Enabled = true;
            buttonReset.Enabled = false;

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
                labelBPM.Invoke(new Action(() => { labelBPM.BackColor = Color.Red; }));
                int beatskip = rnd.Next(1, 3);

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

                    beatsdetected = 0;
                }
                else
                {
                    beatsdetected += 1;
                }
                labelBeatCount.Invoke(new Action(() => { labelBeatCount.Text = "Beats Detected: " + beatsdetected.ToString(); }));
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

        private void AudioDevices_SelectedDeviceChanged(object sender, EventArgs e)
        {
            if (!nodevices)
            {
                waveIn.RecordingStopped += WaveIn_RecordingReset;
                waveIn.StopRecording();
            }
        }

        private void WaveIn_RecordingReset(object sender, StoppedEventArgs e)
        {
            // reset recording
            waveIn.DataAvailable -= OnDataAvailable;
            waveIn.Dispose();
            Trace.WriteLine("Wave In device is now paused.");
            waveIn.DeviceNumber = AudioDevices.SelectedIndex;
            waveIn.BufferMilliseconds = 24;
            waveIn.WaveFormat = new WaveFormat(samplerate, channels);
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();
            Trace.WriteLine("Recording - Sample rate: " + waveIn.WaveFormat.SampleRate.ToString() + "; Bit depth: " + waveIn.WaveFormat.BitsPerSample.ToString() + "; Channels: " + waveIn.WaveFormat.Channels.ToString());
            waveIn.RecordingStopped -= WaveIn_RecordingReset;
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
            buttonStart.Enabled = !buttonStart.Enabled;
            buttonStop.Enabled = !buttonStop.Enabled;
            buttonReset.Enabled = !buttonReset.Enabled;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stopAudio();
            buttonStart.Enabled = !buttonStart.Enabled;
            buttonStop.Enabled = !buttonStop.Enabled;
            buttonReset.Enabled = !buttonReset.Enabled;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            beatsdetected = 0;
        }
    }
}
