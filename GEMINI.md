# YayogApp - Engineering Standards & Architecture

This document defines the foundational mandates for development on the YayogApp project.

## 🏛 Architecture: Blazor Web App (Interactive Auto)

We use the **Vertical Slice Architecture** pattern where possible, organized into:
- **`src/YayogApp` (Server):** Data access, API endpoints, and initial SSR.
- **`src/YayogApp.Client` (WebAssembly):** UI components and client-side logic.
- **`src/YayogApp.Shared`:** Domain models, DTOs, and shared validation logic.

### Dependency Injection (DI) Strategy
To support **Interactive Auto**, services must be defined by interfaces in `Shared`.
- The Server registers a "Direct" implementation (e.g., hitting DB/EfCore).
- The Client registers an "API" implementation (using `HttpClient`).
- Components use the interface, remaining agnostic of the rendering mode.

## 🏆 Testing Strategy: The Test Trophy
We prioritize tests based on their ROI (Return on Investment), following the Test Trophy:

1.  **Integration Tests (The Bulk):** Focus on `tests/YayogApp.IntegrationTests`. Use `WebApplicationFactory` to test the interaction between components, APIs, and services.
2.  **Unit Tests (The Core):** For complex business logic in `Shared` or critical service methods.
3.  **Static Analysis:** Rigorous use of .NET Analyzers and linting.
4.  **E2E (The Top):** Minimal, high-value flows (e.g., Playwright).

**Mandate:** Every new feature MUST include an integration test covering the primary "Happy Path."

## 🛠 .NET & C# Conventions
- **C# 13/14 Features:** Use Primary Constructors, Raw String Literals, and Collection Expressions.
- **File-Scoped Namespaces:** Always use `namespace MyNamespace;`.
- **Nullable Reference Types:** Strict enforcement (`<Nullable>enable</Nullable>`).
- **Minimal APIs:** Use for backend endpoints instead of heavy Controllers.
- **Blazor:** Prefer `@rendermode InteractiveAuto` for high-interactivity components.

## 📦 Project Structure
- `/src`: Application source code.
- `/tests`: All test projects.
- `/docs`: Architectural Decision Records (ADR).
