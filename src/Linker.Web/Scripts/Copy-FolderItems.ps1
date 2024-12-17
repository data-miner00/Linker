<#
.Synopsis
    Copy files and folders
.Description
    Copy files and folders under a certain directory and place to a destination directory.
.Example
	Copy-FolderItems -SourceFolder myFolder/* -DestinationFolder secondFolder
.Example
	Copy-FolderItems -SourceFolder ~/rootFolder/* -DestinationFolder .
#>

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
