FROM mcr.microsoft.com/dotnet/runtime:8.0

ENV DEBIAN_FRONTEND=noninteractive

# Install Chromium's runtime dependencies and XVFB.
RUN apt-get update && \
    apt-get -y install --no-install-recommends \
        libgtk2.0-0 \
        libgtk-3-0 \
        libgbm1 \
        libnotify4 \
        libgconf-2-4 \
        libnss3 \
        libxss1 \
        libasound2 \
        libxtst6 \
        xauth \
        xvfb \
        x11-xserver-utils && \
    rm -rf /var/lib/apt/lists/*

# Set the working directory.
WORKDIR /Example.Docker

# Copy a script that starts a virtual X11 server and launches the application.
COPY startup.sh .

# Make sure the script is executable.
RUN chmod +x startup.sh

# Copy the project files.
COPY out/ .

# Set the entrypoint.
ENTRYPOINT ["/Example.Docker/startup.sh"]
