#addin "nuget:?package=CommandLineParser&version=2.4.3"
#addin "nuget:?package=Cake.FileHelpers&version=4.0.1"

#tool "nuget:?package=NuGet.CommandLine&version=6.3.1"

var target = Argument("target", "Build-Solutions");
var configuration = Argument("configuration", "Debug");
var lang = Argument("lang", "csharp");
FilePathCollection slnFiles = new FilePathCollection();

//////////////////////////////////////////////////////////////////////
// SETUP
//////////////////////////////////////////////////////////////////////
Setup(setupContext =>
{
    if (lang != "csharp" && lang != "vbnet")
    {
        throw new Exception("The --lang argument should be set to either \"csharp\" or \"vbnet\".");
    }
});
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Locate-Solutions")
    .Does(()=>
{ 
    slnFiles.Add(GetFiles($"./{lang}/**/*.sln"));  
    Console.WriteLine("Solutions found:");
    foreach (var f in slnFiles)
    {
        Console.WriteLine($"- {f}");
    } 
});
Task("Build-Solutions")
    .IsDependentOn("Locate-Solutions")
    .DoesForEach(GetFiles($"./{lang}/**/*.sln"), (slnFile) =>
{  
    if (!slnFile.ToString().Contains("MyOutlookAddIn"))
    {    
        Console.WriteLine($"--- Building solution: {slnFile}");

        NuGetRestore(slnFile);

        MSBuildSettings settings = new MSBuildSettings{
            Restore = true
        };

        settings.SetConfiguration(configuration)
                .SetPlatformTarget(PlatformTarget.MSIL)
                .WithTarget("Clean")
                .WithTarget("Rebuild")
                .WithProperty("RegisterForComInterop", "false");

        MSBuild(slnFile, settings);
    }
}).DeferOnError();
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);