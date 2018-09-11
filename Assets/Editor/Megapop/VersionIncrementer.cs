using System;
using UnityEditor;
using UnityEngine;

public class VersionIncrementer : ScriptableObject
{
    [MenuItem("Megapop/Version/Increment Major Version (X.0.0)", false, 2)]
    public static void IncrementMajorVersion()
    {
        var version = new Version(PlayerSettings.bundleVersion);
        var major = FindMajor(version);
        var bundleVersion = new Version(major + 1, 0, 0);
        var packageVersion = new Version(major + 1, 0, 0, 0);

        UpdateAssets(bundleVersion, packageVersion);
    }

    [MenuItem("Megapop/Version/Increment Minor Version (0.X.0)", false, 1)]
    public static void IncrementMinorVersion()
    {
        var version = new Version(PlayerSettings.bundleVersion);
        var major = FindMajor(version);
        var minor = FindMinor(version);
        var bundleVersion = new Version(major, minor + 1, 0);
        var packageVersion = new Version(major, minor + 1, 0, 0);

        UpdateAssets(bundleVersion, packageVersion);
    }

    [MenuItem("Megapop/Version/Increment Revision (0.0.X)", false, 0)]
    public static void IncrementRevision()
    {
        var version = FindVersion();
        var major = FindMajor(version);
        var minor = FindMinor(version);
        var build = FindBuild(version);

        var bundleVersion = new Version(major, minor, build + 1);
        var packageVersion = new Version(major, minor, build + 1, 0);

        UpdateAssets(bundleVersion, packageVersion);
    }


    private static Version FindVersion()
    {
        return new Version(PlayerSettings.bundleVersion);
    }

    private static int FindBuild(Version version)
    {
        return version.Build < 0 ? 0 : version.Build;
    }

    private static int FindMinor(Version version)
    {
        return version.Minor < 0 ? 0 : version.Minor;
    }

    private static int FindMajor(Version version)
    {
        return version.Major < 0 ? 0 : version.Major;
    }

    public static void UpdateAssets(Version bundleVersionString, Version packageVersion)
    {
        PlayerSettings.bundleVersion = bundleVersionString.ToString();
        PlayerSettings.Android.bundleVersionCode++;

        Debug.Log("bundleVersion: " + bundleVersionString + " packageVersion: " + packageVersion);

        // Save PlayerSettings.asset

        AssetDatabase.SaveAssets();

        // Save text file containing the current version.

        UpdateClientVersion.UpdateBuildVersionFile(bundleVersionString.ToString());
    }
}