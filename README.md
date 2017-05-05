# SmartMirror

This project consist of building a smartmirror having those features

  - Windows 10 IOT based (C# code)
  - 24 inch monitor for display
  - CFF (SBB) integration
  - Phillips Hue light integration
  - Sonos music intetgration
  - Weather integration
  - News integration
  - Voice recognition

## Architecture

Subdivided in subfolder and sub-branches

  - CFF
    - CFFHandler.cs
    - Location.cs
  - Content
    - Message.cs
  - Hue
    - Bridge.cs
    - HsbColor.cs
    - HueHandler.cs
    - Light.cs
  - News
  - Pages
    - MainPage.xaml
    - Mainpage.xaml.cs
  - Sonos
    - Music.cs
    - Song.cs
  - Voice
    - SRGS
      - Sonos.xml
    - Otto.cs
    - VoiceHandler.cs
  - WeatherAPI
    - Data.cs
    - LocationManager.cs
    - OpenWeatherMapProxy.cs
    - WeatherHandler.cs

### CFF (SBB)

This section call OpenTransport API to get transport information in realTime. 

### Content

This is a debug section with use of `Message.show("My debug message");`

### Phillips Hue

