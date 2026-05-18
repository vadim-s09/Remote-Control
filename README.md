
## Module Directives

  [Network Sockets]     [OS Subprocesses]     [Win32 P/Invoke]
  - Public IP Api        - Netsh Interface     - Key Telemetry
  - Local Adapters       - Profile Parsing     - GDI Screen Buffer

1. **`controlHost` (Server Console Interface)**
   * Implements an asynchronous dynamic binding algorithm traversing ports `5000` through `5100` with native socket error (`AddressAlreadyInUse`) fallback containment.
   * Utilizes full-duplex communication pipelines separating outbound user-input tasks and inbound data serialization streams across thread boundaries.
2. **`control` (Client Execution Agent)**
   * **Network Auditing:** Executes regular expression parsing over subprocess standard output (`netsh.exe`/`powershell.exe`) pipelines to catalog logical configurations. Performs asynchronous REST requests against `api.ipify.org` to ascertain perimeter address mapping.
   * **WLAN Configuration Auditing:** Traverses interface profiles to isolate active Service Set Identifiers (SSIDs) and audit key storage visibility parameters.
   * **GDI Interface Acquisition:** Uses `System.Drawing` to bind screen coordinate space definitions dynamically via `Screen.FromPoint` to commit desktop visual state buffers directly into `.png` data models.
   * **Low-Level Native Input Telemetry:** Employs Windows Win32 API abstractions via Platform Invoke (`user32.dll` -> `GetAsyncKeyState`, `ToUnicode`, `MapVirtualKey`) to register layout hardware polling matrix behavior safely over isolated runtime threads.

---

## Developmental Status & AI Attribution

* **Experimental Target:** This repository constitutes a design prototype developed strictly for lab benchmarking, protocol tracing, and educational analysis of native platform interoperability. It lacks comprehensive exception isolation, advanced memory footprint telemetry, and transport-layer cryptographic sealing (e.g., TLS/mTLS).
* **AI Debugging & Optimization:** **Artificial Intelligence workflows were fully integrated during development to perform systematic optimization, code refactoring, structural code review, and thread concurrency debugging.** Specifically, AI utilities were used to validate structural safety barriers within `streamLock` mutex definitions, optimize the memory usage of the virtual keyboard state buffer, and ensure deterministic termination controls via `CancellationTokenSource` objects.

---

## Technical Specifications

### Minimum Requirements
* **SDK Compatibility:** .NET 6.0 LTS / .NET 8.0 LTS or higher
* **Target OS Infrastructure:** Microsoft Windows 10 / Windows 11 (X64 architecture required for reliable mapping of P/Invoke entry points and native execution binaries)
* **Assembly Requirements:** `System.Drawing.Common` (Requires explicit OS compilation target configuration to run smoothly on contemporary .NET architectures)

### Build & Package Automation

To compile both target solutions under the optimization configuration flag (`Release`), run the following commands within your terminal environment:

```bash
# Build the Host Server Environment
dotnet build ./controlHost/controlHost.csproj -c Release

# Build the Client Agent Service
dotnet build ./control/control.csproj -c Release
