using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;
using PewPew_Paradise.GameLogic;
using System.Windows.Input;
using PewPew_Paradise.GameLogic.SpriteComponents;
using System.Windows;

namespace PewPew_Paradise.GameLogic
{
    /// <summary>
    /// Sprite type of the players
    /// </summary>
    public class PlayerSprite : Sprite
    {
        /// <summary>
        /// which player it is (first or secong player)
        /// </summary>
        public int player_id;
        /// <summary>
        /// projectile type
        /// </summary>
        public string projectile;
        /// <summary>
        /// Constrols
        /// </summary>
        private List<Key> _keys = new List<Key>();
        public double timer = 100000;
        /// <summary>
        /// hp of the player
        /// </summary>
        private int life = 3;
        /// <summary>
        /// maximum life of players
        /// </summary>
        public static int maxLife = 3;
        /// <summary>
        /// death animation timer
        /// </summary>
        public double dietimer;
        /// <summary>
        /// Sprites of the hp bar
        /// </summary>
        List<Sprite> lifeSprites = new List<Sprite>();

        /// <summary>
        /// When player life changes this updatedes the healthbar
        /// Healthbar is made of 2 sprites
        /// </summary>
        public int Life
        {
            get { return life; }
            set {
                for (int i = 0; i < lifeSprites.Count; i++)
                {
                    lifeSprites[i].Destroy();
                }
                lifeSprites.Clear();
                life = value;
                if (MainWindow.Instance.PlayingField.Visibility == Visibility.Visible) {
                    for (int i = 0; i < maxLife; i++)
                    {
                        Vector2 HpPos;
                        Vector2 HpDir;
                        if (player_id == 1)
                        {
                            HpPos = new Vector2(4.5, 0.5);
                            HpDir = new Vector2(0.5, 0);
                        }
                        else
                        {
                            HpPos = new Vector2(11.5, 0.5);
                            HpDir = new Vector2(-0.5, 0);
                        }
                        if (i < life)
                        {
                            lifeSprites.Add(new Sprite("hp_full", HpPos + HpDir * i, Vector2.One * 0.5, true, 2));
                        }
                        else
                        {
                            lifeSprites.Add(new Sprite("hp_empty", HpPos + HpDir * i, Vector2.One * 0.5, true, 2));
                        }
                    }
                }
               
            }
        }
        /// <summary>
        /// Adding components, keys for controls, projectile string for shooting the right sprite
        /// PlayerSprites are created with maxLife
        /// </summary>
        /// <param name="image"></param>
        /// <param name="player_id"></param>
        /// <param name="projectile"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="active"></param>
        public PlayerSprite(string image, int player_id, string projectile, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            AddComponent<PortalComponent>();
            AddComponent<AnimatorComponent>().SetAnimation("Player");

            this.player_id = player_id;
            this.projectile = projectile;
            if(player_id == 1)
            {
                _keys.Add(Key.W);
                _keys.Add(Key.A);
                _keys.Add(Key.D);
                _keys.Add(Key.LeftCtrl);
            }
            else
            {
                _keys.Add(Key.Up);
                _keys.Add(Key.Left);
                _keys.Add(Key.Right);
                _keys.Add(Key.RightCtrl);
            }
            Life = maxLife;
        }
        /// <summary>
        /// Moving PlayerSprite left
        /// </summary>
        /// <param name="player_number"></param>

