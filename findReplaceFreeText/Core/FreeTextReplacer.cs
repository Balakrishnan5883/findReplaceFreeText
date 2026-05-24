
using PdfSharp.Pdf;
using PdfSharp.Pdf.Annotations;
using PdfSharp.Pdf.IO;
using System.Globalization;
using PdfSharp.Drawing;
using Models;
using System.IO;
using System.Reflection;
using PdfSharp.Fonts;
using Microsoft.VisualBasic;


namespace Core;

public class EmbeddedFontResolver : IFontResolver
{

    public required string FontResourcePath  ;
    public byte[]? GetFont(string faceName)
    {
        string resourceName = $"{FontResourcePath}.{faceName}.ttf";
        Assembly assembly =Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream(resourceName) 
              ?? throw new FileNotFoundException($"Resource not found {resourceName}");
        using(MemoryStream ms = new())
        {
            stream.CopyTo(ms);
            

            return ms.ToArray();
            
        }
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        if (familyName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
        {
            if (bold && italic) return new FontResolverInfo("arialbi");
            if (bold) return new FontResolverInfo("arialbd");
            if (italic) return new FontResolverInfo("ariali");
            return new FontResolverInfo("arial");
        }
        
        else if (familyName.Equals("Times New Roman", StringComparison.OrdinalIgnoreCase))
        {
            if (bold && italic) return new FontResolverInfo("timesbi");
            if (bold) return new FontResolverInfo("timesbd");
            if (italic) return new FontResolverInfo("timesi");
            return new FontResolverInfo("times");
        }


        return new FontResolverInfo("arial");
    }
}

public class FreeTextReplacer
{
    public FreeTextReplacer(string fontResourcePath)
    {
        GlobalFontSettings.FontResolver = new EmbeddedFontResolver()
        {
            FontResourcePath = fontResourcePath
        };
    }
        public static List<PdfInputStore> TransformInputStringToRequiredObjects(string[] inputStrings)
    {
        List<PdfInputStore> output = [];
        foreach(string pdfString in inputStrings)
        {
            if (!pdfString.Contains('|'))
            {
                throw new ArgumentException($"invalid input argument {pdfString} character | missing");
                
            }

            string[] splittedPdfInputs =pdfString.Split('|');

            if(splittedPdfInputs.Length<3)
            {
                throw new ArgumentException($"invalid input argument {pdfString} expecting 2 or more | characters");
   
            }

            string sourcePdfPath =   splittedPdfInputs[0];

            if (Path.Exists(sourcePdfPath)==false)
            {
                throw new FileNotFoundException($"Invalid input argument {sourcePdfPath} source pdf path is not found");
                
            }

            string destinationPdfFilePath =   splittedPdfInputs[1];
            string destinationPdfFolderPath =Path.GetDirectoryName(destinationPdfFilePath)??throw new DirectoryNotFoundException(destinationPdfFilePath);
            DirectoryInfo destinationFolderPathObj = new(destinationPdfFolderPath );

            if (destinationFolderPathObj.Exists==false)
            {
                if(destinationFolderPathObj.Parent==null ||destinationFolderPathObj.Parent.Exists == false)
                {
                    throw new DirectoryNotFoundException($"invalid input argument {destinationPdfFilePath} destination folder path is not found");
                }
                else
                {
                    destinationFolderPathObj.Create();
                }
            }
            Dictionary<string,string> findReplaceTextPairs = [];
            for (int i = 2 ; i<splittedPdfInputs.Length;i += 2)
            {

                string findText = splittedPdfInputs[i];
                string replaceText;

                if(i+1>=splittedPdfInputs.Length)
                {
                    Console.WriteLine($"Skipping leftover input find text ({findText}) without replace pair");
                    break;
                }
                replaceText = splittedPdfInputs[i+1];   
                if (findReplaceTextPairs.ContainsKey(findText))
                {
                    Console.WriteLine($"Skipping duplicated find text {findText}");
                    continue;
                }
                findReplaceTextPairs.Add(findText,replaceText);

            }

            if (findReplaceTextPairs.Count == 0)
            {
                throw new ArgumentException($"No find replace text pairs found or all invalid pairs for source pdf {sourcePdfPath}");
            }

            output.Add(new PdfInputStore()
            {
                SourcePdfPath = sourcePdfPath,
                DestinationPdfPath = destinationPdfFilePath,
                FindReplaceTextPairs = findReplaceTextPairs
                
            });
                

        }
        if (output.Count==0)
        {
            throw new ArgumentException($"No valid strings found in the received arguments");
        }
        return output;

    }


