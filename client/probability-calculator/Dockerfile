# Base image for building the application
FROM node:alpine AS builder
WORKDIR /site

# Install dependencies
COPY ./ ./
RUN npm install
RUN npm run build:docker
RUN ls -la

# Production image
FROM node:alpine AS runner
WORKDIR /site

ENV NODE_ENV=production
ENV GENERATE_SOURCEMAP=false

# Copy only necessary files from the builder stage
COPY --from=builder /site/next.config.ts ./
COPY --from=builder /site/.next ./.next
COPY --from=builder /site/node_modules ./node_modules
COPY --from=builder /site/package.json ./package.json

# Set up user and permissions
RUN addgroup -g 1001 -S nodejs && adduser -S nextjs -u 1001 && chown -R nextjs:nodejs /site/.next
USER nextjs

# Expose port and start the application
EXPOSE 3000
CMD npm run start

