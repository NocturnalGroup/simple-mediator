<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="center" style="list-style: none;">
    <summary>
      <h1>SimpleMediator</h1>
    </summary>
  </ul>
</div>

### A dead simple mediator implementation

[About](readme.md) · [Getting Started](tutorial.md) · [License](license.txt) · Contributing

---

Thank you for your interest in contributing to SimpleMediator!

## Initial Setup

1. Download and install the .NET 9 SDK:

- Instructions can be found [here](https://learn.microsoft.com/en-us/dotnet/core/install/).

2. If you don't have contributor access to this repository:

- Fork the repository on GitHub
- Clone your fork: `git clone https://github.com/{username}/{repo-name}.git`

3. If you have contributor access:

- Clone directly: `git clone https://github.com/NocturnalGroup/simple-mediator.git`

4. From the repository root, restore the required .NET tools:

```bash
dotnet tool restore
```

## Code Style

We maintain a consistent code style across the project using several tools:

### EditorConfig

At the root of the project there is an `.editorconfig` file that helps ensure consistent:

- Indentation
- Line endings
- Character encoding

Please make sure you have the EditorConfig plugin installed in your editor:

- [Rider (Included)](https://plugins.jetbrains.com/plugin/7294-editorconfig/)
- [Visual Studio (Included)](https://learn.microsoft.com/en-us/visualstudio/releasenotes/vs2017-relnotes-v15.0#coding-convention-support-through-editorconfig)
- [VS Code](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig)
- [Other Editors](https://editorconfig.org/#download)

### CSharpier

We use CSharpier to ensure consistent C# code formatting.
You can format the entire project by running:

```bash
dotnet csharpier .
```

Or format a specific file:

```bash
dotnet csharpier YourFile.cs
```

For the best development experience, we recommend installing the CSharpier plugin for your editor:

- [Rider](https://plugins.jetbrains.com/plugin/18243-csharpier)
- [Visual Studio](https://marketplace.visualstudio.com/items?itemName=csharpier.CSharpier)
- [VS Code](https://marketplace.visualstudio.com/items?itemName=csharpier.csharpier-vscode)
- [Other Editors](https://csharpier.com/docs/Editors)

## General Workflow

1. Create a branch for your changes.
2. Make your changes.
3. Validate your changes:

```bash
dotnet test
dotnet csharpier .
```

4. Commit your changes (see Commit Guidelines below).
5. Push your changes.
6. Open a Pull Request to the `main` branch.

## Commit Guidelines

### Message Format

We follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification for our commit messages.
Each commit message should match the following format:

```
{type}({scope}): {description}
```

#### Types

We use a simplified set of commit types.
The available types are:

| Type     | Description                          |
| -------- | ------------------------------------ |
| build    | All build system changes.            |
| feat     | Code changes that add a new feature. |
| fix      | Code changes that fix a bug.         |
| refactor | All other code changes.              |
| docs     | All documentation changes.           |
| repo     | All other changes.                   |
| revert   | Reverting a previous commit.         |

#### Scope (Optional)

In most cases, it's a good idea to provide a scope.
A scope helps people to identify which section of the codebase your commit makes changes to.

```
# Without scope
feat: add endpoint for user preferences

# With Scope
feat(api): add an endpoint for user preferences
```

#### Breaking Changes

If your commit includes a breaking change, you should signify this by adding a `!` after the scope.

```
{type}({scope})!: {description}
```

#### Description

The description of your commit should be written in the imperative mood.
An easy way to achieve this is to complete the following sentence:

```
If applied, this commit will {description}
```

#### Examples

```
feat(api): add an endpoint for user preferences
fix(auth): resolve token validation issue
docs: update installation instructions
```

### Focused Commits

Commits should be focused and address a single logical change.
Multipurpose commits can be harder to understand and revert.
Please keep commits reasonably sized and focused on one subject.

For example, instead of combining multiple changes into a single commit like this:

```
fix(users): add last name field to search endpoint and changed its ratelimit
```

It's better to break them down into separate, focused commits:

```
feat(users): add support for querying by last name
fix(users): increase the query endpoint rate limit allowance
```

## Questions or Problems?

If you have any questions or run into issues, please feel free to open an issue.
We're happy to help!

Thanks again for your interest in contributing!
We look forward to your contributions!
