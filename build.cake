#tool "GitVersion.CommandLine"

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        Information("Default");

        var versionInfo = GitVersion(new GitVersionSettings() {
            NoFetch = true,
            UpdateAssemblyInfo = true,
            UpdateAssemblyInfoFilePath = "src/Nest.Queryify5/Properties/VersionAssemblyInfo.cs"
        });

        var versionSuffix = versionInfo.CommitsSinceVersionSource;

        Information(versionSuffix);

        DotNetCoreRestore("src/Nest.Queryify5/");

        var buildSettings = new DotNetCoreBuildSettings(){
            Configuration = "Release",
            NoIncremental = true
        };

        DotNetCoreBuild("src/Nest.Queryify5/");

        DotNetCorePack("src/Nest.Queryify5/", new DotNetCorePackSettings() {
            Configuration = "Release",
            OutputDirectory = Directory("./artifacts/build/")
        });
    });


RunTarget(target);