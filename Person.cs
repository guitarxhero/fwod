﻿using System;
using System.Collections.Generic;

/*
    A Person.
    Can be a Player, Enemy, etc.
*/

namespace fwod
{
    #region Person
    internal class Person
    {
        #region Constants
        const int BUBBLE_PADDING_X = 0;
        const int BUBBLE_TEXTMAXLEN = 25;
        #endregion

        #region Object properties
        #region Position
        int _posx;
        /// <summary>
        /// Sets or gets the Player position (Left).
        /// </summary>
        internal int PosX
        {
            get { return _posx; }
            set
            {
                if (value != _posx)
                {
                    char futrG = Core.GetCharAt(Core.Layer.Game, value, PosY);
                    char futrP = Core.GetCharAt(Core.Layer.People, value, PosY);
                    bool futrIsSolid = futrG.IsSolidObject();
                    bool futrIsSomeone = futrP.IsPersonObject();
                    if (!futrIsSolid)
                    {
                        if (futrIsSomeone)
                        {
                            Person futrPerson = Game.GetPersonObjectAt(value, PosY);

                            if (futrPerson != this && futrPerson is Enemy)
                            {
                                Attack(futrPerson);
                            }
                        }
                        else
                        {
                            Move(PosX, PosY, value, PosY);
                        }
                    }
                }
            }
        }

        int _posy;
        /// <summary>
        /// Sets or gets the Player position (Top).
        /// </summary>
        internal int PosY
        {
            get { return _posy; }
            set
            {
                if (value != _posy)
                {
                    char futrG = Core.GetCharAt(Core.Layer.Game, PosX, value);
                    char futrP = Core.GetCharAt(Core.Layer.People, PosX, value);
                    bool futrIsSolid = futrG.IsSolidObject();
                    bool futrIsEnemy = futrP.IsPersonObject();
                    if (!futrIsSolid)
                    {
                        if (futrIsEnemy)
                        {
                            Person futrPerson = Game.GetPersonObjectAt(PosX, value);

                            if (futrPerson != this && futrPerson is Enemy)
                            {
                                Attack(futrPerson);
                            }
                        }
                        else
                        {
                            Move(PosX, PosY, PosX, value);
                        }
                    }
                }
            }
        }

        void Move(int pastX, int pastY, int newX, int newY)
        {
            // Get old char
            char pastchar = Core.GetCharAt(Core.Layer.Game, pastX, pastY);

            // Place old char
            Console.SetCursorPosition(pastX, pastY);
            Console.Write(pastchar == '\0' ? ' ' : pastchar);

            // Update values
            _posy = newY;
            _posx = newX;

            // Move player
            Core.Write(Core.Layer.People, CharacterChar, newX, newY);

            if (this is Player)
                Game.StatStepsTaken++;
        }
        #endregion

        #region Health
        int _hp;
        /// <summary>
        /// Gets or sets the HP.
        /// </summary>
        internal int HP
        {
            get { return _hp; }
            set
            {
                //TODO: Check if correct algorithm
                if (this is Player)
                    Game.StatDamageReceived += value > _hp ? (uint)(_hp - value) : 0;

                if (value > _maxhp)
                    _hp = _maxhp;
                else
                    _hp = value;

                if (this is Player)
                {
                    Console.SetCursorPosition(29, 0);
                    Console.Write(new string(' ', 11));
                    Console.SetCursorPosition(29, 0); //HP: 000/000
                    Console.Write(string.Format("HP: {0:000}/{1:000}", new object[] { _hp, _maxhp }));
                }

                if (_hp <= 0)
                    Destroy();
            }
        }

        int _maxhp;
        internal int MaxHP
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
        string _characterName;
        /// <summary>
        /// Gets or sets the name of the character.
        /// </summary>
        internal string CharacterName
        {
            get { return _characterName; }
            set
            {
                _characterName = value;

                if (this is Player)
                {
                    // Clear name and redraw it (in case of shorter name)
                    Console.SetCursorPosition(1, 0);
                    Console.Write(new string(' ', 25));
                    Console.SetCursorPosition(1, 0);
                    Console.Write(_characterName);
                }
            }
        }

        /// <summary>
        /// The Player's literal character.
        /// </summary>
        internal char CharacterChar;
        #endregion

