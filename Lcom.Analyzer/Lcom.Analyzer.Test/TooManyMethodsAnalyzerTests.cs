using Microsoft;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VerifyCS = Lcom.Analyzer.Test.CSharpAnalyzerVerifier<Lcom.Analyzer.LcomAnalyzer>;

namespace Lcom.Analyzer.Test;

[TestClass]
public class TooManyMethodsAnalyzerTests
{
    [TestMethod]
    public async Task WithMethodsPropsAndConstructor()
    {
        string test = @"
    namespace ConsoleApplication1
    {
        class Test
        {   
            public Test() {}

            private int f1 = 0;
            public int P1 
            {
                get => f1;
                set => f1 = value;
            }

            private int f2 = 0;
            public int P2 
            {
                get => f2;
                set => f2 = value;
            }

            private int f3 = 0;
            public int P3 
            {
                get => f3;
                set => f3 = value;
            }

            private int f4 = 0;
            public int P4 
            {
                get => f4;
                set => f4 = value;
            }

            private int f5 = 0;
            public int P5 
            {
                get => f5;
                set => f5 = value;
            }

            void M1() {}
            void M2() {}
            void M3() {}
            void M4() {}
            void M5() {}
            void M6() {}
            void M7() {}
            void M8() {}
            void M9() {}
            void M10() {}
        }
    }";
        var tooManyMethods = VerifyCS.Diagnostic("NH003").WithLocation(4, 9).WithArguments("Test", 21);
        var lcom1 = VerifyCS.Diagnostic("NH004").WithLocation(4, 9).WithArguments("Test", 0.90);
        
        await VerifyCS.VerifyAnalyzerAsync(test, tooManyMethods,lcom1);
    }

    [TestMethod]
    public async Task WithMethodsRecord()
    {
        string test = @"
    namespace ConsoleApplication1
    {
        public record Test(int P1, int P2, int P3, int P4, int P5, int P6);
    }";
        var analyzerTest = new CSharpNextAnalyserTest<LcomAnalyzer, MSTestVerifier>
        {
            TestState =
            {
                Sources = { test },
                ReferenceAssemblies = new ReferenceAssemblies(
                    "net6.0",
                    new PackageIdentity(
                        "Microsoft.NETCore.App.Ref", "6.0.0"),
                        Path.Combine("ref", "net6.0"))
            }
            
        };

        await analyzerTest.RunAsync();
    }
}

public class CSharpNextAnalyserTest<TAnalyzer, TVerifier> : CSharpAnalyzerTest<TAnalyzer, TVerifier>
    where TAnalyzer : DiagnosticAnalyzer, new ()
    where TVerifier : IVerifier, new ()
{
    protected override CompilationOptions CreateCompilationOptions()
    => new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);

    protected override ParseOptions CreateParseOptions()
        => new CSharpParseOptions(LanguageVersion.LatestMajor, DocumentationMode.Diagnose);
}