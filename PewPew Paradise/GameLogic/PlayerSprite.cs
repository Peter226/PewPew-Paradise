using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.Maths;

namespace PewPew_Paradise.GameLogic
{
    public class PlayerSprite : Sprite
    {
        public int player_id;
        public PlayerSprite(string image,int player_id, Vector2 position, Vector2 size, bool active = true) : base(image, position, size, active)
        {
            this.player_id = player_id;
        }
    }
}
