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
        public List<Sprite> chars1 = new List<Sprite>();
        public List<Sprite> chars2 = new List<Sprite>();
        Vector2 select_pos1 = new Vector2(3, 13);
        Vector2 select_pos2 = new Vector2(10, 13);
        Vector2 select_size = new Vector2(3, 3);
        public int chars_number1 = 0;
        public int chars_number2 = 0;
        public CharacterSelect()
        {
            SpriteManager.LoadImage("Images/Sprites/Characters/Unicorn.png", "unicorn");
            SpriteManager.LoadImage("Images/Sprites/Characters/OrkPM.png", "ork");
            SpriteManager.LoadImage("Images/Sprites/Characters/MrPlaceholder1.png", "MrPlaceholder");
            Sprite unicorn1 = new Sprite("unicorn", select_pos1, select_size, false);
            chars1.Add(unicorn1);
            Sprite ork1 = new Sprite("ork", select_pos1, select_size, false);
            chars1.Add(ork1);
            Sprite MrPlaceholder1 = new Sprite("MrPlaceholder", select_pos1, select_size, false);
            chars1.Add(MrPlaceholder1);
            Sprite unicorn2 = new Sprite("unicorn", select_pos2, select_size, false);
            chars2.Add(unicorn2);
            Sprite ork2 = new Sprite("ork", select_pos2, select_size, false);
            chars2.Add(ork2);
            Sprite MrPlaceholder2 = new Sprite("MrPlaceholder", select_pos2, select_size, false);
            chars2.Add(MrPlaceholder2);
        }
        public void LoadChar() 
        {
            chars_number1 = 0;
            chars1[chars_number1].IsActive = true;
            chars_number2 = 0;
            chars2[chars_number2].IsActive = true;
        }
        public void NextChar(int player_number) 
        {
            if (player_number == 1)
            {
                UnLoadChar(1);
                chars_number1++;
                chars_number1 = chars_number1 % chars1.Count;
                chars1[chars_number1].IsActive = true;
            }
            else
            {
                UnLoadChar(2);
                chars_number2++;
                chars_number2 = chars_number2 % chars2.Count;
                chars2[chars_number2].IsActive = true;
            }
        }
        public void PreChar(int player_number)
        {
            if (player_number == 1)
            {
                UnLoadChar(1);
                if (chars_number1 == 0)
                {
                    chars_number1 = chars1.Count - 1;
                    chars1[chars_number1].IsActive = true;
                }
                else
                {
                    chars_number1--;
                    chars1[chars_number1].IsActive = true;
                }
            }
            else
            {
                UnLoadChar(2);
                if (chars_number2 == 0)
                {
                    chars_number2 = chars2.Count - 1;
                    chars2[chars_number2].IsActive = true;
                }
                else
                {
                    chars_number2--;
                    chars2[chars_number2].IsActive = true;
                }
            }
        }
        
        public void UnLoadChar(int player_number) 
        {
            if (player_number == 1)
            {
                chars1[chars_number1].IsActive = false;
            }
            else
            {
                chars2[chars_number2].IsActive = false;
            }
        }
        public Sprite SelectedChar(int player_number)
        {
            if (player_number == 1)
                return chars1[chars_number1];
            else
                return chars2[chars_number2];
        }
    }
}
