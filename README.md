# UnityExternal
Unity project demonstrating the use of an external C# project with Visual Studio Tools for Unity (VSTU).

As using external projects with VSTU generated solutions is not yet supported, here is a possible workaround, using file generation hooks:
https://msdn.microsoft.com/en-us/library/dn940021.aspx.

Here is a working prototype :
* Using the Solution hook, we add the external project to the solution.
* Using the Project hook, we add the external project as a reference for the game assembly.
* Using the Project hook, we add a postbuild event to copy the compiled assembly DLL to the Unity asset folder (because we have to compile both in VS and Unity).
* Using the Project hook, we remove the reference to the assembly dll to avoid collisions with the project reference. (as we deploy a DLL in the asset folder, VSTU will try to add a reference to the DLL).
