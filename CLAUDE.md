# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

### Building
```bash
dotnet build
dotnet build --configuration Release
```

### TypeScript Compilation
The project includes TypeScript files in `wwwroot/` which are automatically compiled using MSBuild TypeScript integration. TypeScript settings are configured in `tsconfig.json` with ES2022 target and comments removed.

### Testing
No test projects found in current repository structure. This appears to be a library project with external test dependencies referenced in `Directory.Build.props`.

### Packaging
```bash
dotnet pack --configuration Release
```

## Architecture Overview

### Core Purpose
This is a WebAssembly-specific services infrastructure library (`Cirreum.Services.Client`) that provides foundational services for Blazor WebAssembly applications. It follows the Cirreum Foundation Framework pattern with layered simplicity.

### Key Service Categories

**State Management**
- `StateManager`: Thread-safe application state management with subscriber notifications and caching
- `StateContainer`, `StateHandle`: Generic state containers with persistence capabilities
- Specialized states: `LocalState`, `SessionState`, `PageState`, `ThemeState`
- `PersistableStateContainer`: State that can be persisted across sessions

**Session & Authorization**
- `SessionManager`: Sophisticated session lifecycle management with configurable stages (SafeZone/WatchZone)
- `SessionOptions`, `SessionStage`: Configurable session timeout and activity monitoring
- `AuthorizationRoleRegistry`: Role-based authorization support
- `SessionHttpHandler`: HTTP activity detection for session extension

**Browser Integration**
- `LocalStorageService`, `SessionStorageService`: Browser storage abstractions
- `WasmFileSystem`: File system operations for WebAssembly
- `SessionActivityMonitor` (TypeScript): DOM activity monitoring with throttling

**Infrastructure Services**
- `DateTimeService`: Clock abstraction using `TimeProvider`
- `UserPresenceMonitor`, `UserPresenceService`: User presence tracking
- `CsvFileBuilder`, `CsvFileReader`: CSV file operations
- `CspBuilder`: Content Security Policy management

### Hosting Extensions Pattern
The library uses a fluent hosting extensions pattern in `HostingExtensions.Core.cs`:
- `AddCoreServices()`: Registers all core services with optional storage configuration
- Individual service registration methods for modular setup

### Dependency Integration
- Built on Microsoft.Extensions.* patterns (DI, logging, configuration)
- Integrates with ASP.NET Core Components and WebAssembly Authentication
- Uses Cirreum.Core, Cirreum.Startup, and Cirreum.Storage.Browser packages

### State Management Architecture
The state system uses a central `StateManager` with:
- Type-safe state retrieval and subscription
- Automatic interface resolution for state contracts
- Cached subscriber lists with version tracking for performance
- Support for both parameterless and parameterized state notifications

### Session Management Flow
Sessions use a two-stage approach:
1. **SafeZone** (0-90% of timeout): Minimal activity monitoring
2. **WatchZone** (90-100%): Active monitoring with debounced extension

Activity is detected through DOM events (TypeScript) and HTTP calls (.NET), with configurable throttling multipliers per stage.

## Project Structure
- `src/Cirreum.Services.Client/`: Main library code
- `wwwroot/`: TypeScript/JavaScript browser integration files
- `build/`: Shared MSBuild properties and configuration
- Single `.slnx` solution file for modern .NET project management

## Framework Targets
- .NET 10.0 with C# latest language version
- Browser platform support only (WebAssembly)
- Nullable reference types enabled