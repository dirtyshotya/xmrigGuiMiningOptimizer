Xmrig Ranch Launcher

Instructions:

1. Place xmrig.exe and config.json in the XmrigLauncher folder.
2. Open PowerShell inside this folder.
3. Build the launcher:
   dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
4. The ready-to-run executable will be in:
   bin\Release\net7.0-windows\win-x64\publish\
5. Double-click XmrigLauncher.exe to start the GUI.
