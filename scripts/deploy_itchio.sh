#!/usr/bin/env bash
set -euo pipefail

CHANNEL="vgwb/antura:windows"   # itch.io channel
BUILD_DIR="${1:-${UNITY_BUILD_OUTPUT:-${BUILD_PATH:-$PWD}}}"

# Read version from project repo
VERSION_FILE="docs/public/latest-version.txt"
if [ ! -f "$VERSION_FILE" ]; then
    echo "ERROR: version file '$VERSION_FILE' not found." >&2
    exit 1
fi
VERSION=$(tr -d '[:space:]' < "$VERSION_FILE")

echo "==> Using version from $VERSION_FILE: ${VERSION}"
echo "==> Build dir: ${BUILD_DIR:-<not set>} (override via first script arg)"
echo "==> Target itch.io channel: ${CHANNEL}"

# Validate build dir
if [ -z "${BUILD_DIR}" ]; then
    echo "ERROR: Build directory not set. Pass it as the first argument or set UNITY_BUILD_OUTPUT/BUILD_PATH." >&2
    exit 1
fi
if [ ! -d "${BUILD_DIR}" ]; then
    echo "ERROR: build dir '${BUILD_DIR}' does not exist." >&2
    exit 1
fi

# Validate API key
if [ -z "${ITCHIO_API_KEY:-}" ]; then
    echo "ERROR: ITCHIO_API_KEY is not set. Export your itch.io API key in this env var." >&2
    exit 1
fi

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
START_DIR="$PWD"

echo "==> Zipping build to ${ZIP_NAME}..."
cd "$BUILD_DIR"
zip -r "${START_DIR}/${ZIP_NAME}" .

cd "$START_DIR"
echo "==> Pushing to itch.io..."
./butler push "${ZIP_NAME}" "${CHANNEL}" --userversion "${VERSION}"

echo "==> Upload complete."
