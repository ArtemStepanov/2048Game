
# 2048 Game

[![codecov](https://codecov.io/github/ArtemStepanov/2048Game/graph/badge.svg?token=SD342CTD6X)](https://codecov.io/github/ArtemStepanov/2048Game)
[![Build and Test](https://github.com/ArtemStepanov/2048Game/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/ArtemStepanov/2048Game/actions/workflows/dotnet-ci.yml)
[![pages-build-deployment](https://github.com/ArtemStepanov/2048Game/actions/workflows/pages/pages-build-deployment/badge.svg)](https://github.com/ArtemStepanov/2048Game/actions/workflows/pages/pages-build-deployment)
[![Qodana](https://github.com/ArtemStepanov/2048Game/actions/workflows/qodana-ci.yml/badge.svg)](https://github.com/ArtemStepanov/2048Game/actions/workflows/qodana-ci.yml)

A C# implementation of the classic 2048 game. The goal is to combine tiles with the same number to create a tile with the number 2048.

## Features

- Classic 2048 gameplay
- Configurable board size (4x4 to 8x8)
- Score tracking
- Save and load game states
- Console-based user interface

## Table of Contents

- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Tests](#running-the-tests)
- [How to Play](#how-to-play)
- [Contributing](#contributing)
- [Contact](#contact)
- [License](#license)
- [Acknowledgements](#acknowledgements)

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing.

### Prerequisites

- .NET 8.0 SDK or later

### Installation

1. Clone the repository
   ```shell
   git clone https://github.com/yourusername/2048Game.git
   cd 2048Game
   ```

2. Restore project dependencies
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

## Running the Tests

This project uses xUnit and Moq for unit testing. To run the tests, execute the following command:

```shell
dotnet test
```

## How to Play

- Use the arrow keys to move the tiles.
- Tiles with the same number merge into one when they touch.
- Press 'R' to restart the game.
- Press 'Q' to quit the game.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create your feature branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -m 'Add your feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Open a pull request.

## Contact

For any questions or feedback, please contact Artem Stepanov at [stxima@stxima.com](mailto:stxima@stxima.com).

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- Inspired by the original [2048 game](https://github.com/gabrielecirulli/2048) by Gabriele Cirulli.
