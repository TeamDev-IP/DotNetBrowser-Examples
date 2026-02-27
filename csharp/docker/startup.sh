#!/bin/sh

set -e

# Launch a virtual X11 server for Chromium.
#
# If DISPLAY is not set, start a virtual X server (headless mode).
# If DISPLAY is set, connect to the host X server (desktop mode).
if [ -z "${DISPLAY:-}" ]; then
  Xvfb :0 -screen 0 1920x1080x24 &
  export DISPLAY=:0
fi

# Start the .NET application.
exec dotnet Example.Docker.dll
