using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using AltaSoft.Simpra.Metadata;
using AltaSoft.Simpra.Metadata.Models;
using AltaSoft.Simpra.Tests.Models;

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
        Assert.Contains(typeModel.Properties, p => p.Name != nameof(MyChildClass.Name));
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyChildClass.Age));
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyClass.Base));
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyChildClass.Country));
        Assert.Contains(typeModel.Properties, p => p.Name == $"{nameof(MyChildClass.Countries)}[]");
        Assert.Contains(typeModel.Properties, p => p.Name == $"{nameof(MyChildClass.Dictionary)}[]");
        Assert.Contains(typeModel.Properties, p => p.Name == "CollectionClass1[].CollectionMyClasses[].Base");
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
        Assert.Equal("MyClass", typeModel.Name);
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(MyClass.Base));
        Assert.Contains(typeModel.Functions, f => f.Name == nameof(MyClass.Father));
    }

    [Fact]
    public void GetTypeModel_Should_NotStackOverflow_WithXmlElementProperty()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(ReproModelWithXmlElement),
            ExternalFunctionsType = null,
            Name = "ReproModelWithXmlElement"
        }]);

        var typeModel = metadataService.GetTypeModel("ReproModelWithXmlElement");

        Assert.NotNull(typeModel);
        Assert.Equal("ReproModelWithXmlElement", typeModel.Name);
        Assert.NotEmpty(typeModel.Properties);
    }

    [Fact]
    public void GetTypeModel_Should_NotStackOverflow_WithNestedXmlElement()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(ReproSupplementaryDataModel),
            ExternalFunctionsType = null,
            Name = "ReproSupplementaryDataModel"
        }]);

        var typeModel = metadataService.GetTypeModel("ReproSupplementaryDataModel");

        Assert.NotNull(typeModel);
        Assert.Equal("ReproSupplementaryDataModel", typeModel.Name);
        Assert.NotEmpty(typeModel.Properties);
    }

    [Fact]
    public void GetTypeModel_Should_NotStackOverflow_WithXmlNode()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(ReproModelWithXmlNode),
            ExternalFunctionsType = null,
            Name = "ReproModelWithXmlNode"
        }]);

        var typeModel = metadataService.GetTypeModel("ReproModelWithXmlNode");

        Assert.NotNull(typeModel);
        Assert.Equal("ReproModelWithXmlNode", typeModel.Name);
        Assert.NotEmpty(typeModel.Properties);
    }

    [Fact]
    public void GetTypeModel_Should_HandleCircularDependency_WithoutStackOverflow()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(CircularModelA),
            ExternalFunctionsType = null,
            Name = "CircularModelA"
        }]);

        var typeModel = metadataService.GetTypeModel("CircularModelA");

        Assert.NotNull(typeModel);
        Assert.Equal("CircularModelA", typeModel.Name);
        Assert.NotEmpty(typeModel.Properties);

        // Verify the circular property is handled (should be present but not infinitely expanded)
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(CircularModelA.Name));
        Assert.Contains(typeModel.Properties, p => p.Name.StartsWith(nameof(CircularModelA.RefToB)));
    }

    [Fact]
    public void GetTypeModel_Should_HandleCircularDependency_StartingFromModelB()
    {
        var metadataService = new SimpraMetadataService([new MetadataDetails
        {
            ModelType = typeof(CircularModelB),
            ExternalFunctionsType = null,
            Name = "CircularModelB"
        }]);

        var typeModel = metadataService.GetTypeModel("CircularModelB");

        Assert.NotNull(typeModel);
        Assert.Equal("CircularModelB", typeModel.Name);
        Assert.NotEmpty(typeModel.Properties);

        // Verify properties are present
        Assert.Contains(typeModel.Properties, p => p.Name == nameof(CircularModelB.Value));
        Assert.Contains(typeModel.Properties, p => p.Name.StartsWith(nameof(CircularModelB.RefToA)));
    }
}

public class MyChildClass : MyClass
{
    [JsonIgnore]
    public required string Name { get; set; }
    [XmlIgnore]
    public int Age { get; set; }
    public required MyClass1[] CollectionClass1 { get; set; }
    public required CountryCode[] Countries { get; set; }
    public required CountryCode Country { get; set; }
    public required Dictionary<int, int> Dictionary { get; set; }

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

public class MyClass1
{
    public int Info { get; set; }
    public required List<MyClass> CollectionMyClasses { get; set; }
}

/// <summary>
/// Minimal repro model containing XmlElement property to trigger cyclic traversal
/// </summary>
public sealed class ReproModelWithXmlElement
{
    public ReproEnvelopeWithXmlElement Envelope { get; set; } = new();
}

public sealed class ReproEnvelopeWithXmlElement
{
    public XmlElement? Element { get; set; }
}

/// <summary>
/// More complex repro model with nested XmlElement similar to actual issue
/// </summary>
public sealed class ReproSupplementaryDataModel
{
    public ReproSupplementaryData[] SupplementaryData { get; set; } = [];
}

public sealed class ReproSupplementaryData
{
    public ReproSupplementaryDataEnvelope Envelope { get; set; } = new();
}

public sealed class ReproSupplementaryDataEnvelope
{
    public XmlElement? Any { get; set; }
}

/// <summary>
/// Repro model with XmlNode property
/// </summary>
public sealed class ReproModelWithXmlNode
{
    public ReproNodeContainer NodeContainer { get; set; } = new();
}

public sealed class ReproNodeContainer
{
    public XmlNode? Node { get; set; }
}

/// <summary>
/// Circular dependency models A  B to test cycle detection (non-XML scenario)
/// </summary>
public sealed class CircularModelA
{
    public required string Name { get; set; }
    public CircularModelB? RefToB { get; set; }
}

public sealed class CircularModelB
{
    public int Value { get; set; }
    public CircularModelA? RefToA { get; set; }
}
