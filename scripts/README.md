# ITCH.IO desktop build deploy

To manually push a locale build to itch.io:
docs: https://itch.io/docs/butler/pushing.html


### Manual version
butler push antura_zip.zip vgwb/antura:windows --userversion 2025.11.75

### Automatic version
butler push antura_zip.zip vgwb/antura:windows --userversion "$(cat ../../docs/public/latest-version.txt)"

### check status
butler status vgwb/antura:windows

### update butler
butler upgrade
