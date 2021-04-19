using PewPew_Paradise.GameLogic;
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
        public static Vector2 select_pos1 = new Vector2(4, 9);
        public static Vector2 select_pos2 = new Vector2(12, 9);
        public static Vector2 select_pos_mid = new Vector2(8, 9);
        Vector2 select_size = new Vector2(3, 3);
        public int chars_number1;
        public int chars_number2;
        /// <summary>
        /// Loading and saving character sprites into a list
        /// </summary>
        public CharacterSelect()
        {

            //HP
            SpriteManager.LoadImage("Images/Sprites/Utility/hp_empty.png", "hp_empty");
            SpriteManager.LoadImage("Images/Sprites/Utility/hp_full.png", "hp_full");

            //Characters
            SpriteManager.LoadImage("Images/Sprites/Characters/Unicorn.png", "unicorn");
            SpriteManager.LoadImage("Images/Sprites/Characters/MrPlaceholder.png", "MrPlaceholder");
            SpriteManager.LoadImage("Images/Sprites/Characters/rat.png", "rat");
            SpriteManager.LoadImage("Images/Sprites/Characters/ork.png", "ork");
            SpriteManager.LoadImage("Images/Sprites/Characters/cactus.png", "cactus");
            SpriteManager.LoadImage("Images/Sprites/Characters/monkey.png", "monkey");
            SpriteManager.LoadImage("Images/Sprites/Characters/muffin.png", "muffin");
            SpriteManager.LoadImage("Images/Sprites/Characters/penguin.png", "penguin");
            SpriteManager.LoadImage("Images/Sprites/Characters/toast.png", "toast");
            SpriteManager.LoadImage("Images/Sprites/Characters/turtle.png", "turtle");
            SpriteManager.LoadImage("Images/Sprites/Characters/slime.png", "slime");
            SpriteManager.LoadImage("Images/Sprites/Characters/dino.png", "dino");
            SpriteManager.LoadImage("Images/Sprites/Characters/koala.png", "koala");

            //Projectiles
            SpriteManager.LoadImage("Images/Sprites/Projectiles/unicorn_projectile.png", "unicorn_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/OrkP.png", "ork_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/rat_projectile.png", "rat_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/MrPlaceholder_projectile.png", "MrPlaceholder_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/cactus_projectile.png", "cactus_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/monkey_projectile.png", "monkey_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/muffin_projectile.png", "muffin_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/penguin_projectile.png", "penguin_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/toast_projectile.png", "toast_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/turtle_projectile.png", "turtle_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/slime_projectile.png", "slime_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/dino_projectile.png", "dino_projectile");
            SpriteManager.LoadImage("Images/Sprites/Projectiles/koala_projectile.png", "koala_projectile");
            //Player1
            PlayerSprite unicorn1 = new PlayerSprite("unicorn", 1, "unicorn_projectile", select_pos1, select_size, false);
            chars1.Add(unicorn1);
            PlayerSprite ork1 = new PlayerSprite("ork", 1, "ork_projectile", select_pos1, select_size, false);
            chars1.Add(ork1);
            PlayerSprite MrPlaceholder11 = new PlayerSprite("rat", 1, "rat_projectile", select_pos1, select_size, false);
            chars1.Add(MrPlaceholder11);
            PlayerSprite MrPlaceholder1 = new PlayerSprite("MrPlaceholder", 1, "MrPlaceholder_projectile", select_pos1, select_size, false);
            chars1.Add(MrPlaceholder1);
            PlayerSprite cactus1 = new PlayerSprite("cactus", 1, "cactus_projectile", select_pos1, select_size, false);
            chars1.Add(cactus1);
            PlayerSprite monkey1 = new PlayerSprite("monkey", 1, "monkey_projectile", select_pos1, select_size, false);
            chars1.Add(monkey1);
            PlayerSprite muffin1 = new PlayerSprite("muffin", 1, "muffin_projectile", select_pos1, select_size, false);
            chars1.Add(muffin1);
            PlayerSprite penguin1 = new PlayerSprite("penguin", 1, "penguin_projectile", select_pos1, select_size, false);
            chars1.Add(penguin1);
            PlayerSprite toast1 = new PlayerSprite("toast", 1, "toast_projectile", select_pos1, select_size, false);
            chars1.Add(toast1);
            PlayerSprite turtle1 = new PlayerSprite("turtle", 1, "turtle_projectile", select_pos1, select_size, false);
            chars1.Add(turtle1);
            PlayerSprite slime1 = new PlayerSprite("slime", 1, "slime_projectile", select_pos1, select_size, false);
            chars1.Add(slime1);
            PlayerSprite dino1 = new PlayerSprite("dino", 1, "dino_projectile", select_pos1, select_size, false);
            chars1.Add(dino1);
            PlayerSprite koala1 = new PlayerSprite("koala", 1, "koala_projectile", select_pos1, select_size, false);
            chars1.Add(koala1);



            //Player2
            PlayerSprite unicorn2 = new PlayerSprite("unicorn", 2, "unicorn_projectile", select_pos2, select_size, false);
            chars2.Add(unicorn2);
            PlayerSprite ork2 = new PlayerSprite("ork", 2, "ork_projectile", select_pos2, select_size, false);
            chars2.Add(ork2);
            PlayerSprite MrPlaceholder22 = new PlayerSprite("rat", 2, "rat_projectile", select_pos2, select_size, false);
            chars2.Add(MrPlaceholder22);
            PlayerSprite MrPlaceholder2 = new PlayerSprite("MrPlaceholder", 2, "MrPlaceholder_projectile", select_pos2, select_size, false);
            chars2.Add(MrPlaceholder2);
            PlayerSprite cactus2 = new PlayerSprite("cactus", 2, "cactus_projectile", select_pos2, select_size, false);
            chars2.Add(cactus2);
            PlayerSprite monkey2 = new PlayerSprite("monkey", 2, "monkey_projectile", select_pos2, select_size, false);
            chars2.Add(monkey2);
            PlayerSprite muffin2 = new PlayerSprite("muffin", 2, "muffin_projectile", select_pos2, select_size, false);
            chars2.Add(muffin2);
            PlayerSprite penguin2 = new PlayerSprite("penguin", 2, "penguin_projectile", select_pos2, select_size, false);
            chars2.Add(penguin2);
            PlayerSprite toast2 = new PlayerSprite("toast", 2, "toast_projectile", select_pos2, select_size, false);
            chars2.Add(toast2);
            PlayerSprite turtle2 = new PlayerSprite("turtle", 2, "turtle_projectile", select_pos2, select_size, false);
            chars2.Add(turtle2);
            PlayerSprite slime2 = new PlayerSprite("slime", 2, "slime_projectile", select_pos2, select_size, false);
            chars2.Add(slime2);
            PlayerSprite dino2 = new PlayerSprite("dino", 2, "dino_projectile", select_pos2, select_size, false);
            chars2.Add(dino2);
            PlayerSprite koala2 = new PlayerSprite("koala", 2, "koala_projectile", select_pos2, select_size, false);
            chars2.Add(koala2);


        }
        /// <summary>
        /// Load Characters to Selection screen
        /// </summary>
        public void LoadChar(int player_number)
        {
            SelectedChar(1).Size = select_size;
            SelectedChar(1).Position = select_pos1;
            chars1[chars_number1].IsActive = true;
            MainWindow.Instance.lb_charname1.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number1);
            if (player_number != 1)
            {

                SelectedChar(2).Size = select_size;
                SelectedChar(1).Position = select_pos1;
                SelectedChar(2).Position = select_pos2;
                chars2[chars_number2].IsActive = true;
                MainWindow.Instance.lb_charname2.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number2);
            }
            else
            {
                SelectedChar(1).Position = select_pos_mid;
            }
        }
        /// <summary>
        /// Loading the next character to Selection screen and loading charcter names from the database
        /// player_number for 1 or 2 player -> same for all the incoming functions
        /// If singleplayer it loads it in the middle of the screen
        /// </summary>
        /// <param name="player_number"></param>
        public void NextChar(int player_number)
        {
            if (player_number == 1)
            {
                UnLoadChar(1);
                chars_number1++;
                chars_number1 = chars_number1 % chars1.Count;
                SelectedChar(1).Size = select_size;
                chars1[chars_number1].IsActive = true;
                MainWindow.Instance.lb_charname1.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number1);
            }
            else
            {
                UnLoadChar(2);
                chars_number2++;
                chars_number2 = chars_number2 % chars2.Count;
                SelectedChar(2).Size = select_size;
                chars2[chars_number2].IsActive = true;
                MainWindow.Instance.lb_charname2.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number2);
            }
            if (chars2[chars_number2].IsActive)
            {
                SelectedChar(1).Position = select_pos1;
            }
            else
            {
                SelectedChar(1).Position = select_pos_mid;
            }
        }
        /// <summary>
        /// Loading the previous character to Selection screen and loading charcter names from the database
        /// If singleplayer it loads it in the middle of the screen
        /// </summary>
        /// <param name="player_number"></param>
        public void PreChar(int player_number)
        {
            if (player_number == 1)
            {
                UnLoadChar(1);
                if (chars_number1 == 0)
                {
                    chars_number1 = chars1.Count-1;
                    chars_number1 = chars_number1 % chars1.Count;
                    SelectedChar(1).Size = select_size;
                    chars1[chars_number1].IsActive = true;
                    MainWindow.Instance.lb_charname1.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number1);
                }
                else
                {
                    chars_number1--;
                    chars_number1 = chars_number1 % chars1.Count;
                    SelectedChar(1).Size = select_size;
                    chars1[chars_number1].IsActive = true;
                    MainWindow.Instance.lb_charname1.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number1);
                }
            }
            else
            {
                UnLoadChar(2);
                if (chars_number2 == 0)
                {
                    chars_number2 = chars2.Count - 1;
                    //chars_number2 = chars_number2 % chars2.Count;
                    SelectedChar(2).Size = select_size;
                    chars2[chars_number2].IsActive = true;
                    MainWindow.Instance.lb_charname2.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number2);
                }
                else
                {
                    chars_number2--;
                    chars_number2 = chars_number2 % chars2.Count;
                    SelectedChar(2).Size = select_size;
                    chars2[chars_number2].IsActive = true;
                    MainWindow.Instance.lb_charname2.Content = MainWindow.Instance.scoreManager.GetCharName(chars_number2);
                }
            }
            if (chars2[chars_number2].IsActive)
            {
                SelectedChar(1).Position = select_pos1;
            }
            else
            {
                SelectedChar(1).Position = select_pos_mid;
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
        /// Returns which sprite was active when Selection screen was closed
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
        /// Load characters to maps and give them components
        /// Makes a character to not load if its life is 0
        /// </summary>
        /// <param name="player_number"></param>
        public void CharacterLoad(int player_number)
        {
            Vector2 load_pos1 = new Vector2(4.5,2);
            Vector2 load_pos2 = new Vector2(11.5, 2);
            Vector2 load_size = new Vector2(1, 1);

            PlayerSprite chosen_char1 = SelectedChar(1);

            chosen_char1.Position = load_pos1;
            chosen_char1.Size = load_size;
            if (MainWindow.Instance.characterSelector.SelectedChar(1).Life != 0)
            chosen_char1.IsActive = true;
            SelectedChar(1).GetComponent<PhysicsComponent>().IsActive = true;
            SelectedChar(1).GetComponent<CollideComponent>().IsActive = true;
            if (player_number!=1)
            {
                PlayerSprite chosen_char2 = SelectedChar(2);

                chosen_char2.Size = load_size;
                chosen_char2.Position = load_pos2;
                if (MainWindow.Instance.characterSelector.SelectedChar(2).Life != 0)
                    chosen_char2.IsActive = true;
                SelectedChar(2).GetComponent<PhysicsComponent>().IsActive = true;
                SelectedChar(2).GetComponent<CollideComponent>().IsActive = true;
            }
        }
        /// <summary>
        /// Unload both player sprite in maps and deactive components
        /// </summary>
        /// <param name="player_number"></param>
        public void UnLoadCharacter(int player_number)
        {
            SelectedChar(player_number).IsActive = false;
            SelectedChar(player_number).GetComponent<PhysicsComponent>().IsActive = false;
            SelectedChar(player_number).GetComponent<CollideComponent>().IsActive = false;

        }
        /// <summary>
        /// Returns a character current position
        /// </summary>
        /// <param name="player_number"></param>
        /// <returns></returns>
        public Vector2 PlayerCurrentPosition(int player_number)
        {

                PlayerSprite chosen_char = SelectedChar(player_number);
                return chosen_char.Position;

        }


    }
}
