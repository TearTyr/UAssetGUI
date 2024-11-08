# UAssetGUI
[![Release](https://img.shields.io/github/v/release/atenfyr/UAssetGUI.svg?style=flat-square)](https://github.com/atenfyr/UAssetGUI/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/atenfyr/UAssetGUI/total.svg?style=flat-square)](https://github.com/atenfyr/UAssetGUI/releases)
[![Issues](https://img.shields.io/github/issues/atenfyr/UAssetGUI.svg?style=flat-square)](https://github.com/atenfyr/UAssetGUI/issues)
[![CI Status](https://img.shields.io/github/actions/workflow/status/atenfyr/UAssetGUI/build.yml?label=CI)](https://github.com/atenfyr/UAssetGUI/actions)
[![License](https://img.shields.io/github/license/atenfyr/UAssetGUI.svg?style=flat-square)](https://github.com/atenfyr/UAssetGUI/blob/master/LICENSE.md)

UAssetGUI is a tool designed for low-level examination and modification of Unreal Engine game assets by hand.

<img src="https://i.imgur.com/cibmlbW.png" align="center">

## Installation
You can find pre-built binaries of UAssetGUI in the [Releases tab of this repository](https://github.com/atenfyr/UAssetGUI/releases).

## Command line arguments
You can run the program with command line arguments to export to and import from JSON, without opening the GUI.

### Export to JSON
```
UAssetGUI tojson <source> <destination> <engine version> [mappings name]
```
Example: `UAssetGUI tojson A.uasset B.json VER_UE4_25`


### Import from JSON
```
UAssetGUI fromjson <source> <destination> [mappings name]
```

Example: `UAssetGUI fromjson B.json A.umap Outriders`

## Compilation
If you'd like to compile UAssetGUI for yourself, read on:

### Prerequisites
* Visual Studio 2022 or later
* Git

### Initial Setup
1. Clone the UAssetGUI repository:

```sh
git clone https://github.com/atenfyr/UAssetGUI.git
```

2. Switch to the new UAssetGUI directory:

```sh
cd UAssetGUI
```

3. Pull the required submodules:

```sh
git submodule update --init
```

4. Open the `UAssetGUI.sln` solution file in Visual Studio, right-click on the UAssetGUI project in the Solution Explorer, and click "Set as Startup Project."

5. Right-click on the solution name in the Solution Explorer, and press "Restore Nuget Packages."

6. Press the "Start" button or press F5 to compile and open UAssetGUI.

## Contributing
Any contributions, whether through pull requests or issues, that you make are greatly appreciated.

If you have an Unreal Engine .uasset file that displays "failed to maintain binary equality," feel free to submit an issue on [the UAssetAPI issues page](https://github.com/atenfyr/UAssetAPI/issues) with a copy of the asset in question along with the name of the game, the Unreal version that it was cooked with, and a mappings file for the game, if needed.

## License
UAssetAPI and UAssetGUI are distributed under the MIT license, which you can view in detail in the [LICENSE file](LICENSE).
