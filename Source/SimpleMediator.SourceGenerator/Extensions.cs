using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NocturnalGroup.SimpleMediator.SourceGenerator;

internal static class StringExtensions
{
	/// <summary>
	/// Adds an indentation to the <see cref="StringBuilder"/>.
	/// </summary>
	public static StringBuilder AppendIndent(this StringBuilder builder, int amount = 1)
	{
		return builder.Append(new string(' ', amount * 4));
	}
}

internal static class RoslynExtensions
{
	/// <summary>
	/// Gets the name of the interface, prefixed with its namespace.
	/// </summary>
	public static string GetNamespacedName(this BaseTypeSyntax baseTypeSyntax, GeneratorSyntaxContext ctx)
	{
		return ctx.SemanticModel.GetSymbolInfo(baseTypeSyntax.Type).Symbol?.ToDisplayString() ?? "";
	}

	/// <summary>
	/// Gets the name of the type, prefixed with its namespace.
	/// </summary>
	public static string GetNamespacedName(this BaseTypeDeclarationSyntax baseTypeDeclaration)
	{
		// Types can be contained inside other types.
		// So we need to walk through every parent to find the full namespace.
		var nameBuilder = new StringBuilder(baseTypeDeclaration.Identifier.ToString());
		var currentParent = baseTypeDeclaration.Parent;

		// Work back through all the parent types.
		while (currentParent is BaseTypeDeclarationSyntax parentSyntax)
		{
			nameBuilder.Insert(0, '.');
			nameBuilder.Insert(0, parentSyntax.Identifier.ToString());
			currentParent = parentSyntax.Parent;
		}

		// Work back through the namespaces
		while (currentParent is BaseNamespaceDeclarationSyntax namespaceParent)
		{
			nameBuilder.Insert(0, '.');
			nameBuilder.Insert(0, namespaceParent.Name.ToString());
			currentParent = namespaceParent.Parent;
		}

		// Combine all the parts
		return nameBuilder.ToString();
	}
}
