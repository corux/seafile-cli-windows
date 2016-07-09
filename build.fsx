// include Fake lib
#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile
open Fake.Git.Information
open Fake.ProcessHelper
open Fake.EnvironmentHelper
open System
open System.IO

// Properties
let buildDir = "bin/build"
let nugetPackages = "src/packages"
let sln = "src/SeafileCli.sln"
let solutionInfo = "src/SolutionInfo.cs"
let nugetExe = "packages/NuGet.CommandLine/tools/NuGet.exe"
let name = "Seafile CLI"
let publisher = "Tobias Wallura"

let buildVersion =
    match buildServer with
        | LocalBuild -> "0"
        | _ -> buildVersion
let gitVersion = getLastTag() |> SemVerHelper.parse
let makeVersion (semver: SemVerHelper.SemVerInfo) (buildVersion: string) =
    sprintf "%d.%d.%d.%s" semver.Major semver.Minor semver.Patch buildVersion
let version = makeVersion (gitVersion) (buildVersion)

// Targets
Target "NugetRestore" (fun _ ->
    sln
        |> RestoreMSSolutionPackages (fun p ->
            { p with
                OutputPath = nugetPackages
                ToolPath = nugetExe })
)

Target "Clean" (fun _ ->
   CleanDirs [buildDir]
)

Target "Build" (fun _ ->
    MSBuildDefaults <- { MSBuildDefaults with NodeReuse = false }

    CreateCSharpAssemblyInfo solutionInfo [
        Attribute.Version version
        Attribute.FileVersion version
        Attribute.InformationalVersion (version + " Build " + getCurrentSHA1("."))
        Attribute.Company publisher
        Attribute.Copyright ("Copyright (c) " + DateTime.Now.Year.ToString())
        Attribute.Metadata("githash", getCurrentSHA1("."))
    ]

    !! sln
        |> MSBuildRelease buildDir "Build"
        |> Log "Build-Output: "
)

Target "Default" DoNothing
Target "Exe" DoNothing

// Dependencies
"Clean"
   ==> "Build"
   ==> "Exe"
   ==> "Default"

"NugetRestore" ==> "Build"

RunTargetOrDefault "Default"
