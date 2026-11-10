# VanijLiquor - web frontend (the app UI, also the frontend of the Tauri
# desktop build). A tiny zero-dependency Node server serves the static assets
# and proxies /api to the backend service.
#
# Build:  docker build -t vanijliquor-web new_code/web
FROM node:24-slim

ENV NODE_ENV=production \
    HOST=0.0.0.0 \
    PORT=8080 \
    API_URL=http://backend:7100

WORKDIR /app
COPY . .

EXPOSE 8080

HEALTHCHECK --interval=10s --timeout=3s --start-period=5s --retries=5 \
  CMD node -e "fetch('http://127.0.0.1:8080/').then(r=>process.exit(r.ok?0:1)).catch(()=>process.exit(1))"

CMD ["node", "server.js"]
