# iTimeslot

The `iTimeslot` is a freestyle timer utility for you to focus on completing tasks.
It is very easy to use, and it's free with no ads or subscriptions, although the UI is a bit plain."

It allows you to define different timeslots that used to focus on tasks. 

In theory, you can use it with your 🍅`Pomodoro Technique`, but it's more flexible. For instance, most of the time, I prefer to extend the timer for longer periods. Some people may want to use it to simulate the IELTS test, such as timing themselves, while others may use it to remind themselves to drink water 🥃... 


## Overview

`iTimeslot` was written in C# and is based on the awesome `avalonia` UI framework. Both of them are Cross-Platform and open sourced,so this project can be built and can run on most platforms(macOS, Windows, Linux, iOS, Android etc. )

## Compatibility

All features have beens tested on macOS 14.3.1.

Some basic functions have also been tested on arm64-based Windows 11 and amd64 based Debian 12(xfce4 DE).
 
 There is a known compatibility issue with `GNOME DE` when running in background mode.


## Usage

Download from release and just double click to open it. 


## Build && Installation

Build for specific OS and arch:
`dotnet build iTimeSlot.csproj -f net8.0 -r linux-arm64  --self-contained true`

release for specific OS with single file and Aot:
`dotnet publish iTimeSlot.csproj -r osx-arm64  -o _out -c Release  -f net8.0  --self-contained true -t:BundleApp`

some options were already defined in .csproj such as:
```xml
    <PublishAot>true</PublishAot>
    <PublishTrimmed>true</PublishTrimmed>
```

For macOS users:
You may need to drag iTimeslot.app to your `Application` folder then launch it.

Other installers are on the way ... 🤔

## Screenshots

MacOS with default theme
![macOS-default-theme](/docs/images/macos-default.png)

Material Theme (haven't released)
![macOS-material-theme](/docs/images/macos-material.png)

Windows and Linux
![windows-and-linux](/docs/images/win-linux.png)

Tray icon, Popup, Installed view
![tray-popup-install](/docs/images/install-trayicon-popup.png)


## Todo
- [ ] Support Statistics
- [ ] Publish installers
- [ ] Combine with OS's 'focus'
- [ ] Themes
- [ ] Better UI
- [ ] Setup github action
- [ ] Test