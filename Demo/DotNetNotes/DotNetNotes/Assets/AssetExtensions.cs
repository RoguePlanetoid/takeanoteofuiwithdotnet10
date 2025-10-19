namespace DotNetNotes.Assets;

/// <summary>
/// Asset Extensions
/// </summary>
internal static class AssetExtensions
{
    private const string asset_svg_tag = "svg";
    private const string asset_svg_id_attribute = "//*[@id='{0}']/@{1}";    
    private const string asset_svg_name_space = "http://www.w3.org/2000/svg";
    private const string asset_svg_data_image = "data:image/svg+xml;base64,";

    /// <summary>
    /// Get SVG Document
    /// </summary>
    /// <param name="content">Content</param>
    /// <param name="manager">XML Namespace Manager</param>
    /// <returns>XML Document</returns>
    internal static XmlDocument? GetSvgDocument(this string? content, out XmlNamespaceManager? manager)
    {
        if (content != null)
        {
            var svg = new XmlDocument();
            svg.LoadXml(content);
            if (svg != null)
            {
                var navigator = svg.CreateNavigator();
                if (navigator != null)
                {
                    manager = new XmlNamespaceManager(navigator.NameTable);
                    manager.AddNamespace(asset_svg_tag, asset_svg_name_space);
                    return svg;
                }
            }
        }
        manager = null;
        return null;
    }

    /// <summary>
    /// Get Asset Attribute
    /// </summary>
    /// <param name="svg">XML Document</param>
    /// <param name="id">Id</param>
    /// <param name="attribute">Attribute</param>
    /// <param name="manager">XML Namespace Manager</param>
    /// <returns>Xml Node</returns>
    internal static XmlNode? GetAssetAttribute(this XmlDocument? svg, string id, string attribute, XmlNamespaceManager? manager) =>
        manager != null ? svg?.SelectSingleNode(string.Format(asset_svg_id_attribute, id, attribute), manager) : null;

    /// <summary>
    /// Get As Data Uri
    /// </summary>
    /// <param name="assetResource">Asset Resource</param>
    /// <returns>Data Uri</returns>
    internal static string? GetAsDataUri(this AssetResource assetResource)
    {
        if (assetResource != null)
        {
            var svg = assetResource?.ToBase64EncodedSvgString();
            return string.IsNullOrWhiteSpace(svg) ? null : $"{asset_svg_data_image}{svg}";
        }
        return null;
    }
}
