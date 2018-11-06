# BTrack-Wrapper
A CLI library wrapper for adamstark's BTrack library
====================================================

Find the original library here: https://github.com/adamstark/BTrack/

Everything related to BTrack library has been done by adamstark. This is just a C# wrapper.

Usage:
---------------
**1.** Compile source with Visual Studio 2017

**2.** Add reference to compiled BTrackWrapper.dll to your C# project

**3.** Add the namespace

		using BWrapper; 

**4.** Create a new BTrackWrapper object to interface with library, with first integer argument being hopSize, second being frameSize,ie.: 

		BTrackWrapper btw = new BTrackWrapper(512, 1024); 

**5.** Fill the array with samples (create a frame)

**6.** Call processAudioFrameWrapper with pointer to frame as an argument, ie.: 

		btw.processAudioFrameWrapper(framePtr);

**7.** Check if beat has been detected in current frame by calling beatDueInCurrentFrameWrapper and checking the return value. ie.: 

		if (btw.beatDueInCurrentFrameWrapper()) {do stuff here} 

Example Source:
---------------
Clone this repo and check out the example project. Compile with Visual Studio 2017.
Testing can be done with some external audio source or virtual audio cable.
Application listens to selected input and counts beats. Every 16, 24 or 32 beats it "switches the camera".
This example has been pulled from my primary project - automated video mixer and graphics playout control.
It switched cameras on the livestream reliably for more than 10 hours at once during the music festival (using master stage audio as a beat detection source).