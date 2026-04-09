# Document Number Validator .NET 10.0 Upgrade Tasks

## Overview

This document tracks the tier-by-tier upgrade of 46 projects from netstandard2.0/net6.0 to .NET 10.0 using the Bottom-Up strategy. Foundation libraries will be upgraded first, followed by common libraries, specialized libraries with early tests, and finally top-level test projects.

**Progress**: 14/14 tasks complete (100%) ![0%](https://progress-bar.xyz/100)

---

## Tasks

### [✓] TASK-001: Update Tier 1 foundation project files to net10.0 *(Completed: 2026-04-09 14:39)*
**References**: Plan §Tier 1, Plan §Project-by-Project Plans §1-6

- [✓] (1) Update `<TargetFramework>` from `netstandard2.0` to `net10.0` in all 6 Tier 1 project files per Plan §Tier 1 (ValidatorAbstractions, PaymentCardNumber.Generator.Common, Portugal.BankAccountNumber.Generator, Portugal.CitizenCard.Generator, Portugal.Nif.Generator, Portugal.Niss.Generator)
- [✓] (2) All 6 project files updated to net10.0 (**Verify**)

---

### [✓] TASK-002: Build and validate Tier 1 projects *(Completed: 2026-04-09 14:40)*
**References**: Plan §Tier 1 Summary, Plan §Tier 1 Completion Criteria

- [✓] (1) Build all 6 Tier 1 projects
- [✓] (2) All projects build successfully with 0 errors (**Verify**)
- [✓] (3) All projects build with 0 warnings (**Verify**)
- [✓] (4) No dependency conflicts reported (**Verify**)

---

### [✓] TASK-003: Commit Tier 1 changes *(Completed: 2026-04-09 14:42)*
**References**: Plan §Source Control Strategy

- [✓] (1) Commit Tier 1 changes with message: "Upgrade Tier 1 (Foundation Libraries) to .NET 10.0 - Updated 6 foundation projects from netstandard2.0 to net10.0"

---

### [✓] TASK-004: Update Tier 2 common library project files to net10.0 *(Completed: 2026-04-09 14:42)*
**References**: Plan §Tier 2, Plan §Project-by-Project Plans §7-12

- [✓] (1) Update `<TargetFramework>` from `netstandard2.0` to `net10.0` in all 6 Tier 2 project files per Plan §Tier 2 (InternationalBankAccountNumber.Validator, PaymentCardNumber.Common, PaymentCardNumber.MaestroUK.Generator, Portugal.CitizenCard.Validator, Portugal.Nif.Validator, Portugal.Niss.Validator)
- [✓] (2) All 6 project files updated to net10.0 (**Verify**)

---

### [✓] TASK-005: Build and validate Tier 2 projects *(Completed: 2026-04-09 14:43)*
**References**: Plan §Tier 2 Summary, Plan §Tier 2 Completion Criteria

- [✓] (1) Build all 6 Tier 2 projects
- [✓] (2) All projects build successfully with 0 errors (**Verify**)
- [✓] (3) All projects build with 0 warnings (**Verify**)
- [✓] (4) All Tier 1 dependencies resolve correctly (**Verify**)
- [✓] (5) No dependency conflicts reported (**Verify**)

---

### [✓] TASK-006: Commit Tier 2 changes *(Completed: 2026-04-09 14:43)*
**References**: Plan §Source Control Strategy

- [✓] (1) Commit Tier 2 changes with message: "Upgrade Tier 2 (Common Libraries) to .NET 10.0 - Updated 6 common/domain projects from netstandard2.0 to net10.0"

---

### [✓] TASK-007: Update Tier 3 specialized library and test project files to net10.0 *(Completed: 2026-04-09 14:45)*
**References**: Plan §Tier 3, Plan §Project-by-Project Plans §13-32

- [✓] (1) Update `<TargetFramework>` from `netstandard2.0` to `net10.0` in all 11 Tier 3 source project files per Plan §Tier 3 (all payment card generators and validators, Portugal.BankAccountNumber.Validator)
- [✓] (2) Update `<TargetFrameworks>` from multi-targeting to `<TargetFramework>net10.0</TargetFramework>` in all 8 Tier 3 test project files per Plan §Tier 3 (PaymentCardNumber.Common.Tests, InternationalBankAccountNumber.Validator.Tests, 6 Portugal test projects)
- [✓] (3) All 20 project files updated to net10.0 (**Verify**)

---

### [✓] TASK-008: Build and validate Tier 3 projects *(Completed: 2026-04-09 16:49)*
**References**: Plan §Tier 3 Summary

- [✓] (1) Build all 20 Tier 3 projects (11 source + 8 tests)
- [✓] (2) All 20 projects build successfully with 0 errors (**Verify**)
- [✓] (3) All 20 projects build with 0 warnings (**Verify**)
- [✓] (4) All dependencies to Tiers 1-2 resolve correctly (**Verify**)
- [✓] (5) No dependency conflicts reported (**Verify**)

---

### [✓] TASK-009: Run Tier 3 test suite and validate *(Completed: 2026-04-09 17:52)*
**References**: Plan §Tier 3 Summary, Plan §Testing & Validation Strategy §Tier 3 Validation

- [✓] (1) Run all 8 test projects in Tier 3 per Plan §Tier 3 Summary
- [✓] (2) Fix any test failures (reference Plan §Breaking Changes if needed)
- [✓] (3) Re-run tests after fixes
- [✓] (4) All tests pass with 0 failures (**Verify**)

---

### [✓] TASK-010: Commit Tier 3 changes *(Completed: 2026-04-09 16:55)*
**References**: Plan §Source Control Strategy

- [✓] (1) Commit Tier 3 changes with message: "Upgrade Tier 3 (Specialized Libraries & Early Tests) to .NET 10.0 - Updated 11 source + 8 test projects from netstandard2.0/multi-target to net10.0"

---

### [✓] TASK-011: Update Tier 4 test project files to net10.0 *(Completed: 2026-04-09 16:56)*
**References**: Plan §Tier 4, Plan §Project-by-Project Plans §33-46

- [✓] (1) Update `<TargetFrameworks>` from multi-targeting to `<TargetFramework>net10.0</TargetFramework>` in all 14 Tier 4 test project files per Plan §Tier 4 (all payment card and Portugal banking test projects)
- [✓] (2) All 14 project files updated to net10.0 (**Verify**)

---

### [✓] TASK-012: Build and validate Tier 4 projects *(Completed: 2026-04-09 16:56)*
**References**: Plan §Tier 4 Summary

- [✓] (1) Build all 14 Tier 4 test projects
- [✓] (2) All 14 projects build successfully with 0 errors (**Verify**)
- [✓] (3) All 14 projects build with 0 warnings (**Verify**)
- [✓] (4) All dependencies to Tiers 1-3 resolve correctly (**Verify**)
- [✓] (5) No dependency conflicts across entire solution (**Verify**)

---

### [✓] TASK-013: Run full test suite and validate upgrade *(Completed: 2026-04-09 17:56)*
**References**: Plan §Tier 4 Summary, Plan §Testing & Validation Strategy §Tier 4 Validation, Plan §Success Criteria

- [✓] (1) Run all 14 test projects in Tier 4 plus re-run 8 from Tier 3 (22 total) per Plan §Tier 4 Summary
- [✓] (2) Fix any test failures (reference Plan §Breaking Changes if needed)
- [✓] (3) Re-run tests after fixes
- [✓] (4) All 22 test projects pass with 0 failures (**Verify**)
- [✓] (5) Full solution builds successfully (all 46 projects) (**Verify**)

---

### [✓] TASK-014: Commit Tier 4 changes and complete upgrade *(Completed: 2026-04-09 16:58)*
**References**: Plan §Source Control Strategy, Plan §Success Criteria

- [✓] (1) Commit Tier 4 changes with message: "Upgrade Tier 4 (Final Test Projects) to .NET 10.0 - Updated 14 test projects from multi-target to net10.0, all 46 projects now on .NET 10.0"

---



















