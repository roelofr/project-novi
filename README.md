# Project Novi

This project, created way back in 2014, is the software running on some touchscreens
located on the ground floor of the T-building of Windesheim University of Applied Sciences.

It was created as a school project with the aim of providing guests in the building a pleasant,
welcome experience. The screens (and this software) has since not been updated, so a lot of systems
no longer work (like the "Vertrektijden" screen, as NS has discontinued the API version we're using).

## Features

- Friendly homescreen with idle logic (return to home screen, say hi every now and then)
- Touch-friendly UI allowing the user to pick 'apps'
- Virtual assistant (very basic), using Google TTS to relay audio messages
  - Recordings are cached; so it works on existing screens, but the Google API has been locked down.
- RSS feed display
- Weather forecast (🚧 broken)
- Train departure information (🚧 broken)
- Twitter feed (🚧 broken)
- Map of the building, with instructions (out of date, but functional)
- Extensive documentation

## Getting started

1. Clone the repository
2. Launch the repository in Visual Studio
3. Build the application

## License

The software is proprietary, it's been created as a student endeavour, and Windesheim is free
to hand off this software to it's students for extension or to replace it.

To be explicitly clear, assume the following copyright header for each file:

```
Copyright 2014. _Arjan van der Weide_, _Robert Leeuwis_, _Joram Schrijver_, _Wouter Vogelzang_, _Evan van Urk_, _Janjaap Ree_, _Roelof Roos_.

All Rights Reserved. No parts of this application may be reproduced elsewhere without the written consent of the authors listed above.
```
