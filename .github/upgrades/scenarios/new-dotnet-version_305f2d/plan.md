# .NET 10.0 Upgrade Plan

## Table of Contents
1. [Executive Summary](#executive-summary)
2. [Migration Strategy](#migration-strategy)
3. [Detailed Dependency Analysis](#detailed-dependency-analysis)
4. [Project-by-Project Plans](#project-by-project-plans)
5. [Risk Management](#risk-management)
6. [Testing & Validation Strategy](#testing--validation-strategy)
7. [Complexity & Effort Assessment](#complexity--effort-assessment)
8. [Source Control Strategy](#source-control-strategy)
9. [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
Upgrade all projects in the document-number-validator solution from their current target frameworks to .NET 10.0 (Long Term Support).

### Scope

**Current State:**
- 24 source library projects targeting `netstandard2.0`
- 22 test projects multi-targeting: `net6.0;net7.0;net462;net472;net48;net481`
- Clear 4-level dependency hierarchy (Level 0 through Level 3)
- No external dependencies or packages requiring updates

**Target State:**
- All source libraries upgraded to `net10.0`
- All test projects updated to target `net10.0` only (removing multi-targeting)
- Maintain existing project structure and dependencies

### Selected Strategy: Bottom-Up (Dependency-First)

This upgrade follows a **Bottom-Up Strategy**, migrating projects tier-by-tier starting from foundation libraries (no dependencies) and progressing upward through the dependency chain to test projects.

**Rationale:**
- Clear 4-level dependency hierarchy enables clean tier-based progression
- Each tier builds on stable, already-upgraded dependencies
- No multi-targeting complexity needed (dependencies always on same or newer framework)
- Lowest risk approach with clear validation points after each tier

### Complexity Assessment

**Classification: Medium Complexity**

**Discovered Metrics:**
- **Project count**: 46 (24 source + 22 tests)
- **Dependency depth**: 4 levels
- **Issues**: 22 mandatory (all test projects - framework updates only)
- **Risk indicators**: None (no security vulnerabilities, no breaking changes detected)

**Justification:**
- Solution size is moderate (46 projects across 4 dependency tiers)
- Changes are straightforward (framework version updates only)
- No package updates or code modifications required
- All changes are low-risk framework identifier updates

### Critical Issues
- None identified - this is a clean framework upgrade with no breaking changes

### Recommended Approach
**Incremental tier-by-tier upgrade** following Bottom-Up principles:
- Migrate Level 0 (foundation libraries) first
- Progress through each level after validating the previous level
- Complete with Level 3 (test projects) last

### Iteration Strategy
**Phase-based approach** (one iteration per dependency level):
- Phase 1 (Discovery & Classification): Complete
- Phase 2 (Foundation): Create dependency analysis, migration strategy, project stubs
- Phase 3 (Detail Generation): One iteration per dependency level (4 iterations for Levels 0-3)
- **Total expected iterations**: 8

---

## Migration Strategy

### Approach Selection

**Selected Approach: Incremental Tier-by-Tier Migration**

**Justification:**
- **Project count** (46 projects) warrants a structured, phased approach
- **Clear dependency hierarchy** (4 distinct levels) enables natural tier-based progression
- **Bottom-Up Strategy alignment**: Upgrading foundation libraries first minimizes risk and eliminates the need for multi-targeting
- **Validation checkpoints**: Each tier completion provides a stable validation point before proceeding
- **Risk mitigation**: Issues isolated to current tier, not cascading across entire solution

**Why not All-At-Once:**
- 46 projects is too large for simultaneous upgrade
- Phased approach enables better tracking and validation
- Tier-by-tier progression provides clear rollback points

### Bottom-Up Strategy Rationale

This upgrade follows the **Bottom-Up (Dependency-First) Strategy**:

**Core Principle**: Upgrade projects sequentially starting from leaf nodes (projects with no internal dependencies) and progressing upward through the dependency chain to test applications.

**Specific Advantages for This Solution:**
1. **No Multi-Targeting Needed**: Since we upgrade dependencies before their consumers, all projects always reference same-or-newer framework versions
2. **Stable Foundation**: Each tier builds on already-upgraded, tested dependencies
3. **Clear Validation**: Test projects (migrated last) validate entire upgraded stack
4. **Isolated Issues**: Problems surface in current tier only, not across multiple layers
5. **Learning Curve**: Lessons from foundation libraries (Tier 1) inform later tiers

### Dependency-Based Ordering Rationale

**Ordering Principles Applied:**

1. **Tier 0 (implicit)**: No external NuGet packages to update
2. **Tier 1 (Level 0 projects)**: 6 projects with zero internal dependencies
   - Must be upgraded first as all other projects depend on them
   - Can be upgraded in parallel (no interdependencies)
3. **Tier 2 (Level 1 projects)**: 6 projects depending only on Tier 1
   - Upgrade after Tier 1 is validated
   - Can be upgraded in parallel within this tier
4. **Tier 3 (Level 2 projects)**: 20 projects depending on Tiers 1-2
   - Includes 11 source projects + 8 test projects
   - Upgrade after Tier 2 is validated
5. **Tier 4 (Level 3 projects)**: 14 test projects depending on Tiers 1-3
   - Final tier - validates entire upgraded solution
   - Can be upgraded in parallel within this tier

**Critical Ordering Rule**: Cannot start Tier N+1 until Tier N is fully validated (builds successfully, tests pass).

### Parallel vs Sequential Execution

**Within Each Tier: Parallel Execution Possible**
- Projects within the same tier have no dependencies on each other
- Tier 1: All 6 projects can be updated simultaneously
- Tier 2: All 6 projects can be updated simultaneously
- Tier 3: All 20 projects can be updated simultaneously
- Tier 4: All 14 projects can be updated simultaneously

**Between Tiers: Sequential Execution Required**
- Must complete and validate Tier N before starting Tier N+1
- Ensures upgraded dependencies are stable before consumers are upgraded

**Implementation Approach:**
- **For execution**: Batch all projects within a tier into a single operation
- **For validation**: Test entire tier together before proceeding

### Phase Definitions

#### **Phase 1: Foundation Libraries (Tier 1)**
- **Projects**: 6 foundation libraries (Level 0)
- **Changes**: Update `TargetFramework` from `netstandard2.0` to `net10.0`
- **Validation**: All 6 projects build successfully with no errors/warnings
- **Estimated Complexity**: Low

#### **Phase 2: Common Libraries (Tier 2)**
- **Projects**: 6 common/domain libraries (Level 1)
- **Changes**: Update `TargetFramework` from `netstandard2.0` to `net10.0`
- **Validation**: All projects build, consumers can still reference them
- **Estimated Complexity**: Low

#### **Phase 3: Specialized Libraries & Early Tests (Tier 3)**
- **Projects**: 11 specialized source projects + 8 test projects (Level 2)
- **Changes**: 
  - Source projects: Update `TargetFramework` from `netstandard2.0` to `net10.0`
  - Test projects: Update `TargetFrameworks` from `net6.0;net7.0;net462;net472;net48;net481` to `net10.0`
- **Validation**: All projects build, early test projects pass their tests
- **Estimated Complexity**: Low (straightforward framework updates)

#### **Phase 4: Final Test Projects (Tier 4)**
- **Projects**: 14 top-level test projects (Level 3)
- **Changes**: Update `TargetFrameworks` from multi-targeting to `net10.0` only
- **Validation**: All tests pass, full solution validated
- **Estimated Complexity**: Low

### Execution Sequence

```
Start
  ↓
Phase 1: Tier 1 (Foundation - 6 projects)
  → Update all 6 project files
  → Build all 6 projects
  → Validate no errors/warnings
  ↓
Phase 2: Tier 2 (Common - 6 projects)
  → Update all 6 project files
  → Build all 6 projects
  → Validate no errors/warnings
  ↓
Phase 3: Tier 3 (Specialized - 20 projects)
  → Update all 20 project files (11 source + 8 tests)
  → Build all 20 projects
  → Run tests for 8 test projects in this tier
  → Validate no errors/warnings
  ↓
Phase 4: Tier 4 (Final Tests - 14 projects)
  → Update all 14 project files
  → Build all 14 projects
  → Run all 14 test projects
  → Full solution validation
  ↓
Complete
```

### Tier Completion Criteria

**Phase 1 Complete When:**
- [ ] All 6 project files updated to `net10.0`
- [ ] All 6 projects build without errors
- [ ] All 6 projects build without warnings
- [ ] No dependency conflicts reported

**Phase 2 Complete When:**
- [ ] All 6 project files updated to `net10.0`
- [ ] All 6 projects build without errors
- [ ] All 6 projects build without warnings
- [ ] Consumers (Tier 3) can still reference Tier 2 projects
- [ ] No dependency conflicts

**Phase 3 Complete When:**
- [ ] All 20 project files updated to `net10.0`
- [ ] All 20 projects build without errors
- [ ] All 20 projects build without warnings
- [ ] 8 test projects in this tier pass all tests
- [ ] No dependency conflicts

**Phase 4 Complete When:**
- [ ] All 14 project files updated to `net10.0`
- [ ] All 14 projects build without errors
- [ ] All 14 projects build without warnings
- [ ] All 22 test projects pass all tests (cumulative)
- [ ] Full solution builds successfully
- [ ] No dependency conflicts across entire solution

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution has a clear 4-level dependency hierarchy with no circular dependencies:

```
Level 3 (Test Projects - 22 projects)
  ↓ (depend on)
Level 2 (Generators, Validators - 17 projects)
  ↓ (depend on)
Level 1 (Common Libraries - 3 projects)
  ↓ (depend on)
Level 0 (Foundation - 6 projects, no dependencies)
```

### Project Groupings by Migration Tier

Following Bottom-Up Strategy principles, projects are grouped into tiers based on their dependency level:

#### **Tier 1 (Level 0 - Foundation Libraries)**
**6 projects with zero internal dependencies:**

1. `DocumentNumber.PaymentCardNumber.Generator.Common` - Base generator utilities
2. `DocumentNumber.Portugal.BankAccountNumber.Generator` - Bank account number generation
3. `DocumentNumber.Portugal.CitizenCard.Generator` - Citizen card generation
4. `DocumentNumber.Portugal.Nif.Generator` - NIF (tax ID) generation
5. `DocumentNumber.Portugal.Niss.Generator` - NISS (social security) generation
6. `DocumentNumber.ValidatorAbstractions` - Validator interfaces

**Rationale**: These projects have no dependencies and are consumed by multiple higher-level projects. Upgrading them first establishes a stable foundation.

**Issues**: 0 mandatory

#### **Tier 2 (Level 1 - Common & Domain Libraries)**
**3 projects depending only on Tier 1:**

1. `DocumentNumber.InternationalBankAccountNumber.Validator` 
   - Depends on: `ValidatorAbstractions`
2. `DocumentNumber.PaymentCardNumber.Common`
   - Depends on: `ValidatorAbstractions`
3. `DocumentNumber.PaymentCardNumber.MaestroUK.Generator`
   - Depends on: `PaymentCardNumber.Generator.Common`
4. `DocumentNumber.Portugal.CitizenCard.Validator`
   - Depends on: `ValidatorAbstractions`, `Portugal.CitizenCard.Generator`
5. `DocumentNumber.Portugal.Nif.Validator`
   - Depends on: `ValidatorAbstractions`, `Portugal.Nif.Generator`
6. `DocumentNumber.Portugal.Niss.Validator`
   - Depends on: `Portugal.Niss.Generator`, `ValidatorAbstractions`

**Rationale**: These projects depend only on Tier 1 (already upgraded) and provide domain-specific validation/generation logic used by higher tiers.

**Issues**: 0 mandatory

#### **Tier 3 (Level 2 - Specialized Validators & Generators)**
**17 projects depending on Tiers 1-2:**

**Payment Card Projects:**
1. `DocumentNumber.PaymentCardNumber.AmericanExpress.Generator`
   - Depends on: `PaymentCardNumber.Common`, `PaymentCardNumber.Generator.Common`
2. `DocumentNumber.PaymentCardNumber.AmericanExpress.Validator`
   - Depends on: `PaymentCardNumber.Common`, `ValidatorAbstractions`
3. `DocumentNumber.PaymentCardNumber.Maestro.Generator`
   - Depends on: `PaymentCardNumber.Common`, `PaymentCardNumber.Generator.Common`
4. `DocumentNumber.PaymentCardNumber.Maestro.Validator`
   - Depends on: `PaymentCardNumber.Common`, `ValidatorAbstractions`
5. `DocumentNumber.PaymentCardNumber.MaestroUK.Validator`
   - Depends on: `PaymentCardNumber.Common`, `ValidatorAbstractions`
6. `DocumentNumber.PaymentCardNumber.Mastercard.Generator`
   - Depends on: `PaymentCardNumber.Common`, `PaymentCardNumber.Generator.Common`
7. `DocumentNumber.PaymentCardNumber.Mastercard.Validator`
   - Depends on: `PaymentCardNumber.Common`, `ValidatorAbstractions`
8. `DocumentNumber.PaymentCardNumber.VISA.Generator`
   - Depends on: `PaymentCardNumber.Common`, `PaymentCardNumber.Generator.Common`
9. `DocumentNumber.PaymentCardNumber.VISA.Validator`
   - Depends on: `PaymentCardNumber.Common`, `ValidatorAbstractions`
10. `DocumentNumber.PaymentCardNumber.VISAElectron.Generator`
    - Depends on: `PaymentCardNumber.Common`, `PaymentCardNumber.Generator.Common`
11. `DocumentNumber.PaymentCardNumber.VISAElectron.Validator`
    - Depends on: `PaymentCardNumber.Common`, `ValidatorAbstractions`

**Portugal Banking Projects:**
12. `DocumentNumber.Portugal.BankAccountNumber.Validator`
    - Depends on: `ValidatorAbstractions`, `InternationalBankAccountNumber.Validator`

**Test Projects (1 in this tier):**
13. `DocumentNumber.PaymentCardNumber.Common.Tests`
    - Depends on: `PaymentCardNumber.Common`
14. `DocumentNumber.InternationalBankAccountNumber.Validator.Tests`
    - Depends on: `InternationalBankAccountNumber.Validator`
15. `DocumentNumber.Portugal.CitizenCard.Generator.Tests`
    - Depends on: `Portugal.CitizenCard.Validator`
16. `DocumentNumber.Portugal.Nif.Generator.Tests`
    - Depends on: `Portugal.Nif.Validator`
17. `DocumentNumber.Portugal.Niss.Generator.Tests`
    - Depends on: `Portugal.Niss.Validator`
18. `Portugal.CitizenCard.Validator.Tests`
    - Depends on: `Portugal.CitizenCard.Validator`
19. `Portugal.Nif.Validator.Tests`
    - Depends on: `Portugal.Nif.Validator`
20. `Portugal.Niss.Validator.Tests`
    - Depends on: `Portugal.Niss.Validator`

**Rationale**: These projects implement specific card types and validation logic. They depend on common libraries from Tiers 1-2.

**Issues**: 8 mandatory (test projects only - framework updates)

#### **Tier 4 (Level 3 - Top-Level Test Projects)**
**14 test projects depending on Tiers 1-3:**

1. `DocumentNumber.PaymentCardNumber.AmericaExpress.Validator.Tests`
2. `DocumentNumber.PaymentCardNumber.AmericanExpress.Generator.Tests`
3. `DocumentNumber.PaymentCardNumber.Maestro.Generator.Tests`
4. `DocumentNumber.PaymentCardNumber.Maestro.Validator.Tests`
5. `DocumentNumber.PaymentCardNumber.MaestroUK.Generator.Tests`
6. `DocumentNumber.PaymentCardNumber.MaestroUK.Validator.Tests`
7. `DocumentNumber.PaymentCardNumber.Mastercard.Generator.Tests`
8. `DocumentNumber.PaymentCardNumber.Mastercard.Validator.Tests`
9. `DocumentNumber.PaymentCardNumber.VISA.Generator.Tests`
10. `DocumentNumber.PaymentCardNumber.VISA.Validator.Tests`
11. `DocumentNumber.PaymentCardNumber.VISAElectron.Generator.Tests`
12. `DocumentNumber.PaymentCardNumber.VISAElectron.Validator.Tests`
13. `DocumentNumber.Portugal.BankAccountNumber.Generator.Tests`
14. `DocumentNumber.Portugal.BankAccountNumber.Validator.Tests`

**Rationale**: Test projects depend on their corresponding generator/validator projects. These are migrated last to ensure all dependencies are on net10.0.

**Issues**: 14 mandatory (framework updates)

### Critical Path Identification

**Critical Path**: Tier 1 → Tier 2 → Tier 3 → Tier 4

- **Tier 1 is most critical**: All other projects directly or indirectly depend on these foundation libraries
- **ValidatorAbstractions** is the highest-impact project (used by 12 projects directly)
- **PaymentCardNumber.Common** and **PaymentCardNumber.Generator.Common** are critical for all payment card projects

### Circular Dependencies
**None identified** - The dependency graph is acyclic with clear hierarchical levels.

---

## Project-by-Project Plans

---

## **TIER 1: Foundation Libraries (Level 0)**

### Overview
- **Projects**: 6 foundation libraries with no internal dependencies
- **Current Framework**: `netstandard2.0`
- **Target Framework**: `net10.0`
- **Issues**: 0 mandatory
- **Complexity**: Low
- **Can Execute in Parallel**: Yes (no interdependencies)

---

### 1. DocumentNumber.ValidatorAbstractions

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: None (Level 0)
- **Dependents**: 12 projects (highest impact in solution)
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - .NET 10.0 SDK installed
   - Project file accessible (no locks)

2. **Framework Update**
   - Open `src\DocumentNumber.ValidatorAbstractions\DocumentNumber.ValidatorAbstractions.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None - netstandard2.0 APIs are fully compatible with net10.0

5. **Code Modifications**
   - None required - interface definitions remain unchanged

6. **Testing Strategy**
   - Build project to ensure no compilation errors
   - Verify no warnings generated
   - No unit tests in this project (abstractions only)

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 2. DocumentNumber.PaymentCardNumber.Generator.Common

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: None (Level 0)
- **Dependents**: 6 generator projects
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - .NET 10.0 SDK installed
   - Project file accessible

2. **Framework Update**
   - Open `src\DocumentNumber.PaymentCardNumber.Generator.Common\DocumentNumber.PaymentCardNumber.Generator.Common.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify clean build (no errors/warnings)

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 3. DocumentNumber.Portugal.BankAccountNumber.Generator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: None (Level 0)
- **Dependents**: 1 test project
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - .NET 10.0 SDK installed
   - Project file accessible

2. **Framework Update**
   - Open `src\DocumentNumber.Portugal.BankAccountNumber.Generator\DocumentNumber.Portugal.BankAccountNumber.Generator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 4. DocumentNumber.Portugal.CitizenCard.Generator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: None (Level 0)
- **Dependents**: 1 validator project
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - .NET 10.0 SDK installed
   - Project file accessible

2. **Framework Update**
   - Open `src\DocumentNumber.Portugal.CitizenCard.Generator\DocumentNumber.Portugal.CitizenCard.Generator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 5. DocumentNumber.Portugal.Nif.Generator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: None (Level 0)
- **Dependents**: 1 validator project
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - .NET 10.0 SDK installed
   - Project file accessible

2. **Framework Update**
   - Open `src\DocumentNumber.Portugal.Nif.Generator\DocumentNumber.Portugal.Nif.Generator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 6. DocumentNumber.Portugal.Niss.Generator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: None (Level 0)
- **Dependents**: 1 validator project
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - .NET 10.0 SDK installed
   - Project file accessible

2. **Framework Update**
   - Open `src\DocumentNumber.Portugal.Niss.Generator\DocumentNumber.Portugal.Niss.Generator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### Tier 1 Summary

**Batch Operation Approach:**
All 6 projects in Tier 1 can be updated in a single batch operation:

1. Update all 6 project files simultaneously (change `netstandard2.0` → `net10.0`)
2. Build all 6 projects together
3. Validate all 6 projects have clean builds

**Tier 1 Completion Criteria:**
- [ ] All 6 project files updated to `net10.0`
- [ ] All 6 projects build without errors
- [ ] All 6 projects build without warnings
- [ ] No dependency conflicts reported
- [ ] Ready to proceed to Tier 2

---

---

## **TIER 2: Common & Domain Libraries (Level 1)**

### Overview
- **Projects**: 6 common/domain libraries depending only on Tier 1
- **Current Framework**: `netstandard2.0`
- **Target Framework**: `net10.0`
- **Issues**: 0 mandatory
- **Complexity**: Low
- **Can Execute in Parallel**: Yes (no interdependencies within tier)
- **Prerequisites**: Tier 1 must be complete and validated

---

### 7. DocumentNumber.InternationalBankAccountNumber.Validator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: `ValidatorAbstractions` (Tier 1 - already upgraded)
- **Dependents**: 2 projects (1 validator, 1 test)
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - Tier 1 complete (ValidatorAbstractions on net10.0)
   - .NET 10.0 SDK installed

2. **Framework Update**
   - Open `src\DocumentNumber.InternationalBankAccountNumber.Validator\DocumentNumber.InternationalBankAccountNumber.Validator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None - dependency (ValidatorAbstractions) already on net10.0

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify dependency resolution (ValidatorAbstractions reference works)
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] ValidatorAbstractions reference resolved correctly
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 8. DocumentNumber.PaymentCardNumber.Common

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: `ValidatorAbstractions` (Tier 1 - already upgraded)
- **Dependents**: 11 projects (high impact)
- **Packages**: None
- **Risk Level**: Very Low (but high impact)

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - Tier 1 complete (ValidatorAbstractions on net10.0)
   - .NET 10.0 SDK installed

2. **Framework Update**
   - Open `src\DocumentNumber.PaymentCardNumber.Common\DocumentNumber.PaymentCardNumber.Common.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify ValidatorAbstractions reference
   - Verify clean build
   - **Note**: High impact project - 11 consumers will upgrade in Tier 3

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] ValidatorAbstractions reference resolved
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 9. DocumentNumber.PaymentCardNumber.MaestroUK.Generator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: `PaymentCardNumber.Generator.Common` (Tier 1 - already upgraded)
- **Dependents**: 1 test project
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - Tier 1 complete (Generator.Common on net10.0)
   - .NET 10.0 SDK installed

2. **Framework Update**
   - Open `src\DocumentNumber.PaymentCardNumber.MaestroUK.Generator\DocumentNumber.PaymentCardNumber.MaestroUK.Generator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify Generator.Common reference
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] Generator.Common reference resolved
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 10. DocumentNumber.Portugal.CitizenCard.Validator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: 
  - `ValidatorAbstractions` (Tier 1)
  - `Portugal.CitizenCard.Generator` (Tier 1)
- **Dependents**: 2 test projects
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - Tier 1 complete (both dependencies on net10.0)
   - .NET 10.0 SDK installed

2. **Framework Update**
   - Open `src\DocumentNumber.Portugal.CitizenCard.Validator\DocumentNumber.Portugal.CitizenCard.Validator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify both dependencies resolve correctly
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] All 2 dependency references resolved
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 11. DocumentNumber.Portugal.Nif.Validator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: 
  - `ValidatorAbstractions` (Tier 1)
  - `Portugal.Nif.Generator` (Tier 1)
- **Dependents**: 2 test projects
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - Tier 1 complete (both dependencies on net10.0)
   - .NET 10.0 SDK installed

2. **Framework Update**
   - Open `src\DocumentNumber.Portugal.Nif.Validator\DocumentNumber.Portugal.Nif.Validator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify both dependencies resolve correctly
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] All 2 dependency references resolved
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### 12. DocumentNumber.Portugal.Niss.Validator

**Current State:**
- **Framework**: `netstandard2.0`
- **Project Type**: ClassLibrary
- **Dependencies**: 
  - `Portugal.Niss.Generator` (Tier 1)
  - `ValidatorAbstractions` (Tier 1)
- **Dependents**: 2 test projects
- **Packages**: None
- **Risk Level**: Very Low

**Target State:**
- **Framework**: `net10.0`
- **All other aspects**: Unchanged

**Migration Steps:**

1. **Prerequisites**
   - Tier 1 complete (both dependencies on net10.0)
   - .NET 10.0 SDK installed

2. **Framework Update**
   - Open `src\DocumentNumber.Portugal.Niss.Validator\DocumentNumber.Portugal.Niss.Validator.csproj`
   - Change `<TargetFramework>netstandard2.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None

5. **Code Modifications**
   - None required

6. **Testing Strategy**
   - Build project
   - Verify both dependencies resolve correctly
   - Verify clean build

7. **Validation Checklist**
   - [ ] Project file updated to `net10.0`
   - [ ] Project builds successfully
   - [ ] All 2 dependency references resolved
   - [ ] No compilation errors
   - [ ] No build warnings
   - [ ] No dependency conflicts

---

### Tier 2 Summary

**Batch Operation Approach:**
All 6 projects in Tier 2 can be updated in a single batch operation:

1. Update all 6 project files simultaneously (change `netstandard2.0` → `net10.0`)
2. Build all 6 projects together
3. Validate all 6 projects have clean builds
4. Verify dependency references to Tier 1 projects resolve correctly

**Tier 2 Completion Criteria:**
- [ ] Tier 1 fully validated and complete
- [ ] All 6 project files updated to `net10.0`
- [ ] All 6 projects build without errors
- [ ] All 6 projects build without warnings
- [ ] All Tier 1 dependencies resolve correctly
- [ ] No dependency conflicts reported
- [ ] Ready to proceed to Tier 3

---

## **TIER 3: Specialized Libraries & Early Test Projects (Level 2)**

### Overview
- **Projects**: 20 total (11 source libraries + 8 test projects)
- **Current Framework**: 
  - Source: `netstandard2.0`
  - Tests: Multi-targeting `net6.0;net7.0;net462;net472;net48;net481`
- **Target Framework**: `net10.0` (all projects)
- **Issues**: 8 mandatory (all test projects - framework updates)
- **Complexity**: Low
- **Can Execute in Parallel**: Yes within tier
- **Prerequisites**: Tiers 1-2 must be complete and validated

---

### Source Projects (11 Projects)

All source projects in Tier 3 follow the same pattern: update `netstandard2.0` → `net10.0`, verify dependencies to Tiers 1-2 resolve correctly.

#### 13. DocumentNumber.PaymentCardNumber.AmericanExpress.Generator

**Current State:**
- **Framework**: `netstandard2.0`
- **Dependencies**: `PaymentCardNumber.Common` (Tier 2), `PaymentCardNumber.Generator.Common` (Tier 1)
- **Dependents**: 1 test project
- **Packages**: None
- **Risk Level**: Very Low

**Target State:** `net10.0`

**Migration Steps:**
1. Prerequisites: Tiers 1-2 complete
2. Update `src\DocumentNumber.PaymentCardNumber.AmericanExpress.Generator\DocumentNumber.PaymentCardNumber.AmericanExpress.Generator.csproj`: `netstandard2.0` → `net10.0`
3. Build and verify dependency resolution

**Validation Checklist:**
- [ ] Framework updated to `net10.0`
- [ ] Builds successfully
- [ ] Dependencies resolved (Common, Generator.Common)
- [ ] No errors/warnings

---

#### 14. DocumentNumber.PaymentCardNumber.AmericanExpress.Validator

**Current State:**
- **Framework**: `netstandard2.0`
- **Dependencies**: `PaymentCardNumber.Common` (Tier 2), `ValidatorAbstractions` (Tier 1)
- **Dependents**: 2 test projects
- **Packages**: None
- **Risk Level**: Very Low

**Target State:** `net10.0`

**Migration Steps:**
1. Prerequisites: Tiers 1-2 complete
2. Update `src\DocumentNumber.PaymentCardNumber.AmericanExpress.Validator\DocumentNumber.PaymentCardNumber.AmericanExpress.Validator.csproj`: `netstandard2.0` → `net10.0`
3. Build and verify

**Validation Checklist:**
- [ ] Framework updated to `net10.0`
- [ ] Builds successfully
- [ ] Dependencies resolved
- [ ] No errors/warnings

---

#### 15-23. Additional Payment Card Projects

The following 9 projects follow the identical pattern to #13-14 above:

**Generators (5 projects):**
- `DocumentNumber.PaymentCardNumber.Maestro.Generator`
- `DocumentNumber.PaymentCardNumber.Mastercard.Generator`
- `DocumentNumber.PaymentCardNumber.VISA.Generator`
- `DocumentNumber.PaymentCardNumber.VISAElectron.Generator`

Each depends on: `PaymentCardNumber.Common` (Tier 2) + `PaymentCardNumber.Generator.Common` (Tier 1)

**Validators (4 projects):**
- `DocumentNumber.PaymentCardNumber.Maestro.Validator`
- `DocumentNumber.PaymentCardNumber.MaestroUK.Validator`
- `DocumentNumber.PaymentCardNumber.Mastercard.Validator`
- `DocumentNumber.PaymentCardNumber.VISA.Validator`
- `DocumentNumber.PaymentCardNumber.VISAElectron.Validator`

Each depends on: `PaymentCardNumber.Common` (Tier 2) + `ValidatorAbstractions` (Tier 1)

**Migration for all 9**: Update project files `netstandard2.0` → `net10.0`, build, verify.

---

#### 24. DocumentNumber.Portugal.BankAccountNumber.Validator

**Current State:**
- **Framework**: `netstandard2.0`
- **Dependencies**: `ValidatorAbstractions` (Tier 1), `InternationalBankAccountNumber.Validator` (Tier 2)
- **Dependents**: 2 test projects
- **Packages**: None
- **Risk Level**: Very Low

**Target State:** `net10.0`

**Migration Steps:**
1. Prerequisites: Tiers 1-2 complete
2. Update `src\DocumentNumber.Portugal.BankAccountNumber.Validator\DocumentNumber.Portugal.BankAccountNumber.Validator.csproj`: `netstandard2.0` → `net10.0`
3. Build and verify dependencies

**Validation Checklist:**
- [ ] Framework updated to `net10.0`
- [ ] Builds successfully
- [ ] Dependencies resolved (ValidatorAbstractions, IBAN.Validator)
- [ ] No errors/warnings

---

### Test Projects (8 Projects in Tier 3)

All test projects in Tier 3 follow a common pattern: change from multi-targeting to single `net10.0` target.

**Current Multi-Target**: `<TargetFrameworks>net6.0;net7.0;net462;net472;net48;net481</TargetFrameworks>`  
**New Single Target**: `<TargetFramework>net10.0</TargetFramework>` (note: singular, not plural)

---

#### 25. DocumentNumber.PaymentCardNumber.Common.Tests

**Current State:**
- **Frameworks**: Multi-targeting `net6.0;net7.0;net462;net472;net48;net481`
- **Dependencies**: `PaymentCardNumber.Common` (Tier 2)
- **Issues**: 1 mandatory (framework update)
- **Risk Level**: Low

**Target State:** `net10.0` (single target)

**Migration Steps:**
1. Prerequisites: Tiers 1-2 complete, PaymentCardNumber.Common upgraded
2. Update `tests\DocumentNumber.PaymentCardNumber.Common.Tests\DocumentNumber.PaymentCardNumber.Common.Tests.csproj`
   - Change `<TargetFrameworks>net6.0;net7.0;net462;net472;net48;net481</TargetFrameworks>`
   - To `<TargetFramework>net10.0</TargetFramework>` (singular)
3. Build project
4. **Run tests** to validate PaymentCardNumber.Common functionality

**Testing Strategy:**
- Execute all unit tests in this project
- Verify tests pass with upgraded dependency
- Check for any runtime behavior changes

**Validation Checklist:**
- [ ] Framework updated to `net10.0` (singular, not multi-target)
- [ ] Project builds successfully
- [ ] PaymentCardNumber.Common reference resolved
- [ ] All unit tests execute
- [ ] All unit tests pass
- [ ] No errors/warnings

---

#### 26. DocumentNumber.InternationalBankAccountNumber.Validator.Tests

**Current State:**
- **Frameworks**: Multi-targeting `net6.0;net7.0;net462;net472;net48;net481`
- **Dependencies**: `InternationalBankAccountNumber.Validator` (Tier 2)
- **Issues**: 1 mandatory (framework update)
- **Risk Level**: Low

**Target State:** `net10.0`

**Migration Steps:**
1. Prerequisites: Tier 2 complete, IBAN.Validator upgraded
2. Update `tests\DocumentNumber.InternationalBankAccountNumber.Validator.Tests\DocumentNumber.InternationalBankAccountNumber.Validator.Tests.csproj`
   - Change `<TargetFrameworks>` to `<TargetFramework>net10.0</TargetFramework>`
3. Build and run tests

**Validation Checklist:**
- [ ] Framework updated to `net10.0`
- [ ] Builds successfully
- [ ] IBAN.Validator reference resolved
- [ ] Tests execute and pass
- [ ] No errors/warnings

---

#### 27-32. Additional Tier 3 Test Projects

The following 6 test projects follow identical pattern to #25-26:

1. `DocumentNumber.Portugal.CitizenCard.Generator.Tests` (depends on Portugal.CitizenCard.Validator - Tier 2)
2. `DocumentNumber.Portugal.Nif.Generator.Tests` (depends on Portugal.Nif.Validator - Tier 2)
3. `DocumentNumber.Portugal.Niss.Generator.Tests` (depends on Portugal.Niss.Validator - Tier 2)
4. `Portugal.CitizenCard.Validator.Tests` (depends on Portugal.CitizenCard.Validator - Tier 2)
5. `Portugal.Nif.Validator.Tests` (depends on Portugal.Nif.Validator - Tier 2)
6. `Portugal.Niss.Validator.Tests` (depends on Portugal.Niss.Validator - Tier 2)

**For each:**
- Change `<TargetFrameworks>net6.0;net7.0;net462;net472;net48;net481</TargetFrameworks>`
- To `<TargetFramework>net10.0</TargetFramework>`
- Build, run tests, verify passes

---

### Tier 3 Summary

**Batch Operation Approach:**

**Step 1: Update all 20 project files**
- 11 source projects: Change `netstandard2.0` → `net10.0`
- 8 test projects: Change `<TargetFrameworks>net6.0;...` → `<TargetFramework>net10.0</TargetFramework>`

**Step 2: Build all 20 projects**
- Verify all builds succeed
- Check dependency resolution to Tiers 1-2

**Step 3: Run tests (8 test projects)**
- Execute unit tests in all 8 test projects
- Validate functionality of upgraded libraries
- First validation checkpoint with actual test execution

**Tier 3 Completion Criteria:**
- [ ] Tiers 1-2 fully validated and complete
- [ ] All 20 project files updated to `net10.0`
- [ ] All 11 source projects build without errors/warnings
- [ ] All 8 test projects build without errors/warnings
- [ ] All dependencies to Tiers 1-2 resolve correctly
- [ ] All 8 test projects execute successfully
- [ ] All tests in 8 test projects pass
- [ ] No dependency conflicts reported
- [ ] Ready to proceed to Tier 4

---

## **TIER 4: Final Test Projects (Level 3)**

### Overview
- **Projects**: 14 test projects (top level of dependency hierarchy)
- **Current Framework**: Multi-targeting `net6.0;net7.0;net462;net472;net48;net481`
- **Target Framework**: `net10.0` (single target)
- **Issues**: 14 mandatory (all test projects - framework updates)
- **Complexity**: Low
- **Can Execute in Parallel**: Yes (no interdependencies)
- **Prerequisites**: Tiers 1-3 must be complete and validated
- **Purpose**: Comprehensive validation of entire upgraded solution

---

### Test Projects Pattern

All 14 projects in Tier 4 follow the same pattern:
- **Current**: `<TargetFrameworks>net6.0;net7.0;net462;net472;net48;net481</TargetFrameworks>`
- **Target**: `<TargetFramework>net10.0</TargetFramework>` (singular element)
- **Dependencies**: 2-3 projects from Tiers 2-3 (generator + validator typically)
- **Migration**: Update project file, build, run tests

---

#### 33. DocumentNumber.PaymentCardNumber.AmericaExpress.Validator.Tests

**Current State:**
- **Frameworks**: Multi-targeting (6 targets)
- **Dependencies**: `PaymentCardNumber.AmericanExpress.Validator` (Tier 3)
- **Issues**: 1 mandatory (framework update)
- **Risk Level**: Low

**Target State:** `net10.0`

**Migration Steps:**
1. Prerequisites: Tiers 1-3 complete
2. Update `tests\DocumentNumber.PaymentCardNumber.AmericaExpress.Validator.Tests\DocumentNumber.PaymentCardNumber.AmericaExpress.Validator.Tests.csproj`
   - Change `<TargetFrameworks>` to `<TargetFramework>net10.0</TargetFramework>`
3. Build project
4. Run all tests

**Validation Checklist:**
- [ ] Framework updated to `net10.0`
- [ ] Builds successfully
- [ ] AmericanExpress.Validator reference resolved
- [ ] Tests execute successfully
- [ ] All tests pass
- [ ] No errors/warnings

---

#### 34. DocumentNumber.PaymentCardNumber.AmericanExpress.Generator.Tests

**Current State:**
- **Frameworks**: Multi-targeting
- **Dependencies**: `PaymentCardNumber.AmericanExpress.Generator` (Tier 3), `PaymentCardNumber.AmericanExpress.Validator` (Tier 3)
- **Issues**: 1 mandatory
- **Risk Level**: Low

**Target State:** `net10.0`

**Migration Steps:**
1. Update `tests\DocumentNumber.PaymentCardNumber.AmericanExpress.Generator.Tests\DocumentNumber.PaymentCardNumber.AmericanExpress.Generator.Tests.csproj`
   - Change to `<TargetFramework>net10.0</TargetFramework>`
2. Build and run tests

**Validation Checklist:**
- [ ] Framework updated
- [ ] Builds successfully
- [ ] Both dependencies resolved (Generator + Validator)
- [ ] Tests pass
- [ ] No errors/warnings

---

#### 35-46. Remaining Tier 4 Test Projects

The following 12 test projects follow the identical pattern:

**Payment Card Tests (10 projects):**
1. `DocumentNumber.PaymentCardNumber.Maestro.Generator.Tests`
   - Dependencies: Maestro.Generator, Maestro.Validator (both Tier 3)
2. `DocumentNumber.PaymentCardNumber.Maestro.Validator.Tests`
   - Dependencies: Maestro.Validator (Tier 3)
3. `DocumentNumber.PaymentCardNumber.MaestroUK.Generator.Tests`
   - Dependencies: MaestroUK.Generator, MaestroUK.Validator (Tier 3)
4. `DocumentNumber.PaymentCardNumber.MaestroUK.Validator.Tests`
   - Dependencies: MaestroUK.Validator (Tier 3)
5. `DocumentNumber.PaymentCardNumber.Mastercard.Generator.Tests`
   - Dependencies: Mastercard.Generator, Mastercard.Validator (Tier 3)
6. `DocumentNumber.PaymentCardNumber.Mastercard.Validator.Tests`
   - Dependencies: Mastercard.Validator (Tier 3)
7. `DocumentNumber.PaymentCardNumber.VISA.Generator.Tests`
   - Dependencies: VISA.Generator, VISA.Validator (Tier 3)
8. `DocumentNumber.PaymentCardNumber.VISA.Validator.Tests`
   - Dependencies: VISA.Validator (Tier 3)
9. `DocumentNumber.PaymentCardNumber.VISAElectron.Generator.Tests`
   - Dependencies: VISAElectron.Generator, VISAElectron.Validator (Tier 3)
10. `DocumentNumber.PaymentCardNumber.VISAElectron.Validator.Tests`
    - Dependencies: VISAElectron.Validator (Tier 3)

**Portugal Banking Tests (2 projects):**
11. `DocumentNumber.Portugal.BankAccountNumber.Generator.Tests`
    - Dependencies: Portugal.BankAccountNumber.Generator (Tier 1), Portugal.BankAccountNumber.Validator (Tier 3)
12. `DocumentNumber.Portugal.BankAccountNumber.Validator.Tests`
    - Dependencies: Portugal.BankAccountNumber.Validator (Tier 3)

**For each project:**
- Change `<TargetFrameworks>net6.0;net7.0;net462;net472;net48;net481</TargetFrameworks>`
- To `<TargetFramework>net10.0</TargetFramework>`
- Build project
- Run all tests
- Verify all tests pass

---

### Tier 4 Summary

**Batch Operation Approach:**

**Step 1: Update all 14 project files**
- Change all 14 test projects from multi-targeting to `net10.0`
- Ensure `<TargetFramework>` (singular) not `<TargetFrameworks>` (plural)

**Step 2: Build all 14 projects**
- Verify all builds succeed
- Check dependency resolution to Tiers 2-3

**Step 3: Run comprehensive test suite**
- Execute all 14 test projects (plus 8 from Tier 3 = 22 total)
- Validate entire solution functionality
- Final comprehensive validation checkpoint

**Tier 4 Completion Criteria:**
- [ ] Tiers 1-3 fully validated and complete
- [ ] All 14 project files updated to `net10.0`
- [ ] All 14 test projects build without errors
- [ ] All 14 test projects build without warnings
- [ ] All dependencies to previous tiers resolve correctly
- [ ] All 14 test projects execute successfully
- [ ] All tests in all 14 projects pass
- [ ] No dependency conflicts reported
- [ ] **Full solution validation**: All 46 projects on net10.0
- [ ] **Complete test coverage**: All 22 test projects passing

---

## **UPGRADE COMPLETE**

Once Tier 4 completion criteria are met:
- All 46 projects successfully upgraded to .NET 10.0
- All 24 source libraries on net10.0
- All 22 test projects on net10.0 (single target, not multi-targeting)
- Full test suite validates upgraded solution
- No compilation errors, warnings, or dependency conflicts
- Solution ready for deployment

---

## Risk Management

### High-Level Risk Assessment

**Overall Risk Level: LOW**

This upgrade presents minimal risk due to:
- Simple framework identifier changes only (no code modifications needed)
- No external package dependencies requiring updates
- No breaking API changes between netstandard2.0 and net10.0
- No security vulnerabilities identified
- Clear dependency structure with no circular references

### Risk Factors by Tier

| Tier | Risk Level | Description | Mitigation |
|------|-----------|-------------|------------|
| **Tier 1** | Very Low | Foundation libraries with no dependencies. Changes limited to framework target. | Validate builds immediately after update. Roll back single tier if issues arise. |
| **Tier 2** | Very Low | Common libraries depending only on Tier 1 (already upgraded). | Ensure Tier 1 validation complete before proceeding. |
| **Tier 3** | Low | Specialized libraries + early test projects. Test projects validate library upgrades. | Run test projects in this tier to catch any unexpected issues. |
| **Tier 4** | Low | Final test projects. Comprehensive validation of entire solution. | Full test suite execution provides complete validation. |

### Bottom-Up Strategy Risk Considerations

**Advantages Applied:**
- **Stable Foundation**: Each tier upgrades after its dependencies are validated
- **No Multi-Targeting**: Eliminates version mismatch complexities
- **Isolated Changes**: Issues confined to current tier, not rippling across solution
- **Progressive Validation**: Each tier's tests validate cumulative progress

**Potential Challenges:**
- **Timeline**: Sequential tier progression takes longer than all-at-once
  - *Mitigation*: Parallel execution within tiers, batch updates reduce overhead
- **Coordination**: Must ensure tier completion before advancing
  - *Mitigation*: Clear completion criteria per tier, automated validation

### Security Vulnerabilities
**None identified** - Assessment found no security issues with current dependencies or frameworks.

### Contingency Plans

#### If Build Fails in Any Tier
1. **Identify failing project(s)** within the tier
2. **Isolate the issue**: Check project file syntax, dependency references
3. **Rollback tier**: Revert all project files in the tier to previous framework
4. **Investigate**: Examine build errors for root cause
5. **Fix and retry**: Apply fix, re-upgrade tier
6. **Escalate if persistent**: Consult documentation or community if issue unclear

#### If Tests Fail After Tier 3 or 4 Upgrade
1. **Identify failing tests**: Determine which test projects/methods fail
2. **Analyze failures**: Check if runtime behavior changed (unlikely for framework-only upgrade)
3. **Verify dependencies**: Ensure all referenced projects correctly upgraded
4. **Check test infrastructure**: Verify test framework compatibility with net10.0
5. **Rollback tier if critical**: If failures are widespread and unexplained
6. **Incremental fix**: Address test failures individually if isolated

#### If Unexpected Breaking Changes Surface
1. **Document the breaking change**: API, behavior, or configuration change
2. **Assess impact**: How many projects affected
3. **Research mitigation**: Check .NET migration guides for known issues
4. **Apply code fixes**: Update affected code patterns
5. **Re-validate tier**: Ensure fixes resolve all issues before proceeding

#### If Tier Takes Longer Than Expected
- Not applicable - this upgrade has no time estimates (complexity is relative only)

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

This upgrade employs a **progressive validation strategy** aligned with the Bottom-Up tier structure. Each tier is validated before proceeding to the next, ensuring cumulative stability.

---

### Tier-by-Tier Testing Requirements

#### **Tier 1 Validation (Foundation Libraries)**

**Scope**: 6 foundation libraries with no dependencies

**Validation Steps:**
1. **Build Validation** (primary checkpoint)
   - All 6 projects build without errors
   - All 6 projects build without warnings
   - No dependency conflicts (no dependencies to validate)

2. **Project File Validation**
   - Verify `<TargetFramework>net10.0</TargetFramework>` in all 6 .csproj files
   - Ensure no syntax errors in project files

3. **Dependency Graph Validation**
   - No validation needed (no dependencies)

**No unit tests** in this tier - these are foundation libraries/abstractions

**Success Criteria:**
- [ ] All 6 projects build successfully
- [ ] Zero compilation errors
- [ ] Zero build warnings
- [ ] Clean build output for entire tier

---

#### **Tier 2 Validation (Common Libraries)**

**Scope**: 6 common/domain libraries depending on Tier 1

**Validation Steps:**
1. **Build Validation**
   - All 6 projects build without errors
   - All 6 projects build without warnings

2. **Dependency Resolution Validation**
   - All 6 projects correctly reference Tier 1 projects
   - No version conflicts with Tier 1 dependencies
   - Dependency graph remains consistent

3. **Integration with Tier 1**
   - Tier 2 projects can consume Tier 1 APIs
   - No breaking changes from Tier 1 upgrade

**No unit tests** in this tier (validators without tests yet)

**Success Criteria:**
- [ ] All 6 projects build successfully
- [ ] All Tier 1 dependencies resolved correctly
- [ ] Zero compilation errors
- [ ] Zero build warnings
- [ ] No dependency version conflicts

---

#### **Tier 3 Validation (Specialized Libraries & Early Tests)**

**Scope**: 11 source libraries + 8 test projects

**Validation Steps:**

1. **Build Validation (Source Projects)**
   - All 11 source projects build without errors
   - All 11 source projects build without warnings

2. **Build Validation (Test Projects)**
   - All 8 test projects build without errors
   - All 8 test projects build without warnings
   - Test framework compatible with net10.0

3. **Dependency Resolution Validation**
   - All 20 projects correctly reference Tiers 1-2
   - No dependency conflicts across 3 tiers

4. **Unit Testing (First Test Execution)**
   - **Execute** all 8 test projects:
     - `DocumentNumber.PaymentCardNumber.Common.Tests`
     - `DocumentNumber.InternationalBankAccountNumber.Validator.Tests`
     - `DocumentNumber.Portugal.CitizenCard.Generator.Tests`
     - `DocumentNumber.Portugal.Nif.Generator.Tests`
     - `DocumentNumber.Portugal.Niss.Generator.Tests`
     - `Portugal.CitizenCard.Validator.Tests`
     - `Portugal.Nif.Validator.Tests`
     - `Portugal.Niss.Validator.Tests`

5. **Test Analysis**
   - All tests pass (no failures)
   - No skipped tests
   - No runtime errors
   - No unexpected warnings in test output

**Success Criteria:**
- [ ] All 20 projects build successfully
- [ ] All dependencies to Tiers 1-2 resolved
- [ ] Zero compilation errors
- [ ] Zero build warnings
- [ ] All 8 test projects execute
- [ ] All tests in 8 projects pass (0 failures)
- [ ] No runtime errors during test execution
- [ ] **First validation checkpoint**: Libraries validated by their tests

---

#### **Tier 4 Validation (Final Test Projects)**

**Scope**: 14 top-level test projects

**Validation Steps:**

1. **Build Validation**
   - All 14 test projects build without errors
   - All 14 test projects build without warnings

2. **Dependency Resolution Validation**
   - All 14 projects correctly reference Tiers 2-3
   - No dependency conflicts across entire solution (46 projects)

3. **Comprehensive Unit Testing**
   - **Execute** all 14 test projects in Tier 4
   - **Re-execute** all 8 test projects from Tier 3 (full regression)
   - **Total**: 22 test projects executed

4. **Full Solution Validation**
   - Build entire solution (all 46 projects)
   - No errors anywhere in solution
   - No warnings anywhere in solution
   - Dependency graph integrity validated

5. **Test Analysis**
   - All 22 test projects pass (cumulative)
   - No test failures across entire solution
   - No skipped tests
   - No runtime errors

**Success Criteria:**
- [ ] All 14 Tier 4 projects build successfully
- [ ] All 46 solution projects build successfully
- [ ] All dependencies resolved correctly
- [ ] Zero compilation errors (solution-wide)
- [ ] Zero build warnings (solution-wide)
- [ ] All 22 test projects execute successfully
- [ ] All tests pass (0 failures across entire test suite)
- [ ] No runtime errors
- [ ] **Final validation checkpoint**: Entire solution validated

---

### Smoke Tests (Quick Validation After Each Project)

Not applicable for this upgrade - batch operations per tier eliminate need for per-project smoke tests.

**Alternative**: After each tier, perform tier-level smoke test:
1. Build all projects in tier
2. Check for errors/warnings
3. For test tiers: Run quick test sample to verify infrastructure

---

### Comprehensive Validation (Before Phase Completion)

**Before marking any tier complete:**

| Validation Check | Tier 1 | Tier 2 | Tier 3 | Tier 4 |
|------------------|--------|--------|--------|--------|
| All projects build successfully | ✓ | ✓ | ✓ | ✓ |
| Zero compilation errors | ✓ | ✓ | ✓ | ✓ |
| Zero build warnings | ✓ | ✓ | ✓ | ✓ |
| Dependencies resolve correctly | N/A | ✓ | ✓ | ✓ |
| No version conflicts | N/A | ✓ | ✓ | ✓ |
| Unit tests execute | N/A | N/A | ✓ | ✓ |
| All tests pass | N/A | N/A | ✓ | ✓ |
| No runtime errors | N/A | N/A | ✓ | ✓ |
| Full solution builds | - | - | - | ✓ |

---

### Test Execution Strategy

#### Test Framework Compatibility
- Verify test frameworks (xUnit/NUnit/MSTest) compatible with net10.0
- No test framework package updates expected (compatible out-of-box)

#### Test Execution Order
1. **Tier 3**: Execute 8 test projects (early validation)
2. **Tier 4**: Execute 14 test projects + re-run 8 from Tier 3 (22 total)

#### Test Failure Handling
- **If Tier 3 tests fail**: 
  - Identify failing tests
  - Check if library behavior changed unexpectedly
  - Verify dependencies upgraded correctly
  - Rollback tier if failures widespread

- **If Tier 4 tests fail**: 
  - Same analysis as Tier 3
  - Check for integration issues between tiers
  - Verify final dependency resolution

#### Performance Validation
- Not explicitly required (framework upgrade shouldn't impact performance)
- Monitor test execution time as sanity check
- If tests significantly slower, investigate (unlikely)

---

### Validation Tools

**Build Validation:**
- `dotnet build` for each tier
- Check exit codes (0 = success)
- Parse build output for errors/warnings

**Test Execution:**
- `dotnet test` for each test project
- Capture test results (passed/failed/skipped counts)
- Log any test failures with details

**Dependency Validation:**
- Visual Studio Solution Explorer (check for warning icons)
- `dotnet list package` to verify package graph
- Build output for dependency warnings

---

### Rollback Triggers

**Trigger immediate rollback if:**
- Any tier has compilation errors that cannot be quickly resolved
- Test failures exceed 10% of tests in a tier
- Dependency conflicts cannot be resolved
- Unexpected runtime errors in test execution
- Critical functionality broken (determined by specific test failures)

**Rollback Procedure:**
- Revert all project files in failed tier to previous framework
- Re-build tier to confirm stability
- Investigate root cause before retrying

---

## Complexity & Effort Assessment

### Relative Complexity Ratings

Complexity ratings are relative to each other within this solution (Low/Medium/High). No time estimates are provided.

#### Per-Tier Complexity

| Tier | Projects | Complexity | Dependencies | Risk | Rationale |
|------|----------|-----------|--------------|------|-----------|
| **Tier 1** | 6 | **Low** | None (Level 0) | Very Low | Foundation libraries with zero dependencies. Simple framework target change. Highest impact but lowest complexity. |
| **Tier 2** | 6 | **Low** | Tier 1 only | Very Low | Common libraries depending on stable Tier 1. Same simple framework change. |
| **Tier 3** | 20 (11 src + 8 tests) | **Low** | Tiers 1-2 | Low | Specialized libraries + early tests. Framework changes only. First tier with test execution. |
| **Tier 4** | 14 (tests only) | **Low** | Tiers 1-3 | Low | Test projects only. Framework changes. Comprehensive validation tier. |

#### Per-Project Complexity (Tier 1 - Foundation)

| Project | Complexity | Dependencies | Used By | Notes |
|---------|-----------|--------------|---------|-------|
| `ValidatorAbstractions` | **Low** | 0 | 12 projects | Highest impact (most consumers) but simple change |
| `PaymentCardNumber.Generator.Common` | **Low** | 0 | 6 projects | Base generator utilities |
| `Portugal.BankAccountNumber.Generator` | **Low** | 0 | 1 project | Domain-specific generator |
| `Portugal.CitizenCard.Generator` | **Low** | 0 | 1 project | Domain-specific generator |
| `Portugal.Nif.Generator` | **Low** | 0 | 1 project | Domain-specific generator |
| `Portugal.Niss.Generator` | **Low** | 0 | 1 project | Domain-specific generator |

**Tier 1 Summary**: All projects rated Low complexity - simple framework target changes with no code modifications.

#### Per-Project Complexity (Tier 2 - Common Libraries)

| Project | Complexity | Dependencies | Used By | Notes |
|---------|-----------|--------------|---------|-------|
| `InternationalBankAccountNumber.Validator` | **Low** | 1 (ValidatorAbstractions) | 2 projects | Simple validator implementation |
| `PaymentCardNumber.Common` | **Low** | 1 (ValidatorAbstractions) | 11 projects | High impact (many consumers) |
| `PaymentCardNumber.MaestroUK.Generator` | **Low** | 1 (Generator.Common) | 1 project | Specialized generator |
| `Portugal.CitizenCard.Validator` | **Low** | 2 (ValidatorAbstractions, Generator) | 2 projects | Domain validator |
| `Portugal.Nif.Validator` | **Low** | 2 (ValidatorAbstractions, Generator) | 2 projects | Domain validator |
| `Portugal.Niss.Validator` | **Low** | 2 (ValidatorAbstractions, Generator) | 2 projects | Domain validator |

**Tier 2 Summary**: All projects rated Low complexity - dependencies already upgraded in Tier 1.

#### Per-Project Complexity (Tier 3 - Specialized + Early Tests)

**Source Projects (11):**

| Project | Complexity | Dependencies | Notes |
|---------|-----------|--------------|-------|
| Payment card generators (5) | **Low** | 2 each (Common + Generator.Common) | Standardized pattern |
| Payment card validators (5) | **Low** | 2 each (Common + ValidatorAbstractions) | Standardized pattern |
| `Portugal.BankAccountNumber.Validator` | **Low** | 2 (ValidatorAbstractions + IBAN.Validator) | Simple validator |

**Test Projects (8):**

| Project | Complexity | Dependencies | Notes |
|---------|-----------|--------------|-------|
| All 8 test projects | **Low** | 1-2 each (their target libraries) | Framework change + first test execution tier |

**Tier 3 Summary**: All projects rated Low complexity. Source projects follow consistent patterns. Test projects provide first validation checkpoint.

#### Per-Project Complexity (Tier 4 - Final Tests)

| Project | Complexity | Dependencies | Notes |
|---------|-----------|--------------|-------|
| All 14 test projects | **Low** | 2-3 each (generators + validators) | Framework change only. Comprehensive validation. |

**Tier 4 Summary**: All projects rated Low complexity - standardized test projects validating entire solution.

### Phase Complexity Assessment with Dependency Ordering

| Phase | Tier | Projects | Overall Complexity | Dependency Order | Notes |
|-------|------|----------|-------------------|------------------|-------|
| **Phase 1** | Tier 1 | 6 | **Low** | No dependencies - can proceed immediately | Foundation layer - must complete first |
| **Phase 2** | Tier 2 | 6 | **Low** | Depends on Tier 1 completion | Common libraries - second priority |
| **Phase 3** | Tier 3 | 20 | **Low** | Depends on Tiers 1-2 completion | Specialized logic + early validation |
| **Phase 4** | Tier 4 | 14 | **Low** | Depends on Tiers 1-3 completion | Final validation layer |

**Key Insight**: Despite 46 projects, all phases rated Low complexity due to:
- Uniform change type (framework target only)
- No code modifications required
- No package updates needed
- Clear dependency structure
- Batch operations within tiers

### Resource Requirements

**Skill Levels:**
- **Foundation work (Tiers 1-2)**: Entry-level .NET knowledge sufficient
  - Requires: Understanding of project file structure, basic .NET build process
- **Validation work (Tiers 3-4)**: Intermediate .NET knowledge
  - Requires: Test execution, interpreting test results, basic debugging

**Parallel Capacity:**
- Within each tier: Projects can be updated simultaneously (no interdependencies)
- Between tiers: Sequential progression required
- Recommended: Batch all projects in a tier into single operation for efficiency

**Testing Infrastructure:**
- Requires: .NET 10.0 SDK installed
- Requires: Test runner compatible with net10.0 (xUnit, NUnit, MSTest)
- No special test infrastructure changes needed

### Effort Distribution

**By Phase** (relative effort):
- Phase 1 (Tier 1): 15% - 6 projects, foundational importance
- Phase 2 (Tier 2): 15% - 6 projects, common libraries
- Phase 3 (Tier 3): 40% - 20 projects, includes first test execution
- Phase 4 (Tier 4): 30% - 14 projects, comprehensive testing

**By Activity Type** (relative effort):
- Project file updates: 30% - Mechanical changes across 46 files
- Build validation: 20% - Ensuring clean builds per tier
- Test execution: 40% - Running 22 test projects, analyzing results
- Documentation: 10% - Updating completion status, capturing issues

---

## Source Control Strategy

### Branching Strategy

**Branch Structure:**
- **Source Branch**: `several-us-marco` (starting point)
- **Upgrade Branch**: `upgrade-to-NET10` (all upgrade work occurs here)
- **Target Branch**: `main` or `several-us-marco` (final merge destination after validation)

**Branch Policy:**
- All upgrade changes committed to `upgrade-to-NET10`
- No commits to source branch during upgrade
- Keep upgrade branch up-to-date with source branch if needed (merge source → upgrade)
- Final PR from `upgrade-to-NET10` → source/main after complete validation

---

### Commit Strategy

#### Commit Frequency

**Per-Tier Commits** (Recommended):
- Commit after each tier completion and validation
- Results in 4 commits total (1 per tier)
- Clean commit history aligned with tier structure
- Easy to review and rollback if needed

**Commit Structure:**
```
Commit 1: "Upgrade Tier 1 (Foundation) to .NET 10.0"
Commit 2: "Upgrade Tier 2 (Common Libraries) to .NET 10.0"
Commit 3: "Upgrade Tier 3 (Specialized Libraries & Early Tests) to .NET 10.0"
Commit 4: "Upgrade Tier 4 (Final Test Projects) to .NET 10.0"
```

#### Commit Message Format

**Template:**
```
Upgrade [Tier Name] to .NET 10.0

- Updated [N] projects from [old framework] to net10.0
- Projects: [list key project names or count]
- Validation: [build status, test results]
- Issues: [any issues encountered and resolved]

Tier Completion: [checklist status]
```

**Example (Tier 1):**
```
Upgrade Tier 1 (Foundation Libraries) to .NET 10.0

- Updated 6 foundation projects from netstandard2.0 to net10.0
- Projects: ValidatorAbstractions, PaymentCardNumber.Generator.Common,
  Portugal.BankAccountNumber.Generator, Portugal.CitizenCard.Generator,
  Portugal.Nif.Generator, Portugal.Niss.Generator
- Validation: All projects build successfully, zero errors/warnings
- Issues: None

Tier 1 Completion Criteria: ✓ All met
```

#### Checkpoint Commits

**After Each Tier:**
1. Complete all tier updates
2. Validate tier completion criteria
3. Commit changes with detailed message
4. Tag commit (optional): `tier-1-complete`, `tier-2-complete`, etc.

**Why Per-Tier Commits:**
- Each tier is a logical unit of work
- Tier validation provides natural commit boundary
- Easy rollback to last good tier
- Clear progression in git history
- Aligns with Bottom-Up Strategy principles

---

### Review and Merge Process

#### Pull Request Requirements

**Single PR After Complete Upgrade:**
- Create PR from `upgrade-to-NET10` → `several-us-marco` (or main)
- PR Title: "Upgrade solution to .NET 10.0 (46 projects)"
- PR Description: Include summary, tier breakdown, validation results

**PR Description Template:**
```markdown
## .NET 10.0 Upgrade - Complete Solution

### Summary
Upgraded all 46 projects from netstandard2.0/net6.0 to .NET 10.0 following Bottom-Up tier-by-tier strategy.

### Changes by Tier

#### Tier 1: Foundation Libraries (6 projects)
- Updated: ValidatorAbstractions, PaymentCardNumber.Generator.Common, [4 Portugal generators]
- Framework: netstandard2.0 → net10.0
- Validation: ✓ All builds, zero errors/warnings

#### Tier 2: Common Libraries (6 projects)
- Updated: [list projects]
- Framework: netstandard2.0 → net10.0
- Validation: ✓ Builds, dependencies resolved

#### Tier 3: Specialized & Early Tests (20 projects)
- Updated: 11 source + 8 test projects
- Framework: netstandard2.0/multi-target → net10.0
- Validation: ✓ Builds, 8 test projects pass

#### Tier 4: Final Tests (14 projects)
- Updated: 14 test projects
- Framework: multi-target → net10.0
- Validation: ✓ Builds, all 22 test projects pass

### Validation Results
- ✓ All 46 projects build successfully
- ✓ Zero compilation errors
- ✓ Zero build warnings
- ✓ All 22 test projects pass (100% pass rate)
- ✓ No dependency conflicts
- ✓ Full solution validated on .NET 10.0

### Breaking Changes
None - straightforward framework upgrade with no code modifications.

### Rollback Plan
Revert commits tier-by-tier if issues discovered post-merge.
```

#### PR Checklist

**Before Creating PR:**
- [ ] All 4 tiers complete and committed
- [ ] All tier completion criteria met
- [ ] Full solution builds successfully
- [ ] All 22 test projects pass
- [ ] No uncommitted changes
- [ ] Branch up-to-date with source (if needed)

**During PR Review:**
- [ ] Reviewer validates commit structure (4 logical tier commits)
- [ ] Reviewer checks project file changes (framework identifiers only)
- [ ] Reviewer verifies test results documented
- [ ] Reviewer confirms no code changes (only project files)

#### Merge Criteria

**Approve and merge when:**
- [ ] All CI/CD pipelines pass (if configured)
- [ ] Code review approved
- [ ] All validation documented
- [ ] No merge conflicts
- [ ] Test suite passes on target branch after merge

**Merge Strategy:**
- **Recommended**: Squash merge (combines 4 tier commits into 1 for cleaner main branch)
- **Alternative**: Merge commit (preserves tier commit history)
- **Not recommended**: Rebase (loses tier commit structure)

---

### Rollback Strategy

#### Immediate Rollback (During Upgrade)

**If tier fails validation:**
1. Do not commit failed tier
2. Revert project file changes in failed tier
3. Investigate and fix issues
4. Retry tier upgrade
5. Commit only after tier validation passes

#### Post-Commit Rollback (Tier Merged but Issues Found)

**Rollback single tier:**
```bash
# Revert last commit (if Tier 4 needs rollback)
git revert HEAD

# Or revert specific tier commit
git revert <tier-commit-hash>
```

**Rollback multiple tiers:**
```bash
# Revert to end of Tier 2 (example)
git revert <tier-4-commit>..<tier-3-commit>
```

#### Post-PR-Merge Rollback (Issues in Production)

**Full rollback:**
```bash
# Revert entire upgrade PR
git revert -m 1 <merge-commit-hash>
```

**Partial rollback:**
- Cherry-pick specific tier commits to new branch
- Create new PR with partial upgrade

---

### Git Workflow Summary

**Preparation:**
1. Starting branch: `several-us-marco`
2. Create upgrade branch: `upgrade-to-NET10`
3. Switch to upgrade branch

**During Upgrade:**
1. Complete Tier 1 → validate → commit
2. Complete Tier 2 → validate → commit
3. Complete Tier 3 → validate → commit
4. Complete Tier 4 → validate → commit

**After Upgrade:**
1. Final validation (full solution)
2. Create PR: `upgrade-to-NET10` → `several-us-marco`
3. Code review
4. Merge PR
5. Validate on target branch
6. Tag release (optional): `v-net10.0` or similar

**Commit History (ideal):**
```
* Upgrade Tier 4 (Final Test Projects) to .NET 10.0
* Upgrade Tier 3 (Specialized Libraries & Early Tests) to .NET 10.0
* Upgrade Tier 2 (Common Libraries) to .NET 10.0
* Upgrade Tier 1 (Foundation Libraries) to .NET 10.0
* [source branch commits...]
```

---

### Bottom-Up Strategy Source Control Considerations

**Alignment with Strategy:**
- Tier-based commits match Bottom-Up progression (Level 0 → Level 3)
- Each commit represents a stable, validated foundation
- Clear dependency progression in git history
- Easy to identify which tier introduced issues (if any)

**Benefits:**
- Clean, reviewable history
- Natural rollback points at tier boundaries
- Documentation of dependency-first approach
- Audit trail of progressive validation

---

## Success Criteria

The .NET 10.0 upgrade is considered **complete and successful** when all criteria below are met.

---

### Technical Criteria

#### 1. All Projects Migrated
- [ ] **All 24 source library projects** upgraded to `net10.0`
  - Changed from `netstandard2.0` → `net10.0`
  - Includes all 6 Tier 1, 6 Tier 2, 11 Tier 3 source projects

- [ ] **All 22 test projects** upgraded to `net10.0` (single target)
  - Changed from multi-targeting (`net6.0;net7.0;net462;net472;net48;net481`) → `net10.0`
  - Includes all 8 Tier 3 + 14 Tier 4 test projects

- [ ] **Total**: All 46 projects successfully migrated

#### 2. Package Updates Applied
- [ ] No package updates required (verified by assessment)
- [ ] All projects reference correct framework libraries for net10.0

#### 3. Builds Succeed
- [ ] **Tier 1**: All 6 projects build successfully
- [ ] **Tier 2**: All 6 projects build successfully
- [ ] **Tier 3**: All 20 projects (11 source + 8 tests) build successfully
- [ ] **Tier 4**: All 14 test projects build successfully
- [ ] **Full solution**: All 46 projects build together without errors

#### 4. Tests Pass
- [ ] **Tier 3 tests**: All 8 test projects execute with 100% pass rate
- [ ] **Tier 4 tests**: All 14 test projects execute with 100% pass rate
- [ ] **Full test suite**: All 22 test projects pass (cumulative 100% pass rate)
- [ ] **Zero test failures** across entire solution
- [ ] **Zero skipped tests** (unless intentional)

#### 5. No Compilation Errors or Warnings
- [ ] Zero compilation errors in all 46 projects
- [ ] Zero build warnings in all 46 projects
- [ ] Clean build output for entire solution

#### 6. No Package Dependency Conflicts
- [ ] No dependency version conflicts reported
- [ ] All project references resolve correctly
- [ ] Dependency graph integrity maintained (no circular dependencies)
- [ ] All Tier 1 dependencies referenced correctly by Tiers 2-4
- [ ] All Tier 2 dependencies referenced correctly by Tiers 3-4
- [ ] All Tier 3 dependencies referenced correctly by Tier 4

#### 7. No Security Vulnerabilities
- [ ] No security vulnerabilities introduced during upgrade
- [ ] No new vulnerable package versions
- [ ] Assessment confirmed no existing vulnerabilities

---

### Quality Criteria

#### 1. Code Quality Maintained
- [ ] No code modifications required (framework-only upgrade confirmed)
- [ ] All existing functionality preserved (validated by tests)
- [ ] No degradation in code structure or patterns
- [ ] Project file changes only (no .cs, .vb, or other code files modified)

#### 2. Test Coverage Maintained
- [ ] All 22 existing test projects still functional
- [ ] No tests disabled or removed
- [ ] Test coverage level unchanged (framework upgrade doesn't affect coverage)
- [ ] All test patterns still work on net10.0

#### 3. Documentation Updated
- [ ] Assessment documented: `assessment.md`
- [ ] Plan documented: `plan.md`
- [ ] Execution tracked: `tasks.md` (created during execution phase)
- [ ] Commit messages document tier progression
- [ ] PR description summarizes complete upgrade

---

### Process Criteria

#### 1. Bottom-Up Strategy Followed
- [ ] **Tier 1 (Foundation)** upgraded first
- [ ] **Tier 2 (Common)** upgraded after Tier 1 validated
- [ ] **Tier 3 (Specialized)** upgraded after Tier 2 validated
- [ ] **Tier 4 (Final Tests)** upgraded after Tier 3 validated
- [ ] No tier started before previous tier completed
- [ ] Dependency-first ordering respected throughout

#### 2. Bottom-Up Strategy Principles Applied
- [ ] Projects upgraded in dependency order (leaves → roots)
- [ ] No multi-targeting used (dependencies always same-or-newer framework)
- [ ] Each tier validated before proceeding to next
- [ ] Issues isolated to current tier (no cascading failures)
- [ ] Stable foundation built progressively (Tier N stable before Tier N+1)

#### 3. Source Control Strategy Followed
- [ ] All work performed on `upgrade-to-NET10` branch
- [ ] Tier-based commits created (4 commits: 1 per tier)
- [ ] Commit messages follow defined format
- [ ] PR created from upgrade branch to source branch
- [ ] PR description complete with tier breakdown and validation results
- [ ] Code review completed
- [ ] PR merged successfully

#### 4. Validation Strategy Executed
- [ ] **Tier 1 validation**: Build validation complete
- [ ] **Tier 2 validation**: Build + dependency validation complete
- [ ] **Tier 3 validation**: Build + dependency + test validation complete (8 test projects)
- [ ] **Tier 4 validation**: Full solution validation complete (22 test projects)
- [ ] No tier marked complete without meeting completion criteria
- [ ] All validation checkpoints documented

---

### Deployment Criteria

#### 1. Solution Ready for Use
- [ ] Full solution builds on any machine with .NET 10.0 SDK
- [ ] All tests pass in CI/CD pipeline (if configured)
- [ ] No environment-specific issues
- [ ] Documentation reflects .NET 10.0 target

#### 2. Migration Path Documented
- [ ] Assessment provides baseline understanding
- [ ] Plan provides detailed migration roadmap
- [ ] Execution history (tasks.md) documents actual process
- [ ] Any deviations from plan documented
- [ ] Lessons learned captured (if any issues encountered)

#### 3. Rollback Available
- [ ] Each tier commit provides rollback point
- [ ] Rollback procedures documented
- [ ] Source branch unchanged (safe fallback)

---

### Final Checklist

**Before declaring upgrade complete:**

**Technical Validation:**
- [ ] 46 projects on net10.0
- [ ] 46 projects build successfully
- [ ] 22 test projects pass (100%)
- [ ] Zero errors, warnings, conflicts

**Quality Validation:**
- [ ] Code unchanged (project files only)
- [ ] Tests unchanged (all still functional)
- [ ] Documentation complete

**Process Validation:**
- [ ] Bottom-Up Strategy followed
- [ ] Tier-by-tier progression completed
- [ ] All 4 tier completion criteria met
- [ ] Source control strategy followed
- [ ] PR merged successfully

**Deployment Readiness:**
- [ ] Solution usable with .NET 10.0 SDK
- [ ] No blockers for downstream consumers
- [ ] Rollback plan documented

---

### Upgrade Success Confirmation

When all criteria above are met:

✅ **The .NET 10.0 upgrade is COMPLETE**

**Final State:**
- **All 46 projects** successfully upgraded from netstandard2.0/net6.0 → net10.0
- **All 22 test projects** passing with 100% success rate
- **Zero issues** (errors, warnings, test failures, dependency conflicts)
- **Bottom-Up Strategy** successfully applied
- **Solution ready** for production use on .NET 10.0

**Next Steps:**
- Deploy to appropriate environments
- Monitor for any runtime issues (unlikely given comprehensive testing)
- Update CI/CD pipelines to use .NET 10.0 SDK
- Update documentation/README if needed
- Announce upgrade completion to team/stakeholders
