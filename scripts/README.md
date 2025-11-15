# ITCH.IO desktop build deploy

To manually push a locale build to itchio:
docs: https://itch.io/docs/butler/pushing.html

butler push 567035-antura-windows-main-76.zip vgwb/antura:windows --userversion 2025.11.75

butler push 567035-antura-windows-main-78.zip vgwb/antura:windows --userversion "$(cat ../../docs/latest-version.txt)"

butler status vgwb/antura:windows