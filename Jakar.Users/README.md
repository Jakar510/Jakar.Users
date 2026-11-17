# Jakar.Users

User, group, role and address models (Guid- and long-keyed), login requests, claims, permissions and JWT parsing.

```sh
dotnet add package Jakar.Users
```

Split out of [Jakar.Extensions](https://github.com/Jakar510/Jakar.Extensions) in 10.12.0. Since 10.2.0 it depends on Jakar.Files, Jakar.Core, Jakar.Logging and Jakar.Collections (not Jakar.Extensions; Jakar.Extensions 10.13 depends on it and type-forwards the types that moved).nuget.org/packages/Jakar.Extensions/) and [Jakar.Files](https://www.nuget.org/packages/Jakar.Files/). The types keep the `Jakar.Extensions` namespace, so code that used them from Jakar.Extensions only needs this package reference (and a recompile).

## What's in it

- `UserModel`, `CreateUserModel`, `GroupModel`, `RoleModel`, `UserAddress`, `FileData`, `SessionToken`, ... in `UserGuid` and `UserLong` flavours, plus the generic bases to build your own.
- `LoginRequest`, `LoginRequest<TValue>`, `LoginRequestVersion`, `LoginRequestValue`.
- `Claims`, `ClaimType`, `UserRights`, `Permissions` and `JwtParser`.
- `EmailBuilder`.

`Email` and `IUserName` stay in Jakar.Extensions.

## Versioning

Versions are `<.NET major>.<library major>.<library minor>`: a breaking change bumps the middle number (`10.1.0` → `10.2.0`), anything else the last one.

## Repository

| Path | What it is |
|---|---|
| `Jakar.Users/` | The package. |
| `Jakar.Users.Tests/` | NUnit tests. |
| `.github/workflows/ci.yml` | Build and test on Ubuntu and Windows, for every push and pull request. |
| `.github/workflows/publish.yml` | On `master`: CI, then pack and push to nuget.org (Trusted Publishing). |