        #region Stats
        int _s1;
        /// <summary>
        /// Strength
        /// </summary>
        internal int S1
        {
            get { return _s1 ; }
            set
            {
                if (value > 10)
                    _s1 = 10;
                else if (value >= 0)
                    _s1 = value;
            }
        }

        int _s2;
        /// <summary>
        /// Dexterity
        /// </summary>
        internal int S2
        {
            get { return _s2; }
            set
            {
                if (value > 10)
                    _s2 = 10;
                else if (value >= 0)
                    _s2 = value;
            }
        }

        int _s3;
        /// <summary>
        /// Agility
        /// </summary>
        internal int S3
        {
            get { return _s3; }
            set
            {
                if (value > 10)
                    _s3 = 10;
                else if (value >= 0)
                    _s3 = value;
            }
        }

        int _s4;
        /// <summary>
        /// 
        /// </summary>
        internal int S4
        {
            get { return _s4; }
            set
            {
                if (value > 10)
                    _s4 = 10;
                else if (value >= 0)
                    _s4 = value;
            }
            
        }

        int _s5;
        /// <summary>
        /// 
        /// </summary>
        internal int S5
        {
            get { return _s5; }
            set
            {
                if (value > 10)
                    _s5 = 10;
                else if (value >= 0)
                    _s5 = value;
            }
        }

        int _s6;
        /// <summary>
        /// 
        /// </summary>
        internal int S6
        {
            get { return _s6; }
            set
            {
                if (value > 10)
                    _s6 = 10;
                else if (value >= 0)
                    _s6 = value;
            }
        }

        int _s7;
        /// <summary>
        /// 
        /// </summary>
        internal int S7
        {
            get { return _s7; }
            set
            {
                if (value > 10)
                    _s7 = 10;
                else if (value >= 0)
                    _s7 = value;
            }
        }
        #endregion

        #region Weapon
        /// <summary>
        /// Current EquipedWeapon in this Person.
        /// </summary>
        internal Weapon EquipedWeapon
        {
            get; set;
        }
        #endregion

        #region Armor
        /// <summary>
        /// Current EquipedArmor in this Person.
        /// </summary>
        internal Armor EquipedArmor
        {
            get; set;
        }
        #endregion

        #region Money
        int _money;
        /// <summary>
        /// Current sum of money in this Person.
        /// The sum is display via the inventory for Player.
        /// </summary>
        internal int Money
        {
            get { return _money; }
            set
            {
                if (value < 1000000)
                    _money = value;

                if (this is Player)
                {
                    Console.SetCursorPosition(43, 0);
                    Console.Write(new string(' ', 8));
                    Console.SetCursorPosition(43, 0); //0'000'000$
                    Console.Write(string.Format("{0:0000000}$", _money));
                }
            }
        }
        #endregion

        #region Inventory
        List<Item> Inventory = new List<Item>();
        #endregion
        #endregion

        #region Construction
        internal Person()
            : this(ConsoleTools.BufferWidth / 2, ConsoleTools.BufferHeight / 2)
        {

        }

        internal Person(int pX, int pY)
            : this(pX, pY, 10)
        {

        }

        internal Person(int pX, int pY, int pHP)
        {
            _hp = pHP;
            _maxhp = _hp;
            _money = (int)(pHP * 0.5);
            _posx = pX;
            _posy = pY;
            _s1 = 1;
            _s2 = 1;
            _s3 = 1;
            _s4 = 1;
            _s5 = 1;
            _s6 = 1;
            _s7 = 1;
            CharacterChar = 'P';
            EquipedWeapon = new Weapon("None", 0);
            EquipedArmor = new Armor("None", 0);
        }
        #endregion

        #region Init
        /// <summary>
        /// Places the Person on screen.
        /// </summary>
        internal void Initialize()
        {
            Core.Write(Core.Layer.People, CharacterChar, _posx, _posy);
        }
        #endregion

