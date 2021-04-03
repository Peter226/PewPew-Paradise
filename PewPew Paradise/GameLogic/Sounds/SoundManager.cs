using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio;
using NAudio.Mixer;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace PewPew_Paradise.GameLogic.Sounds
{
    class SoundManager
    {
        public static MixingSampleProvider mixer;
        static WaveOut mixerOut;
        public static void Init()
        {
            Thread thread = new Thread(InitThread);
            thread.Start();
        }



        public static void InitThread()
        {

            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));



            mixerOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            mixerOut.Init(mixer);
            mixerOut.Volume = 0.05f;

            while (MainWindow.Instance != null && MainWindow.Instance.Visibility == System.Windows.Visibility.Visible)
            {
                Thread.Sleep(100);
            }
        }

        public static void PlaySong()
        {

            try
            {

                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"PewPew_Paradise.Sounds.Music.MainMenu_1.mp3";
                Stream stream = assembly.GetManifestResourceStream(resourceName);

                //var ms = File.OpenRead("D:/Unity Projects/Bloat2.mp3");
                var rdr = new Mp3FileReader(stream);
                var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr);
                var baStream = new BlockAlignReductionStream(wavStream);
                mixer.AddMixerInput(new AutoDisposeFileReader(baStream));
                if (mixerOut.PlaybackState == PlaybackState.Stopped)
                {
                    mixerOut.Play();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



        }


        public static void Test()
        {
            Thread playSong = new Thread(PlaySong);
            playSong.Start();


            //mixer.ReadFully = true;
            //var input = new AudioFileReader("D:/Unity Projects/Bloat2.mp3");
            //var input = new AudioFileReader("D:/GAMF/4. félév/Vizprog/PewPew Paradise/PewPew-Paradise/PewPew Paradise/Sounds/Music/MainMenu_1.mp3");
            
        }




    }


    class AutoDisposeFileReader : IWaveProvider
    {
        private readonly BlockAlignReductionStream reader;
        private bool isDisposed;
        public AutoDisposeFileReader(BlockAlignReductionStream reader)
        {
            this.reader = reader;
            this.WaveFormat = reader.WaveFormat;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (isDisposed)
                return 0;
            int read = reader.Read(buffer, offset, count);
            if (read == 0)
            {
                reader.Dispose();
                isDisposed = true;
            }
            return read;
        }

        public WaveFormat WaveFormat { get; private set; }
    }
}
