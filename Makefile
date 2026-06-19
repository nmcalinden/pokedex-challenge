.PHONY: help install dev server ui db migrate down clean

DOTNET_ROOT ?= /usr/local/opt/dotnet/libexec
export DOTNET_ROOT
export PATH := $(DOTNET_ROOT):$(DOTNET_ROOT)/tools:$(PATH)

help: ## Show available targets
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-16s\033[0m %s\n", $$1, $$2}'

# ── Install ──────────────────────────────────────────────────────

install: ## Install dependencies for both projects
	cd ui && npm install --include=dev
	cd server && dotnet restore

# ── Run ───────────────────────────────────────────────────────────

dev: db migrate ## Run the app: DB + migrations in Docker, API + UI natively (Ctrl+C stops both)
	cd ui && npm install
	@echo "API → http://localhost:5001   UI → http://localhost:3000   (Ctrl+C stops both)"
	@trap 'kill 0' EXIT; \
		( cd server && ASPNETCORE_URLS=http://localhost:5001 dotnet run --project Pokedex.Api ) & \
		( cd ui && npm run dev ) & \
		wait

server: ## Run just the API on :5001 (dotnet; needs the DB)
	cd server && ASPNETCORE_URLS=http://localhost:5001 dotnet run --project Pokedex.Api

ui: ## Run just the UI on :3000 (npm)
	cd ui && npm run dev

# ── Database (Docker) ─────────────────────────────────────────────

db: ## Start PostgreSQL (Docker)
	docker compose up db -d

migrate: ## Apply Flyway migrations + local seed (scans database/; needs the DB)
	docker compose run --rm flyway -connectRetries=10 migrate

down: ## Stop the Docker services (database)
	docker compose down

# ── Cleanup ──────────────────────────────────────────────────────

clean: ## Remove build artifacts
	cd server && dotnet clean
	rm -rf ui/dist ui/node_modules/.vite
