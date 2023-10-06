using System.Linq;
namespace Shared.Code.Block_System;

/// <summary>
///     The BlockManager contains all of the block types in <see cref="AllBlocks">an array of blocks</see> and a
///     <see cref="BlockNames">list of block names.</see>
/// </summary>
public abstract class BlockManagerShared {
    /// <summary> Array which stores all block instances for referencing as if they were globals. </summary>
    private static BlockShared[] _allBlocks;


    /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
    private static string[] _blockNames;

    /// <summary> Array which stores all block instances for referencing as if they were globals. </summary>
    public static BlockShared[] AllBlocks {
        get => _allBlocks;
        private set => _allBlocks = value;
    }
    
    /// <summary> List used to store the names of blocks. The indexes are the corresponding block's ID. </summary>
    public static string[] BlockNames {
        get => _blockNames;
        private set => _blockNames = value;
    }


    /// <summary>
    ///     Compiles all block subtypes into <see cref="AllBlocks">an array of blocks</see> and a
    ///     <see cref="BlockNames">list of block names.</see>
    /// </summary>
    public static void Initialize() {
        // This mess gets all subtypes of Block and puts the types in a list.
        Type[] allBlockTypes = (
            from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
            from assemblyType in domainAssembly.GetTypes()
            where assemblyType.IsSubclassOf(typeof(BlockShared))
            select assemblyType).ToArray();

        AllBlocks = new BlockShared[allBlockTypes.Length];
        BlockNames = new string[allBlockTypes.Length];

        // For loops to populate main allBlocks array.
        for (int i = 0; i < allBlockTypes.Length; i++) {
            Type newBlockType = allBlockTypes[i];
            BlockShared? newBlock = (BlockShared?)Activator.CreateInstance(newBlockType);
            if (newBlock == null) {
                Console.WriteLine($"Failed to create instance of {newBlockType}!");
                continue;
            }
            newBlock.Initialize();
            if (newBlock.BlockId == -1) {
                newBlock.BlockId = i;
            }
            if (AllBlocks[newBlock.BlockId] != null) {
                Console.WriteLine(
                    $"Block {newBlock} conflicts with block {AllBlocks[newBlock.BlockId]}! (Block ID: {newBlock.BlockId})");
            } else if (newBlock.BlockId > AllBlocks.Length || newBlock.BlockId < 0) {
                Console.WriteLine($"Block {newBlock} has invalid ID {newBlock.BlockId}! (Max ID {AllBlocks.Length})");
            }
            BlockNames[newBlock.BlockId] = newBlock.BlockName;
            AllBlocks[newBlock.BlockId] = newBlock;
        }
    }
}