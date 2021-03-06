﻿namespace ConsoleWorld.GameLogic.Controllers
{
    using Core;
    using Data;
    using Enums;
    using Models;
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Models.Enemies;
    using Handler;
    using Screens;
    using System.Collections.Generic;

    public class PlayerController
    {
        private Random random = new Random();
        private MessageHandler messageHandler = new MessageHandler();

        // 0 - nothing
        // 1 - update status
        // 2 - next dungeon
        public int Action(ScreenHandler screenHandler,Dungeon dungeon, Character character, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    return MovePlayer(dungeon, character, key) ? 1 : 0;
                case ConsoleKey.O:
                    screenHandler.SelectOptionFromImproveStatsScreen(dungeon,character);
                    return 1;
                case ConsoleKey.I:
                    screenHandler.SelectOptionFromItemScreen(dungeon, character);
                    return 1;
                case ConsoleKey.K:
                    this.ExecuteAttack(character, dungeon);
                    return 1;
                case ConsoleKey.L:
                    this.GiveMoneyToMagicWell(dungeon, character);
                    return 1;
                case ConsoleKey.J:
                    return 2;
            }

            return 0;
        }

        // returns true if status needs to be updated
        public bool MovePlayer(Dungeon dungeon, Character character, ConsoleKey key)
        {
            this.messageHandler.EraseMessage();
            Tile tile = new Tile(TileType.Unused);
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    tile = dungeon.GetTile(character.X, character.Y - 1);
                    if (tile.IsFree && !tile.IsEnemy)
                    {
                        dungeon.GetTile(character.X, character.Y).IsFree = true;
                        dungeon.GetTile(character.X, character.Y).IsPlayer = false;
                        dungeon.DrawTile(character.X, character.Y);
                        character.Y--;
                        character.Draw();
                        tile.IsFree = false;
                        tile.IsPlayer = true;
                    }
                    else if (tile.Type == TileType.ClosedDoor)
                    {
                        if (Utility.CheckIfCharacterHasItem(character.Id, "Master Key"))
                        {
                            this.UnlockDoor(dungeon, character.X, character.Y - 1);
                            this.messageHandler.UnlockDoorMasterKeyMessage(character);
                        }
                        else if (Utility.CheckIfCharacterHasItem(character.Id, "Normal Key"))
                        {
                            this.UseItem(character.Id, "Normal Key");
                            this.UnlockDoor(dungeon, character.X, character.Y - 1);
                            this.messageHandler.UnlockDoorNormalKeyMessage(character, Utility.GetItemQuantity(character.Id, 10));
                        }
                    }

                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    tile = dungeon.GetTile(character.X, character.Y + 1);
                    if (tile.IsFree && !tile.IsEnemy)
                    {
                        dungeon.GetTile(character.X, character.Y).IsFree = true;
                        dungeon.GetTile(character.X, character.Y).IsPlayer = false;
                        dungeon.DrawTile(character.X, character.Y);
                        character.Y++;
                        character.Draw();
                        tile.IsFree = false;
                        tile.IsPlayer = true;
                    }

                    else if (tile.Type == TileType.ClosedDoor)
                    {
                        if (Utility.CheckIfCharacterHasItem(character.Id, "Master Key"))
                        {
                            this.UnlockDoor(dungeon, character.X, character.Y + 1);
                            this.messageHandler.UnlockDoorMasterKeyMessage(character);
                        }
                        else if (Utility.CheckIfCharacterHasItem(character.Id, "Normal Key"))
                        {
                            this.UseItem(character.Id, "Normal Key");
                            this.UnlockDoor(dungeon, character.X, character.Y + 1);
                            this.messageHandler.UnlockDoorNormalKeyMessage(character, Utility.GetItemQuantity(character.Id, 10));
                        }
                    }

                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    tile = dungeon.GetTile(character.X + 1, character.Y);
                    if (tile.IsFree && !tile.IsEnemy)
                    {
                        dungeon.GetTile(character.X, character.Y).IsFree = true;
                        dungeon.GetTile(character.X, character.Y).IsPlayer = false;
                        dungeon.DrawTile(character.X, character.Y);
                        character.X++;
                        character.Draw();
                        tile.IsFree = false;
                        tile.IsPlayer = true;
                    }

                    else if (tile.Type == TileType.ClosedDoor)
                    {
                        if (Utility.CheckIfCharacterHasItem(character.Id, "Master Key"))
                        {
                            this.UnlockDoor(dungeon, character.X + 1, character.Y);
                            this.messageHandler.UnlockDoorMasterKeyMessage(character);
                        }
                        else if (Utility.CheckIfCharacterHasItem(character.Id, "Normal Key"))
                        {
                            this.UseItem(character.Id, "Normal Key");
                            this.UnlockDoor(dungeon, character.X + 1, character.Y);
                            this.messageHandler.UnlockDoorNormalKeyMessage(character, Utility.GetItemQuantity(character.Id, 10));
                        }
                    }

                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    tile = dungeon.GetTile(character.X - 1, character.Y);
                    if (tile.IsFree && !tile.IsEnemy)
                    {
                        dungeon.GetTile(character.X, character.Y).IsFree = true;
                        dungeon.GetTile(character.X, character.Y).IsPlayer = false;
                        dungeon.DrawTile(character.X, character.Y);
                        character.X--;
                        character.Draw();
                        tile.IsFree = false;
                        tile.IsPlayer = true;
                    }

                    else if (tile.Type == TileType.ClosedDoor)
                    {
                        if (Utility.CheckIfCharacterHasItem(character.Id, "Master Key"))
                        {
                            this.UnlockDoor(dungeon, character.X - 1, character.Y);
                            this.messageHandler.UnlockDoorMasterKeyMessage(character);
                        }
                        else if (Utility.CheckIfCharacterHasItem(character.Id, "Normal Key"))
                        {
                            this.UseItem(character.Id, "Normal Key");
                            this.UnlockDoor(dungeon, character.X - 1, character.Y);
                            this.messageHandler.UnlockDoorNormalKeyMessage(character, Utility.GetItemQuantity(character.Id, 10));
                        }
                    }
                    break;
                
            }

            return this.CheckIfCharacterIsOnSpecialTile(dungeon, character, tile);
        }

        public void ExecuteAttack(Character character, Dungeon dungeon)
        {
            Queue<Tuple<int[], Tile>> tiles = new Queue<Tuple<int[], Tile>>();
            List<int> visitedTiles = new List<int>();
            tiles.Enqueue(new Tuple<int[], Tile>(new int[] { character.X, character.Y, 0 }, dungeon.GetTile(character.X, character.Y)));
            while (tiles.Count > 0)
            {
                var tuple = tiles.Dequeue();
                int x = tuple.Item1[0];
                int y = tuple.Item1[1];
                int currRange = tuple.Item1[2];
                visitedTiles.Add(x + y * dungeon.Width);
                if (tuple.Item2.IsEnemy)
                {
                    int damage = character.AttackUnit(dungeon.Enemies[x + y * dungeon.Width]);
                    if (damage >= 0)
                    {
                        dungeon.Enemies[x + y * dungeon.Width].Draw(ConsoleColor.Red);
                        this.messageHandler.BattleMessage(character, dungeon.Enemies[x + y * dungeon.Width], damage);
                        Thread.Sleep(50);
                        dungeon.Enemies[x + y * dungeon.Width].Draw();
                    }
                    else
                    {
                        this.messageHandler.MissMessage(character);
                    }
                }

                if (currRange < character.Range)
                {
                    Tile upTile = dungeon.GetTile(x, Math.Max(y - 1, 0));
                    if ((upTile.IsFree || upTile.IsEnemy) && !visitedTiles.Contains(x + Math.Max(y - 1, 0) * dungeon.Width))
                    {
                        tiles.Enqueue(new Tuple<int[], Tile>(new int[] { x, Math.Max(y - 1, 0), currRange + 1 }, upTile));
                    }

                    Tile downTile = dungeon.GetTile(x, Math.Min(y + 1, dungeon.Height));
                    if ((downTile.IsFree || downTile.IsEnemy) && !visitedTiles.Contains(x + Math.Min(y + 1, dungeon.Height) * dungeon.Width))
                    {
                        tiles.Enqueue(new Tuple<int[], Tile>(new int[] { x, Math.Min(y + 1, dungeon.Height), currRange + 1 }, downTile));
                    }

                    Tile rightTile = dungeon.GetTile(Math.Min(x + 1, dungeon.Width), y);
                    if ((rightTile.IsFree || rightTile.IsEnemy) && !visitedTiles.Contains(Math.Min(x + 1, dungeon.Width) + y * dungeon.Width))
                    {
                        tiles.Enqueue(new Tuple<int[], Tile>(new int[] { Math.Min(x + 1, dungeon.Width), y, currRange + 1 }, rightTile));
                    }

                    Tile leftTile = dungeon.GetTile(Math.Max(x - 1, 0), y);
                    if ((leftTile.IsFree || leftTile.IsEnemy) && !visitedTiles.Contains(Math.Max(x - 1, 0) + y * dungeon.Width))
                    {
                        tiles.Enqueue(new Tuple<int[], Tile>(new int[] { Math.Max(x - 1, 0), y, currRange + 1 }, leftTile));
                    }
                }
            }
        }

        private void UnlockDoor(Dungeon dungeon, int x, int y)
        {
            dungeon.SetTile(x, y, new Tile(TileType.OpenDoor));
            dungeon.DrawTile(x, y);
        }

        private void UseItem(int characterId, string itemName)
        {
            // TODO: add item effects
            Utility.RemoveOneItemFromCharacter(characterId, itemName);
        }

        private bool CheckIfCharacterIsOnSpecialTile(Dungeon dungeon, Character character, Tile tile)
        {
            switch (tile.Type)
            {
                case TileType.Hole:
                    this.messageHandler.HoleMessage();
                    break;
                case TileType.MagicWell:
                    this.messageHandler.MagicWellTutorialMessage(character);
                    break;
                case TileType.Money:
                    this.PickUpMoney(dungeon, character);
                    return true;
                case TileType.Item:
                    this.PickUpItem(dungeon, character);
                    return true;
            }

            return false;
        }

        private void PickUpMoney(Dungeon dungeon, Character character)
        {
            int amount = random.Next(1, 11);
            character.Money += random.Next(1, 11);
            dungeon.SetTile(character.X, character.Y, new Tile(TileType.Floor));
            this.messageHandler.MoneyMessage(character, amount);
        }

        private void PickUpItem(Dungeon dungeon, Character character)
        {
            Utility.AddItemToCharacter(character.Id, dungeon.Items[character.X + character.Y * dungeon.Width].Id);
            this.messageHandler.ItemMessage(character, dungeon.Items[character.X + character.Y * dungeon.Width]);
            dungeon.Items.Remove(character.X + character.Y * dungeon.Width);
            dungeon.SetTile(character.X, character.Y, new Tile(TileType.Floor));
        }

        private void GiveMoneyToMagicWell(Dungeon dungeon, Character character)
        {
            if (dungeon.GetTile(character.X, character.Y).Type == TileType.MagicWell)
            {
                int money = character.Money;
                character.Money = 0;
                if (money >= 50 && random.Next(100) < 100)
                {
                    var weapon = Utility.GetWeaponForMagicWell(character, money);
                    Utility.EquipWeaponToCharacter(character, weapon);
                    this.messageHandler.MagicWellWeaponMessage(character, weapon);
                }
                else
                {
                    var items = Utility.GetItemsForMagicWell(money);
                    foreach (var item in items)
                    {
                        Utility.AddItemToCharacter(character.Id, item.Id);
                    }

                    this.messageHandler.MagicWellMessage(character, items);
                }
            }
        }
    }
}