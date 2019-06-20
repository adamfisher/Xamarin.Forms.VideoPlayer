# Package Nuget
dotnet pack src\Octane.Xamarin.Forms.VideoPlayer\Octane.Xamarin.Forms.VideoPlayer.csproj -c Release --no-build
Move-Item src\Octane.Xamarin.Forms.VideoPlayer\bin\Release\*.nupkg -Destination . -force

pause