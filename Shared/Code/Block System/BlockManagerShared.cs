using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;
namespace Shared.Code.Block_System;

/// <summary>
///     The BlockManager contains all of the block types in
///     <see cref="AllBlocks">a dictionary of blocks indexed by their name</see>
/// </summary>
public abstract class BlockManagerShared {
    /// <summary> Array which stores all block instances for referencing as if they were globals. </summary>
    public static Dictionary<string, BlockShared> AllBlocks { get; private set; }


    /// <summary>
    ///     Compiles all block subtypes into <see cref="AllBlocks">a dictionary of blocks indexed by their name</see>
    /// </summary>
    public static void Initialize() {
        IDeserializer deserialize = new DeserializerBuilder().Build();
        Assembly assembly = typeof(BlockManagerShared).Assembly;
        string[] assemblyNames = assembly.GetManifestResourceNames();
        var blockNames = assemblyNames.Where(x => x.StartsWith("Shared.Content.Blocks."));

        AllBlocks = new Dictionary<string, BlockShared>();
        foreach (string resourceName in blockNames) {
            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) {
                continue;
            }
            using StreamReader reader = new(stream);
            string yaml = reader.ReadToEnd();
            BlockShared? block = deserialize.Deserialize<BlockShared?>(yaml);
            if (block == null) {
                continue;
            }
            block.BlockUid = block.BlockName.ToLower().Replace(" ", "_");

            if (!AllBlocks.TryAdd(block.BlockUid, block)) {
                Console.WriteLine($"File {resourceName} contains duplicate definition of block {block.BlockName}!");
            }
        }
    }
}