using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace PewPew_Paradise.GameLogic.Sounds
{
    /// <summary>
    /// Sound manager of the game (not instanced, static)
    /// </summary>
    class SoundManager
    {
        //main mixer
        public static PewPewSoundMixer mixer;
        //main mixer output
        static WaveOut masterMixerOut;
        //sounds loaded on game start
        static Dictionary<string, CachedSound> cachedSounds = new Dictionary<string, CachedSound>();
        //current songs that are playing
        static Queue<MusicReader> songsPlaying = new Queue<MusicReader>();
        //latest song that was queued for playing
        private static string _songPlaying;
        /// <summary>
        /// Initialize
        /// </summary>
        public static void Init()
        {
            Thread thread = new Thread(InitThread);
            thread.Start();
            var executingAssembly = Assembly.GetExecutingAssembly();
            string folderName = string.Format("{0}.Resources.Folder", executingAssembly.GetName().Name);
            GameManager.OnUpdate += Update;
            //LOAD SOUND EFFECTS HERE
            LoadSoundEffect("ButtonClick.mp3");
        }
        /// <summary>
        /// Load a sound effect from Sounds/Effects for playing later
        /// </summary>
        /// <param name="soundName"></param>
        public static void LoadSoundEffect(string soundName)
        {
            cachedSounds.Add(soundName, new CachedSound(soundName));
        }

        /// <summary>
        /// Update for smooth transition between songs
        /// </summary>
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
        /// <summary>
        /// Initialize mixer on a different thread
        /// </summary>
        private static void InitThread()
        {
            mixer = new PewPewSoundMixer();
            masterMixerOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            masterMixerOut.Init(mixer);
            masterMixerOut.Volume = 1.0f;
            while (MainWindow.Instance != null && MainWindow.Instance.isWindowOpen)
            {
                Thread.Sleep(100);
            }
            masterMixerOut.Stop();
        }

        /// <summary>
        /// Thread for playing a song. Needed because file reading is slow.
        /// </summary>
        /// <param name="song"></param>
        private static void PlaySongThread(object song)
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

        /// <summary>
        /// Start playing a song from Sounds/Music path
        /// </summary>
        /// <param name="songName"></param>
        public static void PlaySong(string songName)
        {
            if (_songPlaying != songName)
            {
                Thread playSong = new Thread(new ParameterizedThreadStart(PlaySongThread));
                playSong.Start(songName);
                _songPlaying = songName;
            }
        }
        /// <summary>
        /// Play a sound effect from Sounds/Effects path
        /// </summary>
        /// <param name="soundEffectName"></param>
        public static void PlaySoundEffect(string soundEffectName)
        {
            mixer.AddEffectSample(new CachedSoundSampleProvider(cachedSounds[soundEffectName]));
            if (masterMixerOut.PlaybackState == PlaybackState.Stopped)
            {
                masterMixerOut.Play();
            }
        }



    }

    /// <summary>
    /// Custom made sound mixer for the game, that supports different master volumes for music and sounds
    /// </summary>
    public class PewPewSoundMixer : ISampleProvider
    {
        //output format
        public WaveFormat WaveFormat { get; }
        //buffer for mixing sounds
        private float[] _innerBuffer;
        //current music samples
        private List<ISampleProvider> _musicSamples;
        //current effect samples
        private List<ISampleProvider> _effectSamples;
        /// <summary>
        /// get or set the volume of the music
        /// </summary>
        public float musicVolume;
        /// <summary>
        /// get or set the volume of the sound effects
        /// </summary>
        public float effectVolume;

        public PewPewSoundMixer()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            _innerBuffer = new float[short.MaxValue];
            _musicSamples = new List<ISampleProvider>();
            _effectSamples = new List<ISampleProvider>();
        }
        /// <summary>
        /// Add an effect sample for playing
        /// </summary>
        /// <param name="effectSample"></param>
        public void AddEffectSample(ISampleProvider effectSample)
        {
            _effectSamples.Add(effectSample);
        }
        /// <summary>
        /// Add a music sample for playing
        /// </summary>
        /// <param name="musicSample"></param>
        public void AddMusicSample(ISampleProvider musicSample)
        {
            _musicSamples.Add(musicSample);
        }
        /// <summary>
        /// used for reading the stream of the mixer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(float[] buffer, int offset, int count)
        {
            if (_innerBuffer.Length < count)
            {
                _innerBuffer = new float[buffer.Length];
            }
            for (int i = offset; i < count; i++)
            {
                buffer[i] = 0;
            }

            int effectCount = _effectSamples.Count;
            for (int i = _effectSamples.Count - 1; i >= 0; i--)
            {
                if (_effectSamples[i] == null)
                {
                    _effectSamples.RemoveAt(i);
                    continue;
                }
                int read = _effectSamples[i].Read(_innerBuffer, offset, count);
                if (read < 1)
                {
                    _effectSamples.RemoveAt(i);
                    continue;
                }
                for (int f = offset; f < read; f++)
                {
                    buffer[f] += _innerBuffer[f];
                }
            }

            for (int f = offset; f < count; f++)
            {
                buffer[f] /= (effectCount + 1) / effectVolume;
            }

            for (int i = _musicSamples.Count - 1; i >= 0; i--)
            {
                if (_musicSamples[i] == null)
                {
                    _musicSamples.RemoveAt(i);
                    continue;
                }
                int read = _musicSamples[i].Read(_innerBuffer, offset, count);
                if (read < 1)
                {
                    _musicSamples.RemoveAt(i);
                    continue;
                }

                for (int f = offset; f < read; f++)
                {
                    buffer[f] += _innerBuffer[f] * musicVolume;
                }
            }
            return count;
        }
    }


    /// <summary>
    /// Sound provider for cached sounds
    /// </summary>
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
    /// <summary>
    /// Music sample provider
    /// </summary>
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
            if (isDisposed)
                return 0;

            if (_innerBuffer.Length < buffer.Length * 4)
            {
                _innerBuffer = new byte[buffer.Length * 4];
            }
            int read = reader.Read(_innerBuffer, offset * 4, count * 4);
            int maxRead = read / 4;
            for (int i = 0; i < maxRead; i++)
            {
                int indx = i * 4;
                float dest;
                float* fp = &dest;
                byte* bp = (byte*)fp;
                bp[0] = _innerBuffer[indx];
                bp[1] = _innerBuffer[indx + 1];
                bp[2] = _innerBuffer[indx + 2];
                bp[3] = _innerBuffer[indx + 3];
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
    /// <summary>
    /// Looping stream for music
    /// </summary>
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
    /// <summary>
    /// Cached sound that we only load once and re-use
    /// </summary>
    class CachedSound
    {
        public float[] AudioData { get; set; }
        public WaveFormat WaveFormat { get; private set; }
        public unsafe CachedSound(string soundFileName)
        {
            string songName = (string)soundFileName;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"PewPew_Paradise.Sounds.Effects." + songName;
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            var rdr = new Mp3FileReader(stream);
            var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr);
            var baStream = new BlockAlignReductionStream(wavStream);
            var wave32stream = new WaveChannel32(baStream, 1.0f, 0);
            var mreader = new MusicReader(wave32stream);
            WaveFormat = mreader.WaveFormat;
            float[] buffer = new float[mreader.reader.Length];
            int read = mreader.Read(buffer, 0, (int)mreader.reader.Length);

            int newlen = buffer.Length;
            for (int i = buffer.Length; i >= 1; i--)
            {
                if (buffer[i - 1] != 0)
                {
                    newlen = i;
                    break;
                }
            }
            AudioData = new float[newlen];
            Array.Copy(buffer, AudioData, newlen);
            for (int j = 0; j < AudioData.Length; j++)
            {
                //COMMENT THIS LINE OUT IF WE MAKE PROPER SOUND EFFECT FILES (procedural sound generation code)
                AudioData[j] = (float)Math.Sin(j * 0.04) * (float)Math.Sin((float)j / (float)AudioData.Length * 6.28 * 100.5f) * 0.1f * (float)Math.Sin((float)j / (float)AudioData.Length * 6.28 * 1.5f);
                AudioData[j] *= Math.Min(1, Math.Min(j * 0.1f, (AudioData.Length - j) * 0.00003f));
            }
        }
    }
}
