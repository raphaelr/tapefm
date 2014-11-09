========================================
tapeFM - Private music streaming service
========================================

tapeFM is a *very* simple music streaming service you can run at your home server. Think Grooveshark
or Spotify, but it uses your own music library. You're supposed to use your smartphone's browser as
the client, if you are on a PC just use Winamp or Clementine or something.

It's implemented with ASP.NET WebApi (on the server) and HTML+JavaScript (on the client), so it
should run on any reasonably recent smartphone you can find. The requested songs are transcoded
into (currently nonconfigurable; but that's coming soon) 533 kbit/s OGG Opus by default, which
should be fine for some network connections and browsers.

Currently *very much* in development. If you are a web designer, please don't hurt me. There is a
more stable version on the `v1.0` branch.

Client features
---------------
* Pick a specific song to listen to
* If you don't do that, it will keep playing randomly picked songs from your library

Coming soon features
--------------------
* Skipping a song
* Pausing/Resuming playback

Coming maybe soon but probably not in the most closest future
-------------------------------------------------------------
* Playlist support
* Adapting the streaming bitrate to the network connection of the client (e.g. when switching
  from an HSDPA to an EDGE signal)
* Scrobbling to last.fm

Installation
------------
Make sure you have `ffmpeg` (to decode your music) and `opus` (to encode it again) installed.
You will also need to have `redis` installed and running.  Use `nuget update TapeFM.sln` to
install the required dependencies.

Finally change the path to your music library in `TapeFM.Server/tapefm.config`.

A word on mono
--------------
If you intend to run TapeFM on mono, you may run into a Redis-related problem.
The StackExchange.Redis library may not be able to connect to the server, even if it's running on
localhost. In this case you may have to build StackExchange.Redis yourself and use your self-built
.dll in place of the nuget provided one. See [this StackOverflow question](https://stackoverflow.com/q/23871110)
for details.

Running
-------
Run `..\packages\OwinHost.3.0.0\tools\OwinHost.exe` inside the TapeFM.Server directory. It will
only listen on localhost by default, so you probably want to use the `-u` argument to specify
the URL to listen on. For example, `-u http://*:5000` will listen on all addresses on port 5000.

Also be sure to make this accessible from your VPN only because there is no authentication.

Listening
---------
You can either click on the speaker icon in the bottom-right corner of the web page, or you can
connect with any media player that supports the Opus codec by opening the URL
`http://your.host.example/listen`.

License
-------
| Copyright (c) 2014, Raphael Robatsch
| All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted
provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions
   and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions
   and the following disclaimer in the documentation and/or other materials provided with the
   distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF
THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

