using System;
using System.Security.Cryptography;

namespace TapeFM.Server.Code
{
    public class BetterRandomNumberGenerator
    {
        private readonly RNGCryptoServiceProvider _rng;
        private readonly byte[] _buffer;

        public BetterRandomNumberGenerator()
        {
            _rng = new RNGCryptoServiceProvider();
            _buffer = new byte[4];
        }

        // Taken from OpenBSD arc4random_uniform

        // Copyright (c) 2008, Damien Miller <djm@openbsd.org>
        //
        // Permission to use, copy, modify, and distribute this software for any
        // purpose with or without fee is hereby granted, provided that the above
        // copyright notice and this permission notice appear in all copies.
        //
        // THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
        // WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
        // MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
        // ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
        // WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
        // ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
        // OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
        public uint Next(uint max)
        {
            if (max < 2)
            {
                return 0;
            }

            uint r, min = (uint) -max%max;
            for (;;)
            {
                r = Next();
                if (r >= min)
                {
                    break;
                }
            }

            return r%max;
        }

        private uint Next()
        {
            _rng.GetBytes(_buffer);
            return BitConverter.ToUInt32(_buffer, 0);
        }
    }
}