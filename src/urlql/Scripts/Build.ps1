Param(
  [string]$buildProjectLocation,
  [string]$buildArtifactStagingDirectory,
  [string]$buildBuildNumber
)

$version = $buildBuildNumber.Substring($buildBuildNumber.IndexOf("_") + 1)

Invoke-Expression "nuget pack 
                   $($buildProjectLocation)
                   -NonInteractive 
                   -OutputDirectory $($buildArtifactStagingDirectory) 
                   -Properties Configuration=release -version $($version) 
                   -Verbosity Detailed 
                   -IncludeReferencedProjects"