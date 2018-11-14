nuget pack .\Card\Card.nuspec -Version $env:appveyor_build_version
nuget pack .\Icon\Icon.nuspec -Version $env:appveyor_build_version
nuget pack .\Paging\Paging.nuspec -Version $env:appveyor_build_version
nuget pack .\Repeater\Repeater.nuspec -Version $env:appveyor_build_version
nuget pack .\Book\Book.nuspec -Version $env:appveyor_build_version