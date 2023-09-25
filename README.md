# blocktest
[![Discord](https://img.shields.io/discord/814354060622168064.svg)](https://discord.gg/skHTWtBvEn)
[![GitHub](https://img.shields.io/github/license/blocktest-game/blocktest-MonoGame)](https://mit-license.org/)
[![GitHub last commit](https://img.shields.io/github/last-commit/blocktest-game/blocktest-MonoGame.svg)](https://github.com/blocktest-game/blocktest/commits/dev)
[![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/blocktest-game/blocktest-MonoGame.svg)](http://isitmaintained.com/project/blocktest-game/blocktest "Average time to resolve an issue")

[![forthebadge](https://forthebadge.com/images/badges/designed-in-ms-paint.svg)](https://forthebadge.com)
[![forthebadge](https://forthebadge.com/images/badges/contains-tasty-spaghetti-code.svg)](https://forthebadge.com)

A currently highly indev project aiming to give the experience of games like Terraria and Starbound whilst being open-source and community-driven. Further information can be found in the [blocktest HackMD account](https://hackmd.io/@blocktest).

![current_screenshot](https://user-images.githubusercontent.com/29362068/111568985-cd467b00-876f-11eb-98a0-811d39f13ea0.png)
*A screenshot (taken Mar. 17th, 2021 **of the [Unity version](https://github.com/blocktest-game/blocktest)**) displaying the features which are already functional, most importantly the building system.*

## Downloading the game

Find the latest releases [here](https://github.com/blocktest-game/blocktest-MonoGame/releases). All you need to do is download the correct file for your system, unzip the file, and run the executable.

## Running the game

Blocktest will run in local mode by default. To connect to a server, it must be run with the arguments "connect \<ip\>". For localhost this is "connect localhost".

## Contributing

1. Follow MonoGame's [getting started guide](https://docs.monogame.net/articles/getting_started/0_getting_started.html)
2. Fork this repository
3. Clone your fork with [git](https://git-scm.com)
4. Open solution file (Blocktest.sln) in Visual Studio
5. Edit the code
6. Commit and push all the changes you want with git
7. Make a pull request!

### Compiling after making change to shared folder

After making a change to the shared project, you may need to use the "dotnet clean" and "dotnet build" commands on the shared project to make the changes appear in your dev environment.

## Reporting issues

To report an issue, please fill out a bug report [here](https://github.com/blocktest-game/blocktest-MonoGame/issues/new?assignees=&labels=bug&template=bug_report.md&title=%5BBUG%5D) so that developers can quickly respond to the bug.
