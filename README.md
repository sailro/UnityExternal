# UnityExternal
Unity project demonstrating the -full- use of an external C# project with Visual Studio Tools for Unity (VSTU).

Using external projects with VSTU generated solutions is not yet fully supported. VSTU is able to see external projects in solutions, but VSTU generated projects will use an assembly reference (and not a project reference). 

If you want to keep real project references (so being able to use refactoring or GotoDefinition), there is a possible workaround, using [file generation hooks](https://msdn.microsoft.com/en-us/library/dn940021.aspx).

Here is a working prototype :
* Using the Solution hook, we add the external project to the solution:
```
Project("{C# GUID}") = "ExternalLibrary", "..\ExternalLibrary\ExternalLibrary.csproj", "{Project GUID}"
EndProject
```
* Using the Project hook, we add the external project as a reference for the game assembly:
```
<ItemGroup>
  <ProjectReference Include="..\ExternalLibrary\ExternalLibrary.csproj">
    <Project>{E90EFAFC-D4AC-4514-A0AF-0C8F3888EC47}</Project>
    <Name>ExternalLibrary</Name>
  </ProjectReference>
</ItemGroup>
```
  * Then we add a postbuild event to copy the compiled assembly DLL to the Unity asset folder (because we have to compile both in VS and Unity):
```
<PropertyGroup>
  <RunPostBuildEvent>Always</RunPostBuildEvent>
  <PostBuildEvent>copy /Y $(TargetDir)ExternalLibrary.dll Assets</PostBuildEvent>
</PropertyGroup>
```
  * Finally we remove the reference to the assembly dll to avoid collisions with the project reference. (as we deploy a DLL in the asset folder, VSTU will try to add a reference to the DLL):
<s>
```
<Reference Include="ExternalLibrary">
  <HintPath>Assets\ExternalLibrary.dll</HintPath>
</Reference>
```
</s>
The [interesting part is here](/UnityProject/Assets/Editor), using two Editor scripts.

Now you can easily update your external projects files:
- VS solution and projects will correctly reference your external project.
- Unity will use the newly compiled version after a VS build (thanks to the post build event).
- You can refactor what you want as for any other project.