        public void MoveLeft()
        {
            GetComponent<AnimatorComponent>().PlayAnimation(1);
            Vector2 current_pos = Position;
            Vector2 current_size = Size;
            current_size.x = -1;
            Size = current_size;

            if (current_pos.x > 1)
            {
                current_pos.x -= 0.003*GameManager.DeltaTime;
                Position = current_pos;
            }
        }
        /// <summary>
        /// Moving PlayerSprite right
        /// </summary>
        public void MoveRight()
        {
            GetComponent<AnimatorComponent>().PlayAnimation(1);
            Vector2 current_pos = Position;
            Vector2 current_size = Size;

            current_size.x = 1;
            Size = current_size;

            if (current_pos.x < 15)
            {
                    current_pos.x += 0.003 * GameManager.DeltaTime;
                    Position = current_pos;
            }

        }
        /// <summary>
        /// Jumping PlayerSprite
        /// It cant jump again until its on the ground
        /// </summary>
        public void Jump()
        {
            if (GetComponent<CollideComponent>().isOnGround)
            {
                GetComponent<PhysicsComponent>().speed.y = -6.375;
                GetComponent<AnimatorComponent>().PlayAnimation(2);
            }
        }
        /// <summary>
        /// Shooting ProjectileSprites 
        /// </summary>
        public void Shoot()
        {
            Vector2 size = Size * 0.5;
            Vector2 position = Position;
            if( size.x > 0 )
                position.x += 0.5;
            else
                position.x -= 0.5;
            ProjectileSprite projectile = new ProjectileSprite(this.projectile, position, size, true);
            timer = 0;
            GetComponent<AnimatorComponent>().PlayAnimation(4);
        }


        /// <summary>
        /// Play Animations ignoring priority
        /// </summary>
        public override void OnEnabled()
        {
            GetComponent<AnimatorComponent>().ForcePlayAnimation(0);
        }


