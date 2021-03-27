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
        public double timer =100000;
        public PlayerSprite(string image, int player_id, string projectile, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            AddComponent<Portal>();
            //AddComponent<AnimatorComponent>().SetAnimation("Player");

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
            }
        }
        public void Shoot()
        {
            Vector2 size = Size * 0.5;
            Vector2 position = Position;
            if( size.x > 0 )
                position.x += 1;
            else
                position.x -= 1;
            ProjectileSprite projectile = new ProjectileSprite(this.projectile, position, size, true);
            timer = 0;
        }

        public override void Update()
        {
            if (MainWindow.Instance.PlayingField.IsVisible)
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

        }
        public override void Start()
        {
            AddComponent<PhysicsComponent>().IsActive=false;
            AddComponent<CollideComponent>().IsActive=false;
        }

    }
}
