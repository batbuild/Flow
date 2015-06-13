// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile


RestorePackages()

// Directories
let buildDir  = @".\build\"
let testDir   = @".\test\"
let deployDir = @".\deploy\"
let packagesDir = @".\packages"


// Project info
let authors = ["Andrea Magnorsky";"Andrew O'Connor"]
let projectName = "Flow"
type ProjectInfo = { 
    Name: string;
    Description: string; 
    Version: string;
  }
let info = {
  Name="Flow";
  Description =  "Flow for android";
  Version = if isLocalBuild then "0.2-local" else "0.2."+buildVersion
}

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testDir; deployDir]
)

Target "SetVersions" (fun _ ->
    CreateCSharpAssemblyInfo "./Properties/AssemblyInfo.cs"
        [Attribute.Title info.Name
         Attribute.Description info.Description
         Attribute.Guid "c1dcbc84-7e8b-46f3-a253-9d9527434dee"         
         Attribute.Version info.Version
         Attribute.FileVersion info.Version]
)


Target "Compile" (fun _ ->
    !! @"**\*.csproj"
      |> MSBuildRelease "" "Build"      
      |> Log "AppBuild-Output: "
)

Target "CompileTest" (fun _ ->
    !! @"**\Test*.csproj"
      |> MSBuildDebug testDir "Build"
      |> Log "TestBuild-Output: "
)

Target "NUnitTest" (fun _ ->
    !! (testDir + @"\Test*.dll")
      |> NUnit (fun p ->
                 {p with
                   DisableShadowCopy = true;
                   OutputFile = testDir + @"TestResults.xml"})
)

Target "CreatePackage" (fun _ ->    
    let nugetPath = ".nuget/NuGet.exe"
    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName            
            Version = info.Version
            Description = info.Description                                           
            OutputPath = deployDir            
            ToolPath = nugetPath
            Summary = info.Description                               
            PublishUrl = getBuildParamOrDefault "nugetrepo" ""
            AccessKey = getBuildParamOrDefault "nugetkey" ""            
            Publish = hasBuildParam "nugetkey"  
            }) 
            "Flow.0.1.0.0.nuspec"
)

Target "AndroidPack" (fun _ ->    
    let nugetPath = ".nuget/NuGet.exe"
    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName+".Android"
            Version = info.Version
            Description = info.Description                                           
            OutputPath = deployDir            
            ToolPath = nugetPath
            Summary = info.Description            
            Tags = "Coroutine library C# for Android"           
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey"
            PublishUrl = getBuildParamOrDefault "nugetUrl" ""
            }) 
            "nuget/Flow.Android.nuspec"
)


// Dependencies
"Clean"
  ==> "SetVersions"
  ==> "Compile"
  ==> "CompileTest"
  ==> "NUnitTest"
  ==> "CreatePackage"

"Clean"
  ==> "SetVersions"
  ==> "Compile"
  ==> "AndroidPack"

// start build
RunTargetOrDefault "CreatePackage"