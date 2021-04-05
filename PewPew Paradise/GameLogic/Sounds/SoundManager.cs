﻿using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace PewPew_Paradise.GameLogic.Sounds
{
    class SoundManager
    {
        public static PewPewSoundMixer mixer;
        static WaveOut masterMixerOut;
        static Dictionary<string, CachedSound> cachedSounds = new Dictionary<string, CachedSound>();
        static Queue<MusicReader> songsPlaying = new Queue<MusicReader>();
        private static string _songPlaying;
        public static Random random = new Random();
        public static void Init()
        {
            Thread thread = new Thread(InitThread);
            thread.Start();

            var executingAssembly = Assembly.GetExecutingAssembly();
            string folderName = string.Format("{0}.Resources.Folder", executingAssembly.GetName().Name);

            LoadSoundEffect("ButtonClick.mp3");


            GameManager.OnUpdate += Update;

        }

        public static void LoadSoundEffect(string soundName)
        {
            cachedSounds.Add(soundName,new CachedSound(soundName));
        }


        static void Update()
        {
            if (songsPlaying.Count > 0)
            {
                if (songsPlaying.Count > 1)
                {
                    WaveChannel32 channel = songsPlaying.First().reader;
                    channel.Volume = Math.Max(0, channel.Volume -= 0.001f * (float)GameManager.DeltaTime);
                    if (channel.Volume == 0)
                    {
                        songsPlaying.First().Dispose();
                        songsPlaying.Dequeue();
                    }
                    WaveChannel32 lastChannel = songsPlaying.Last().reader;
                    lastChannel.CurrentTime = TimeSpan.FromSeconds(0);
                }
                else
                {
                    WaveChannel32 channel = songsPlaying.First().reader;
                    channel.Volume = Math.Min(1, channel.Volume += 0.001f * (float)GameManager.DeltaTime);
                }
            }
        }

        public static void InitThread()
        {
            mixer = new PewPewSoundMixer();
            masterMixerOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            masterMixerOut.Init(mixer);
            masterMixerOut.Volume = 1.0f;
            Console.WriteLine("finito sound init");
            while (MainWindow.Instance != null && MainWindow.Instance.isWindowOpen)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("thread ended");
            masterMixerOut.Stop();
            Console.WriteLine("mixer stopped");
        }


        public static void PlaySongThread(object song)
        {
                string songName = (string)song;
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"PewPew_Paradise.Sounds.Music." + songName;
                Stream stream = assembly.GetManifestResourceStream(resourceName);
                var rdr = new Mp3FileReader(stream);
                var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr);
                var baStream = new BlockAlignReductionStream(wavStream);
                var loopStream = new LoopStream(baStream);
                var wave32stream = new WaveChannel32(loopStream, 0.0f, 0);
                var mreader = new MusicReader(wave32stream);
                songsPlaying.Enqueue(mreader);
                mixer.AddMusicSample(mreader);
                if (masterMixerOut.PlaybackState == PlaybackState.Stopped)
                {
                    masterMixerOut.Play();
                }

        }


        public static void PlaySong(string songName)
        {
           /* if (_songPlaying != songName) {
                Thread playSong = new Thread(new ParameterizedThreadStart(PlaySongThread));
                playSong.Start(songName);
                _songPlaying = songName;
            }*/
        }
        public static void PlaySoundEffect(string soundEffectName)
        {
            /*Console.WriteLine("Hover detected");
            songMixer.baseMixer.AddMixerInput(new CachedSoundSampleProvider(cachedSounds[soundEffectName]));
            if (masterMixerOut.PlaybackState == PlaybackState.Stopped)
            {
                masterMixerOut.Play();
            }*/
        }



    }


    public class PewPewSoundMixer : ISampleProvider
    {
        public WaveFormat WaveFormat { get; }
        private float[] _innerBuffer;
        private List<ISampleProvider> _musicSamples;
        private List<ISampleProvider> _effectSamples;

        public PewPewSoundMixer()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100,2);
            _innerBuffer = new float[short.MaxValue];
            _musicSamples = new List<ISampleProvider>();
            _effectSamples = new List<ISampleProvider>();
        }

        public void AddEffectSample(ISampleProvider effectSample)
        {
            _effectSamples.Add(effectSample);
        }
        public void AddMusicSample(ISampleProvider musicSample)
        {
            _musicSamples.Add(musicSample);
        }


        public int Read(float[] buffer, int offset, int count)
        {
            Console.WriteLine("Read From Mixer");
            if (_innerBuffer.Length < count)
            {
                _innerBuffer = new float[buffer.Length];
            }

            for (int i = _musicSamples.Count - 1; i >= 0;i--)
            {
                _musicSamples[i].Read(_innerBuffer, 0, count);
                /*if (_musicSamples[i].Read(_innerBuffer, 0, count) < 1)
                {
                    _musicSamples.RemoveAt(i);
                    continue;
                }*/

                /*for (int f = 0;f < count;f++)
                {
                    buffer[f] += _innerBuffer[f];
                }*/
            }
            for (int i = 0;i < count;i++)
            {
                buffer[i] = (float)SoundManager.random.NextDouble() * 0.01f;
            }
            

            return count;
        }
    }



    class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound cachedSound;
        private long position;

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }
        public unsafe int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int)samplesToCopy;
        }

        public WaveFormat WaveFormat { get { return cachedSound.WaveFormat; } }
    }

    class MusicReader : ISampleProvider
    {
        public readonly WaveChannel32 reader;
        private bool isDisposed;
        byte[] _innerBuffer;

        public MusicReader(WaveChannel32 reader)
        {
            this.reader = reader;
            this.WaveFormat = reader.WaveFormat;
            _innerBuffer = new byte[1];
        }
        public void Dispose()
        {
            reader.Dispose();
            isDisposed = true;
        }

        public unsafe int Read(float[] buffer, int offset, int count)
        {
            Console.WriteLine("Read From Music");
            if (isDisposed)
                return 0;

            if (_innerBuffer.Length < buffer.Length * 4)
            {
                _innerBuffer = new byte[buffer.Length * 4];
            }


            int read = reader.Read(_innerBuffer, offset, count * 4);
            int maxRead = read / 4;
            for (int i = 0;i < maxRead;i++)
            {
                int indx = i * 4;
                float dest;
                float* fp = &dest;
                fp[0] = _innerBuffer[indx];
                fp[1] = _innerBuffer[indx + 1];
                fp[2] = _innerBuffer[indx + 2];
                fp[3] = _innerBuffer[indx + 3];
                buffer[i] = dest;
            }
            if (read == 0)
            {
                reader.CurrentTime = TimeSpan.FromSeconds(0);
                read = 4;
            }
            return read / 4;
        }

     
        public WaveFormat WaveFormat { get; private set; }
    }


    public class VolumedMixer : ISampleProvider
    {
        public MixingSampleProvider baseMixer;
        public float Volume = 1.0f;
        public WaveFormat WaveFormat {
            get { return baseMixer.WaveFormat; }
        }

        public VolumedMixer()
        {
            baseMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        }

        public int Read(float[] buffer, int offset, int count)
        {

            int read = baseMixer.Read(buffer, offset, count);
            int offcount = offset + count;
            for (int i = offset; i < offcount; i++)
            {
                buffer[i] = buffer[i] * Volume;
            }
            return read;
        }
    }




    public class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        /// <summary>
        /// Creates a new Loop stream
        /// </summary>
        /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
        /// or else we will not loop to the start again.</param>
        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool EnableLooping { get; set; }

        /// <summary>
        /// Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        /// <summary>
        /// LoopStream simply returns
        /// </summary>
        public override long Length
        {
            get { return sourceStream.Length; }
        }

        /// <summary>
        /// LoopStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }






        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }



    class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string soundFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"PewPew_Paradise.Sounds.Effects." + soundFileName;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            using (var rdr = new Mp3FileReader(stream))
            using (var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr))
            using (var baStream = new BlockAlignReductionStream(wavStream))
            {
                // TODO: could add resampling in here if required
                WaveFormat = baStream.WaveFormat;
                var wholeFile = new List<byte>((int)(baStream.Length));
                var readBuffer = new byte[baStream.WaveFormat.SampleRate * baStream.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = baStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                byte[] fileArray = wholeFile.ToArray();
                AudioData = new float[fileArray.Length / 4];
                Buffer.BlockCopy(fileArray, 0, AudioData, 0, fileArray.Length);
            }
        }
    }


}