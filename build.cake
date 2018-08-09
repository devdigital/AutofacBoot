#addin "Cake.FileHelpers"
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information("Running target " + target + " in configuration " + configuration);

var packageJsonText = FileReadText("./package.json");
var packageJson = Newtonsoft.Json.Linq.JObject.Parse(packageJsonText);
var buildNumber = packageJson.Property("version").Value;

var artifactsDirectory = Directory("./artifacts");

var projects = new List<string>
{
  "./Source/AutofacBoot/AutofacBoot.csproj",
  "./Source/AutofacBoot.Test/AutofacBoot.Test.csproj"
};

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        foreach(var project in projects)
        {
          DotNetCoreRestore(project);
        }
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var solutions = GetFiles("**/*.sln");
        foreach(var solution in solutions)
        {
            Information("Building solution " + solution);
            DotNetCoreBuild(
                solution.ToString(),
                new DotNetCoreBuildSettings()
                {
                    Configuration = configuration,
                });
        }
    });

Task("SetNuSpecVersion")
    .IsDependentOn("Build")
    .Does(() => 
    {
        var nuSpecFiles = new List<string> 
        {
            "./Source/AutofacBoot/AutofacBoot.nuspec",
            "./Source/AutofacBoot.Test/AutofacBoot.Test.nuspec"
        };

        foreach (var nuSpecFile in nuSpecFiles) 
        {
            TransformTextFile(nuSpecFile)
                .WithToken("version",  buildNumber.ToString())
                .Save(nuSpecFile);
        }
    });

Task("Test")
    .IsDependentOn("SetNuSpecVersion")
    .Does(() =>
    {
        var projects = GetFiles("./test/**/*Test.csproj");
        foreach(var project in projects)
        {
            Information("Testing project " + project);
            DotNetCoreTest(
                project.ToString(),
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true
                });
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var version = buildNumber.ToString();
        foreach (var project in projects)
        {
            Information("Packing project " + project);
            DotNetCorePack(
                project.ToString(),
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    OutputDirectory = artifactsDirectory,
                    VersionSuffix = version,
                    ArgumentCustomization  = builder => builder.Append("/p:PackageVersion=" + version),
                });
        }
    });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);
