using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEngine;

[InitializeOnLoad]
public class SolutionGenerationHook
{

	static SolutionGenerationHook()
	{
        ProjectFilesGenerator.SolutionFileGeneration += (name, content) =>
        {
	        content = AddProjectToSolution(content, "ExternalLibrary", @"..\ExternalLibrary\ExternalLibrary.csproj", "{E90EFAFC-D4AC-4514-A0AF-0C8F3888EC47}");

			Debug.Log("SolutionGenerationHook:" + name);
			return content;
		};
	}

	private static string AddProjectToSolution(string content, string projectName, string projectFilePath, string projectGuid)
	{
		if (content.Contains("" + projectName + ""))
			return content; // already added

		var signature = new StringBuilder();
		const string csharpProjectTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
        signature.AppendLine(string.Format("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"", csharpProjectTypeGuid, projectName, projectFilePath, projectGuid));
		signature.AppendLine("Global");

		var regex = new Regex("^Global", RegexOptions.Multiline);
		return regex.Replace(content, signature.ToString());
	}
}