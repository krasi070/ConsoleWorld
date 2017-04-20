﻿namespace ConsoleWorld.Data
{
    using Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public static class Utility
    {
        public static void InitDb()
        {
            var context = new ConsoleWorldContext();
            context.Database.Initialize(true);
        }

        public static bool CheckIfCharacterExists(string name)
        {
            bool exists = false;
            using (var context = new ConsoleWorldContext())
            {
                if (context.Characters.Any(c => c.Name == name))
                {
                    exists = true;
                }
            }
            
            return exists;
        }

        public static Character GetCharacterByName(string name)
        {
            Character character = null;
            using (var context = new ConsoleWorldContext())
            {
                character = context.Characters.FirstOrDefault(c => c.Name == name);
            }

            return character;
        }

        public static bool CheckIfCharacterHasItem(int characterId, string itemName)
        {
            bool hasItem = false;
            using (var context = new ConsoleWorldContext())
            {
                var item = context.Items.FirstOrDefault(i => i.Name == itemName);
                if (item != null)
                {
                    var characterItem = context.CharacterItems.FirstOrDefault(ci => ci.CharacterId == characterId && ci.ItemId == item.Id);
                    if (characterItem != null)
                    {
                        hasItem = characterItem.Quantity > 0;
                    }
                }
            }

            return hasItem;
        }

        public static void RemoveOneItemFromCharacter(int characterId, int itemId)
        {
            using (var context = new ConsoleWorldContext())
            {
                var characterItem = context.CharacterItems.FirstOrDefault(ci => ci.ItemId == itemId && ci.CharacterId == characterId);
                if (characterItem != null)
                {
                    characterItem.Quantity--;
                    context.SaveChanges();
                }
            }
        }

        public static void RemoveOneItemFromCharacter(int characterId, string itemName)
        {
            using (var context = new ConsoleWorldContext())
            {
                var item = context.Items.FirstOrDefault(i => i.Name == itemName);
                if (item != null)
                {
                    var characterItem = context.CharacterItems.FirstOrDefault(ci => ci.ItemId == item.Id && ci.CharacterId == characterId);
                    if (characterItem != null)
                    {
                        characterItem.Quantity--;
                        context.SaveChanges();
                    }
                }
            }
        }

        public static List<CharacterItem> GetCharacterItems(int characterId)

        {
            using (var context = new ConsoleWorldContext())
            {
                List<CharacterItem> items = context.CharacterItems.Where(ci => ci.CharacterId == characterId).ToList();

                return items;
            }
        }

        public static List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            using (var context = new ConsoleWorldContext())
            {
                items.AddRange(context.Items.ToList());
            }

            return items;
        }

        public static void AddItemToCharacter(int characterId, int itemId)
        {
            using (var context = new ConsoleWorldContext())
            {
                if (context.Characters.Any(c => c.Id == characterId) && context.Items.Any(i => i.Id == itemId))
                {
                    var characterItem = context.CharacterItems.FirstOrDefault(ci => ci.CharacterId == characterId && ci.ItemId == itemId);
                    if (characterItem != null)
                    {
                        characterItem.Quantity++;
                    }
                    else
                    {
                        context.CharacterItems.Add(new CharacterItem(characterId, itemId, 1));
                    }

                    context.SaveChanges();
                }
            }
        }

        public static void IncreaseCharacterLevel(int characterId, int additionalExp)
        {
            using (var context = new ConsoleWorldContext())
            {
                var character = context.Characters.FirstOrDefault(c => c.Id == characterId);
                if (character != null && context.Levels.Count() > character.LevelId)
                {
                    int newLevel = character.LevelId + 1;
                    character.LevelId++;
                    character.Level = context.Levels.FirstOrDefault(l => l.LevelId == newLevel);
                    character.Points += character.Level.PointsToReceive;
                    character.Exp = additionalExp;
                }

                context.SaveChanges();
            }
        }

        public static int GetExpToNextLevel(int characterId)
        {
            int exp = 0;
            using (var context = new ConsoleWorldContext())
            {
                var character = context.Characters.FirstOrDefault(c => c.Id == characterId);
                if (character != null)
                {
                    exp = character.Level.ExpToNextLevel;
                }
            }

            return exp;
        }
    }
}