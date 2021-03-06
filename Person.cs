﻿using System;

/*
 * Person.cs
 * Any game characters : Enemies, Player, NPCs.
 */

//TODO: Do a player-follow camera system.
/*
 * Player-follow Camera RFC:
 * 
 * Specifications:
 * - X and Y of the player depends on the map, not the screen.
 * - Screen refreshes on each turn.
 * 
 * Pros:
 * - Allows much bigger maps with more variety.
 * - Allows a much bigger viewport (flexible screen size).
 * Cons:
 * - Have to redraw the map every turn.
 * - Uses more memory for the map itself.
 * - Uses more processing power for centering calculations and the such.
 */

namespace fwod
{
    public enum EnemyType
    {
        Rat, 
    }

    public enum EnemyModifier : byte
    {
        Weak, Strong
    }

    static class EnemyTypeHelper
    {
        public static float GetHPModifier(this EnemyType t)
        {
            switch (t)
            {
                // Common enemies
                default: return 1.8f;
            }
        }

        public static float GetMoneyModifier(this EnemyType t)
        {
            switch (t)
            {
                default: return 0.2f;
            }
        }
    }

    #region Person
    class Person
    {
        #region Constants
        const int BUBBLE_PADDING_HORIZONTAL = 0;
        const int BUBBLE_TEXT_MAXLEN = 25;
        #endregion

