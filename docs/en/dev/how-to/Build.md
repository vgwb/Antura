---
title: Build from sources
---

# Build from sources

This guide shows how to build **Learn with Antura** from the public repository using **Unity 6.x**.

> [!NOTE]
> Antura is an open source Unity project. You can build desktop and mobile targets locally.
> Official releases are in the store; community builds are welcome for macOS/Linux.

---

## 1. Prerequisites

- **Git** (and **Git LFS**)
- **Unity Hub** (3.x) + **Unity 6.x** editor
- Install the modules you need from Hub:
  - **Windows Build Support (IL2CPP)**
  - **Mac Build Support (IL2CPP)** *(optional)*
  - **Linux Build Support (IL2CPP)** *(optional)*
  - **Android Build Support** (SDK, NDK, JDK)
  - **iOS Build Support**

## 2. Get the code

Use a git client like GitHub Desktop or Fork to clone the repository <https://github.com/vgwb/Antura.git> or

```bash
git lfs install
git clone https://github.com/vgwb/Antura.git
cd Antura
git lfs pull
```

> [!TIP]
> If large audio files look like tiny text pointers, you forgot `git lfs pull`.

## 3. Open the project

1. Open **Unity Hub** → **Open** → select the repository folder.
2. Unity will import packages on first open (a few minutes).
3. Open the **main menu scene** (see `/Assets/...`), press **Play** to sanity-check.

## 4. One-time project setup (important)

### Addressables & Localization

- **Window → Asset Management → Addressables → Groups**

  - Play Mode Script: **Fast Mode** (for local testing)
  - **Build → New Build → Default Build Script** before making a player build.

- **Window → Localization → Tables**

  - Ensure String/Asset tables exist for your locales.
  - Voice-over `AudioClip`s referenced by **Asset Tables** must be **Addressable**.
    (We include an editor tool: **Antura → Localization → Normalize Addressables**)

## 5. Build targets

### 💻 Windows

1. **File → Build Settings** → **Windows** → **Switch Platform**
2. **Player Settings**:

   - **Architecture**: x86_64
   - **Scripting Backend**: IL2CPP
3. **Build** (or **Build & Run**)

### 📱 Android

1. **Build Settings** → **Android** → **Switch Platform**
2. **Player Settings**:

   - **Target API Level**: **Highest Installed**
   - **Minimum API Level**: choose your target (Antura supports Android 6.0+)
   - **Scripting Backend**: IL2CPP
   - **Target Architectures**: ARM64 (Play Store requirement)
3. For Play Store: **Build App Bundle (AAB)**
4. **Build**

### 🍏 iOS

1. **Build Settings** → **iOS** → **Switch Platform**
2. **Player Settings**:

   - **Scripting Backend**: IL2CPP
   - Set **Bundle Identifier**, **Version**
3. **Build** → open the generated Xcode project (Xcode 15+), set **Team/Provisioning**, **Run**.

### 💻 macOS / Linux

> [!TIP] Contribute builds
> Built a stable macOS/Linux version? Share it on the forum so others can try it!


## 6. Troubleshooting

- **Black screen / freeze on Play**
  Ensure Localization init runs in `Start()` or via `await`, not in `Awake/OnEnable`.
  Use Addressables **Fast Mode** while testing.

- **Localized audio not playing**
  Rebuild Addressables; verify Asset Table entries have `AudioClip`s assigned for the current locale and are **Addressable** (Analyzer green).

- **Missing large files**
  Run `git lfs pull`.

- **“GetAssetAsync failed to load the asset”**
  Usually: wrong type, missing Addressable flag, or stale “Use Existing Build”. Rebuild Addressables.

## 7. Cloud Build

We currently use **Cloud Build** for internal Android/Windows pipelines.
Local builds are fully supported with the steps above.

## 8. Contribute

- Code & issues: [https://github.com/vgwb/Antura](https://github.com/vgwb/Antura)
- Share builds/feedback: [https://github.com/vgwb/Antura/discussions](https://github.com/vgwb/Antura/discussions)
