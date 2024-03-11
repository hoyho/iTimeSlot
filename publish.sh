# -p:PublishSingleFile=true
rm -rf _out
dotnet clean
dotnet publish iTimeSlot.csproj -r osx-arm64  -o _out -c Release  -f net8.0  --self-contained true -t:BundleApp 