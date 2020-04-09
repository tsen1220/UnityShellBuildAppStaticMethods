using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class iOSShellBuild
{
	private static void Build()
	{
		SpecificDestinationLine();

		PlayerSettings.bundleVersion = _version;
		string destinationPath = Path.Combine(_destinationPath, PlayerSettings.productName);
		destinationPath += ".ipa";
		BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, destinationPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
	}


	private static string _destinationPath;
	private static string _version;
	private static void SpecificDestinationLine()
	{
		Dictionary<string, Action<string>> cmdActions = new Dictionary<string, Action<string>>
		{
			{
				"-destinationPath", delegate(string argument)
				{
					_destinationPath = argument;
				}
			},
			{
				"-version", delegate(string arg)
				{
					_version = arg;
				}
			}
		};
		Action<string> actionCache;
		string[] cmdArguments = Environment.GetCommandLineArgs();

		for (int count = 0; count < cmdArguments.Length; count++)
		{
			if (cmdActions.ContainsKey(cmdArguments[count]))
			{
				actionCache = cmdActions[cmdArguments[count]];
				actionCache(cmdArguments[count + 1]);
			}
		}

		if (string.IsNullOrEmpty(_destinationPath))
		{
			_destinationPath = Path.GetDirectoryName(Application.dataPath);
		}
	}
}
