using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Annotations;
using PdfSharp.Pdf.IO;
using System.Text;

string sourcePdfPath = @"G:\My Drive\Projects\C# Projects\control PDF\sample PDF\metamorphosis.pdf";
string outputPdfPath = @"G:\My Drive\Projects\C# Projects\control PDF\sample PDF\output.pdf";

PdfDocument sourcePdfObject = PdfReader.Open(sourcePdfPath, PdfDocumentOpenMode.Modify);

foreach (PdfPage page in sourcePdfObject.Pages)
{
    if (page.Annotations.Count == 0)
    {
        Console.WriteLine("No annotations found");
        continue;
    }

    foreach (PdfAnnotation annotation in page.Annotations)
    {
        var subtype = annotation.Elements.GetString("/Subtype");
        if (subtype == "/FreeText")
        {
            // Update the contents of the free text annotation
            annotation.Elements.SetValue("/Contents", new PdfString("hii"));

            Console.WriteLine(annotation.Elements.GetValue("/Contents"));

            if (annotation.Elements["/AP"] is PdfDictionary appearanceDictionary &&
                appearanceDictionary.Elements["/N"] is PdfObjectStream normalAppearance)
            {
                // Read the existing appearance stream content
                string existingContent = Encoding.ASCII.GetString(normalAppearance.Stream.Value);

                Console.WriteLine(existingContent);

                // Replace the old text with the new text
                string updatedContent = existingContent.Replace("hello", "hii");

                Console.WriteLine(updatedContent);

                // Write the updated content back to the appearance stream
                normalAppearance.Stream.Value = Encoding.ASCII.GetBytes(updatedContent);

                // Update the appearance stream dictionary
                appearanceDictionary.Elements.SetReference("/N", normalAppearance);
            }
        }
    }
}

sourcePdfObject.Save(outputPdfPath);
