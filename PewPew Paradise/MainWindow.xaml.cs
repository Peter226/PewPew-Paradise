using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
using System.ComponentModel;
using PewPew_Paradise.Editor;
using PewPew_Paradise.GameLogic.SpriteComponents;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using System.IO;
using PewPew_Paradise.Highscore;
using PewPew_Paradise.GameLogic.Sounds;
namespace PewPew_Paradise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow instance;
        
        //used for resize calculations
        double windowDifferenceX;
        double windowDifferenceY;
        double previousWidth;
        double previousHeight;

        //animation timer
        float Timer = 0;
        //highscore data
        private List<Hscore> _highscoreData = new List<Hscore>();
        public MapLoad mapLoader;
        public CharacterSelect characterSelector;
        public Enemy enemyManager;
        //background of the playing field
        public static SolidColorBrush playingFieldBrush;
        public AccessData scoreManager = new AccessData();
        //number of players
        public int player_number;
        //name of the first player
        public string player1_name;
        //name of the second player
        public string player2_name;
        //score of the first player
        public int score1 = 0;
        //score of the second player
        public int score2= 0;
        //timer for map clearing
        public double enemyHitTimer;
        //JSON saving system
        public GameOptions gameOptions = new GameOptions();
        public static JsonSerializer optionsSerializer = new JsonSerializer();
        //JSON saving system end
        //is the window open
        public bool isWindowOpen;

        /// <summary>
        /// Get instance
        /// </summary>
        public static MainWindow Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public MainWindow()
        {
            SoundManager.Init();
            isWindowOpen = true;
            
            scoreManager.InitDB();
            instance = this;
            
            InitializeComponent();
            
            Thickness thickness = new Thickness();
            GameWindow.Margin = thickness;
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;

            GameManager.Init(60);
            GameManager.OnUpdate += Update;
            GameManager.Begin();
            //CollisionEditor.Init(collisionEditor); //COLLISION EDITOR
            SpriteManager.canvases[1] = SpriteCanvas;
            SpriteManager.canvases[2] = FrontSpriteCanvas;
            SpriteManager.mainCanvas = SpriteCanvas;
            AnimationCollection.LoadAll();

            mapLoader = new MapLoad();
            characterSelector = new CharacterSelect();
            LoadGameOptions();
            enemyManager = new Enemy();
            PlayingField.Background = new SolidColorBrush();
            playingFieldBrush = (SolidColorBrush)PlayingField.Background;

            Dummy.Opacity = 0.0;
            DoubleAnimation enslaveWPF = new DoubleAnimation(20.0,30.0,TimeSpan.FromMilliseconds(1));
            enslaveWPF.RepeatBehavior = RepeatBehavior.Forever;
            enslaveWPF.AutoReverse = true;
            Dummy.BeginAnimation(Image.WidthProperty,enslaveWPF);

            FruitSprite.LoadImages();
            Refresh();
            this.Closing += WindowClosing;
            SoundManager.PlaySong("MainMenu.mp3");
        }

        /// <summary>
        /// Before you click the mini x in the corner of the app, this saves data into database 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Confirm.Inst != null && Confirm.Inst.IsForeground() == false)
                Confirm.Inst.Close();
            if (EndGame.Visibility == Visibility.Visible)
            {
                scoreManager.AddScore(new Hscore() { uname = player1_name, score = (int)lb_player1score.Content, floorcount = (int)lb_floor_player1.Content, characterid = characterSelector.chars_number1 });
                if (player_number != 1)
                    scoreManager.AddScore(new Hscore() { uname = player2_name, score = (int)lb_player2score.Content, floorcount = (int)lb_floor_player2.Content, characterid = characterSelector.chars_number2 });
            }
           
        }

        /// <summary>
        /// Refresh the content of the labes in the leaderboard
        /// </summary>
        public void Refresh()
        {
            lb_data_scoresname.Content = "Name:";
            lb_data_scores.Content = "Score:";
            lb_data_scoresfloor.Content = "Floor:";
        }

        /// <summary>
        /// Animation loop
        /// </summary>
        public void Update()
        {
            if (Confirm.Inst != null && Confirm.Inst.IsForeground() == false)
            {
                Confirm.Inst.Close();
            }
            Timer -= (float)(GameManager.DeltaTime * 0.00075);
            // Moving arrows by sin timer
            MatrixTransform mt_left= new MatrixTransform();
            Matrix matrix_left = mt_left.Matrix;
            matrix_left.Translate(10 * Math.Sin(Timer * 10), 0);
            mt_left.Matrix = matrix_left;
            img_left_arrow.RenderTransform = mt_left;

            MatrixTransform mt_right = new MatrixTransform();
            Matrix matrix_right = mt_right.Matrix;
            matrix_right.Translate(-10 * Math.Sin(Timer * 10), 0);
            mt_right.Matrix = matrix_right;
            img_right_arrow.RenderTransform = mt_right;

        }

        /// <summary>
        /// Detect key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (PlayingField.Visibility == Visibility.Visible)
                {
                    if (InGameOptions.Visibility == Visibility.Collapsed)
                    {
                        InGameOptions.Visibility = Visibility.Visible;
                        GameWindow.Children.Remove(InGameOptions);
                        GameWindow.Children.Add(InGameOptions);
                        GameManager.Stop();
                    }
                    else
                    {
                        InGameOptions.Visibility = Visibility.Collapsed;
                        GameManager.Begin();
                    }
                }
                else
                {
                    if (Confirm.Inst != null && Confirm.Inst.IsForeground() == false)
                        Confirm.Inst.Close();
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }

        /// <summary>
        /// Resize GameWindow when MainWindow changes it's size
        /// </summary>
        /// <param name="sizeInfo"></param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {

            if (this.SizeToContent != SizeToContent.Manual)
            {
                windowDifferenceX = this.ActualWidth - this.Width;
                windowDifferenceY = this.ActualHeight - this.Height;
            }

            double minSize = Math.Min(RealWidth, RealHeight);

            double spaceX = RealWidth - minSize;
            double spaceY = RealHeight - minSize;
            GameWindow.Width = minSize;
            GameWindow.Height = minSize;
            Thickness thickness = new Thickness(spaceX * 0.5, spaceY * 0.5, 0, 0);
            GameWindow.Margin = thickness;
        }

        public double RealWidth
        {
            get { return this.ActualWidth - windowDifferenceX; }
        }
        public double RealHeight
        {
            get { return this.ActualHeight - windowDifferenceY; }
        }

        /// <summary>
        /// Resize every UIElement in window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            double ratioW = GameWindow.Width / previousWidth;
            double ratioH = GameWindow.Height / previousHeight;


            foreach (UIElement uIElement in GameWindow.Children)
            {
                ResizeObjectsCascade(uIElement, ratioW, ratioH);
            }
            previousHeight = GameWindow.Height;
            previousWidth = GameWindow.Width;
        }

        /// <summary>
        /// Resize every child of every element
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void ResizeObjectsCascade(UIElement parent, double width, double height)
        {
            if (typeof(FrameworkElement).IsAssignableFrom(parent.GetType()))
            {
                FrameworkElement frameworkElement = (FrameworkElement)parent;
                frameworkElement.Width *= width;
                frameworkElement.Height *= height;
                Thickness margin = frameworkElement.Margin;
                margin.Left *= width;
                margin.Right *= width;
                margin.Top *= height;
                margin.Bottom *= height;

                if (typeof(Control).IsAssignableFrom(parent.GetType()))
                {
                    Control control = (Control)parent;
                    control.FontSize *= width;
                }
                frameworkElement.Margin = margin;


                int childcount = VisualTreeHelper.GetChildrenCount(parent);
                for (int c = 0;c < childcount;c++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent,c);
                    if (typeof(UIElement).IsAssignableFrom(child.GetType()))
                    {
                        ResizeObjectsCascade((UIElement)child, width, height);
                    }
                }
            }
        }
        
        /// <summary>
        /// Save options
        /// </summary>
        void SaveGameOptions()
        {
            gameOptions.musicVolume = sl_music.Value;
            gameOptions.effectVolume = sl_effect.Value;

            gameOptions.charName1 = tb_player1.Text;
            gameOptions.charName2 = tb_player2.Text;
            gameOptions.charID1 = characterSelector.chars_number1;
            gameOptions.charID2 = characterSelector.chars_number2;


            string workingDirectory = Environment.SpecialFolder.ApplicationData.ToString();
            string projectDirectory = Directory.GetParent(workingDirectory).FullName;
            string path = System.IO.Path.Combine(projectDirectory, "GameOptions.json");
            using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                StreamWriter sw = new StreamWriter(fs);
                JsonWriter jw = new JsonTextWriter(sw);
                optionsSerializer.Serialize(jw, gameOptions);
                jw.Close();
                sw.Close();
            }
        }

        /// <summary>
        /// Load options
        /// </summary>
        void LoadGameOptions()
        {
            string workingDirectory = Environment.SpecialFolder.ApplicationData.ToString();
            string projectDirectory = Directory.GetParent(workingDirectory).FullName;
            string path = System.IO.Path.Combine(projectDirectory, "GameOptions.json");
            if (File.Exists(path))
            {
                using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                using (StreamReader reader = new StreamReader(fs))
                {
                    using (JsonReader jreader = new JsonTextReader(reader))
                    {
                        GameOptions go = (GameOptions)optionsSerializer.Deserialize(jreader, typeof(GameOptions));
                        if (go != null)
                        {
                            gameOptions = go;
                        }
                    }
                }
            }
            else
            {
                gameOptions.effectVolume = 50;
                gameOptions.musicVolume = 50;
                gameOptions.charName1 = "Player 1";
                gameOptions.charName2 = "Player 2";
            }
            player1_name = gameOptions.charName1;
            player2_name = gameOptions.charName2;
            tb_player1.Text = gameOptions.charName1;
            tb_player2.Text = gameOptions.charName2;
            characterSelector.chars_number1 = gameOptions.charID1;
            characterSelector.chars_number2 = gameOptions.charID2;
            sl_music.Value = gameOptions.musicVolume;
            sl_effect.Value = gameOptions.effectVolume;
        }

        /// <summary>
        /// Replacing selection arrows with window size changing
        /// </summary>
        /// <param name="button"></param>
        private void  Arrow_placing(Thickness button)
        {   
            Thickness current_button = button;
            current_button.Left -= img_left_arrow.Width;
            Thickness left_arrow = current_button;
            double Left_arrow_place = bt_options.Width + img_right_arrow.Width;
            current_button.Left += Left_arrow_place;
            img_left_arrow.Margin = left_arrow;
            Thickness right_arrow = current_button;
            img_right_arrow.Margin = right_arrow;
            img_left_arrow.Visibility = Visibility.Visible;
            img_right_arrow.Visibility = Visibility.Visible;
            
            

        }

        /// <summary>
        /// Main Menu button events
        /// </summary>
        private void Arrow_clear()
        {
            img_left_arrow.Visibility = Visibility.Collapsed;
            img_right_arrow.Visibility = Visibility.Collapsed;
      
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            GameManager.Stop();
        }

        private void bt_exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void bt_help_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Help.Visibility = Visibility.Visible;

        }

        private void bt_back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
            SoundManager.PlaySong("MainMenu.mp3");
            Help.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Collapsed;
            Leaderboard.Visibility = Visibility.Collapsed;
            Selection.Visibility = Visibility.Collapsed;
            InGameOptions.Visibility = Visibility.Collapsed;
            characterSelector.chars1[scoreManager.GetMostPlayedChar()].IsActive = false;
            characterSelector.UnLoadChar(1);
            characterSelector.UnLoadChar(2);
            SaveGameOptions();
            Refresh();
        }

        private void bt_options_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Visible;


        }
        private void bt_singleplay_Click(object sender, RoutedEventArgs e)
        {
            Selection.Visibility = Visibility.Visible;
            MainMenu.Visibility = Visibility.Collapsed;
            Player1.Visibility = Visibility.Visible;
            Player2.Visibility = Visibility.Collapsed;

            Thickness margin = Player1.Margin;
            margin.Right = GameWindow.Width / 2 - Player1.Width;
            Player1.Margin = margin;

            player_number = 1;
            characterSelector.LoadChar(player_number);
        }
        private void bt_multiplay_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Selection.Visibility = Visibility.Visible;
            Player1.Visibility = Visibility.Visible;
            Player2.Visibility = Visibility.Visible;

            Thickness margin = Player1.Margin;
            margin.Right = Player1.Width;
            Player1.Margin = margin;

            player_number = 2;
            characterSelector.LoadChar(player_number);

        }
        private void bt_play_Click(object sender, RoutedEventArgs e)
        {
            Selection.Visibility = Visibility.Collapsed;
            PlayingField.Visibility = Visibility.Visible;
            player1_name = tb_player1.Text;
            lb_player1_name.Content = player1_name;
            lb_player1name.Content = player1_name;
            lb_player1_score.Content = score1;
            lb_floor_player1.Content = 0;
            characterSelector.SelectedChar(1).Life = PlayerSprite.maxLife;
            if (player_number != 1)
            { 
                player2_name = tb_player2.Text;
                lb_player2_name.Content = player2_name;
                lb_player2name.Content = player2_name;
                lb_floor_player2.Content = 0;
                lb_player2_score.Content = score2;
                lb_player2_name.Visibility = Visibility.Visible;
                lb_player2_score.Visibility = Visibility.Visible;
                characterSelector.SelectedChar(2).Life = PlayerSprite.maxLife;
            }

            mapLoader.LoadMap(player_number);
            
        }
        private void bt_leaderboard_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Leaderboard.Visibility = Visibility.Visible;
            Vector2 mostplayedsize = new Vector2(2, 2);
            Vector2 mostplayedpos = new Vector2(6, 13.5);
            int amount = scoreManager.ScoreAmount();
            if (amount != 0)
            {
                characterSelector.chars1[scoreManager.GetMostPlayedChar()].Position = mostplayedpos;
                characterSelector.chars1[scoreManager.GetMostPlayedChar()].Size = mostplayedsize;
                characterSelector.chars1[scoreManager.GetMostPlayedChar()].IsActive = true;
                lb_mostplayedname.Content = scoreManager.GetCharName(scoreManager.GetMostPlayedChar());
            }
            _highscoreData = scoreManager.GetOrderedScore();
            
            for (int i =0; i < amount; i++)
            {
                if (i == 10)
                    break;
                Console.WriteLine(_highscoreData[i].characterid);
                lb_data_scoresname.Content += "\n" + _highscoreData[i].uname ;
                lb_data_scores.Content += "\n" + _highscoreData[i].score.ToString();
                lb_data_scoresfloor.Content += "\n" + _highscoreData[i].floorcount.ToString();
                
            }
            

        }

        private void bt_menu_Click(object sender, RoutedEventArgs e)
        {
            
            InGameOptions.Visibility = Visibility.Collapsed;
            PlayingField.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Visible;
            mapLoader.ClearAll();
            SoundManager.PlaySong("MainMenu.mp3");
            GameManager.Begin();
        }
        private void bt_resume_Click(object sender, RoutedEventArgs e)
        {
            InGameOptions.Visibility = Visibility.Collapsed;
            GameManager.Begin();
        }
        private void bt_exit_end_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(characterSelector.chars_number1 + "+" + characterSelector.chars_number2);
            scoreManager.AddScore(new Hscore() { uname = player1_name, score = (int)lb_player1score.Content, floorcount = (int)lb_floor_player1.Content, characterid = characterSelector.chars_number1 });
            if (player_number != 1)
                scoreManager.AddScore(new Hscore() { uname = player2_name, score = (int)lb_player2score.Content, floorcount = (int)lb_floor_player2.Content, characterid = characterSelector.chars_number2 });
            System.Windows.Application.Current.Shutdown();
        }
        private void bt_tofloors_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            _highscoreData = scoreManager.GetOrderedFloor();
            int amount = scoreManager.ScoreAmount();
            for (int i = 0; i < amount; i++)
            {
                if (i == 10)
                    break;
                lb_data_scoresname.Content += "\n" + _highscoreData[i].uname;
                lb_data_scores.Content += "\n" + _highscoreData[i].score.ToString();
                lb_data_scoresfloor.Content += "\n" + _highscoreData[i].floorcount.ToString();

            }
        }
        private void bt_toscore_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            _highscoreData = scoreManager.GetOrderedScore();
            int amount = scoreManager.ScoreAmount();
            for (int i = 0; i < amount; i++)
            {
                if (i == 10)
                    break;
                lb_data_scoresname.Content += "\n" + _highscoreData[i].uname;
                lb_data_scores.Content += "\n" + _highscoreData[i].score.ToString();
                lb_data_scoresfloor.Content += "\n" + _highscoreData[i].floorcount.ToString();

            }
        }
        private void bt_Clear_Click(object sender, RoutedEventArgs e)
        {
            Confirm confirm = new Confirm();
            Confirm.Inst.Activate();
            Confirm.Inst.Show();

            
        }
        private void bt_mainmenu_Click(object sender, RoutedEventArgs e)
        {

            scoreManager.AddScore(new Hscore() { uname = player1_name, score = (int)lb_player1score.Content, floorcount = (int)lb_floor_player1.Content, characterid = characterSelector.chars_number1 });
            if (player_number != 1)
                scoreManager.AddScore(new Hscore() { uname = player2_name, score = (int)lb_player2score.Content, floorcount = (int)lb_floor_player2.Content, characterid = characterSelector.chars_number2 });
            MainMenu.Visibility = Visibility.Visible;
            SoundManager.PlaySong("MainMenu.mp3");
            EndGame.Visibility = Visibility.Collapsed;
            lb_floor_player1.Content = 0;
            lb_floor_player2.Content = 0;
            lb_player1score.Content = 0;
            lb_player2score.Content = 0;
        }
        /// <summary>
        /// Selection arrow moving events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_options_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            Arrow_placing(bt_options.Margin);
        }

        private void bt_options_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();   
        }
        private void bt_help_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            Arrow_placing(bt_help.Margin);
        }
        private void bt_help_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }
        private void bt_exit_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            Arrow_placing(bt_exit.Margin);
        }
        private void bt_exit_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        private void bt_multiplay_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            Arrow_placing(bt_multiplay.Margin);
        }

        private void bt_multiplay_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        private void bt_singleplay_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            Arrow_placing(bt_singleplay.Margin);
        }

        private void bt_singleplay_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        /// <summary>
        /// Options button events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void sl_effect_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float k = (float)sl_effect.Value / 100.0f;
            SoundManager.mixer.effectVolume = k * k * 4;
        }

        private void sl_music_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float k = (float)sl_music.Value / 100.0f;
            SoundManager.mixer.musicVolume = k * k * 0.3f;
        }

        private void bt_charsselect1_r_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            characterSelector.NextChar(1);
        }

        private void bt_charsselect2_r_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            characterSelector.NextChar(2);
        }

        private void bt_charsselect1_l_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            characterSelector.PreChar(1);
        }

        private void bt_charsselect2_l_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySoundEffect("ButtonClick.mp3");
            characterSelector.PreChar(2);
        }
        /// <summary>
        /// Save Game Options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveGameOptions();
            isWindowOpen = false;
        }

        
    }
}
