StoreandConvert
===============
The idea behind this is to use a bookmarklet to store urls for later conversion.

This is similar to the read later website, but read later stores the page there and then, which it's good in that you will get the page there and then stored but it's not so good in that you have to store the page in its entirety. This also gets around paywall issues (I think) but they have not been a concern for me, so...

Furthemore the conversion to ebook that read later does was a lot worse than the one that calibre does, of course it probably does not justify spending all the time that I have on this.

Anyway, there are two windows services:
Store:
This stores urls and the page title to a local file. This is done via a Restful WCF service, which is invoked with a bookmarklet.

In order to deal with secure sites a certificate is required, it can be self signed.

Convert:
This effectively uses ebook-convert from the calibre tool to convert the stored urls into a kindle ebook in periodical format.

There is an installer that takes care of most things, apart from adding a self signed certificate.
