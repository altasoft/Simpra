using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AltaSoft.Simpra.Metadata;

/// <summary>
/// Utility class to provide documentation for various types where available with the assembly.
/// </summary>
internal static class XmlDocExt
{
    /// <summary>
    /// Provides the documentation comments for a specific member.
    /// </summary>
    /// <param name="memberInfo">The MemberInfo (reflection data) of the member to find documentation for.</param>
    /// <returns>The XML fragment describing the member.</returns>
    public static XmlElement? GetDocumentation(this MemberInfo memberInfo) =>
        // First character [0] of member type is prefix character in the name in the XML
        XmlFromName(memberInfo.DeclaringType, memberInfo.MemberType.ToString()[0], memberInfo.Name);

    /// <summary>
    /// Provides the documentation comments for a specific member with commandType.
    /// </summary>
    /// <param name="memberInfo">The MemberInfo (reflection data) of the member to find documentation for.</param>
    /// <param name="commandType">The command type to include in the documentation lookup.</param>
    /// <returns>The XML fragment describing the member with commandType.</returns>
    public static XmlElement? GetDocumentation(this MemberInfo memberInfo, Type commandType)
    {
        var memberInfoNameWithCommandType = memberInfo.Name + "(" + commandType + ")";
        // First character [0] of member type is prefix character in the name in the XML
        return XmlFromName(memberInfo.DeclaringType, memberInfo.MemberType.ToString()[0], memberInfoNameWithCommandType);
    }

    /// <summary>
    /// Returns the XML documentation summary comment for this member and commandType.
    /// </summary>
    /// <param name="memberInfo">The MemberInfo (reflection data) of the member to find documentation for.</param>
    /// <param name="commandType">The command type to include in the documentation lookup.</param>
    /// <returns>The summary comment as a string.</returns>
    public static string GetSummary(this MemberInfo memberInfo, Type commandType)
    {
        var element = memberInfo.GetDocumentation(commandType);
        var summaryElm = element?.SelectSingleNode("summary");
        return summaryElm is null ? string.Empty : summaryElm.InnerText.Trim();
    }

    /// <summary>
    /// Returns the XML documentation summary comment for this member without throwing exceptions.
    /// </summary>
    /// <param name="memberInfo">The MemberInfo (reflection data) of the member to find documentation for.</param>
    /// <returns>The summary comment as a string, or null if an exception occurs.</returns>
    public static string? GetSummaryWithoutException(this MemberInfo memberInfo)
    {
        try
        {
            return GetSummary(memberInfo);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the XML documentation returns tag for this member and commandType.
    /// </summary>
    /// <param name="memberInfo">The MemberInfo (reflection data) of the member to find documentation for.</param>
    /// <param name="commandType">The command type to include in the documentation lookup.</param>
    /// <returns>The returns tag as a string.</returns>
    public static string GetReturnsTag(this MemberInfo memberInfo, Type commandType)
    {
        var element = memberInfo.GetDocumentation(commandType);
        var summaryElm = element?.SelectSingleNode("returns");
        return summaryElm is null ? string.Empty : summaryElm.InnerText.Trim();
    }

    /// <summary>
    /// Returns the XML documentation summary comment for this member.
    /// </summary>
    /// <param name="memberInfo">The MemberInfo (reflection data) of the member to find documentation for.</param>
    /// <returns>The summary comment as a string.</returns>
    public static string GetSummary(this MemberInfo memberInfo)
    {
        var element = memberInfo.GetDocumentation();
        var summaryElm = element?.SelectSingleNode("summary");
        return summaryElm is null ? string.Empty : summaryElm.InnerText.Trim();
    }

    /// <summary>
    /// Provides the documentation comments for a specific type.
    /// </summary>
    /// <param name="type">The type to find the documentation for.</param>
    /// <returns>The XML fragment that describes the type.</returns>
    public static XmlElement? GetDocumentation(this Type type) =>
        // Prefix in type names is T
        XmlFromName(type, 'T', "");

    /// <summary>
    /// Gets the summary portion of a type's documentation or returns an empty string if not available.
    /// </summary>
    /// <param name="type">The type to find the documentation for.</param>
    /// <returns>The summary comment as a string.</returns>
    public static string GetSummary(this Type type)
    {
        var element = type.GetDocumentation();
        var summaryElm = element?.SelectSingleNode("summary");
        return summaryElm is null ? string.Empty : summaryElm.InnerText.Trim();
    }

    /// <summary>
    /// Obtains the XML Element that describes a reflection element by searching the
    /// members for a member that has a name that describes the element.
    /// </summary>
    /// <param name="type">The type or parent type, used to fetch the assembly.</param>
    /// <param name="prefix">The prefix as seen in the name attribute in the documentation XML.</param>
    /// <param name="name">Where relevant, the full name qualifier for the element.</param>
    /// <returns>The member that has a name that describes the specified reflection element.</returns>
    private static XmlElement? XmlFromName(this Type? type, char prefix, string name)
    {
        if (type is null)
            return null;

        var fullName = string.IsNullOrEmpty(name)
            ? prefix + ":" + type.Namespace + "." + type.Name
            : prefix + ":" + type.Namespace + "." + type.Name + "." + name;
        var xmlDocument = XmlFromAssembly(type.Assembly);

        var matchedElement = xmlDocument["doc"]?["members"]?.SelectSingleNode("member[@name='" + fullName + "']") as XmlElement;

        return matchedElement == null && type.BaseType != null && type.BaseType != typeof(object)
            ? XmlFromName(type.BaseType, prefix, name)
            : matchedElement;
    }

    /// <summary>
    /// A cache used to remember XML documentation for assemblies.
    /// </summary>
    private static readonly Dictionary<Assembly, XmlDocument> s_cache = new();

    /// <summary>
    /// A cache used to store failure exceptions for assembly lookups.
    /// </summary>
    private static readonly Dictionary<Assembly, Exception> s_failCache = new();

    /// <summary>
    /// Obtains the documentation file for the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to find the XML document for.</param>
    /// <returns>The XML document.</returns>
    /// <remarks>This version uses a cache to preserve the assemblies, so that
    /// the XML file is not loaded and parsed on every single lookup.</remarks>
    public static XmlDocument XmlFromAssembly(this Assembly assembly)
    {
        if (s_failCache.TryGetValue(assembly, out var value))
        {
            throw value;
        }

        try
        {
            s_cache.TryAdd(assembly, XmlFromAssemblyNonCached(assembly));
            return s_cache[assembly];
        }
        catch (Exception exception)
        {
            s_failCache[assembly] = exception;
            throw;
        }
    }

    /// <summary>
    /// Loads and parses the documentation file for the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to find the XML document for.</param>
    /// <returns>The XML document.</returns>
    private static XmlDocument XmlFromAssemblyNonCached(Assembly assembly)
    {
        var assemblyFilename = assembly.Location;

        StreamReader? streamReader;
        try
        {
            var path = Path.ChangeExtension(assemblyFilename, ".xml");
            streamReader = new StreamReader(path);
        }
        catch (FileNotFoundException exception)
        {
            throw new InvalidOperationException("XML documentation not present (make sure it is turned on in project properties when building)", exception);
        }

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(streamReader);
        return xmlDocument;
    }
}
