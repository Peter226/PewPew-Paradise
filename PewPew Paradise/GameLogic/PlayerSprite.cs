using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;
using PewPew_Paradise.GameLogic;
using PewPew_Paradise.GameLogic.SpriteComponents;

namespace PewPew_Paradise.GameLogic
{
    public class PlayerSprite : Sprite
    {
        public int player_id;
        public PlayerSprite(string image, int player_id, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            this.player_id = player_id;
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
                current_pos.x -= 0.1;
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
                    current_pos.x += 0.1;
                    Position = current_pos;
            }

        }
        public void Jump()
        {
            Vector2 current_pos = Position;
            current_pos.y -= 1;
            Position = current_pos;

        }
        public override void Update()
        {
        }
        public override void Start()
        {
            AddComponent<PhysicsComponent>().IsActive=false;
            AddComponent<CollideComponent>().IsActive=false;
        }

    }
}
