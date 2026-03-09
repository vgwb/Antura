# ITCH.IO desktop build deploy

To manually push a locale build to itchio:
docs: https://itch.io/docs/butler/pushing.html

butler upgrade

### Manual version
butler push 567035-antura-windows-main-76.zip vgwb/antura:windows --userversion 2025.11.75

### Automatic version
butler push 567035-antura-windows-main-80.zip vgwb/antura:windows --userversion "$(cat ../../docs/public/latest-version.txt)"

### check status
butler status vgwb/antura:windows
