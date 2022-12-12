using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Lcom.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LcomAnalyzer : DiagnosticAnalyzer
    {
        public const int FieldsThreshold = 5;
        public const int DynamicsThreshold = 20; 
        public const float Lcom1Threshold = 0.8f;

        public const string TooManyFieldsDiagnosticId = "NH002";
        public const string TooManyMethodsDiagnosticId = "NH003";
        public const string LcomDiagnosticId = "NH004";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for TooManyFieldsTitle and TooManyFieldsMessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString TooManyFieldsTitle = new LocalizableResourceString(nameof(Resources.TooManyFieldsTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString TooManyFieldsMessageFormat = new LocalizableResourceString(nameof(Resources.TooManyFieldsMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString TooManyFieldsDesc = new LocalizableResourceString(nameof(Resources.TooManyFieldsId), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString TooManyMethodsTitle = new LocalizableResourceString(nameof(Resources.TooManyMethodsTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString TooManyMethodsMessageFormat = new LocalizableResourceString(nameof(Resources.TooManyMethodsMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString TooManyMethodsDesc = new LocalizableResourceString(nameof(Resources.TooManyMethodsId), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString LcomTitle = new LocalizableResourceString(nameof(Resources.LcomTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString LcomMessageFormat = new LocalizableResourceString(nameof(Resources.LcomMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString LcomDesc = new LocalizableResourceString(nameof(Resources.LcomId), Resources.ResourceManager, typeof(Resources));

        private const string Category = "Maintenability";

        private static readonly DiagnosticDescriptor TooManyFieldsRule = new DiagnosticDescriptor(TooManyFieldsDiagnosticId, TooManyFieldsTitle, TooManyFieldsMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: TooManyFieldsDesc);
        private static readonly DiagnosticDescriptor TooManyMethodsRule = new DiagnosticDescriptor(TooManyMethodsDiagnosticId, TooManyMethodsTitle, TooManyMethodsMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: TooManyMethodsDesc);
        private static readonly DiagnosticDescriptor LcomRule = new DiagnosticDescriptor(LcomDiagnosticId, LcomTitle, LcomMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: LcomDesc);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(TooManyFieldsRule, TooManyMethodsRule, LcomRule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            //
            context.RegisterSyntaxNodeAction(
                syntaxAnalysisContext => ComputeLcomForClass(syntaxAnalysisContext.Node as BaseTypeDeclarationSyntax, syntaxAnalysisContext),
                SyntaxKind.ClassDeclaration);

            context.RegisterSyntaxNodeAction(
                syntaxAnalysisContext => ComputeLcomForClass(syntaxAnalysisContext.Node as BaseTypeDeclarationSyntax, syntaxAnalysisContext),
                SyntaxKind.StructDeclaration);

            context.RegisterSyntaxNodeAction(
                syntaxAnalysisContext => ComputeLcomForClass(syntaxAnalysisContext.Node as BaseTypeDeclarationSyntax, syntaxAnalysisContext),
                SyntaxKind.RecordDeclaration);
        }

        private void ComputeLcomForClass(BaseTypeDeclarationSyntax typeDeclarationSyntax, SyntaxNodeAnalysisContext syntaxAnalysisContext)
        {
            if (typeDeclarationSyntax.Modifiers.Any(token => token.IsKind(SyntaxKind.AbstractKeyword)))
            {
                return;
            }

            if (!typeDeclarationSyntax.SemicolonToken.FullSpan.IsEmpty)
            {
                return;
            }

            var fieldsIdentifiers = GetFieldsIdentifiers(typeDeclarationSyntax);

            if (fieldsIdentifiers.Count > FieldsThreshold)
            {
                var diagnostic = Diagnostic.Create(
                    TooManyFieldsRule,
                    typeDeclarationSyntax.GetLocation(),
                    typeDeclarationSyntax.Identifier,
                    fieldsIdentifiers.Count
                );
                syntaxAnalysisContext.ReportDiagnostic(diagnostic);
            }

            var bodies = GetMethodBodies(typeDeclarationSyntax);

            if (bodies.Count > DynamicsThreshold)
            {
                var diagnostic = Diagnostic.Create(
                    TooManyMethodsRule,
                    typeDeclarationSyntax.GetLocation(),
                    typeDeclarationSyntax.Identifier,
                    bodies.Count
                );
                syntaxAnalysisContext.ReportDiagnostic(diagnostic);
            }

            float sumOfFields = fieldsIdentifiers.Select(
                id => bodies.Count(body => body.ToString().Contains(id.Text))).Sum();
            int dataTimesMethod = bodies.Count * fieldsIdentifiers.Count;
            float com1 = dataTimesMethod == 0 ? float.PositiveInfinity: sumOfFields/dataTimesMethod;
            float lcom1 = 1 - com1;

            if (lcom1 >= Lcom1Threshold)
            {
                var diagnostic = Diagnostic.Create(
                    LcomRule,
                    typeDeclarationSyntax.GetLocation(),
                    typeDeclarationSyntax.Identifier,
                    ((int)(lcom1*100))/100.0f
                );
                syntaxAnalysisContext.ReportDiagnostic(diagnostic);
            }
        }

        private List<SyntaxToken> GetFieldsIdentifiers(BaseTypeDeclarationSyntax classDeclarationSyntax)
        {
            var fields = classDeclarationSyntax
                .DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(fds => fds.Modifiers.All(t => !t.IsKind(SyntaxKind.StaticKeyword)))
                .Where(fds => fds.Modifiers.All(t => !t.IsKind(SyntaxKind.ConstKeyword)))
                .SelectMany(fds => fds.Declaration.Variables.Select(declaration => declaration.Identifier));

            var properties = classDeclarationSyntax
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(fds => fds.AccessorList != null && IsAutoImplemented(fds.AccessorList.Accessors))
                .Select(fds => fds.Identifier);

            return fields.Concat(properties).ToList();
        }

        private List<string> GetMethodBodies(BaseTypeDeclarationSyntax classDeclarationSyntax)
        {
            IEnumerable<string> methodBodies = classDeclarationSyntax
                .DescendantNodes()
                .OfType<BaseMethodDeclarationSyntax>()
                .Where(mds => mds.Modifiers.All(token => !token.IsKind(SyntaxKind.StaticKeyword)))
                .Select(mds => mds.ToString());

            IEnumerable<string> propertiesBodies = classDeclarationSyntax
                .DescendantNodes()
                .OfType<BasePropertyDeclarationSyntax>()
                .Where(pds => pds.Modifiers.All(token => !token.IsKind(SyntaxKind.StaticKeyword)))
                .Where(pds => pds.AccessorList == null || !IsAutoImplemented(pds.AccessorList.Accessors))
                .SelectMany(pds => pds.AccessorList?.Accessors.Select(a => a.ToString()) ?? new[] { pds.ToString() });
                
            return methodBodies.Concat(propertiesBodies).ToList();
        }

        private bool IsAutoImplemented(SyntaxList<AccessorDeclarationSyntax> accessors)
            => !accessors.Any(a => a.Body != null || a.ExpressionBody != null);
    }
}
