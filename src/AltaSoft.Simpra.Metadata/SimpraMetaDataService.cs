using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using AltaSoft.Simpra.Metadata.Models;

namespace AltaSoft.Simpra.Metadata;

/// <summary>
/// SimpraMetadataService is a service that provides metadata information for Simpra models.
/// </summary>
public sealed class SimpraMetadataService
{
    private readonly FrozenDictionary<string, Lazy<TypeMetadata>> _typeCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpraMetadataService"/> class.
    /// </summary>
    public SimpraMetadataService(List<MetadataDetails> metadata)
    {
        _typeCache = metadata.ToDictionary(k => k.Name ?? k.ModelType.FullName ?? k.ModelType.Name, v => new Lazy<TypeMetadata>(() => new TypeMetadata
        {
            ModelType = v.ModelType,
            ExternalFunctionsType = v.ExternalFunctionsType,

            ModelInfo = new ModelInfo
            {
                Name = v.Name ?? v.ModelType.FullName ?? v.ModelType.Name,
                Properties = v.ModelType.ProcessProperties(),
                Functions = GetFunctions(v.ModelType, v.ExternalFunctionsType)
            }
        })).ToFrozenDictionary();
        return;

        List<FunctionModel> GetFunctions(Type modelType, Type? externalFunctionsType)
        {
            var result = modelType.ProcessFunctions();
            if (externalFunctionsType is not null)
            {
                result.AddRange(externalFunctionsType.ProcessFunctions());
            }
            return result;
        }
    }

    /// <summary>
    /// Gets type models metadata
    /// </summary>
    public ModelInfo? GetTypeModel(string modelName)
    {
        return !_typeCache.TryGetValue(modelName, out var value) ? null : value.Value.ModelInfo;
    }

    /// <summary>
    /// List type models metadata
    /// </summary>
    public List<ModelInfo> ListTypeModels()
    {
        return _typeCache.Values.Select(v => v.Value.ModelInfo).ToList();
    }
}
