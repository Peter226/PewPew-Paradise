﻿using PewPew_Paradise.GameLogic;
using PewPew_Paradise.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew_Paradise.GameLogic.SpriteComponents;

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
        /// <summary>
        /// Loading and saving character sprites into a list
        /// </summary>
        public CharacterSelect()
        {
            SpriteManager.LoadImage("Images/Sprites/Characters/Unicorn.png", "unicorn");
            SpriteManager.LoadImage("Images/Sprites/Characters/OrkPM.png", "ork");
            SpriteManager.LoadImage("Images/Sprites/Characters/MrPlaceholder1.png", "MrPlaceholder");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/unicorn_projectile.png", "unicorn_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/OrkP.png", "ork_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/MrPlaceholder_projectile.png", "MrPlaceholder_projectile");
            PlayerSprite unicorn1 = new PlayerSprite("unicorn", 1, "unicorn_projectile", select_pos1, select_size, false);
            chars1.Add(unicorn1);
            PlayerSprite ork1 = new PlayerSprite("ork", 1, "ork_projectile", select_pos1, select_size, false);
            chars1.Add(ork1);
            PlayerSprite MrPlaceholder1 = new PlayerSprite("MrPlaceholder", 1, "MrPlaceholder_projectile", select_pos1, select_size, false);
            chars1.Add(MrPlaceholder1);
            PlayerSprite unicorn2 = new PlayerSprite("unicorn", 2, "unicorn_projectile", select_pos2, select_size, false);
            chars2.Add(unicorn2);
            PlayerSprite ork2 = new PlayerSprite("ork", 2, "ork_projectile", select_pos2, select_size, false);
            chars2.Add(ork2);
            PlayerSprite MrPlaceholder2 = new PlayerSprite("MrPlaceholder", 2, "MrPlaceholder_projectile", select_pos2, select_size, false);
            chars2.Add(MrPlaceholder2);

        }
        /// <summary>
        /// Load Characters to options at first time
        /// </summary>
        public void LoadChar() 
        {
            chars_number1 = 0;
            chars1[chars_number1].IsActive = true;
            chars_number2 = 0;
            chars2[chars_number2].IsActive = true;
        }
        /// <summary>
        /// Loading the next char in the list
        /// player_number for player1 or 2 -> same for all the incoming functions
        /// </summary>
        /// <param name="player_number"></param>
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
        /// <summary>
        /// Loading the prev char in the list
        /// </summary>
        /// <param name="player_number"></param>
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
        /// <summary>
        /// Unload characters
        /// </summary>
        /// <param name="player_number"></param>
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
        /// <summary>
        /// Returns which sprite was active when options was closed
        /// </summary>
        /// <param name="player_number"></param>
        /// <returns></returns>
        public PlayerSprite SelectedChar(int player_number)
        {
            if (player_number == 1)
                return chars1[chars_number1];
            else
                return chars2[chars_number2];
        }
        /// <summary>
        /// Load characters to maps
        /// </summary>
        /// <param name="player_number"></param>
        public void CharacterLoad(int player_number)
        {
            Vector2 load_pos1 = new Vector2(4.5,2);
            Vector2 load_pos2 = new Vector2(11.5, 2);
            Vector2 load_size = new Vector2(1, 1);

            PlayerSprite chosen_char1 = SelectedChar(1);
            SelectedChar(1).GetComponent<PhysicsComponent>().IsActive = true;
            SelectedChar(1).GetComponent<CollideComponent>().IsActive = true;
            chosen_char1.Position = load_pos1;
            chosen_char1.Size = load_size;
            chosen_char1.IsActive = true;

            if(player_number!=1)
            {
                PlayerSprite chosen_char2 = SelectedChar(2);
                SelectedChar(2).GetComponent<PhysicsComponent>().IsActive = true;
                SelectedChar(2).GetComponent<CollideComponent>().IsActive = true;
                chosen_char2.Size = load_size;
                chosen_char2.Position = load_pos2;
                chosen_char2.IsActive = true;
            }
        }
        /// <summary>
        /// Unload both player sprite in maps
        /// </summary>
        /// <param name="player_number"></param>
        public void UnLoadCharacter(int player_number)
        {
            SelectedChar(player_number).IsActive = false;
            SelectedChar(player_number).GetComponent<PhysicsComponent>().IsActive = false;
            SelectedChar(player_number).GetComponent<CollideComponent>().IsActive = false;

        }
        public Vector2 PlayerCurrentPosition(int player_number)
        {

                PlayerSprite chosen_char = SelectedChar(player_number);
                return chosen_char.Position;

        }

       
    }
}
