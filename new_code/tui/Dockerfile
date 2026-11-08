# Madhushala Pro - terminal frontend (TUI) served in the browser via ttyd.
#
# The TUI is a normal terminal app, so there is nothing to "build": this image
# ships Node (to run it) and ttyd (to expose it over HTTP/WebSocket). Browsing
# to the published port opens a full terminal running the TUI, which talks to
# the backend over the compose network.
#
# Build:  docker build -t madhushala-tui new_code/tui
FROM node:24-slim

ARG TTYD_VERSION=1.7.7
ENV NODE_ENV=production \
    API_URL=http://backend:7100

RUN apt-get update \
 && apt-get install -y --no-install-recommends ca-certificates curl \
 && curl -fsSL -o /usr/local/bin/ttyd \
      "https://github.com/tsl0922/ttyd/releases/download/${TTYD_VERSION}/ttyd.x86_64" \
 && chmod +x /usr/local/bin/ttyd \
 && apt-get purge -y curl \
 && apt-get autoremove -y \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY package.json ./
COPY src ./src

EXPOSE 7681

HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=5 \
  CMD ttyd --version >/dev/null 2>&1 || exit 1

# --writable: allow keyboard input. The TUI is launched fresh for every
# browser session so each visitor gets their own login prompt.
CMD ["ttyd", "--writable", "--port", "7681", "node", "src/index.js"]
