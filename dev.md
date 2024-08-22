# Development

## Test Without Unity

1. Build gen_game_unity

```bash
cd gen_game_unity
dotnet build
```

2. Create new .NET project

```
mkdir my_project
cd my_project
touch my_project.csproj
touch Program.cs
```

3. Edit my_project.csproj, change $YOUR_PATH with your actual path.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <ProjectReference Include="$YOUR_PATH\gen_game_unity\gen_game_unity.csproj" />
  </ItemGroup>

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <RootNamespace>my_project</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <TargetFramework>net8</TargetFramework>
    <LangVersion>12</LangVersion>
  </PropertyGroup>

</Project>
```

5. Edit Program.cs

```cs
using System.Diagnostics;
using GenGame;

// connect to GenGame server and then return a single object
// that used to communicate with the server
var genGame = new Client("localhost", 4000);
await genGame.Connect();

// create user and authenticate it
await genGame.AuthenticateDevice("dev-123");

// try to ping to the server
var resPing = await genGame.Ping();
Debug.Assert(resPing == "pong");

// create match and return GenGame.Game object
var match = await genGame.CreateMatch();

Console.WriteLine("new match created:");
Console.WriteLine(match);

Thread.Sleep(2000);

```

6. Run the program

```bash
dotnet run
```
