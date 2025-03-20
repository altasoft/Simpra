using AltaSoft.Simpra.Metadata;
using AltaSoft.Simpra.Metadata.Models;

namespace AltaSoft.Simpra.Tests;

public class SimpraMetaDataTest
{
    [Fact]
    public void GetTypeModel_Should_ReturnNotNull_ForValidModelType()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(MyChildClass),
            ExternalFunctionsType = null,
            Name = "MyChildClass"
        }]);

        var typeModel = metadataService.GetTypeModel("MyChildClass");

        Assert.NotNull(typeModel);
        Assert.Equal("MyChildClass", typeModel.Name);
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyChildClass.Name));
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyChildClass.Age));
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyClass.Base));
        Assert.Contains(typeModel.Functions, f => f.Name == nameof(MyChildClass.Child));
        Assert.Contains(typeModel.Functions, f => f.Name == nameof(MyClass.Father));
    }

    [Fact]
    public void GetTypeModel_Should_ReturnNull_ForInvalidModelType()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(MyChildClass),
            ExternalFunctionsType = null,
            Name = "MyChildClass"
        }]);

        var typeModel = metadataService.GetTypeModel("NonExistentClass");

        Assert.Null(typeModel);
    }

    [Fact]
    public void GetTypeModel_Should_ReturnCorrectModel_ForBaseClass()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(MyChildClass),
            ExternalFunctionsType = null,
            Name = "MyClass"
        }]);

        var typeModel = metadataService.GetTypeModel("MyClass");

        Assert.NotNull(typeModel);
        Assert.NotNull(typeModel);
        Assert.Equal("MyClass", typeModel.Name);
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyClass.Base));
        Assert.Contains(typeModel.Functions, f => f.Name == nameof(MyClass.Father));
    }
}

public class MyChildClass : MyClass
{
    public required string Name { get; set; }
    public int Age { get; set; }

    public void Child()
    {
    }
}

public class MyClass
{
    public required string Base { get; set; }

    public void Father()
    {
    }
}
