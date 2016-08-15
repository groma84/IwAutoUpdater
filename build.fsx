// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake

RestorePackages()

// Properties
let buildDir = "./build/"

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "BuildRelease" (fun _ ->
    !! "./IwAutoUpdater.sln"
    |> MSBuildRelease buildDir "Build"
    |> Log "Build-Output: "
)

Target "DeleteConfigFiles" (fun _ ->
    !! (buildDir + "config*.json")
    |> DeleteFiles
)

Target "Default" (fun _ ->
    trace ("Building with FAKE")
)

// Dependencies
"Clean"
    ==> "BuildRelease"
    ==> "DeleteConfigFiles"
    ==> "Default"

// start build
RunTargetOrDefault "Default"
