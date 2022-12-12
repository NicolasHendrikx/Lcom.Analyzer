using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Lcom.Analyzer.Test.CSharpAnalyzerVerifier<Lcom.Analyzer.LcomAnalyzer>;

namespace Lcom.Analyzer.Test;

[TestClass]
public class TooManyFieldsAnalyzerTests
{
    //No diagnostics expectedAlert to show up
    [TestMethod]
    public async Task WarnsTooManyFieldsWithFields()
    {
        var test = @"
    namespace ConsoleApplication1
    {
        class Test
        {   
            internal int F1 = 1 ;
            internal int F2 = 2 ;
            internal int F3 = 3 ;
            internal int F4 = 4 ;
            internal int F5 = 5 ;
            internal int F6 = 6 ;
        }
    }";

        var expectedAlert = VerifyCS.Diagnostic("NH002").WithLocation(4, 9).WithArguments("Test", 6);

        await VerifyCS.VerifyAnalyzerAsync(test, expectedAlert);
    }

    [TestMethod]
    public async Task WarnsTooManyFieldsWithAutoImplementedProperties()
    {
        var test = @"
    namespace ConsoleApplication1
    {
        class Test
        {   
            internal int F1 {get;set;}
            internal int F2 {get;set;}
            internal int F3 {get;set;}
            internal int F4 {get;set;}
            internal int F5 {get;set;}
            internal int F6 {get;set;}
        }
    }";

        var expected = VerifyCS.Diagnostic("NH002").WithLocation(4,9).WithArguments("Test", 6);
        await VerifyCS.VerifyAnalyzerAsync(test, expected);
    }


    [TestMethod]
    public async Task WarnsTooManyFieldsWithBothFieldsAndAutoProperties()
    {
        var test = @"
    namespace ConsoleApplication1
    {
        class Test
        {   
            const int C1 = 1;
            static readonly string CF1 = string.Empty;

            internal int F1 = 1 ;
            internal int F2 = 2 ;
            internal int F3 = 3 ;
            internal int F4 {get;set;}
            internal int F5 {get;set;}
            internal int F6 {get;set;}
        }
    }";

        var expected = VerifyCS.Diagnostic("NH002").WithLocation(4, 9).WithArguments("Test", 6);
        await VerifyCS.VerifyAnalyzerAsync(test, expected);
    }
}

//No diagnostics expectedAlert to show up
