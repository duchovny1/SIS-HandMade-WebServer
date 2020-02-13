using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Reflection;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework
{
    public class ViewEngine : IViewEngine
    {
        public string GetHtml(string template, object model)
        {
            var methodCode = PrepareCSharpCode(template);
            var typeName = model?.GetType().FullName ?? "";
            if (model?.GetType().IsGenericType == true)
            {
                typeName = model.GetType().Name.Replace("`1", "") + "<"
                    + model.GetType().GenericTypeArguments.First() + ">";
            }
            string code = $@"using system;
using System.Text;
using System.Linq;
using System.Collection.Generic;
namespace AppViewNameSpace
{{
     public class AppViewCode : IView
       {{
              public string GetHtml(object model)
              {{
                   var Model = model as {typeName};
                   var html = new StringBuilder();
                   object User = null;
                    {methodCode}

                    return html.ToString();
              }}


       }}
}}";

            IView view = GetInstanceFromCode(code, model);
            string html = view.GetHtml(model);
            return html;
        }

        private IView GetInstanceFromCode(string code, object model)
        {
            var compilation = CSharpCompilation.Create("AppViewAssembly").WithOptions
                 (
                  new CSharpCompilationOptions
                  (
                    OutputKind.DynamicallyLinkedLibrary
                   )
                )
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            if (model != null)
            {

                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));
            }

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compilation.AddReferences
                    (MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));
            using var memoryStream = new MemoryStream();
            var compilationResult = compilation.Emit(memoryStream);
            if (!compilationResult.Success)
            {
                return new ErrorView(compilationResult.Diagnostics
                    .Where(x => x.Severity == DiagnosticSeverity.Error)
                    .Select(x => x.GetMessage()).ToList());

            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            var assemblyByteArray = memoryStream.ToArray();
            var assembly = Assembly.Load(assemblyByteArray);
            var type = assembly.GetType("AppViewNameSpace.AppViewCode");
            var instance = Activator.CreateInstance(type) as IView;
            return instance;

        }

        private object PrepareCSharpCode(string template)
        {
            var cSharpExpressionRegex = new Regex(@"[^\<\""\s]+", RegexOptions.Compiled);
            var supportedOperators = new[] { "if", "for", "foreach", "else" };

            StringBuilder cSharpCode = new StringBuilder();

            StringReader reader = new StringReader(template);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.TrimStart().StartsWith("{") || line.TrimStart().EndsWith("}"))
                {
                    cSharpCode.Append(line);
                }
                else if (supportedOperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    var indexOfAt = line.IndexOf("@");
                    line = line.Remove(indexOfAt, 1);
                    cSharpCode.AppendLine(line);
                }
                else
                {
                    var currentCSharpLine = new StringBuilder("html.AppendLine(@\"");
                    while (line.Contains("@"))
                    {
                        var atLocation = line.IndexOf("@");
                        var before = line.Substring(0, atLocation);
                        currentCSharpLine.Append(before.Replace("\"", "\"\"") + "\" + ");
                        var cSharpAndEndOfLine = line.Substring(atLocation + 1);
                        var cSharpExpression = cSharpExpressionRegex.Match(cSharpAndEndOfLine);
                        currentCSharpLine.Append(cSharpExpression.Value + " + @\"");
                        var after = cSharpAndEndOfLine.Substring(cSharpAndEndOfLine.Length);
                        line = after;
                        //before
                        //csharp code => + c# code + 
                        //line = after
                    }
                    currentCSharpLine.Append(line.Replace("\"", "\"\"") + "\");");
                    cSharpCode.AppendLine(currentCSharpLine.ToString());
                }


            }


            return cSharpCode.ToString();
        }
    }
}
