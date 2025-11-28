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

DETECTED_OS=$(echo "${BUILDER_OS:-$(uname -s)}" | tr '[:lower:]' '[:upper:]')
BUTLER_BIN="butler"
BUTLER_URL="https://broth.itch.ovh/butler/linux-amd64/LATEST/archive/default"

case "$DETECTED_OS" in
    WINDOWS*|MINGW*|CYGWIN*)
        BUTLER_BIN="butler.exe"
        BUTLER_URL="https://broth.itch.ovh/butler/windows-amd64/LATEST/archive/default"
        ;;
    DARWIN*|MAC*)
        BUTLER_URL="https://broth.itch.ovh/butler/darwin-amd64/LATEST/archive/default"
        ;;
    *)
        ;; # keep defaults for Linux
esac

extract_zip()
{
    local zip_file="$1"
    local dest_dir="$2"

    if command -v unzip >/dev/null 2>&1; then
        unzip -o "$zip_file" -d "$dest_dir"
        return 0
    fi

    if command -v python3 >/dev/null 2>&1; then
        python3 - "$zip_file" "$dest_dir" <<'PY'
import sys, zipfile
zipfile.ZipFile(sys.argv[1]).extractall(sys.argv[2])
PY
        return 0
    fi

    if command -v python >/dev/null 2>&1; then
        python - "$zip_file" "$dest_dir" <<'PY'
import sys, zipfile
zipfile.ZipFile(sys.argv[1]).extractall(sys.argv[2])
PY
        return 0
    fi

    if [[ "$DETECTED_OS" == WINDOWS* ]] && command -v powershell >/dev/null 2>&1; then
        powershell -NoProfile -Command "Add-Type -AssemblyName System.IO.Compression.FileSystem; [IO.Compression.ZipFile]::ExtractToDirectory('$zip_file', '$dest_dir', \$true)"
        return 0
    fi

    echo "ERROR: Could not extract $zip_file - no unzip/python/powershell available." >&2
    return 1
}

if [ ! -f "$BUTLER_BIN" ]; then
    echo "==> Downloading Butler for ${DETECTED_OS:-unknown}..."
    TMP_DIR=$(mktemp -d 2>/dev/null || mktemp -d -t butler)
    ZIP_PATH="$TMP_DIR/butler.zip"
    curl -L -o "$ZIP_PATH" "$BUTLER_URL"

    EXTRACT_DIR="$TMP_DIR/extracted"
    mkdir -p "$EXTRACT_DIR"
    extract_zip "$ZIP_PATH" "$EXTRACT_DIR"

    FOUND_BIN=$(find "$EXTRACT_DIR" -name "$BUTLER_BIN" -print -quit)
    if [ -z "$FOUND_BIN" ]; then
        echo "ERROR: Butler executable '$BUTLER_BIN' not found inside archive." >&2
        exit 1
    fi

    rm -f "$BUTLER_BIN"
    cp "$FOUND_BIN" "$BUTLER_BIN"

    if [[ "$DETECTED_OS" != WINDOWS* ]]; then
        chmod +x "$BUTLER_BIN"
    fi

    rm -rf "$TMP_DIR"
fi

BUTLER_CMD="./$BUTLER_BIN"
export BUTLER_API_KEY="$ITCHIO_API_KEY"

PUSH_PATH="$BUILD_DIR"
if [[ "$DETECTED_OS" == WINDOWS* ]]; then
    if command -v cygpath >/dev/null 2>&1; then
        PUSH_PATH=$(cygpath -w "$BUILD_DIR")
    elif command -v powershell >/dev/null 2>&1; then
        PUSH_PATH=$(powershell -NoProfile -Command "(Resolve-Path '$BUILD_DIR').Path")
    fi
fi

echo "==> Pushing build directory to itch.io (incremental upload)..."
"$BUTLER_CMD" push "${PUSH_PATH}" "${CHANNEL}" --userversion "${VERSION}"

echo "==> Upload complete."
