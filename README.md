# PDFSigner

A highly customizable command line utility for signing PDF documents using PKCS #12 certificate files. Perfect for integrating into scripts, batch processes, or legacy applications that lack built-in PDF signing capabilities.

![Demo](PDFSign.gif)

## Features

- **Customizable signature appearance** and positioning
- **Configuration stored in SQLite database** for easy management
- **Cross-platform compatibility** across Windows, macOS, and Linux
- **Command-line interface** for seamless integration with existing workflows

## Installation

1. Download the latest release from the [Releases page](https://github.com/yourusername/PDFSigner/releases)
2. Extract the files to your preferred location
3. Add the directory to your system PATH (optional, for easier access)

## Requirements

- .NET Runtime (version X.X or higher)
- A valid PKCS #12 certificate file (.pfx or .p12)

## Usage

### Initial Setup

Configure your certificates with the setup wizard:

```bash
PDFSign.exe --setup
```

### Basic Signing

Sign a PDF with a configured certificate:
```bash
PDFSign.exe -n MyCertificate -p "path/to/document.pdf"
```

### Options
| Option | Short | Description |
|--------|-------|-------------|
| `--name` | `-n` | Certificate name (configured in setup) |
| `--id` | `-i` | Set business id (alternative to using name) |
| `--pdfpath` | `-p` | Path to the PDF file to sign |
| `--keep` | `-k` | Keep original PDF file with "_original" suffix (default: true) |
| `--verbose` | `-v` | Set output to verbose messages |
| `--setup` | `-s` | Launch the certificate configuration wizard |
| `--help` | `-h` | Display help information |
| `--version` | | Display version information |

## Configuration Database

The configuration is stored in a SQLite database file that contains:

- Certificate information and paths
- Default signature appearance settings
- Custom parameters

You can manage configurations through the `--setup` wizard or directly modify the database with SQLite tools.

## Troubleshooting

### Common Issues

- **Certificate not found**: Ensure the certificate name matches what you configured in setup
- **Invalid certificate**: Verify your certificate is valid and has not expired
- **Permission denied**: Make sure you have read/write access to the PDF files

## License

MIT License

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.