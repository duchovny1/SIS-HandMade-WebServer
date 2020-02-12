using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Reflection;
using System.IO;

namespace SIS.MvcFramework
{
    public class ViewEngine : IViewEngine
    {
        public string GetHtml(string template, object model)
        {
            var methodCode = PrepareCSharpCode(template);
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
                   var html = new StringBuilder();

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
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));
            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var library in libraries)
            {
                compilation.AddReferences
                    (MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));
            using var memoryStream = new MemoryStream();
            compilation.Emit(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var assemblyByteArray = memoryStream.ToArray();
            var assembly = Assembly.Load(assemblyByteArray);
            var type = assembly.GetType("AppViewNameSpace.AppViewCode");
            var instance =  Activator.CreateInstance(type) as IView;
            return instance;

        }

        private object PrepareCSharpCode(string template)
        {
            return "";
        }
    }
}
