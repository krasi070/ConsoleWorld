﻿namespace ConsoleWorld.Data
{
    using Models;
    using System;
    using System.Linq;

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
    }
}