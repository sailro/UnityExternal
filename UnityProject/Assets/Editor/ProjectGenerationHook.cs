using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEngine;

[InitializeOnLoad]
public class ProjectGenerationHook
{

	static ProjectGenerationHook()
	{
		ProjectFilesGenerator.ProjectFileGeneration += (name, content) =>
		{
			// Do not reference in Editor projects
			if (name.Contains(".Editor"))
				return content;

			content = RemoveAssemblyReferenceFromProject(content, "ExternalLibrary");
			content = AddProjectReferenceToProject(content, "ExternalLibrary", @"..\ExternalLibrary\ExternalLibrary.csproj", "{E90EFAFC-D4AC-4514-A0AF-0C8F3888EC47}");

			Debug.Log("ProjectGenerationHook:" + name);
			return content;
		};
	}

	private static string RemoveAssemblyReferenceFromProject(string content, string assemblytName)
	{
		var regex = new Regex(string.Format(@"^\s*<Reference Include=""{0}"">\r\n\s*<HintPath>.*{0}.dll</HintPath>\r\n\s*</Reference>\r\n", assemblytName), RegexOptions.Multiline);
		return regex.Replace(content, String.Empty);
	}

	private static string AddProjectReferenceToProject(string content, string projectName, string projectFilePath, string projectGuid)
	{
		if (content.Contains(">" + projectName + "<"))
			return content; // already added

		var signature = new StringBuilder();
		signature.AppendLine("  <ItemGroup>");
		signature.AppendLine(string.Format("    <ProjectReference Include=\"{0}\">", projectFilePath));
		signature.AppendLine(string.Format("      <Project>{0}</Project>", projectGuid));
		signature.AppendLine(string.Format("      <Name>{0}</Name>", projectName));
		signature.AppendLine("    </ProjectReference>");
		signature.AppendLine("  </ItemGroup>");
		signature.AppendLine("</Project>");

		var regex = new Regex("^</Project>", RegexOptions.Multiline);
		return regex.Replace(content, signature.ToString());
	}
}