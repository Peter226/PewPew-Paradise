using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;
using PewPew_Paradise.GameLogic;
using System.Windows.Input;
using PewPew_Paradise.GameLogic.SpriteComponents;

namespace PewPew_Paradise.GameLogic
{
    public class PlayerSprite : Sprite
    {
        public int player_id;
        public List<Key> _keys = new List<Key>();
        public PlayerSprite(string image, int player_id, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            this.player_id = player_id;
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
            if(GetComponent<CollideComponent>().isOnGround)
            GetComponent<PhysicsComponent>().speed.y = -0.075;


        }
        public override void Update()
        {
            if (Keyboard.IsKeyDown(_keys[1]))
                MoveLeft();
            if (Keyboard.IsKeyDown(_keys[2]))
                MoveRight();
            base.Update();
        }
        public override void Start()
        {
            AddComponent<PhysicsComponent>().IsActive=false;
            AddComponent<CollideComponent>().IsActive=false;
        }

    }
}
