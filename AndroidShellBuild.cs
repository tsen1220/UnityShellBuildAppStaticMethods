using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

// ARM64 && ARMv7 Structure Build
public class AndroidIL2CPPShellBuild
{
	private static void Build()
	{
		SpecificDestinationLine();
		PlayerSettings.bundleVersion = _version;
		PlayerSettings.Android.bundleVersionCode = _bundleVersion;
		string destinationPath = Path.Combine(_destinationPath, PlayerSettings.productName);
		destinationPath += ".apk";
		PlayerSettings.Android.keystoreName = "yours.keystore";
		PlayerSettings.Android.keystorePass = "password";
		PlayerSettings.Android.keyaliasName = "yours";
		PlayerSettings.Android.keyaliasPass = "password";

		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
		AndroidArchitecture androidArchitecture = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
		PlayerSettings.Android.targetArchitectures = androidArchitecture;
		PlayerSettings.Android.useAPKExpansionFiles = true;


		BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, destinationPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
	}


	private static string _destinationPath;
	private static string _version;
	private static int _bundleVersion;
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
			},
			{
				"-bundleCodeVersion", delegate(string arg)
				{
					int convertArg = Int32.Parse(arg);
					_bundleVersion = convertArg;
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

// Mono Build (static methods ,useless now)
public class AndroidMonoShellBuild
{
	private static void Build()
	{
		SpecificDestinationLine();
		string destinationPath = Path.Combine(_destinationPath, PlayerSettings.productName);
		destinationPath += ".apk";
		PlayerSettings.Android.keystoreName = "yours.keystore";
		PlayerSettings.Android.keystorePass = "password";
		PlayerSettings.Android.keyaliasName = "yoursalias";
		PlayerSettings.Android.keyaliasPass = "password";

		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
		AndroidArchitecture androidArchitecture = AndroidArchitecture.ARMv7;
		PlayerSettings.Android.targetArchitectures = androidArchitecture;
		PlayerSettings.Android.useAPKExpansionFiles = false;

		BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, destinationPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
	}


	private static string _destinationPath;
	private static void SpecificDestinationLine()
	{
		Dictionary<string, Action<string>> cmdActions = new Dictionary<string, Action<string>>
		{
			{
				"-destinationPath", delegate(string argument)
				{
					_destinationPath = argument;
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
