# BTrack-Wrapper
A CLI library wrapper for adamstark's BTrack library
====================================================

Usage:

1. Compile source with Visual Studio 2017
2. Add reference to compiled BTrackWrapper.dll to your C# project
3. Add "using BWrapper;" to your source
4. Create a new BTrackWrapper object to interface with library,
ie.: BTrackWrapper btw = new BTrackWrapper(512, 1024); first integer argument being hopSize, second being frameSize
