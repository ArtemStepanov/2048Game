# 2048 Game

[![Build and Test](https://github.com/ArtemStepanov/2048Game/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/ArtemStepanov/2048Game/actions/workflows/dotnet-ci.yml)
[![Qodana](https://github.com/ArtemStepanov/2048Game/actions/workflows/qodana.yml/badge.svg)](https://github.com/ArtemStepanov/2048Game/actions/workflows/qodana.yml)
[![CodeQL](https://github.com/ArtemStepanov/2048Game/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/ArtemStepanov/2048Game/actions/workflows/github-code-scanning/codeql)

This is a C# implementation of the popular 2048 game. The goal is to combine tiles with the same number to create a tile with the number 2048.

## Features

- Classic 2048 gameplay
- Configurable board size (between 4x4 and 8x8)
- Score tracking
- Save and load game state
- Console-based user interface

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 8.0 SDK or later

### Installing

1. Clone the repository
   ```shell
   git clone https://github.com/yourusername/2048Game.git
   cd 2048Game
   ```

2. Restore the project dependencies
   ```shell
   dotnet restore
   ```

3. Build the project
   ```shell
    dotnet build
    ```

4. Run the project
    ```shell
    dotnet run --project src/2048Game
    ```

### Running the Tests

This project uses xUnit and Moq for unit testing. To run the tests, use the following command:

```shell
dotnet test
```

### How to Play

- Use the arrow keys to move the tiles in the desired direction.
- Tiles with the same number merge into one when they touch.
- Press 'R' to restart the game.
- Press 'Q' to quit the game.

### License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### Acknowledgements

- Inspired by the original 2048 game by Gabriele Cirulli