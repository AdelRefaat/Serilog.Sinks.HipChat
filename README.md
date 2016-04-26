# Serilog.Sinks.HipChat

## Configuration using AppSettings
```xml
  <add key="serilog:using:HipChat" value="Serilog.Sinks.HipChat" />
  <add key="serilog:write-to:HipChat"/>
  <add key="serilog:write-to:HipChat.token" value="{HipChat Token goes here!}" />
  <add key="serilog:write-to:HipChat.roomId" value="{HipChat RoomId goes here!}" />
```
