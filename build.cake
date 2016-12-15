#tool "GitVersion.CommandLine"

var target = Argument("target", "Default");

Task("Default")
    .Does(() => {
        Information("Default");

        var versionInfo = GitVersion(new GitVersionSettings() {
            NoFetch = true
        });

        DotNetCoreRestore("src/Nest.Queryify5/");

        var buildSettings = new DotNetCoreBuildSettings(){
            Configuration = "Release",
            VersionSuffix = "preview-1",
            NoIncremental = true
        };

        DotNetCoreBuild("src/Nest.Queryify5/");

        DotNetCorePack("src/Nest.Queryify5/", new DotNetCorePackSettings() {
            Configuration = "Release",
            VersionSuffix = "preview.1",
            OutputDirectory = Directory("./artifacts/build/")
        });
    });


RunTarget(target);