        #region Bubble
        /// <summary>
        /// Generates a bubble for this Person.
        /// </summary>
        /// <param name="pStartX">Top starting position</param>
        /// <param name="pStartY">Left starting position</param>
        /// <param name="pWidth">Bubble width</param>
        /// <param name="pHeight">Bubble height</param>
        void GenerateBubble(int pStartX, int pStartY, int pWidth, int pHeight)
        {
            Game.GenerateBox(Core.Layer.None,
                Game.TypeOfLine.Single,
                pStartX, pStartY, pWidth, pHeight);

            // Bubble chat "connector"
            if (pStartY < PosY) // Over Person
            {
                Console.SetCursorPosition(PosX, PosY - 2);
                Console.Write(Game.Graphics.Lines.SingleConnector[2]);
            }
            else // Under Person
            {
                Console.SetCursorPosition(PosX, PosY + 2);
                Console.Write(Game.Graphics.Lines.SingleConnector[1]);
            }
        }

        /// <summary>
        /// Clear the past bubble and reprint chars from game layer.
        /// Assumes it's over-the-head (for now)
        /// TODO: Fix assuming
        /// </summary>
        /// <param name="pLength">Length of the string</param>
        void ClearBubble(int pLength)
        {
            int height = (pLength / (BUBBLE_TEXTMAXLEN + 1)) + 3;
            int width = pLength + (BUBBLE_PADDING_X * 2) + 2;
            int startx = PosX - ((pLength / 2) + BUBBLE_PADDING_X + 1);
            int starty = PosY - (pLength / BUBBLE_TEXTMAXLEN) - 3;
            ClearBubble(startx, starty, width, height);
        }

        /// <summary>
        /// Clear the past bubble and reprint chars from game layer.
        /// </summary>
        /// <param name="pStartX">Top starting position</param>
        /// <param name="pStartY">Left starting position</param>
        /// <param name="pWidth">Bubble width</param>
        /// <param name="pHeight">Bubble height</param>
        void ClearBubble(int pStartX, int pStartY, int pWidth, int pHeight)
        {
            char c;
            int colmax = pWidth + pStartX;
            int rowmax = pHeight + pStartY;
            for (int row = pStartY; row < rowmax; row++)
            {
                for (int col = pStartX; col < colmax; col++)
                {
                    Console.SetCursorPosition(col, row);
                    c = Core.GetCharAt(Core.Layer.Game, col, row);
                    Console.Write(c == '\0' ? ' ' : c);
                }
            }
        }
        #endregion

        #region Conversation
        /// <summary>
        /// Makes the character talk.
        /// </summary>
        /// <param name="pText">Dialog</param>
        internal void Say(string pText)
        {
            Say(pText, true);
        }

        /// <summary>
        /// Makes the Person talk.
        /// </summary>
        /// <param name="pText">Dialog</param>
        /// <param name="pWait">Wait for keydown</param>
        internal void Say(string pText, bool pWait)
        {
            string[] Lines = new string[] { pText };

            if (pText.Length > BUBBLE_TEXTMAXLEN)
            {
                int ci = 0; // Multiline scenario row index
                int start = 0; // Multiline cutting index
                Lines = new string[(pText.Length / (BUBBLE_TEXTMAXLEN + 1)) + 1];

                // This block seperates the input into BUBBLE_MAXLEN characters each lines equally.
                do
                {
                    if (start + BUBBLE_TEXTMAXLEN > pText.Length)
                        Lines[ci] = pText.Substring(start, pText.Length - start);
                    else
                        Lines[ci] = pText.Substring(start, BUBBLE_TEXTMAXLEN);
                    ci++;
                    start += BUBBLE_TEXTMAXLEN;
                } while (start < pText.Length);
            }
            
            Say(Lines, pWait);
        }

        /// <summary>
        /// Makes the Person say a few lines.
        /// </summary>
        /// <param name="pLines">Lines of dialog</param>
        /// <param name="pWait">Wait for keydown</param>
        internal void Say(string[] pLines, bool pWait)
        {
            int arrlen = pLines.Length;
            int strlen = arrlen > 1 ?
                ConsoleTools.GetLonguestString(pLines) : pLines[0].Length;
            int width = strlen + (BUBBLE_PADDING_X * 2) + 2;
            int height = arrlen + 2;
            int startX = PosX - (strlen / 2) - 1;
            int startY = PosY - (arrlen) - 3;

            // Re-places StartX if it goes further than the display buffer
            if (startX + width > ConsoleTools.BufferWidth)
                startX = ConsoleTools.BufferWidth - width;
            else if (startX < 0)
                startX = 0;

            // Re-places StartY if it goes further than the display buffer
            if (startY > ConsoleTools.BufferWidth)
                startY = ConsoleTools.BufferWidth - (arrlen - 2);
            else if (startY < 3)
                startY = 3;

            // Generate the bubble
            GenerateBubble(startX, startY, width, height);

            int TextStartX = startX + 1;
            int TextStartY = startY + 1;

            // Insert Text
            for (int i = 0; i < arrlen; i++)
            {
                Console.SetCursorPosition(TextStartX, TextStartY + i);
                Console.Write(pLines[i]);
            }

            if (pWait)
            {
                Console.ReadKey(true);
                ClearBubble(startX, startY, width, height);
            }
            else
            {
                // Prepare for text
                Console.SetCursorPosition(TextStartX, TextStartY);
            }
        }

