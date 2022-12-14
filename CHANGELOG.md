# Changelog
All notable changes to this project will be documented in this file.

## [Unreleased]


## [1.2.1] - 2022-12-25
### Fixed
- Constructor on ServerJarsExt missing


## [1.2.0] - 2022-12-25
### Added
- Add UserAgent to HTTP client if not already set by the calling assembly
- Set default Accept Heager on HTTP client
- Add Model JarDetailsExt to include Type, Category and DownloadUrl
- Add ServerJarsExt class which returns the JarTypes as dictionary by default and the JarDetailsExt instead of JarDetails

### Changed
- Changed dispose HTTP client flag in ServerJars constructor to be TRUE as a default
- Adjusted README to include description for ServerJarsExt


## [1.1.1] - 2022-12-06
### Changed
- Replace ToList with AsDictionary extension
- Remove JarTypeItems model


## [1.1.0] - 2022-12-06
### Changed
- Remove JsonPropertyName attribute from models and work with JsonSerializerOptions case insensitive property instead

### Added
- Add ToList method on JarTypes Model to return a list of JarTypeItems for easier access via enumerable


## [1.0.2] - 2022-12-02
### Changed
- Progress calculation for downloader doesn't depend on an external Nuget anymore
- Reintroduce functionality to pass own HTTP client


## [1.0.1] - 2022-12-02
### Added
- New GetJar Method with possibility to pass IProgress for progress reporting

### Changed
- GetJar Method downloads header first to verify status code, then downloads rest

### Removed
- Possibility to pass own HTTP client


## [1.0.0] - 2022-11-30
### Added
- Create Nuget and publish on nuget.org


## [0.1.0] - 2022-11-29
### Added
- First working version



This project is MIT Licensed // Created & maintained by Patrick Weiss