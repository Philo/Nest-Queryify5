#tool "GitVersion.CommandLine"

var target              = Argument("target", "Default");
var artifacts           = Directory("./artifacts");
var buildOutput         = Directory("./artifacts/build");
var configuration       = Argument("configuration", "Release");

Task("Default")
    .Does(() => {
        Information("Default");

        var versionInfo = GitVersion(new GitVersionSettings() {
            NoFetch = true,
            UpdateAssemblyInfo = true,
            UpdateAssemblyInfoFilePath = "src/Nest.Queryify5/Properties/VersionAssemblyInfo.cs"
        });

        DotNetCoreRestore("src/Nest.Queryify5/");

        EnsureDirectoryExists(buildOutput);

        var buildSettings = new DotNetCoreBuildSettings(){
            Configuration = configuration,
            NoIncremental = true,
            OutputDirectory = buildOutput,
            Framework = "netstandard1.3"
        };

        DotNetCoreBuild("src/Nest.Queryify5/", buildSettings);

        var settings = new NuGetPackSettings {
            BasePath = buildOutput,
            Id = "Nest.Queryify",
            Authors = new [] { "Phil Oyston" },
            Owners = new [] {"Phil Oyston", "Storm ID" },
            Description = "Provides a mechanism to interact with Elasticsearch via a query object pattern",
            LicenseUrl = new Uri("https://raw.githubusercontent.com/stormid/nest-queryify/master/LICENSE"),
            ProjectUrl = new Uri("https://github.com/stormid/nest-queryify"),
            IconUrl = new Uri("http://stormid.com/_/images/icons/apple-touch-icon.png"),
            RequireLicenseAcceptance = false,
            // Properties = new Dictionary<string, string> { { "Configuration", configuration }},
            Symbols = false,
            NoPackageAnalysis = true,
            Version = versionInfo.NuGetVersionV2,
            OutputDirectory = artifacts,
            Tags = new[] { "Elasticsearch", "Nest", "Storm" },
            Files = new[] {
                new NuSpecContent { Source = "Nest.Queryify5.dll", Target = "lib/netstandard1.3" },
                new NuSpecContent { Source = "Nest.Queryify5.pdb", Target = "lib/netstandard1.3" }
            },
            Dependencies = new [] {
                new NuSpecDependency { Id = "Nest", Version = "[5,6]", TargetFramework=".NETStandard1.3" },
            }
        };
        NuGetPack("Nest.Queryify.nuspec", settings);
    });


RunTarget(target);