    public static void FindReplaceFreeTextPdf(string sourcePdfPath, Dictionary<string,string> findReplaceStringPairs,
                                string destinationPdfPath)
    {
        static (XFont font,XBrush brush) ParseDefaultAppeareance(string defaultAppeareanceString)
        {
            string fontName = "Arial";
            double fontSize = 12;
            XColor textColor = XColors.Black;
            if(string.IsNullOrWhiteSpace(defaultAppeareanceString))
            {
                return(new XFont(fontName,fontSize,XFontStyleEx.Regular),new XSolidBrush(textColor));     
            }
            string[] parts = defaultAppeareanceString.Split([' '],StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0 ; i<parts.Length; i++)
            {
                if (parts[i]=="Tf" && i>=2)
                {
                    if (double.TryParse(parts[i-1],NumberStyles.Any,CultureInfo.InvariantCulture,out double parsedSize))
                    {
                        fontSize = parsedSize;
                    }
                    string pdfFont = parts[i-2];
                    if (pdfFont.Contains("Helv")) fontName = "Arial";
                    else if (pdfFont.Contains("Times")) fontName = "Times New Roman";
                    else if (pdfFont.Contains("Cour")) fontName = "Courier New";
                }
                else if(parts[i]=="rg" && i>=3)
                {
                    double r = double.Parse(parts[i-3],CultureInfo.InvariantCulture);
                    double g = double.Parse(parts[i-2],CultureInfo.InvariantCulture);
                    double b = double.Parse(parts[i-1],CultureInfo.InvariantCulture);

                    textColor = XColor.FromArgb((int)r*255,(int)(g*255),(int)(b*255));

                }
                else if (parts[i]=="g" && i >=1)
                {
                    double gray = double.Parse(parts[i-1],CultureInfo.InvariantCulture);
                    int grayValue = (int)(gray*255);
                    textColor = XColor.FromArgb(grayValue,grayValue,grayValue);
                }
            }
            return(new XFont(fontName,fontSize,XFontStyleEx.Regular),new XSolidBrush(textColor));
        }
        
        PdfDocument sourcePdfObject = PdfReader.Open(sourcePdfPath, PdfDocumentOpenMode.Modify);
        Console.WriteLine ($"Processing {sourcePdfPath}");
        foreach (PdfPage page in sourcePdfObject.Pages)
        {
            if (page.Annotations.Count == 0)
            {
                //Console.WriteLine($"No annotations found on page ");
                continue;
            }

            for (int i = page.Annotations.Count-1;i>=0;i--)
            {

                PdfAnnotation annotation = page.Annotations[i];
                var subtype = annotation.Elements.GetString("/Subtype");
                if (subtype != "/FreeText")
                {
                    continue;
                }
                string contents = annotation.Elements.GetString("/Contents");
                if (contents == null || !findReplaceStringPairs.TryGetValue(contents, out string? newContents))
                {
                    continue;
                }
                PdfRectangle pdfRectangle = annotation.Rectangle;
                XRect drawRectangle = new(
                    pdfRectangle.X1, 
                    page.Height.Point - pdfRectangle.Y2,
                    pdfRectangle.Width, 
                    pdfRectangle.Height
                );
                string DAstring = annotation.Elements.GetString("/DA");
                (XFont font,XBrush brush) = ParseDefaultAppeareance(DAstring);
                using(  XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    XStringFormat format = new()
                    {
                        Alignment = XStringAlignment.Near,
                        LineAlignment=XLineAlignment.Near
                    };
                    gfx.DrawString(newContents,font,brush,drawRectangle,format);

                }
                page.Annotations.Remove(annotation);

            }
        }
        Console.WriteLine($"Writing to {destinationPdfPath}");
        sourcePdfObject.Save(destinationPdfPath);
        Console.WriteLine(string.Concat(Enumerable.Repeat('=', 100)));
    }
}