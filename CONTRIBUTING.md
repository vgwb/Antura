# Contributing to Antura

We are keen for developers to contribute to open source projects to
keep them great!

Here are a few guidelines that we need contributors to
follow so that we can have a chance of keeping things on top.

## Getting Started

- Make sure you have a GitHub account.
- Create a new issue on the GitHub repository, providing one does not already exist.
- Clearly describe the issue including steps to reproduce it when it is a bug (fill out the issue template).
- Make sure you fill in the earliest version that you know has the issue.
- (Eventually) Fork the repository on GitHub.

## Making Changes

- Create a topic branch from where you want to base your work.
- Name your branch with the type of issue you are fixing; `feat`, `chore`, `docs`.
- Please avoid working directly on your master branch.
- Make sure you set the `Asset Serialization` mode in `Unity->Edit->Project Settings->Editor` to `Force Text`.
- Make commits of logical units.
- Make sure your commit messages are in the proper format. See below for further details.

Following the above method will ensure that all bug fixes are pushed to the `dev` branch while all new features will be pushed to the relevant next release branch. This means that patch releases are much easier to do as the `dev` branch will only contain bug fixes and will be used to fork into new patch releases. Master will then be rebased into the relevant next release branch so the next release  contains the updated bug fixes from the previous patch release.

## Coding Conventions

To ensure all code is consistent and readable, we adhere to the default coding conventions used in Visual Studio. The easiest first step is to auto format the code within Visual Studio with the key combination of `Ctrl + k` then `Ctrl + d` which will ensure the code is correctly formatted and indented.

Spaces should be used instead of tabs for better readability across a number of devices (e.g. spaces look better on Github source view.)

In regards to naming conventions we also adhere to the standard .NET Framework naming convention system which can be [viewed online here](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/general-naming-conventions)

the only difference is that we prefer to have the `if` and `for` statements with inline { like

    ```
    if (this == that) {
      do;
    } else {
      other;
    }

    for () {

    }
    ```

Class methods and parameters should always denote their accessibility
level using the `public` `protected` `private` keywords.

**Incorrect:**

```c#
  void MyMethod()
```

**Correct:**

```c#
  private void MyMethod()
```

All core classes should be within the `Antura` namespace.

Parameters should be defined at the top of the class before any methods are defined.

It is acceptable to have multiple classes defined in the same file as long as the subsequent defined classes are only used by the main class defined in the same file.

Where possible, the structure of the code should also flow with the accessibility level of the method or parameters. So all `public` parameters and methods should be defined first, followed by `protected` parameters and methods with `private` parameters and methods being defined last.

Blocks of code such as conditional statements and loops must always contain the block of code in braces `{ }` even if it is just one line.

**Incorrect:**

```c#
if (this == that) { do; }
```

**Correct:**

```c#
  if (this == that) {
    do;
  }
```

Any method or variable reference should have the most simplified name as possible, which means no additional references should be added where it's not necessary.

`this.transform.rotation` is simplified to `transform.rotation`

`GameObject.FindObjectsOfType` is simplified to `FindObjectsOfType`

All MonoBehaviour inherited classes that implement a MonoBehaviour [Message](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html) method must at least be `protected virtual` to allow any further inherited class to override and extend the methods.

## Documentation

All core scripts, abstractions, controls and prefabs should contain inline code documentation adhering to the .NET Framework XML documentation comments convention which can be [viewed online here](https://msdn.microsoft.com/en-us/library/b2s063f7.aspx)

Public classes, methods, delegate events and unity events should be documented using the XML comments and contain a 1 line `<summary>` with any additional lines included in `<remarks>`.

Public parameters that appear in the inspector do not need XML comments and just require a `[Tooltip("")]` which is used to generate the documentation. However, other public class variables that are hidden from the inspector do need XML style comments to document them.

C# delegate events also require to reference the event payload `struct` which also requires documenting using XML comments.

## Commit Messages

The commit message lines should never exceed 72 characters and should be entered in the following format:

```
<type>(<scope>): <subject>
<BLANK LINE>
<body>
<BLANK LINE>
```

### Type

The type must be one of the following:

- feat: A new feature.
- fix: A bug fix.
- docs: Documentation only changes.
- refactor: A code change that neither fixes a bug or adds a feature.
- perf: A code change that improves performance.
- test: Adding missing tests.
- chore: Changes to the build process or auxiliary tools or libraries such as documentation generation.

### Scope

The scope could be anything specifying the place of the commit change, such as, `Controller`, `Interaction`, `Locomotion`, etc...

### Subject

The subject contains succinct description of the change:

* use the imperative, present tense: "change" not "changed" nor "changes".
* don't capitalize first letter, unless naming something, such as `Bootstrap`.
* no dot (.) at the end of the subject line.

### Body

Just as in the subject, use the imperative, present tense: "change" not "changed" nor "changes" The body should include the motivation for the change and contrast this with previous behavior. References to previous commit hashes is actively encouraged if they are relevant.

  > **Incorrect commit summary:**
  ```
  Added feature to improve teleportation
  ```
  > **Incorrect commit summary:**
  ```
  feat(Teleport): Add feature
  ```
  > **Incorrect commit summary:**
  ```
  feat(my-teleport-feature): my feature.
  ```

  > **Correct commit summary:**
  ```
  feat(Teleport): add fade camera option on teleport
  ```

## Submitting Changes

- Push your changes to your topic branch in your repository.
- Submit a pull request to the repository `vgwb/Antura`.
- If you're submitting a bug fix pull request then target the repository `dev` branch.
- If you're submitting a new feature pull request then target the next release branch in the repository.
- The core team will aim to look at the pull request as soon as possible and provide feedback where required.
