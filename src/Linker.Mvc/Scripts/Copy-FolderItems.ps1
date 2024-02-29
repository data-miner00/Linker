param (
	[Parameter(Mandatory = $true)]
	[String] $SourceFolder,

	[Parameter(Mandatory = $true)]
	[String] $DestinationFolder
)

Copy-Item -Force -Recurse -Verbose $SourceFolder -Destination $DestinationFolder
