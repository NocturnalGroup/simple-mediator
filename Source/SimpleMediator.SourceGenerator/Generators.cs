using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace NocturnalGroup.SimpleMediator.SourceGenerator;

[Generator]
public class RequestExtensionGenerator : IIncrementalGenerator
{
	/// <summary>
	/// This regex looks for the IRequest interface, and extracts the type in the generic brackets.
	/// </summary>
	private static readonly Regex RequestInterfaceRegex = new(
		@"^NocturnalGroup\.SimpleMediator\.Abstractions\.Requests\.IRequest<([a-zA-Z0-9.<>]+)>$"
	);

	public void Initialize(IncrementalGeneratorInitializationContext ctx)
	{
		// Filter for types that implement IRequest<TResponse>.
		var possibleRequestDefinitions = ctx.SyntaxProvider.CreateSyntaxProvider(
			predicate: TypeImplementsAnInterface, // Initial fast filter.
			transform: ParseInterfaces // Slower, more accurate filter.
		);

		// Collect all the request declarations into a single collection.
		var requestDefinitions = ctx.CompilationProvider.Combine(
			possibleRequestDefinitions.Where(static m => m is not null).Collect()
		);

		// Generate the extensions file.
		ctx.RegisterSourceOutput(requestDefinitions, static (ctx, source) => GenerateFile(source.Right, ctx));
	}

	/// <summary>
	/// Performs a quick filter for types that implement an interface.
	/// </summary>
	private static bool TypeImplementsAnInterface(SyntaxNode node, CancellationToken _)
	{
		return node
			is ClassDeclarationSyntax { BaseList.Types.Count: not 0 }
				or RecordDeclarationSyntax { BaseList.Types.Count: not 0 }
				or StructDeclarationSyntax { BaseList.Types.Count: not 0 };
	}

	/// <summary>
	/// Performs a deeper filter for types which implement the IRequest interface.
	/// </summary>
	private static RequestDefinition? ParseInterfaces(GeneratorSyntaxContext ctx, CancellationToken _)
	{
		// As this method runs after the quick filter, we can make some assumptions.
		// We know the provided type is a class, record or struct which implements an interface.
		if (ctx.Node is not TypeDeclarationSyntax typeDeclaration)
		{
			return null;
		}

		// Check if the type implements the IRequest interface.
		var interfaceNames = typeDeclaration.BaseList!.Types.Select(t => t.GetNamespacedName(ctx));
		var interfaceMatches = interfaceNames.Select(name => RequestInterfaceRegex.Match(name));
		var matchingInterface = interfaceMatches.FirstOrDefault(match => match.Success);
		if (matchingInterface is null)
		{
			return null;
		}

		// Create and return the request definition.
		return new RequestDefinition(
			requestType: typeDeclaration.GetNamespacedName(),
			responseType: matchingInterface.Groups[1].Value // Pull response type from the Regex matches.
		);
	}

	/// <summary>
	/// Creates the extensions file with the extensions.
	/// </summary>
	private static void GenerateFile(ImmutableArray<RequestDefinition?> definitions, SourceProductionContext ctx)
	{
		// No-op if we don't have any definitions.
		if (definitions.Length is 0)
		{
			ctx.AddSource("NocturnalGroup.SimpleMediator.Requests.g.cs", SourceText.From("", Encoding.UTF8));
			return;
		}

		var builder = new StringBuilder();
		builder.Append("#nullable enable").AppendLine();
		builder.Append("namespace NocturnalGroup.SimpleMediator").AppendLine();
		builder.Append("{").AppendLine();
		builder.AppendIndent().Append("public static partial class SimpleMediatorRequestExtensions").AppendLine();
		builder.AppendIndent().Append("{").AppendLine();
		foreach (var definition in definitions)
		{
			if (definition is null)
				continue;

			// Function Signature
			builder.AppendIndent(2);
			builder.Append("public ");
			builder.Append("static ");
			builder.Append("Task<").Append(definition.ResponseType).Append("> ");
			builder.Append("SendRequestAsync");
			builder.Append("(");
			builder.Append("this NocturnalGroup.SimpleMediator.Abstractions.Requests.IRequestSender sender, ");
			builder.Append(definition.RequestType).Append(" request, ");
			builder.Append("CancellationToken? ct = null");
			builder.Append(")");
			builder.AppendLine();

			// Function Body Start
			builder.AppendIndent(2);
			builder.Append("{").AppendLine();

			// Function Body
			builder.AppendIndent(3);
			builder.Append("return sender.SendRequestAsync<");
			builder.Append(definition.RequestType);
			builder.Append(", ");
			builder.Append(definition.ResponseType);
			builder.Append(">");
			builder.Append("(request, ct);").AppendLine();

			// Function Body End
			builder.AppendIndent(2);
			builder.Append("}").AppendLine();
		}
		builder.AppendIndent().Append("}").AppendLine();
		builder.Append("}").AppendLine();
		builder.Append("#nullable disable").AppendLine();

		// Add the mapping file to the source generator output.
		ctx.AddSource(
			"NocturnalGroup.SimpleMediator.Requests.g.cs",
			SourceText.From(builder.ToString(), Encoding.UTF8)
		);
	}
}
