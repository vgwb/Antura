#!/usr/bin/env bash
set -euo pipefail

CHANNEL="vgwb/antura:windows"   # itch.io channel
# Prefer explicit UNITY_BUILD_OUTPUT, fall back to BUILD_PATH (Unity Cloud Build)
BUILD_DIR="${UNITY_BUILD_OUTPUT:-${BUILD_PATH:-}}"

# Read version from project repo
VERSION_FILE="docs/latest-version.txt"
if [ ! -f "$VERSION_FILE" ]; then
    echo "ERROR: version file '$VERSION_FILE' not found." >&2
    exit 1
fi
VERSION=$(tr -d '[:space:]' < "$VERSION_FILE")

echo "==> Using version from $VERSION_FILE: ${VERSION}"
echo "==> Build dir: ${BUILD_DIR:-<not set>}"
echo "==> Target itch.io channel: ${CHANNEL}"

# Validate build dir
if [ -z "${BUILD_DIR}" ]; then
    echo "ERROR: UNITY_BUILD_OUTPUT (or BUILD_PATH) is not set. Export the path to your Unity build output folder." >&2
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

echo "==> Pushing build directory to itch.io (incremental upload)..."
# Use butler to push the directory directly for incremental uploads
./butler push "${BUILD_DIR}" "${CHANNEL}" --userversion "${VERSION}"

echo "==> Upload complete."
