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

namespace PewPew_Paradise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow instance;

        double windowDifferenceX;
        double windowDifferenceY;

        double previousWidth;
        double previousHeight;

        float Timer = 0;

        List<Sprite> MrPlaceHolders = new List<Sprite>();
        List<Hscore> data = new List<Hscore>();

        MediaPlayer mp = new MediaPlayer();

        public MapLoad load;
        public CharacterSelect chars;
        public Enemy enemy;
        public static SolidColorBrush playingFieldBrush;
        public AccessData score = new AccessData();

        public int player_number;
        public string player1_name;
        public string player2_name;
        public int score1 = 0;
        public int score2= 0;
        public double enemyHitTimer;



        //JSON saving system
        public GameOptions gameOptions;
        public static JsonSerializer optionsSerializer = new JsonSerializer();
        //JSON saving system



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
            LoadGameOptions();
            score.InitDB();
            Console.WriteLine(score.GetCharName(0));
            Console.WriteLine(score.GetCharName(1));
            Console.WriteLine(score.GetCharName(2));
            Console.WriteLine(score.GetCharName(3));
            Console.WriteLine(score.GetCharName(4));
            Console.WriteLine(score.GetCharName(5));
            Console.WriteLine(score.GetCharName(6));
            Console.WriteLine(score.GetCharName(7));
            Console.WriteLine(score.GetCharName(8));
            Console.WriteLine(score.GetCharName(9));
            Console.WriteLine(score.GetCharName(10));
            Console.WriteLine(score.GetCharName(11));
            Console.WriteLine(score.GetCharName(12));
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

            load = new MapLoad();
            chars = new CharacterSelect();
            enemy = new Enemy();
            PlayingField.Background = new SolidColorBrush();
            playingFieldBrush = (SolidColorBrush)PlayingField.Background;

            string l = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string p = System.IO.Path.GetDirectoryName(l);
            p = System.IO.Path.Combine(p, "PewPew_Paradise_Assets/Music/MainMenu.mp3");
            Console.WriteLine(p);
            mp.Open(new Uri(p));
            mp.MediaOpened += new EventHandler(PlayMedia);


            Dummy.Opacity = 0.0;
            DoubleAnimation enslaveWPF = new DoubleAnimation(20.0,30.0,TimeSpan.FromMilliseconds(1));
            enslaveWPF.RepeatBehavior = RepeatBehavior.Forever;
            enslaveWPF.AutoReverse = true;
            Dummy.BeginAnimation(Image.WidthProperty,enslaveWPF);

            FruitSprite.LoadImages();
            Refresh();

        }
        public void Refresh()
        {
            lb_data_scoresname.Content = "Name:";
            lb_data_scores.Content = "Score:";
            lb_data_scoresfloor.Content = "Floor:";
        }
        /// <summary>
        /// Music
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        public void PlayMedia(object sender, EventArgs a)
        {
            Console.WriteLine("Opened Media");
            mp.Play();
            double k = (double)sl_music.Value;
            mp.Volume = k / 100;
        }


        /// <summary>
        /// Animation loop
        /// </summary>
        public void Update()
        {


            if (Keyboard.IsKeyDown(Key.Left))
            {
                foreach (Sprite sprite in MrPlaceHolders)
                {
                    Vector2 newpos = sprite.Position;
                    newpos.x -= GameManager.DeltaTime * 0.03;
                    sprite.Position = newpos;
                }
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                foreach (Sprite sprite in MrPlaceHolders)
                {
                    Vector2 newpos = sprite.Position;
                    newpos.x += GameManager.DeltaTime * 0.03;
                    sprite.Position = newpos;
                }
            }
            if (Keyboard.IsKeyDown(Key.Up))
            {
                foreach (Sprite sprite in MrPlaceHolders)
                {
                    Vector2 newpos = sprite.Position;
                    newpos.y -= GameManager.DeltaTime * 0.03;
                    sprite.Position = newpos;
                }
            }
            if (Keyboard.IsKeyDown(Key.Down))
            {
                foreach (Sprite sprite in MrPlaceHolders)
                {
                    Vector2 newpos = sprite.Position;
                    newpos.y += GameManager.DeltaTime * 0.03;
                    sprite.Position = newpos;
                }
            }



            Timer -= (float)(GameManager.DeltaTime * 0.00075);
            foreach (Sprite sprite in MrPlaceHolders)
            {
                Vector2 newpos = sprite.Position;
                double deltaX = Math.Sin(Timer * 2.2 + Math.Cos(newpos.y) * 0.1f) * 0.1f;
                if (deltaX < 0)
                {
                    sprite.Size = new Vector2(-Math.Abs(sprite.Size.x),sprite.Size.y);
                }
                else
                {
                    sprite.Size = new Vector2(Math.Abs(sprite.Size.x), sprite.Size.y);
                }
                newpos.x += deltaX;
                newpos.y += Math.Sin(Timer * 3.3 + Math.Cos(newpos.x) * 0.1f) * 0.1f;
                sprite.Position = newpos;
            }
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
            
            if (e.Key == Key.P) {

                EnemySprite mrph = new EnemySprite("MrPlaceHolder", new Maths.Vector2(8, 8), new Maths.Vector2(1, 1));
                MrPlaceHolders.Add(mrph);
                mrph.AddComponent<AnimatorComponent>().SetAnimation("Player");
            }
            if (e.Key == Key.Space)
            {
                foreach (Sprite spriteAnimated in MrPlaceHolders)
                {
                    spriteAnimated.GetComponent<AnimatorComponent>().PlayAnimation(0);
                }
            }
            if (e.Key == Key.Delete)
            {
                for (int i = MrPlaceHolders.Count - 1; i >= 0;i--)
                {
                    MrPlaceHolders[i].Destroy();
                    MrPlaceHolders.RemoveAt(i);
                }
            }
            //This is for just testing now
            if(e.Key == Key.M)
            {
                load.NextMap(2);
            }
            if (e.Key == Key.N)
            {
                load.NextMap(1);
            }



            if (e.Key == Key.Escape)
            {
                if (InGameOptions.Visibility == Visibility.Collapsed) {
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
        }

        /// <summary>
        /// Detect Key lift
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyLift(object sender, KeyEventArgs e)
        {

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
           /* gameOptions.musicVolume = sl_music.Value;
            gameOptions.effectVolume = sl_effect.Value;

            string workingDirectory = Environment.SpecialFolder.LocalApplicationData.ToString();
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            string path = System.IO.Path.Combine(projectDirectory, "MapCollisions", this.image + ".json");
            StreamWriter sw = new StreamWriter(path);
            JsonWriter jw = new JsonTextWriter(sw);
            optionsSerializer.Serialize(jw, gameOptions);
            jw.Close();
            sw.Close();*/
            
        }
        /// <summary>
        /// Load options
        /// </summary>
        void LoadGameOptions()
        {

            /*var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"PewPew_Paradise.MapCollisions.{this.image}.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonReader jreader = new JsonTextReader(reader))
                {
                    gameOptions = (GameOptions)optionsSerializer.Deserialize(jreader, typeof(GameOptions));
                }
            }


            sl_music.Value = gameOptions.musicVolume;
            sl_effect.Value = gameOptions.effectVolume;*/
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
            Help.Visibility = Visibility.Collapsed;
            Options.Visibility = Visibility.Collapsed;
            Leaderboard.Visibility = Visibility.Collapsed;
            Selection.Visibility = Visibility.Collapsed;
            InGameOptions.Visibility = Visibility.Collapsed;
            chars.UnLoadChar(1);
            chars.UnLoadChar(2);
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
            chars.LoadChar(player_number);
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
            chars.LoadChar(player_number);

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
            chars.SelectedChar(1).Life = PlayerSprite.maxLife;
            if (player_number != 1)
            { 
                player2_name = tb_player2.Text;
                lb_player2_name.Content = player2_name;
                lb_player2name.Content = player2_name;
                lb_floor_player2.Content = 0;
                lb_player2_score.Content = score2;
                lb_player2_name.Visibility = Visibility.Visible;
                lb_player2_score.Visibility = Visibility.Visible;
                chars.SelectedChar(2).Life = PlayerSprite.maxLife;
            }

            load.LoadMap(player_number);
            
        }
        private void bt_leaderboard_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Leaderboard.Visibility = Visibility.Visible;
            
            //score.ClearDB();
            data = score.GetOrderedScore();
            int amount = score.ScoreAmount();
            for (int i =0; i < amount; i++)
            {
                if (i == 10)
                    break;
                lb_data_scoresname.Content += "\n" + data[i].uname ;
                lb_data_scores.Content += "\n" + data[i].score.ToString();
                lb_data_scoresfloor.Content += "\n" + data[i].floorcount.ToString();
                
            }
            

        }

        private void bt_menu_Click(object sender, RoutedEventArgs e)
        {
            
            InGameOptions.Visibility = Visibility.Collapsed;
            PlayingField.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Visible;
            load.ClearAll();
            GameManager.Begin();
        }
        private void bt_resume_Click(object sender, RoutedEventArgs e)
        {
            InGameOptions.Visibility = Visibility.Collapsed;
            GameManager.Begin();
        }
        private void bt_exit_end_Click(object sender, RoutedEventArgs e)
        {
            score.AddScore(new Hscore() { uname = player1_name, score = (int)lb_player1score.Content, floorcount = (int)lb_floor_player1.Content, characterid = chars.chars_number1 });
            if (player_number != 1)
                score.AddScore(new Hscore() { uname = player2_name, score = (int)lb_player2score.Content, floorcount = (int)lb_floor_player2.Content, characterid = chars.chars_number2 });
        }
        private void bt_tofloors_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            data = score.GetOrderedFloor();
            int amount = score.ScoreAmount();
            for (int i = 0; i < amount; i++)
            {
                if (i == 10)
                    break;
                lb_data_scoresname.Content += "\n" + data[i].uname;
                lb_data_scores.Content += "\n" + data[i].score.ToString();
                lb_data_scoresfloor.Content += "\n" + data[i].floorcount.ToString();

            }
        }
        private void bt_toscore_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
            data = score.GetOrderedScore();
            int amount = score.ScoreAmount();
            for (int i = 0; i < amount; i++)
            {
                if (i == 10)
                    break;
                lb_data_scoresname.Content += "\n" + data[i].uname;
                lb_data_scores.Content += "\n" + data[i].score.ToString();
                lb_data_scoresfloor.Content += "\n" + data[i].floorcount.ToString();

            }
        }
        private void bt_Clear_Click(object sender, RoutedEventArgs e)
        {
            score.ClearDB();
        }
        /// <summary>
        /// Selection arrow moving events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_options_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_options.Margin);
        }

        private void bt_options_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();   
        }
        private void bt_help_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_help.Margin);
        }
        private void bt_help_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }
        private void bt_exit_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_exit.Margin);
        }
        private void bt_exit_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        private void bt_multiplay_MouseEnter(object sender, MouseEventArgs e)
        {
            Arrow_placing(bt_multiplay.Margin);
        }

        private void bt_multiplay_MouseLeave(object sender, MouseEventArgs e)
        {
            Arrow_clear();
        }

        private void bt_singleplay_MouseEnter(object sender, MouseEventArgs e)
        {
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

        private void sl_music_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double k = (double)sl_music.Value;
            mp.Volume = k/100;
        }
        private void bt_mainmenu_Click(object sender, RoutedEventArgs e)
        {
            
            score.AddScore(new Hscore() { uname = player1_name, score = (int)lb_player1score.Content, floorcount = (int)lb_floor_player1.Content });
            if (player_number != 1)
                score.AddScore(new Hscore() { uname =player2_name, score = (int)lb_player2score.Content, floorcount = (int)lb_floor_player2.Content });
            MainMenu.Visibility = Visibility.Visible;
            EndGame.Visibility = Visibility.Collapsed;
            lb_floor_player1.Content = 0;
            lb_floor_player2.Content = 0;
            lb_player1score.Content = 0;
            lb_player2score.Content = 0;
        }
        private void bt_charsselect1_r_Click(object sender, RoutedEventArgs e)
        {
            chars.NextChar(1);
        }

        private void bt_charsselect2_r_Click(object sender, RoutedEventArgs e)
        {
            chars.NextChar(2);
        }

        private void bt_charsselect1_l_Click(object sender, RoutedEventArgs e)
        {
            chars.PreChar(1);
        }

        private void bt_charsselect2_l_Click(object sender, RoutedEventArgs e)
        {
            chars.PreChar(2);
        }


    }
}
