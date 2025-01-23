<!-- include ../../readme.md#content -->
<!-- #content -->
The most popular and friendly mocking library for .NET

```csharp
  var mock = new Mock<ILoveThisLibrary>();

  // WOW! No record/replay weirdness?! :)
  mock.Setup(library => library.DownloadExists("2.0.0.0"))
      .Returns(true);

  // Use the Object property on the mock to get a reference to the object
  // implementing ILoveThisLibrary, and then exercise it by calling
  // methods on it
  ILoveThisLibrary lovable = mock.Object;
  bool download = lovable.DownloadExists("2.0.0.0");

  // Verify that the given method was indeed called with the expected value at most once
  mock.Verify(library => library.DownloadExists("2.0.0.0"), Times.AtMostOnce());
```

Moq also is the first and only library so far to provide Linq to Mocks, so that the 
same behavior above can be achieved much more succinctly:

```csharp
  ILoveThisLibrary lovable = Mock.Of<ILoveThisLibrary>(l =>
    l.DownloadExists("2.0.0.0") == true);

  // Exercise the instance returned by Mock.Of by calling methods on it...
  bool download = lovable.DownloadExists("2.0.0.0");

  // Simply assert the returned state:
  Assert.True(download);
  
  // If you want to go beyond state testing and want to 
  // verify the mock interaction instead...
  Mock.Get(lovable).Verify(library => library.DownloadExists("2.0.0.0"));
```

You can think of Linq to Mocks as "from the universe of mocks, give me one whose behavior 
matches this expression".

Check out the [Quickstart](https://github.com/devlooped/moq/wiki/Quickstart) for more examples!

<!-- #content -->
<!-- ../../readme.md#content -->
<!-- include ../../readme.md#sponsors -->
<!-- #sponsors -->
<!-- include https://raw.githubusercontent.com/devlooped/sponsors/main/footer.md -->
# Sponsors 

<!-- include sponsors.md -->
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![Kirill Osenkov](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KirillOsenkov.png "Kirill Osenkov")](https://github.com/KirillOsenkov)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![Stephen Shaw](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/decriptor.png "Stephen Shaw")](https://github.com/decriptor)
[![Torutek](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/torutek-gh.png "Torutek")](https://github.com/torutek-gh)
[![DRIVE.NET, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/drivenet.png "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Ashley Medway](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/AshleyMedway.png "Ashley Medway")](https://github.com/AshleyMedway)
[![Keith Pickford](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Keflon.png "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tbolon.png "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/kfrancis.png "Kori Francis")](https://github.com/kfrancis)
[![Toni Wenzel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/twenzel.png "Toni Wenzel")](https://github.com/twenzel)
[![Giorgi Dalakishvili](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Giorgi.png "Giorgi Dalakishvili")](https://github.com/Giorgi)
[![Uno Platform](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/unoplatform.png "Uno Platform")](https://github.com/unoplatform)
[![Dan Siegel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/dansiegel.png "Dan Siegel")](https://github.com/dansiegel)
[![Reuben Swartz](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/rbnswartz.png "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jfoshee.png "Jacob Foshee")](https://github.com/jfoshee)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Mrxx99.png "")](https://github.com/Mrxx99)
[![Eric Johnson](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/eajhnsn1.png "Eric Johnson")](https://github.com/eajhnsn1)
[![Ix Technologies B.V.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/IxTechnologies.png "Ix Technologies B.V.")](https://github.com/IxTechnologies)
[![David JENNI](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/davidjenni.png "David JENNI")](https://github.com/davidjenni)
[![Jonathan ](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Jonathan-Hickey.png "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Oleg Kyrylchuk](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/okyrylchuk.png "Oleg Kyrylchuk")](https://github.com/okyrylchuk)
[![Charley Wu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/akunzai.png "Charley Wu")](https://github.com/akunzai)
[![Jakob Tikjøb Andersen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jakobt.png "Jakob Tikjøb Andersen")](https://github.com/jakobt)
[![Seann Alexander](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/seanalexander.png "Seann Alexander")](https://github.com/seanalexander)
[![Tino Hager](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tinohager.png "Tino Hager")](https://github.com/tinohager)
[![Mark Seemann](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/ploeh.png "Mark Seemann")](https://github.com/ploeh)
[![Ken Bonny](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KenBonny.png "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/SimonCropp.png "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agileworks-eu.png "agileworks-eu")](https://github.com/agileworks-eu)
[![sorahex](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/sorahex.png "sorahex")](https://github.com/sorahex)
[![Zheyu Shen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/arsdragonfly.png "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/vezel-dev.png "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/ChilliCream.png "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/4OTC.png "4OTC")](https://github.com/4OTC)
[![Vincent Limo](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/v-limo.png "Vincent Limo")](https://github.com/v-limo)
[![Brooke Hamilton](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/brooke-hamilton.png "Brooke Hamilton")](https://github.com/brooke-hamilton)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)
<!-- https://raw.githubusercontent.com/devlooped/sponsors/main/footer.md -->
<!-- ../../readme.md#sponsors -->

<!-- Exclude from auto-expansion by devlooped/actions-include GH action -->
<!-- exclude -->