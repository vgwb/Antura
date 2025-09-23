---
title: Build
nav_order: 0
---
# Build Antura

## Desktop
- we are currently using the Could build

## Android
- we are currently using the Could build

## iOS
- latest XCode
- disable the BitCode flag (done automatically)


---

With FB Sdk
iOS:

brew install cocoapods
pod install

XCode:
- Set yes on Always Embed Swift Standard Libraries in target Unity-iPhone in Build Settings.
- Set no on Always Embed Swift Standard Libraries in target UnityFramework in Build Settings.

Enable bitcode in Pods-Unity-iPhone
