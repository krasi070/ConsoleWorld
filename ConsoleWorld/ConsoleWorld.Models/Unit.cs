﻿namespace ConsoleWorld.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    public class Unit
    {
        private int hp;
        private int mp;
        private Random random = new Random();

        [NotMapped]
        public int X { get; set; }

        [NotMapped]
        public int Y { get; set; }

        public char Symbol { get; set; }

        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor BackgroundColor { get; set; } 

        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Money { get; set; }

        [Range(0, int.MaxValue)]
        public int MaxHp { get; set; }

        public int Hp
        {
            get
            {
                return this.hp;
            }

            set
            {
                if (value > this.MaxHp)
                {
                    value = this.MaxHp;
                }

                this.hp = value;
            }
        }

        [Range(0, int.MaxValue)]
        public int MaxMp { get; set; }

        [Range(0, int.MaxValue)]
        public int Mp
        {
            get
            {
                return this.mp;
            }

            set
            {
                if (value > this.MaxMp)
                {
                    value = this.MaxMp;
                }

                this.mp = value;
            }
        }

        [Range(0, int.MaxValue)]
        public int Attack { get; set; }

        [Range(0, int.MaxValue)]
        public int MagicAttack { get; set; }

        [Range(0, int.MaxValue)]
        public int Defense { get; set; }

        [Range(0, int.MaxValue)]
        public int MagicDefense { get; set; }

        [Range(0, 100)]
        public int Evade { get; set; }

        [Range(0, 100)]
        public int Accuracy { get; set; }

        [Range(0, int.MaxValue)]
        public int Range { get; set; }

        [NotMapped]
        public bool IsAlive
        {
            get
            {
                return this.Hp > 0;
            }
        }

        public int? EquippedWeaponId { get; set; }

        public virtual Weapon EquippedWeapon { get; set; }

        public virtual void Draw()
        {
            Console.BackgroundColor = this.BackgroundColor;
            Console.ForegroundColor = this.ForegroundColor;
            Console.SetCursorPosition(this.X, this.Y);
            Console.Write(this.Symbol);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public virtual void Draw(ConsoleColor color)
        {
            Console.BackgroundColor = this.BackgroundColor;
            Console.ForegroundColor = color;
            Console.SetCursorPosition(this.X, this.Y);
            Console.Write(this.Symbol);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        // -1 if missed
        public virtual int AttackUnit(Unit other)
        {
            if (random.Next(100) > other.Evade && random.Next(100) < this.Accuracy)
            {
                int damage = 0;
                if (this.EquippedWeapon != null)
                {
                    if (this.MagicAttack + this.EquippedWeapon.MagicPower > this.Attack + this.EquippedWeapon.Damage && this.Mp > 0)
                    {
                        damage = Math.Max((this.MagicAttack + this.EquippedWeapon.MagicPower) - other.MagicDefense, 0);
                        other.Hp -= damage;
                        this.Mp--;
                    }
                    else
                    {
                        damage = Math.Max((this.Attack + this.EquippedWeapon.Damage) - other.Defense, 0);
                        other.Hp -= damage;
                    }
                }
                else
                {
                    if (this.MagicAttack > this.Attack && this.Mp > 0)
                    {
                        damage = Math.Max(this.MagicAttack - other.MagicDefense, 0);
                        other.Hp -= damage;
                        this.Mp--;
                    }
                    else
                    {
                        damage = Math.Max(this.Attack - other.Defense, 0);
                        other.Hp -= damage;
                    }
                }

                return damage;
            }

            return -1;
        }
    }
}