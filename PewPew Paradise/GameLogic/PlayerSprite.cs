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
    public class PlayerSprite : Sprite
    {
        public int player_id;
        public string projectile;
        public List<Key> _keys = new List<Key>();
        public double timer = 100000;
        public int life = 3;
        public double dietimer;
        
        public PlayerSprite(string image, int player_id, string projectile, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            AddComponent<Portal>();
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
        }
        /// <summary>
        /// Moving left and right
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
        public void Jump()
        {
            if (GetComponent<CollideComponent>().isOnGround)
            {
                GetComponent<PhysicsComponent>().speed.y = -6.375;
                GetComponent<AnimatorComponent>().PlayAnimation(2);
            }
        }
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

        public override void Update()
        {
            Console.WriteLine($"char{player_id}|hp:{life}");


            if (life > 0)
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


            if(MainWindow.Instance.PlayingField.Visibility == Visibility.Visible)
            if (!MainWindow.Instance.load.CurrentMap().just_loaded && life > 0)
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
            if (life > 0)
            {
                for (int i = 0; i < Enemy.enemyList.Count; i++)
                {

                    if (Enemy.enemyList[i].GetRect().IntersectsWith(this.GetRect()) && dietimer > 2.0)
                    {
                        dietimer = 1;
                        life--;
                        GetComponent<AnimatorComponent>().PlayAnimation(5);
                        if (life < 1)
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
            //Fall animation
            if (!GetComponent<CollideComponent>().isOnGround && GetComponent<PhysicsComponent>().speed.y > 0.0)
            {
                GetComponent<AnimatorComponent>().PlayAnimation(3);
            }


        }
        
        void FinishDeath(AnimatorComponent animator, int animation)
        {
            GetComponent<AnimatorComponent>().OnAnimationEnded -= FinishDeath;
            if (MainWindow.Instance.player_number == 1)
            {
                if (MainWindow.Instance.chars.SelectedChar(1).life == 0)
                {
                    MainWindow.Instance.chars.SelectedChar(1).IsActive = false;
                    MainWindow.Instance.PlayingField.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.EndGame.Visibility = Visibility.Visible;

                    MainWindow.Instance.lb_floor_player1.Content = MainWindow.Instance.load.floor;
                    MainWindow.Instance.lb_player1score.Content = MainWindow.Instance.score1;
                    MainWindow.Instance.lb_floortext2.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_scoretext2.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_floor_player2.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_player2score.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.lb_player2name.Visibility = Visibility.Collapsed;
                    MainWindow.Instance.load.ClearAll();
                }
            }
            else
            {
                if (MainWindow.Instance.chars.SelectedChar(1).life == 0)
                {
                    MainWindow.Instance.chars.SelectedChar(1).IsActive = false;
                    if ((int)MainWindow.Instance.lb_floor_player1.Content == 0)
                    {
                        MainWindow.Instance.lb_floor_player1.Content = MainWindow.Instance.load.floor;
                    }
                    MainWindow.Instance.lb_player1score.Content = MainWindow.Instance.score1;
                    if (MainWindow.Instance.chars.SelectedChar(2).life == 0)
                    {
                        if ((int)MainWindow.Instance.lb_floor_player2.Content == 0)
                        {
                            MainWindow.Instance.lb_floor_player2.Content = MainWindow.Instance.load.floor;
                        }
                        MainWindow.Instance.lb_player2score.Content = MainWindow.Instance.score2;
                        MainWindow.Instance.PlayingField.Visibility = Visibility.Collapsed;
                        MainWindow.Instance.EndGame.Visibility = Visibility.Visible;
                        MainWindow.Instance.lb_floortext2.Visibility = Visibility.Visible;
                        MainWindow.Instance.lb_scoretext2.Visibility = Visibility.Visible;
                        MainWindow.Instance.load.ClearAll();
                    }
                }
                if (MainWindow.Instance.chars.SelectedChar(2).life == 0)
                {
                    MainWindow.Instance.chars.SelectedChar(2).IsActive = false;
                    if ((int)MainWindow.Instance.lb_floor_player2.Content == 0)
                    {
                        MainWindow.Instance.lb_floor_player2.Content = MainWindow.Instance.load.floor;
                    }
                    MainWindow.Instance.lb_player2score.Content = MainWindow.Instance.score2;
                }
            }
            RectangleElement.Opacity = 1;
        }



        public override void Start()
        {
            AddComponent<PhysicsComponent>().IsActive=false;
            AddComponent<CollideComponent>().IsActive=false;
        }

    }
}