        #region Properties
        #region Position
        int _x;
        /// <summary>
        /// Set or get the Player position (Left).
        /// </summary>
        public int X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    char mc = MapManager.Map[_y, value];
                    if (mc == '\0' || mc == ' ')
                    {
                        if (Game.People.IsSomeoneAt(value, _y))
                        {
                            Person p =
                                Game.People.GetPersonAt(value, _y);

                            if (p != this && p is Enemy)
                            {
                                Attack(p);
                            }
                        }
                        else
                        {
                            Move(_x, _y, value, _y);
                        }
                    }
                }
            }
        }

        int _y;
        /// <summary>
        /// Set or get the Player position (Top).
        /// </summary>
        public int Y
        {
            get { return _y; }
            set
            {
                if (value != _y)
                {
                    char mc = MapManager.Map[value, _x];
                    if (mc == '\0' || mc == ' ')
                    {
                        if (Game.People.IsSomeoneAt(_x, value))
                        {
                            Person p =
                                Game.People.GetPersonAt(_x, value);

                            if (p != this && p is Enemy)
                            {
                                Attack(p);
                            }
                        }
                        else
                        {
                            Move(_x, _y, _x, value);
                        }
                    }
                }
            }
        }

        void Move(int pastX, int pastY, int newX, int newY)
        {
            // Get old char
            char pastchar = MapManager.Map[pastY, pastX];

            // Place old char
            Console.SetCursorPosition(pastX, pastY);
            Console.Write(pastchar == '\0' ? ' ' : pastchar);

            // Move player
            Console.SetCursorPosition(_x = newX, _y = newY);
            Console.Write(Char);

            if (this is Player)
                Game.Statistics.StepsTaken++;
        }
        #endregion

        #region Health
        int _hp;
        /// <summary>
        /// Get or set the HP.
        /// </summary>
        public int HP
        {
            get { return _hp; }
            set
            {
                if (value > _maxhp)
                    _hp = _maxhp;
                else
                {
                    Game.Statistics.DamageReceived += (uint)(_hp - value);

                    _hp = value;

                    if (value <= 0)
                        Destroy();
                }

                if (this is Player)
                {
                    Console.SetCursorPosition(29, 0);
                    Console.Write(new string(' ', 13));
                    Console.SetCursorPosition(29, 0); //HP: 0000/0000
                    Console.Write($"HP: {_hp:D4}/{_maxhp:D4}");
                }

            }
        }

        int _maxhp;
        public int MaxHP
        {
            get { return _maxhp; }
            set
            {
                if (value < _hp)
                    _maxhp = _hp;
                else
                    _maxhp = value;
            }
        }
        #endregion

        #region Name, appearance
        string _name;
        /// <summary>
        /// Get or set the name of the character.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                if (this is Player)
                {
                    // Clear name and redraw it (in case of shorter name)
                    Console.SetCursorPosition(1, 0);
                    Console.Write(new string(' ', 25));
                    Console.SetCursorPosition(1, 0);
                    Console.Write(_name);
                }
            }
        }

        /// <summary>
        /// Get or set the character displayed on screen.
        /// </summary>
        public char Char { get; set; }
        #endregion

        #region Abilities
        AbilityManager Abilities;
        #endregion

        #region Money
        int _money;
        /// <summary>
        /// Current sum of money in this Person.
        /// The sum is display via the inventory for Player.
        /// </summary>
        public int Money
        {
            get { return _money; }
            set
            {
                if (value <= 1000000) // 1'000'000
                    _money = value;
                
                if (this is Player)
                {
                    Console.SetCursorPosition(45, 0);
                    Console.Write(new string(' ', 8));
                    Console.SetCursorPosition(45, 0); //0'000'000$
                    Console.Write($"{_money:0'000'000}$");
                }
            }
        }
        #endregion

        #region Inventory
        public InventoryManager Inventory { get; }
        #endregion
        #endregion

        #region Construction
        public Person(int x, int y,
            short hp = 10, char c = 'P', bool init = true)
        {
            _maxhp = _hp = hp;
            _x = x;
            _y = y;
            Char = c;
            //TODO: Left and right weapon?
            Inventory = new InventoryManager();
            Abilities = new AbilityManager();

            //Game.PeopleList[Game.CurrentFloor].Add(this);

            if (init)
                Initialize();
        }
        #endregion

        #region Init
        /// <summary>
        /// Place the Person on screen.
        /// </summary>
        public void Initialize()
        {
            Console.SetCursorPosition(_x, _y);
            Console.Write(Char);
        }
        #endregion

        #region Bubble
        /// <summary>
        /// Generates a bubble for this Person.
        /// </summary>
        /// <param name="x">Top starting position</param>
        /// <param name="y">Left starting position</param>
        /// <param name="width">Bubble width</param>
        /// <param name="height">Bubble height</param>
        void GenerateBubble(int x, int y, int width, int height)
        {
            Utils.GenerateBox(x, y, width, height);

            // Bubble chat "connector"
            if (y < _y) // Over Person
            {
                Console.SetCursorPosition(_x, _y - 1);
                Console.Write('┬');
            }
            else // Under Person
            {
                Console.SetCursorPosition(_x, _y + 1);
                Console.Write('┴');
            }
        }

        /// <summary>
        /// Clear the past bubble and reprint chars from game layer.
        /// </summary>
        /// <param name="length">Length of the string</param>
        void ClearBubble(int length)
        {
            MapManager.RedrawMap(
                X - (length / 2) - (BUBBLE_PADDING_HORIZONTAL * 2) - 1,
                Y - (length / BUBBLE_TEXT_MAXLEN) - 3,
                length + (BUBBLE_PADDING_HORIZONTAL * 2) + 2,
                (length / (BUBBLE_TEXT_MAXLEN + 1)) + 4
            );
        }

        /// <summary>
        /// Clear the past bubble and reprint chars from game layer.
        /// </summary>
        /// <param name="x">Top starting position</param>
        /// <param name="y">Left starting position</param>
        /// <param name="width">Bubble width</param>
        /// <param name="height">Bubble height</param>
        unsafe void ClearBubble(int x, int y, int width, int height,
            bool map = true)
        {
            if (map)
                MapManager.RedrawMap(x, y, width, height);
            else
            {
                int yl = y + height;
                string l = new string(' ', width);
                for (; y < yl; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(l);
                }
            }
        }
        #endregion

        #region Conversation
        /// <summary>
        /// Makes the Person talk.
        /// </summary>
        /// <param name="text">Dialog</param>
        /// <param name="wait">Wait for keydown</param>
        /// <param name="map">Redraw map.</param>
        public void Say(string text,
            bool wait = true, bool map = true)
        {
            string[] lines = new string[] { text };

            if (text.Length > BUBBLE_TEXT_MAXLEN)
            {
                int ci = 0; // Multiline scenario row index
                int start = 0; // Multiline cutting index
                lines = new string[(text.Length / (BUBBLE_TEXT_MAXLEN + 1)) + 1];

                // This block seperates the input into BUBBLE_MAXLEN characters each lines equally.
                do
                {
                    if (start + BUBBLE_TEXT_MAXLEN > text.Length)
                        lines[ci] = text.Substring(start, text.Length - start);
                    else
                        lines[ci] = text.Substring(start, BUBBLE_TEXT_MAXLEN);
                    ci++;
                    start += BUBBLE_TEXT_MAXLEN;
                } while (start < text.Length);
            }
            
            Say(lines, wait, map);
        }

        /// <summary>
        /// Makes the Person say a few lines.
        /// </summary>
        /// <param name="lines">Lines of dialog</param>
        /// <param name="wait">Wait for keydown</param>
        /// <param name="map">Redraw map.</param>
        public void Say(string[] lines,
            bool wait = true, bool map = true)
        {
            int arrlen = lines.Length;
            int strlen = arrlen > 1 ?
                lines.GetLonguestStringLength() : lines[0].Length;
            int width = strlen + (BUBBLE_PADDING_HORIZONTAL * 2) + 2;
            int height = arrlen + 2;
            int startX = X - (width / 2);
            int startY = Y - (height);

            // Re-locate startX if it goes further than the display buffer
            if (startX + width > Utils.WindowWidth)
                startX = Utils.WindowWidth - width;
            else if (startX < 0)
                startX = 0;

            // Re-locate startY if it goes further than the display buffer
            if (startY > Utils.WindowWidth)
                startY = Utils.WindowWidth - (width - 2);
            else if (startY < 3)
                startY = 3;

            // Generate the bubble
            GenerateBubble(startX, startY, width, height);

            int textStartX = startX + 1;
            int textStartY = startY + 1;

            // Insert Text
            for (int i = 0; i < arrlen; i++)
            {
                Console.SetCursorPosition(textStartX, textStartY + i);
                Console.Write(lines[i]);
            }

            // Fill the rest of the bubble
            if (lines.Length > 1)
                Console.Write(
                    new string(' ', strlen - lines[arrlen - 1].Length)
                );

            if (wait)
            {
                Console.ReadKey(true);
                ClearBubble(startX, startY, width, height, map);
            }
            else
            {
                // Prepare for text
                Console.SetCursorPosition(textStartX, textStartY);
            }
        }

        /// <summary>
        /// Get input from the Person.
        /// </summary>
        /// <returns>Answer</returns>
        public string GetAnswer()
        {
            return GetAnswer(BUBBLE_TEXT_MAXLEN);
        }

        /// <summary>
        /// Get input from the Person.
        /// </summary>
        /// <param name="limit">Limit in characters.</param>
        /// <returns>Answer</returns>
        public string GetAnswer(int limit = 25)
        {
            Say(new string(' ', limit), false);

            // Read input from this Person
            string t = Utils.ReadLine(limit);

            // Clear bubble
            ClearBubble(limit);

            return t;
        }
        #endregion

        #region Movement
        /// <summary>
        /// Makes the enemy move up one square
        /// </summary>
        public void MoveUp()
        {
            Y--;
        }

        /// <summary>
        /// Makes the enemy move down one square
        /// </summary>
        public void MoveDown()
        {
            Y++;
        }

        /// <summary>
        /// Makes the enemy move left one square
        /// </summary>
        public void MoveLeft()
        {
            X--;
        }

        /// <summary>
        /// Makes the enemy move right one square
        /// </summary>
        public void MoveRight()
        {
            X++;
        }
        #endregion

        #region Attack
        public void Attack(Person person)
        {
            int ap = 1, def = 1;

            if (Inventory.HasWeapon)
            {
                int wd = Inventory.EquippedWeapon.Damage;

                if (Inventory.EquippedWeapon.IsRanged)
                {
                    ap = 2; // Ranged gun's butt/stock.
                    /*Utils.Random.NextDouble() * 10 >= Abilities.Dexterity ?
                    wd : 0;*/
                }
                else
                {
                    ap =
                        ((int)((Abilities.Strength * wd) * 0.2f) + wd) - (def);

                }
            }

            if (Inventory.HasArmor)
            {
                def = Inventory.EquippedArmor.ArmorPoints;
            }

            string s = $": {person.HP} HP - {ap} = {person.HP -= ap} HP ";

            int xp = person.MaxHP * 2;
            Abilities.Experience += xp;

            if (person.HP <= 0) // If killed
                s += $" | +{person.Money}$ | +{xp} XP";

            Game.Statistics.DamageDealt += ap;

            if (person is Enemy)
                Game.Message((person as Enemy).Race + s);
            else
                Game.Message(person.GetType() + s);
        }
        #endregion

        #region Destroy
        /// <summary>
        /// Remove completely the Person from the game.
        /// </summary>
        public void Destroy()
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(' ');

            Game.MainPlayer.Money += Money;
            Game.Statistics.MoneyGained += (uint)Money;
            Game.People[Game.CurrentFloor].Remove(this);

            if (this is Enemy)
            {
                Game.Statistics.EnemiesKilled++;
            }
            else if (this is Player)
            { //TODO: Game over

            }
        }
        #endregion
    }
    #endregion

    #region Player
    class Player : Person
    {
        public Player()
            : this(Utils.WindowWidth / 2, Utils.WindowHeight / 2) {}

        public Player(int X, int Y)
            : base(X, Y, 10, '@')
        {
            Name = null;
        }
    }
    #endregion

    #region Enemy
    class Enemy : Person
    {
        public Enemy(int x, int y, EnemyType type, int level, bool initialize = true)
            : base(x, y, (short)(level * type.GetHPModifier()), '&')
        {
            Money = (int)(HP * 0.5);
        }
        
        public EnemyType Race { get; }
    }
    #endregion
}