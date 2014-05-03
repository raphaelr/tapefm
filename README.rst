========================================
tapeFM - Private music streaming service
========================================

tapeFM is a *very* simple music streaming service you can run at your home server. Think Grooveshark or Spotify, but it uses your own music library. You're supposed to use your smartphone's browser as the client, if you are on a PC just use Winamp or Clementine or something.

It's implemented with Ruby (on the server) and HTML+JavaScript (on the client), so it should run on any reasonably recent smartphone you can find. The requested songs are transcoded into 128 kbit/s OGG Vorbis by default, which should be fine for most network connections and browsers.

Client features
---------------
* Pick a specific song to listen to
* If you don't do that, it will keep playing randomly picked songs from your library
* Skip a song
* Pause/Resume playback

Installation
------------
Make sure you have `lame` (a MP3 decoder) and `oggenc` (an OGG encoder) installed. You may need additional decoders if your music library contains non-MP3 files.

Install ruby (with RVM or without, who cares) and the `bundler` gem.

Clone to some directory. Change the library path in `config.yml`. Run `bundle install`. Done.

Running
-------
Just execute `rackup`. Get fancy with systemd at your own discretion.

Configuration
-------------
Happens in the file `config.yml`.

`:encoder` section
~~~~~~~~~~~~~~~~~~
**Example**::

  :encoder:
    :command: oggenc -Q -b 128 - -o -
    :mime: application/ogg

**Explaination:**

`:command`
  The command used to encode a music file to something the client can play (OGG Vorbis turns out to be the most portable format). It should read from `stdin` and write to `stdout`.
`:mime`
  The MIME type of the encoded data, so that the browser knows whats going on.

`:decoders` section
~~~~~~~~~~~~~~~~~~~
**Example**::

  :decoders:
    mp3: lame -d -c -s "$fn"

**Explanation**:

Contains one entry per file extension to decode. The substring `$fn` is replaced with the path of the file to decode, and the result should be written to `stdout`.

`:library` section
~~~~~~~~~~~~~~~~~~
**Example**::

  :library:
    :path: /srv/music/
    :glob: **/*.{mp3,flac}

**Explaination**:

`:path`
  The root directory of your music library. This is pretty much the only thing you need to customize.
`:glob`
  Glob expression which is used to search for music files. The default is fine for most cases, unless you want to support WMA files or something, in which case you have to add additional file extensions inside the curly braces.

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
