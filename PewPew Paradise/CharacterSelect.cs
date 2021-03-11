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
        public List<PlayerSprite> chars1 = new List<PlayerSprite>();
        public List<PlayerSprite> chars2 = new List<PlayerSprite>();
        Vector2 select_pos1 = new Vector2(3, 13);
        Vector2 select_pos2 = new Vector2(10, 13);
        Vector2 select_size = new Vector2(3, 3);
        public int chars_number1;
        public int chars_number2;
        public CharacterSelect()
        {
            SpriteManager.LoadImage("Images/Sprites/Characters/Unicorn.png", "unicorn");
            SpriteManager.LoadImage("Images/Sprites/Characters/Unicorn.png", "unicorn");
            SpriteManager.LoadImage("Images/Sprites/Characters/OrkPM.png", "ork");
            SpriteManager.LoadImage("Images/Sprites/Characters/MrPlaceholder1.png", "MrPlaceholder");
            PlayerSprite unicorn1 = new PlayerSprite("unicorn", 1, select_pos1, select_size, false);
            chars1.Add(unicorn1);
            PlayerSprite ork1 = new PlayerSprite("ork", 1, select_pos1, select_size, false);
            chars1.Add(ork1);
            PlayerSprite MrPlaceholder1 = new PlayerSprite("MrPlaceholder", 1, select_pos1, select_size, false);
            chars1.Add(MrPlaceholder1);
            PlayerSprite unicorn2 = new PlayerSprite("unicorn", 2, select_pos2, select_size, false);
            chars2.Add(unicorn2);
            PlayerSprite ork2 = new PlayerSprite("ork", 2, select_pos2, select_size, false);
            chars2.Add(ork2);
            PlayerSprite MrPlaceholder2 = new PlayerSprite("MrPlaceholder", 2, select_pos2, select_size, false);
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
        public PlayerSprite SelectedChar(int player_number)
        {
            if (player_number == 1)
                return chars1[chars_number1];
            else
                return chars2[chars_number2];
        }
        public void CharacterLoad(int player_number)
        {
            Vector2 load_pos1 = new Vector2(4.5,2);
            Vector2 load_pos2 = new Vector2(11.5, 2);
            Vector2 load_size = new Vector2(1, 1);
            if (player_number ==1)
            {
                PlayerSprite chosen_char1 = SelectedChar(1);
                chosen_char1.Position = load_pos1;
                chosen_char1.Size = load_size;
                chosen_char1.IsActive = true;
            }
            else
            {
                PlayerSprite chosen_char1 = SelectedChar(1);
                chosen_char1.Position = load_pos1;
                chosen_char1.Size = load_size;
                chosen_char1.IsActive = true;
                PlayerSprite chosen_char2 = SelectedChar(2);
                chosen_char2.Size = load_size;
                chosen_char2.Position = load_pos2;
                chosen_char2.IsActive = true;
            }
        }
        public void UnLoadCharacter(int player_number)
        {
            SelectedChar(1).IsActive = false;
            SelectedChar(2).IsActive = false;
        }
    }
}