        /// <summary>
        /// Get input from the Person.
        /// </summary>
        /// <returns>Answer</returns>
        internal string GetAnswer()
        {
            return GetAnswer(BUBBLE_TEXTMAXLEN);
        }

        /// <summary>
        /// Get input from the Person.
        /// </summary>
        /// <param name="pLimit">Limit in characters.</param>
        /// <returns>Answer</returns>
        internal string GetAnswer(int pLimit)
        {
            Say(new string(' ', pLimit), false);

            // Read input from this Person
            string Out = ConsoleTools.ReadLine(pLimit);

            // Clear bubble
            ClearBubble(pLimit);

            return Out;
        }
        #endregion

        #region Movement
        /// <summary>
        /// Makes the enemy move up one square
        /// </summary>
        internal void MoveUp()
        {
            PosY--;
        }

        /// <summary>
        /// Makes the enemy move down one square
        /// </summary>
        internal void MoveDown()
        {
            PosY++;
        }

        /// <summary>
        /// Makes the enemy move left one square
        /// </summary>
        internal void MoveLeft()
        {
            PosX--;
        }

        /// <summary>
        /// Makes the enemy move right one square
        /// </summary>
        internal void MoveRight()
        {
            PosX++;
        }
        #endregion

        #region Attack
        internal void Attack(Person pPerson)
        {
            //TODO: Attack algorithm
            int AttackPoints = (int)((S1 + EquipedWeapon.BaseDamage) - EquipedArmor.ArmorPoints);

            pPerson.HP -= AttackPoints;

            string atk = " -> " + AttackPoints + " = " + pPerson.HP + "!" +
                (pPerson.HP <= 0 ? " +" + pPerson.Money + "$ *DEAD*" : string.Empty);

            if (pPerson.HP <= 0)
            {
                Game.StatMoneyGained += (uint)pPerson.Money;
            }

            Game.StatDamageDealt += (uint)AttackPoints;

            if (pPerson is Enemy)
                Game.DisplayEvent(((Enemy)pPerson).eType + atk);
            else
                Game.DisplayEvent(pPerson.GetType() + atk);
        }
        #endregion

        #region Destroy
        /// <summary>
        /// Remove completely the Person from game.
        /// </summary>
        internal void Destroy()
        {
            Console.SetCursorPosition(PosX, PosY);
            Console.Write(' ');
            if (this is Enemy)
            {
                Game.MainPlayer.Money += this.Money;
                Game.EnemyList.Remove((Enemy)this);
                Game.StatEnemiesKilled++;
            }
            else if (this is Player)
            { // Game over
                //TODO: Game over

            }
            else
            {
                Game.PeopleList.Remove(this);
            }
        }
        #endregion
    }
    #endregion

    #region Player
    class Player : Person
    {
        internal Player()
            : this(ConsoleTools.BufferWidth / 2, ConsoleTools.BufferHeight / 2)
        {

        }

        internal Player(int X, int Y)
            : base(X, Y)
        {
            CharacterChar = '@';
        }

        
    }
    #endregion

    #region Enemy
    class Enemy : Person
    {
        internal Enemy(int X, int Y, EnemyType pEnemyType, int pHp)
            : base(X, Y)
        {
            HP = pHp;
            CharacterChar = 'E';
        }

        internal enum EnemyType
        {
            Rat
        }

        EnemyType _type;
        internal EnemyType eType
        {
            get { return _type; }
            set { _type = value; }
        }
    }
    #endregion
}