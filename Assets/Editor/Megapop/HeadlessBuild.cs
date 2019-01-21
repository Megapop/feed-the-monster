using System;
using UnityEngine;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;

// ReSharper disable UnusedMember.Local

/// <summary>
/// Only MenuItems that correspond to the current target are shown, to avoid accidentally triggering a full reimport of all assets in the Unity editor.
/// To execute headless build from the command line, do something like this:
/// 
/// Unity.exe -quit -batchmode -executeMethod HeadlessBuild.BuildiOS
/// </summary>

[UsedImplicitly]
public class HeadlessBuild : MonoBehaviour
{
    /// <summary>
    /// Target environments for the headless builds.
    /// </summary>
    private enum BuildSettingsEnvironment
    {
        /// <summary>
        /// Development environment, with all debugging enabled.
        /// </summary>
        Dev,
        /// <summary>
        /// Prod environment, connecting to the prod servers and with all debugging disabled.
        /// </summary>
        Prod
    }

    /// <summary>
    /// Settings for a build.
    /// </summary>
    private class BuildSettings
    {
        public BuildOptions Options = BuildOptions.None;
        public BuildSettingsEnvironment Environment = BuildSettingsEnvironment.Dev;
        public BuildTarget Target;
        public BuildTargetGroup TargetGroup;
    }

    /// <summary>
    /// Find the build nyum
    /// </summary>
    /// <returns></returns>
    private static string BuildNumber
    {
        get
        {
            var commandLineArgs = Environment.GetCommandLineArgs();

            for (var index = 0; index < commandLineArgs.Length; index++)
            {
                var arg = commandLineArgs[index];
                if (arg == "-buildNumber")
                {
                    return commandLineArgs[index + 1];
                }
            }
            return "0";
        }
    }

    private static string KeystorePassword
    {
        get
        {
            var commandLineArgs = Environment.GetCommandLineArgs();

            for (var index = 0; index < commandLineArgs.Length; index++)
            {
                var arg = commandLineArgs[index];
                if (arg == "-storepass")
                {
                    return commandLineArgs[index + 1];
                }
            }
            return "";
        }
    }

    private static string KeyPassword
    {
        get
        {
            var commandLineArgs = Environment.GetCommandLineArgs();

            for (var index = 0; index < commandLineArgs.Length; index++)
            {
                var arg = commandLineArgs[index];
                if (arg == "-keypass")
                {
                    return commandLineArgs[index + 1];
                }
            }
            return "";
        }
    }

    private static string[] Scenes
    {
        get
        {
            return (from scene in EditorBuildSettings.scenes
                where scene.enabled
                select scene.path).ToArray();
        }
    }

    /// <summary>
    /// Get the root path of the Unity project.
    /// </summary>
    private static string ProjectRoot
    {
        get
        {
            return (Application.dataPath.Replace("/Assets", ""));
        }
    }

    /// <summary>
    /// Get the root path for all builds.
    /// </summary>
    private static string OutputPath
    {
        get
        {
            return (ProjectRoot + "/build");
        }
    }

    /// <summary>
    /// Get the full path for the current BuildTarget.
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <returns></returns>
    private static string GetTargetPath(BuildTarget buildTarget)

    {
        // ReSharper disable once SwitchStatementMissingSomeCases
        switch (buildTarget)
        {
            case BuildTarget.iOS:
                return "FeedTheMonster_iOS";
            case BuildTarget.Android:
                return "FeedTheMonster.apk";
            case BuildTarget.StandaloneWindows:
                return "FeedTheMonster.exe";
            default:
                return "";

        }
    }

