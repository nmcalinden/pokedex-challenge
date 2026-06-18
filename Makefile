.PHONY: help install frontend backend db up down test test-backend test-frontend clean

DOTNET_ROOT ?= /usr/local/opt/dotnet/libexec
export DOTNET_ROOT
export PATH := $(DOTNET_ROOT):$(DOTNET_ROOT)/tools:$(PATH)

help: ## Show available targets
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-16s\033[0m %s\n", $$1, $$2}'

# ── Install ──────────────────────────────────────────────────────

install: ## Install dependencies for both projects
	cd frontend && npm install --include=dev
	cd backend && dotnet restore

# ── Local development ────────────────────────────────────────────

frontend: ## Start the React dev server on :3000
	cd frontend && npm run dev

backend: ## Start the .NET API on :5001 (requires PostgreSQL)
	cd backend && dotnet run --project Pokedex.Api

db: ## Start PostgreSQL via Docker Compose
	docker compose up db -d

# ── Docker Compose ───────────────────────────────────────────────

up: ## Start the full stack via Docker Compose
	docker compose up --build

down: ## Stop all Docker Compose services
	docker compose down

# ── Tests ────────────────────────────────────────────────────────

test: test-backend test-frontend ## Run all tests

test-backend: ## Run .NET xUnit tests
	cd backend && dotnet test

test-frontend: ## Run Vitest tests
	cd frontend && npm test

# ── Cleanup ──────────────────────────────────────────────────────

clean: ## Remove build artifacts
	cd backend && dotnet clean
	rm -rf frontend/dist frontend/node_modules/.vite
