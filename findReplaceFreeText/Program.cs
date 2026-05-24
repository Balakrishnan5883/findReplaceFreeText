
using Core;
using Models;
using static Core.FreeTextReplacer;
using System.Reflection;


namespace FindReplaceFreeText;


public static class Program
{
    
    public static void MainProcedure(string[]args)
    {
        string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();

        var _ = new FreeTextReplacer(fontResourcePath:"FindReplaceFreeText.Resources.Fonts");
        if(args.Length == 0)
        {
            throw new ArgumentNullException(nameof(args));
        }
        List<PdfInputStore> pdfData;
        pdfData = TransformInputStringToRequiredObjects(args);
        foreach(PdfInputStore data in pdfData)
        {
            
            FindReplaceFreeTextPdf(sourcePdfPath:data.SourcePdfPath,destinationPdfPath:data.DestinationPdfPath,
                                    findReplaceStringPairs:data.FindReplaceTextPairs);
        }
        
    }

    public static void Main(string[] args)
    {
        try
        {
            MainProcedure(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[{ex.GetType().Name}]-{ex.Message}");
            return ;
        }
    }
    
}

