#!/usr/bin/env bash
set -euo pipefail

CHANNEL="vgwb/antura:windows"   # itch.io channel
BUILD_DIR="${UNITY_BUILD_OUTPUT}"

# Read version from project repo
VERSION=$(cat docs/latest-version.txt | tr -d '[:space:]')

echo "==> Using version from docs/latest-version.txt: ${VERSION}"
echo "==> Build dir: ${BUILD_DIR}"
echo "==> Target itch.io channel: ${CHANNEL}"

# Download Butler if not present
if [ ! -f "butler" ]; then
    echo "==> Downloading Butler..."
    curl -L -o butler.zip https://broth.itch.ovh/butler_linux_amd64.zip
    unzip butler.zip
    chmod +x butler
fi

# Authenticate
./butler login --api-key "$ITCHIO_API_KEY"

# Zip the build folder
ZIP_NAME="build-${VERSION}.zip"

echo "==> Zipping build to ${ZIP_NAME}..."
cd "$BUILD_DIR"
zip -r "../${ZIP_NAME}" .

cd ..
echo "==> Pushing to itch.io..."
./butler push "${ZIP_NAME}" "${CHANNEL}" --userversion "${VERSION}"

echo "==> Upload complete."
