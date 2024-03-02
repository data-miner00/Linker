param (
	[Parameter(Mandatory = $true)]
	[String] $SourceFolder,

	[Parameter(Mandatory = $true)]
	[String] $DestinationFolder
)

if (!(Test-Path -PathType container $DestinationFolder)) {
	New-Item -ItemType Directory -Path $DestinationFolder
}

Copy-Item -Force -Recurse -Verbose $SourceFolder -Destination $DestinationFolder
