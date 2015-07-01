﻿using System;

/*
    This class is about the Player.
    An enemy is basically a player as well, 
    although the player can't control them.
*/

namespace fwod
{
    internal class Player
    {
        #region Properties
        int _posx;
        /// <summary>
        /// Sets or gets the Player position (Left).
        /// </summary>
        internal int PosX
        {
            get { return _posx; }
            set
            {
                char futrchar = Core.GetCharAt(Core.Layer.Game, this.PosX, this.PosY);
                if (!char.IsLetterOrDigit(Core.GetCharAt(Core.Layer.Game, value, this.PosY)))
                {
                    Core.Write(Core.Layer.Game, Core.GetCharAt(Core.Layer.Game, this.PosX, this.PosY), this.PosX, this.PosY);
                    _posx = value;
                    Core.Write(Core.Layer.Game, this.CharacterChar, this.PosX, value);
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
                char futrchar = Core.GetCharAt(Core.Layer.Game, this.PosX, this.PosY);
                if (!char.IsLetterOrDigit(Core.GetCharAt(Core.Layer.Game, this.PosX, value)))
                {
                    Core.Write(Core.Layer.Game, Core.GetCharAt(Core.Layer.Game, this.PosX, this.PosY), this.PosX, this.PosY);
                    _posx = value;
                    Core.Write(Core.Layer.Game, this.CharacterChar, this.PosX, value);
                }
            }
        }

        int _hp;
        /// <summary>
        /// Gets or sets the HP.
        /// </summary>
        int HP
        {
            get { return _hp; }
            set
            {
                _hp = value;
                Console.SetCursorPosition(ConsoleTools.BufferWidth / 2, 0);
                Console.Write("HP: " + _hp);
            }
        }

        /// <summary>
        /// Gets or sets the name of the character.
        /// </summary>
        /// <value>The name of the character.</value>
        internal string CharacterName
        {
            get { return _characterName; }
            set
            {
                _characterName = value;
                // Clear name and redraw it (in case of shorter name)
                Console.SetCursorPosition(1, 0);
                Console.Write(ConsoleTools.RepeatChar(' ', (ConsoleTools.BufferWidth / 2) - 1));
                Console.SetCursorPosition(1, 0);
                Console.Write(_characterName);
            }
        }

        /// <summary>
        /// The character's char.
        /// </summary>
        internal char CharacterChar;

        /// <summary>
        /// The Player's name.
        /// </summary>
        string _characterName;
        #endregion

        #region Construction
        /// <summary>
        /// Creates a new player.
        /// </summary>
        internal Player()
        { // Defaults
            this.CharacterChar = '@';
            this._posx = ConsoleTools.BufferWidth / 2;
            this._posy = ConsoleTools.BufferHeight / 2;
            this._hp = 10;
        }

        /// <summary>
        /// Creates a new player with specific coords.
        /// </summary>
        /// <param name="X">Left value.</param>
        /// <param name="Y">Top value.</param>
        internal Player(int X, int Y)
        {
            this.CharacterChar = '@';
            this._posx = X;
            this._posy = Y;
            this._hp = 10;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Places the Player on screen.
        /// </summary>
        internal void Initialize()
        {
            this.PosX = this._posx;
            this.PosY = this._posy;
        }

        /// <summary>
        /// Makes the character talk.
        /// </summary>
        /// <param name="pText">Text!</param>
        internal void Say(string pText)
        {
            Game.CharacterSays(this, pText);
        }

        /// <summary>
        /// Get input from Player.
        /// </summary>
        /// <returns></returns>
        internal string GetAnswer()
        {
            return Game.GetAnswerFromCharacter(this);
        }

        /// <summary>
        /// Makes the enemy move up one square
        /// </summary>
        internal void MoveUp()
        {
            this.PosY--;
        }

        /// <summary>
        /// Makes the enemy move down one square
        /// </summary>
        internal void MoveDown()
        {
            this.PosY++;
        }

        /// <summary>
        /// Makes the enemy move left one square
        /// </summary>
        internal void MoveLeft()
        {
            this.PosX--;
        }

        /// <summary>
        /// Makes the enemy move right one square
        /// </summary>
        internal void MoveRight()
        {
            this.PosX++;
        }
        #endregion
    }
}