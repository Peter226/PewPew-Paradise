using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise
{
    public class CharacterSelect
    {
        public List<Sprite> chars = new List<Sprite>();
        Vector2 select_pos = new Vector2(3, 13);
        Vector2 select_size = new Vector2(3, 3);
        public int chars_number = 0;
        public CharacterSelect()
        {
            SpriteManager.LoadImage("Images/Sprites/Characters/Unicorn.png", "unicorn");
            SpriteManager.LoadImage("Images/Sprites/Characters/OrkIM.png", "ork");
            SpriteManager.LoadImage("Images/Sprites/Characters/MrPlaceholder.png", "MrPlaceholder");
            Sprite unicorn = new Sprite("unicorn", select_pos, select_size, false);
            chars.Add(unicorn);
            Sprite ork = new Sprite("ork", select_pos, select_size, false);
            chars.Add(ork);
            Sprite MrPlaceholder = new Sprite("MrPlaceholder", select_pos, select_size, false);
            chars.Add(MrPlaceholder);
        }
        public void LoadChar() 
        {
            chars_number = 0;
            chars[chars_number].IsActive = true;
        }
        public void NextChar() 
        {
            UnLoadChar();
            chars_number++;
            chars_number = chars_number % chars.Count;
            chars[chars_number].IsActive = true;

        }
        
        public void UnLoadChar() 
        {
            chars[chars_number].IsActive = false;
        }
        public Sprite SelectedChar()
        { 
            return chars[chars_number];
        }
    }
}
