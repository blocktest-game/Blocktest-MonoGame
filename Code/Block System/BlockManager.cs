using System;
using System.Linq;

namespace Blocktest
{
    /// <summary>
    /// The BlockManager contains all of the block types in <see cref="allBlocks">an array of blocks</see> and a <see cref="blockNames">list of block names.</see>
    /// </summary>
    public static class BlockManager
    {

        /// <summary> Array which stores all block instances for referencing as if they were globals. </summary>
        public static Block[] allBlocks;
        /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
        public static string[] blockNames;

        /// <summary>
        /// Compiles all block subtypes into <see cref="allBlocks">an array of blocks</see> and a <see cref="blockNames">list of block names.</see>
        /// </summary>
        public static void Initialize()
        {
            // This mess gets all subtypes of Block and puts the types in a list.
            Type[] allBlockTypes = (
                            from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                            from assemblyType in domainAssembly.GetTypes()
                            where assemblyType.IsSubclassOf(typeof(Block))
                            select assemblyType).ToArray();

            allBlocks = new Block[allBlockTypes.Length];
            blockNames = new string[allBlockTypes.Length];

            // For loops to populate main allBlocks array.
            for (int i = 0; i < allBlockTypes.Length; i++) {
                Type newBlockType = allBlockTypes[i];
                Block newBlock = (Block)Activator.CreateInstance(newBlockType);
                newBlock.Initialize();
                if (newBlock.blockID == -1) {
                    newBlock.blockID = i;
                }
                if (allBlocks[newBlock.blockID] != null) {
                    Console.WriteLine("Block " + newBlock + " conflicts with block " + allBlocks[newBlock.blockID] + "! (Block ID: " + newBlock.blockID + ")");
                } else if (newBlock.blockID > allBlocks.Length || newBlock.blockID < 0) {
                    Console.WriteLine("Block " + newBlock + " has invalid ID " + newBlock.blockID + "! (Max ID " + allBlocks.Length + ")");
                }
                blockNames[newBlock.blockID] = newBlock.blockName;
                allBlocks[newBlock.blockID] = newBlock;
            }
        }
    }

}