        public override void Update()
        { 
                //If a player die this make it look like disappeare
                if (Life > 0)
                {
                    if (RectangleElement.Opacity != 1) {
                        RectangleElement.Opacity = 1;
                    }
                }
                else
                {
                    RectangleElement.Opacity -= GameManager.DeltaTime * 0.0005;
                    Position -= new Vector2(0, GameManager.DeltaTime * 0.0015);
                }

            //If PlayingField is active and map is loaded makes the player be able to move with the given keys
            //This makes it to not shoot instantly
            if(MainWindow.Instance.PlayingField.Visibility == Visibility.Visible)
            if (!MainWindow.Instance.mapLoader.CurrentMap().just_loaded && Life > 0)
            {
                if (Keyboard.IsKeyDown(_keys[1]))
                    MoveLeft();
                if (Keyboard.IsKeyDown(_keys[2]))
                    MoveRight();
                if (Keyboard.IsKeyDown(_keys[0]))
                    Jump();
                if (timer >= 500)
                    if (Keyboard.IsKeyDown(_keys[3]))
                        Shoot();
            }

            timer += GameManager.DeltaTime;
            dietimer += 0.00075 * GameManager.DeltaTime;
            base.Update();

            //Collecting fruits and increasing scores
            FruitSprite collectFruit = null;
            foreach (FruitSprite fruit in FruitSprite.fruitList)
            {
                Rect fruitHitBox = fruit.GetRect();
                if (fruitHitBox.IntersectsWith(this.GetRect()))
                {
                    collectFruit = fruit;
                    break;
                }
            }
            if (collectFruit != null)
            {
                if (player_id == 1)
                {
                    MainWindow.Instance.score1 += collectFruit.point;
                    MainWindow.Instance.lb_player1_score.Content = MainWindow.Instance.score1;
                }
                else
                {
                    MainWindow.Instance.score2 += collectFruit.point;
                    MainWindow.Instance.lb_player2_score.Content = MainWindow.Instance.score2;
                }
                collectFruit.FruitCollect();
            }

            //Checks if a player get hit by an enemy and reduce it life
            //This makes the PlayerSprite to be able lose life in every 2 seconds
            if (Life > 0)
            {
                for (int i = 0; i < Enemy.enemyList.Count; i++) 
                {
                    if (Enemy.enemyList[i].dead)continue;
                    if (Enemy.enemyList[i].GetRect().IntersectsWith(this.GetRect()) && dietimer > 2.0) 
                    {

                        if (Enemy.enemyList[i].GetRect().IntersectsWith(this.GetRect()) && dietimer > 2.0)
                        {
                            dietimer = 1;
                            Life--;
                            GetComponent<AnimatorComponent>().PlayAnimation(5);
                            if (Life < 1)
                            {
                                GetComponent<CollideComponent>().IsActive = false;
                                GetComponent<PhysicsComponent>().IsActive = false;
                                GetComponent<AnimatorComponent>().PlayAnimation(6);
                                GetComponent<AnimatorComponent>().OnAnimationEnded += FinishDeath;
                            }
                            break;
                        }
                    }
                }
            }
            //Fall animation
            if (!GetComponent<CollideComponent>().isOnGround && GetComponent<PhysicsComponent>().speed.y > 0.0)
            {
                GetComponent<AnimatorComponent>().PlayAnimation(3);
            }


        }
        /// <summary>
        /// Checks which player is dead
        /// In Singleplayer mode if the player is dead EndGame screen become available and show the scores in labels and reset the Game
        /// In Multiplayer mode if 1 player is dead it saves the floor in a label and deactivate the sprite, if both player dead EndGame screen become available and show the scores in labels and reset the Game
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animation"></param>
        void FinishDeath(AnimatorComponent animator, int animation)
        {
            GetComponent<AnimatorComponent>().OnAnimationEnded -= FinishDeath;
            if (MainWindow.Instance.player_number == 1)
            {
                if (MainWindow.Instance.characterSelector.SelectedChar(1).Life == 0)
                {
                    MainWindow.Instance.characterSelector.SelectedChar(1).IsActive = false;
                    MainWindow.Instance.PlayingField.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.EndGame.Visibility = Visibility.Visible;

                    MainWindow.Instance.lb_floor_player1.Content = MainWindow.Instance.mapLoader.floor;
                    MainWindow.Instance.lb_player1score.Content = MainWindow.Instance.score1;
                    MainWindow.Instance.lb_floortext2.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_scoretext2.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_floor_player2.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_player2score.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_player2name.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.mapLoader.ClearAll();
                }
            }
            else
            {
                if (MainWindow.Instance.characterSelector.SelectedChar(1).Life == 0)
                {
                    MainWindow.Instance.characterSelector.SelectedChar(1).IsActive = false;
                    if ((int)MainWindow.Instance.lb_floor_player1.Content == 0)
                    {
                        MainWindow.Instance.lb_floor_player1.Content = MainWindow.Instance.mapLoader.floor;
                    }
                    MainWindow.Instance.lb_player1score.Content = MainWindow.Instance.score1;
                    if (MainWindow.Instance.characterSelector.SelectedChar(2).Life == 0)
                    {
                        if ((int)MainWindow.Instance.lb_floor_player2.Content == 0)
                        {
                            MainWindow.Instance.lb_floor_player2.Content = MainWindow.Instance.mapLoader.floor;
                        }
                        MainWindow.Instance.lb_player2score.Content = MainWindow.Instance.score2;
                        MainWindow.Instance.PlayingField.Visibility = Visibility.Collapsed;
                        MainWindow.Instance.EndGame.Visibility = Visibility.Visible;
                        MainWindow.Instance.lb_floortext2.Visibility = Visibility.Visible;
                        MainWindow.Instance.lb_scoretext2.Visibility = Visibility.Visible;
                        MainWindow.Instance.mapLoader.ClearAll();
                    }
                }
                if (MainWindow.Instance.characterSelector.SelectedChar(2).Life == 0)
                {
                    MainWindow.Instance.characterSelector.SelectedChar(2).IsActive = false;
                    if ((int)MainWindow.Instance.lb_floor_player2.Content == 0)
                    {
                        MainWindow.Instance.lb_floor_player2.Content = MainWindow.Instance.mapLoader.floor;
                    }
                    MainWindow.Instance.lb_player2score.Content = MainWindow.Instance.score2;
                }
            }
            RectangleElement.Opacity = 1;
        }

        /// <summary>
        /// In Selection they have components but we need them to be false
        /// </summary>
        public override void Start()
        {
            AddComponent<PhysicsComponent>().IsActive=false;
            AddComponent<CollideComponent>().IsActive=false;
        }

    }
}
