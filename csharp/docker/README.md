# DotNetBrowser in Docker

This example demonstrates how to run a DotNetBrowser Avalonia application inside a Docker container, with support for
both headless and desktop (X11) modes.

## Prerequisites

- [Docker Engine](https://docs.docker.com/engine/install/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html)

## Configure the License Key

Open `MainWindow.axaml.cs` and set your license key:

```csharp
engine = EngineFactory.Create(new EngineOptions.Builder
{
    LicenseKey = "<your_license_key>",
    ...
}.Build());
```

## Build and Run

### 1. Publish the application

```bash
dotnet publish -c Release -o out
```

### 2. Build the Docker image

```bash
docker build -t dnb-app -f Dockerfile .
```

### 3. Run the container

#### Headless mode

Runs with a virtual X server (Xvfb). Suitable for CI and server environments.

```bash
docker run --rm --shm-size=1g dnb-app
```

When the page loads, you should see the title printed to the console:

```text
Title: Google
```

#### Desktop mode (Linux with X11)

Passes the host X server to the container so the application window appears on your desktop.

First, allow local connections to your X server:

```bash
xhost +local:root
```

Then run the container:

```bash
docker run --rm \
  --shm-size=1g \
  -e DISPLAY=$DISPLAY \
  -v /tmp/.X11-unix:/tmp/.X11-unix \
  dnb-app
```

When you're done, revoke the X server permission:

```bash
xhost -local:root
```

## Remote Debugging

The application exposes Chromium DevTools on port 9222. Since DevTools are bound to the container's localhost, use SSH
port forwarding to access them from the host.

Start the container with the SSH port published:

```bash
docker run -d -p 2222:22 --shm-size=1g dnb-app
```

Open a shell inside the running container:

```bash
docker exec -it <container_id> /bin/bash
```

Install and start the SSH server inside the container:

```bash
apt install -y openssh-server
service ssh start
```

Create a user for SSH access:

```bash
useradd --create-home --shell /bin/bash dnb-app
passwd dnb-app
```

On the host, forward the remote debugging port:

```bash
ssh -L 9222:localhost:9222 -p 2222 dnb-app@localhost
```

Keep this SSH session open, then navigate to `chrome://inspect` in Google Chrome on the host to access DevTools.