    private static void NukeBuild()
    {
        try
        {
            if (Directory.Exists(OutputPath))
            {
                Directory.Delete(OutputPath, true);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to clear build output directory: " + OutputPath + ": " + e);
        }
    }

    private static void PrepareBuild(BuildSettings settings)
    {
        NukeBuild();
        Directory.CreateDirectory(OutputPath);

        var defines = "USES_NOTIFICATION_SERVICE;";

        // Do not include Google Play Games on iOS.

        if (settings.Target == BuildTarget.iOS)
        {
            defines += "NO_GPGS;";
            PlayerSettings.iOS.buildNumber = BuildNumber;
        }

        switch (settings.Environment)
        {
            case BuildSettingsEnvironment.Dev:
                defines += "DEBUG;";
                break;
            case BuildSettingsEnvironment.Prod:
                defines += "LIVE;RELEASE;";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Debug.Log("Defines for " + settings.TargetGroup + ": " + defines);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(settings.TargetGroup, defines);

        // Build with Gradle.
        if (settings.Target == BuildTarget.Android)
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        }
    }

    private static void Build(BuildSettings settings)
    {
        var targetPath = GetTargetPath(settings.Target);

        Debug.Log("Starting " + settings.Target + " build to '" + targetPath + "' using options '" + settings.Options + "'.");

        var report = BuildPipeline.BuildPlayer(Scenes, targetPath, settings.Target, settings.Options);
        var summary = report.summary;

        if (summary.totalErrors > 0)
        {
            Debug.LogError(settings.Target + " build failed with " + summary.totalErrors + " total errors: " + summary);
            throw new Exception(summary.ToString());
        }

        Debug.Log(settings.Target + " build successful.");
    }

    #region iOS

#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    [MenuItem("Megapop/Build/Build iOS")]
#endif
    private static void BuildiOS()
    {
        var settings = new BuildSettings
        {
            Environment = BuildSettingsEnvironment.Dev,
            Target = BuildTarget.iOS,
            TargetGroup = BuildTargetGroup.iOS
        };

        Debug.Log("BuildiOS()");

        PrepareBuild(settings);
        Build(settings);
    }

#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    [MenuItem("Megapop/Build/Build iOS Prod")]
#endif
    private static void BuildiOSProd()
    {
        var settings = new BuildSettings
        {
            Environment = BuildSettingsEnvironment.Prod,
            Target = BuildTarget.iOS,
            TargetGroup = BuildTargetGroup.iOS
        };

        Debug.Log("BuildiOSProd()");

        PrepareBuild(settings);
        Build(settings);
    }

#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    [MenuItem("Megapop/Build/Build and Run iOS")]
#endif
    private static void BuildAndRuniOS()
    {
        var settings = new BuildSettings
        {
            Environment = BuildSettingsEnvironment.Dev,
            Target = BuildTarget.iOS,
            TargetGroup = BuildTargetGroup.iOS,
            Options = BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.Development
        };

        Debug.Log("BuildAndRuniOS()");

        PrepareBuild(settings);
        Build(settings);
    }

    #endregion

    #region Android

    /// <summary>
    /// Build the project for Android.
    /// </summary>
#if UNITY_ANDROID
    [MenuItem("Megapop/Build/Build Android")]
#endif
    private static void BuildAndroid()
    {
        var settings = new BuildSettings
        {
            Environment = BuildSettingsEnvironment.Dev,
            Target = BuildTarget.Android,
            TargetGroup = BuildTargetGroup.Android
        };

        Debug.Log("BuildAndroid()");

        PrepareAndBuildAndroid(settings);
    }

    /// <summary>
    /// Build the project for Android.
    /// </summary>
#if UNITY_ANDROID
    [MenuItem("Megapop/Build/Build Android Live")]
#endif
    private static void BuildAndroidProd()
    {
        var settings = new BuildSettings
        {
            Environment = BuildSettingsEnvironment.Prod,
            Target = BuildTarget.Android,
            TargetGroup = BuildTargetGroup.Android
        };

        Debug.Log("BuildAndroidProd()");

        PrepareAndBuildAndroid(settings);
    }

    /// <summary>
    /// Build and immediately run the project on an attached Android device.
    /// </summary>
#if UNITY_ANDROID
    [MenuItem("Megapop/Build/Build and Run Android")]
#endif
    private static void BuildAndRunAndroid()
    {
        var settings = new BuildSettings
        {
            Environment = BuildSettingsEnvironment.Dev,
            Target = BuildTarget.Android,
            TargetGroup = BuildTargetGroup.Android,
            Options = BuildOptions.AllowDebugging | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler | BuildOptions.Development
        };

        Debug.Log("BuildAndRunAndroid()");

        PrepareAndBuildAndroid(settings);
    }

    private static void PrepareAndBuildAndroid(BuildSettings settings)
    {
        PrepareBuild(settings);

        if (settings.Environment == BuildSettingsEnvironment.Dev)
        {
            Debug.Log("Select debug key from 'debug.keystore'.");

            PlayerSettings.Android.keystoreName = "debug.keystore";
            PlayerSettings.Android.keystorePass = "debug123";
            PlayerSettings.Android.keyaliasName = "debug";
            PlayerSettings.Android.keyaliasPass = "debug123";
        }
        else
        {
            Debug.Log("Select upload key from 'upload.keystore'.");

            PlayerSettings.Android.keystoreName = "upload.keystore";
            PlayerSettings.Android.keystorePass = KeystorePassword;
            PlayerSettings.Android.keyaliasName = "upload";
            PlayerSettings.Android.keyaliasPass = KeyPassword;
        }

        Build(settings);
    }
    #endregion
}
