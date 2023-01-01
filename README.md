# CJCMCG-Modded
A modification of CJC's MIDI Counter Generator that makes generating notecounters for trillion MIDIs super fast and easy, that it can be done within minutes!

# How it works
Basically adds a multiplier parameter such that you can just input the base file of the midi and the multiplier, and the program will do the math for you and generate a visually identical notecounter to the trillion midi.

# How to use
You will need [.NET Framework Runtime v4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net481-web-installer).

Get the download from the [releases](https://github.com/flopp1/CJCMCG-Modded/releases/tag/v0.2).

Extract the archive using an archive extractor, then run the executable inside.

To render the notecount for a trillion midi, just input the full path of the midi (Ctrl+Shift+C on Windows 11) and Ctrl+V, then enter the filename of the resulting video (it's ok most of the time to accept the default), then the delay before the start of the midi you want in the video, then your pattern (you can make your own by following the instructions [here](https://github.com/flopp1/CJCMCG-Modded/blob/master/bin/Release/Patterns/README.txt)), the text colour, then the number of times the base midi was merged to make the trillion midi.

The video will be output in the same folder as the midi.

# How to build [NOTE: WILL NOT WORK AS OF 1/1/23 BECAUSE STUFF IS BROKEN]
* Clone this repo
* Download and install Visual Studio 2022, .NET 7.0 Runtime
* Open the solution and build (Release, x64)
* Done! (Midi Counter Generator.exe in bin/Release)

# Versions
View the [changelog](CHANGELOG.md).

# To-do:
* Implement notecounter text alignment (top/bottom/left/right)
* Implement file explorer selection dialog instead of copying file path
* Finish transition to .NET 7 and fix all the broken stuff