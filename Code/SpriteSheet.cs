using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Blocktest
{
    public class SpriteSheet
    {
        /// <summary>The dictionary of subsprites retrieved from the texture, with the index being the name of the subsprite.</summary>
        public Dictionary<string, Texture2D> spritesDict = new();

        /// <summary>
        /// Create a Sprite Sheet from the path provided.
        /// </summary>
        /// <param name="texturePath">The path to the sprite folder.</param>
        public SpriteSheet(string texturePath)
        {
            string path = texturePath.Replace("Content\\", null);

            string[] files = Directory.GetFiles("Content\\" + path); // HACK: This loads a bunch of sprites from a folder instead of splitting one sprite sheet image into multiple sprites. Needs to be fixed.
            foreach (string v in files) {
                spritesDict.Add(v, Globals.Game.Content.Load<Texture2D>(v.Remove(0, "Content\\".Length)));
            }
        }

    }
}
