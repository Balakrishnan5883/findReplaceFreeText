


namespace TestFindReplaceFreeText;
using FindReplaceFreeText;

public class Common
{

    
    [Fact]
    public void Test_runs_without_errors()
    {

        string sampleFolderPath = Path.Combine(@"G:\My Drive\Projects\C# Projects\Control PDF\FindReplaceFreeText","Sample");
        string inputPdf1 = Path.Combine(sampleFolderPath,"drawings.pdf");
        string inputPdf2 = Path.Combine(sampleFolderPath,"Quotation.pdf");
        
        string tempPath = Path.Combine(Path.GetTempPath(),"DotNetBuilds","findReplaceFreeText");
        string outputPdf1 = Path.Combine(tempPath, "DN12345.pdf");
        string outputPdf2 = Path.Combine(tempPath, "12345.pdf");

        string[] arguments = [@$"{inputPdf1}|{outputPdf1}|STACK_X|20|STACK_Y|460|STACK_Z|255|STACK_DN|DN12345-03|STACK_QTY|12|BASE_X|500|BASE_Y|275|BASE_Z|20|BASE_L|1000|BASE_DN|DN12345-02|BASE_QTY|4|SHELF_X|240|SHELF_Y|500|SHELF_H|1000|SHELF_DN|DN12345|SHELF_QTY|4",
                            
                            @$"{inputPdf2}|{outputPdf2}|QUOTE_NO|12345|HEIGHT|1000|WIDTH|500|DEPTH|275|VOLUME|0.017548|QTY|4|UNIT_COST|19.894|TOTAL_COST|79.576|GRAND_TOTAL|85|TAX|0.08"]; 


        Program.MainProcedure(arguments);
    }
}
