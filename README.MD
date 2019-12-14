![NFSTools Logo](https://s.heyitsleo.io/NFSTools/bigLogo.png)
# VaultLib
VaultLib is a fast, open-source and powerful library for processing databases created with Electronic Arts' **AttribSys** framework.

> NOTE: Full documentation and a fully cleaned-up API are in progress.

## Motivation
A number of factors played a part in the decision to create this library:

- The existence of solely closed source tools as opposed to open source ones
- Lack of a robust framework that can be extended and tweaked
- Necessity for a framework to support custom tools and processes
- A great desire to learn

## Features

- High performance in both loading and saving
- Full AttribSys format support:
  - 64-bit support
  - Big-endian support
  - Legacy (pre-2006) and Modern (2006-onwards) support
  - Supports compressed blobs, static data, etc
- Extensibility is built-in; any game that uses AttribSys can be supported.
  - Flexible type system allows different implementations of data types for specific games
  - AttribSys data structures (such as `ClassLoadData` and `CollectionLoadData`) can be overridden with custom implementations
- Easy to integrate into existing software
- Simple API 