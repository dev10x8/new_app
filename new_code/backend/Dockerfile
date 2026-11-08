# Madhushala Pro - backend image (Node.js + node:sqlite).
#
# There are no third-party runtime dependencies, so the image is just the
# source tree plus Node. The SQLite file lives in /data (a named volume in
# docker-compose.yml) and is created + seeded on first start.
#
# Build:  docker build -t madhushala-backend new_code/backend
FROM node:24-slim

ENV NODE_ENV=production \
    HOST=0.0.0.0 \
    PORT=7100 \
    MADHUSHALA_DB=/data/madhushala.db

WORKDIR /app

COPY package.json ./
COPY src ./src
COPY scripts ./scripts
COPY test ./test

RUN mkdir -p /data
VOLUME ["/data"]

EXPOSE 7100

HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=5 \
  CMD node -e "fetch('http://127.0.0.1:7100/api/health').then(r=>process.exit(r.ok?0:1)).catch(()=>process.exit(1))"

CMD ["node", "src/server.js"]
