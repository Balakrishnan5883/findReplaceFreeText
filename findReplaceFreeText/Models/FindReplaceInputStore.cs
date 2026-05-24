namespace Models;

public class PdfInputStore()
{
    public required string SourcePdfPath;
    public required string DestinationPdfPath;

    public required Dictionary<string,string> FindReplaceTextPairs ;
    
}