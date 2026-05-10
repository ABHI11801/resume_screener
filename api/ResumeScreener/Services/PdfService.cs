using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace ResumeScreener.Services;

public class PdfService
{
    public string ExtractText(string filePath)
    {
        string text = string.Empty;

        using (PdfReader reader = new PdfReader(filePath))
        using (PdfDocument pdf = new PdfDocument(reader))
        {
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                text += PdfTextExtractor.GetTextFromPage(
                    pdf.GetPage(i)
                );
            }
        }

        return text;
    }
}