# Madhushala Pro - decompilation toolchain image.
#
# Contains everything needed to decompile the application's .NET assemblies and
# to inspect / patch the SQL Server .MDF databases:
#   * .NET SDK 8            -> runs ilspycmd
#   * ilspycmd (ILSpy 8.2)  -> .NET decompiler
#   * Mono + monodis        -> extra CLI / disassembly fallback
#   * Python 3              -> dev/scripts/mdf_users.py
#   * git + git-lfs         -> fetch the LFS-stored main EXE
#   * unzip/file            -> general inspection helpers
#
# Build (see docker-compose.yml for the easy path):
#   docker build -t madhushala-decompiler dev/docker
#   # or, from the repository root:
#   docker compose -f dev/docker-compose.yml build
#
FROM mcr.microsoft.com/dotnet/sdk:8.0

ENV DEBIAN_FRONTEND=noninteractive \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=1 \
    DOTNET_ROLL_FORWARD=LatestMajor

# System packages: mono (monodis fallback), python, git + git-lfs, misc tools.
RUN apt-get update \
 && apt-get install -y --no-install-recommends \
      ca-certificates \
      curl \
      file \
      git \
      git-lfs \
      gnupg \
      less \
      mono-devel \
      mono-utils \
      python3 \
      python3-pip \
      unzip \
 && rm -rf /var/lib/apt/lists/*

# ilspycmd - the ILSpy command-line decompiler.
# Install into /usr/local/bin (always on PATH, including login shells) and pin
# the version used to produce the committed decode/ tree (8.2.0.7535).
ARG ILSPY_VERSION=8.2.0.7535
RUN dotnet tool install --tool-path /usr/local/bin ilspycmd --version "${ILSPY_VERSION}" \
 || dotnet tool install --tool-path /usr/local/bin ilspycmd

# Work happens in /work, mounted from the host repository root.
WORKDIR /work

# Default: drop into a shell. The compose file overrides this to run the
# decompile script automatically.
CMD ["/bin/bash"]
