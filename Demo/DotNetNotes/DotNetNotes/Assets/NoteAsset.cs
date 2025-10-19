namespace DotNetNotes.Assets;

/// <summary>
/// Note Asset
/// </summary>
public class NoteAsset : AssetBase<NoteAsset>
{
    private const int size = 467;    
    private const string asset = "Note";    
    private const string asset_svg_path = "d";
    private const string asset_svg_fill = "fill";    
    private const string asset_svg_title = "title";
    private const string asset_svg_content = "content";
    private const string asset_svg_primary = "primary";   
    private const string asset_svg_id = "//*[@id='{0}']";
    private const string asset_svg_secondary = "secondary";
    private const string root = "DotNetNotes.Assets.Resources";
    private static readonly Color source = Color.FromArgb(255, 255, 196, 118);

    /// <summary>
    /// Set Asset Fill
    /// </summary>
    /// <param name="svg">XML Document</param>
    /// <param name="fill">Fill Colour</param>
    /// <param name="manager">XML Namespace Manager</param>
    /// <returns>XML Document</returns>
    private static XmlDocument? SetAssetFill(XmlDocument? svg, Color fill, XmlNamespaceManager? manager)
    {
        var node = svg.GetAssetAttribute(asset_svg_primary, asset_svg_fill, manager);
        if (node?.Value != null)
            node.Value = AsString(node.Value, source, fill);
        return svg;
    }

    /// <summary>
    /// Set Asset Text
    /// </summary>
    /// <param name="svg">XML Document</param>
    /// <param name="id">Id</param>
    /// <param name="value">Value</param>
    /// <param name="manager">XML Namespace Manager</param>
    /// <returns>XML Document</returns>
    private static XmlDocument? SetAssetText(XmlDocument? svg, string id, string? value, XmlNamespaceManager? manager)
    {
        var node = manager != null ? svg?.SelectSingleNode(string.Format(asset_svg_id, id), manager) : null;
        node?.InnerText = value ?? string.Empty; // Null-conditional Assignment
        return svg;
    }

    /// <summary>
    /// Get Asset Resource Path Markup
    /// </summary>
    /// <param name="id">Path Id</param>
    /// <returns>Asset Resource Path Markup</returns>
    private static string GetAssetPathMarkup(string id) => 
        AsString(root, asset).GetSvgDocument(out var manager)
            .GetAssetAttribute(id, asset_svg_path, manager)?.Value ?? string.Empty;

    /// <summary>
    /// Get Asset Resource String
    /// </summary>
    /// <param name="target">Target Colour</param>
    /// <param name="title">Title Text</param>
    /// <param name="content">Content Text</param>
    /// <returns>Asset Resource String</returns>
    private static string? GetAssetResourceString(Color? target = null, string? title = null, string? content = null) =>    
        SetAssetText(
            SetAssetText(
                SetAssetFill(AsString(root, asset)
                    .GetSvgDocument(out var manager), 
                        target ?? source, manager), 
                            asset_svg_title, title, manager), 
                                asset_svg_content, content, manager)?.OuterXml;

    /// <summary>
    /// Get Asset Resource
    /// </summary>
    /// <param name="target">Colour</param>
    /// <param name="title">Title</param>
    /// <param name="content">Content</param>
    /// <returns>Asset Resource</returns>
    public static AssetResource Get(Color? target = null, string? title = null, string? content = null) =>
        new(FromString(GetAssetResourceString(target, title, content) ?? string.Empty) ?? 
            new MemoryStream(), size, size);

    /// <summary>
    /// Get As Data Uri
    /// </summary>
    /// <param name="target">Colour</param>
    /// <param name="title">Title</param>
    /// <param name="content">Content</param>
    /// <returns>Data Uri</returns>
    public static string? GetAsDataUri(Color? target = null, string? title = null, string? content = null) => 
        Get(target, title, content).GetAsDataUri();

    /// <summary>
    /// Get Asset Primary Path Markup
    /// </summary>
    /// <returns>Asset Resource Path Markup</returns>
    public static string GetPrimaryPathMarkup() => 
        GetAssetPathMarkup(asset_svg_primary);

    /// <summary>
    /// Get Asset Secondary Path Markup
    /// </summary>
    /// <returns>Asset Resource Path Markup</returns>
    public static string GetSecondaryPathMarkup() =>
        GetAssetPathMarkup(asset_svg_secondary);
}

