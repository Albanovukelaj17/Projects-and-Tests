# HamsterRPC

Das selbst gestrickte RPC-Protokoll der Open-Source Bibliothek Hamsterlib, 
ist zwar robust und effizient, jedoch ist die Implementierung komplex und 
aufwändig. Das IT-Unternehmen, welches die aktuelle Implementierung des 
RPC-Protokolls vertreibt, verlangt horrende Preise für die Wartung. Um 
auf lange Sicht Geld zu sparen, beschließt das westhessische 
Hamsterverwahrungsunternehmen in eine standardisierte RPC-Lösung zu 
investieren und somit nicht mehr von einem Unternehmen abhängig zu sein.
Das gRPC Protokoll ist allgemein verfügbar, es ist für gute Effizienz bekannt
und es erlaubt durch die Verfügbarkeit von Implementierungen in verschiedenen Programmiersprachen und -plattformen problemlos die Weiterverwendung
der Hamsterbibliothek. Deshalb entscheidet man sich für diesen Standard.

In Ihrem Git-Repository finden Sie
die bereits bekannte Hamsterlib. 
Unter *src* finden Sie auch den Quellcode eines einfachen Menüprogramms, das die API der
Hamsterlib verwendet. Zudem ist auch eine Vorlage für den Client enthalten, in dem das Parsen der CLI-Schnittstelle
bereits implementiert ist, die vom Parser aufgerufenen Funktionen aber noch leer sind.

Ihre Aufgabe ist es nun, dieses Programm in ein verteiltes Programm,
bestehend aus einem *Server* und einem *Client*, umzuwandeln. Dabei sollen Server und Client mittels gRPC miteinander kommunizieren.
Wichtig ist hierbei, dass die Ausgabe des neuen Clients identisch mit der Ausgabe des bestehenden Client-Programms ist.

Dazu sind folgende Schritte erforderlich:

- Erstellen einer RPC *Schnittstellenspezifikation* `hamster.proto`
	im Unterverzeichnis `proto`.
- Automatisches Generieren der *RPC stubs*. Details hierzu finden Sie in den technologiespezifischen Hinweisen.
- Manuelles Anpassen der client- und serverseitigen Templates so, dass der Server
	die Schnittstelle der Hamsterlib aufruft und der Client eine zu dieser
	Schittstelle identische, auf RPC aufgesetzte Schnittstelle bietet.
- Kompilieren der client- und serverseitigen Programmteile

## Tests

Die Testsuite für diese Aufgabe ist dieselbe wie beim letzten Übungsblatt, allerdings wird der Testtreiber nun
Ihren Client aufrufen, anstatt wie beim letzten mal direkt als Client zu agieren.

**Wichtig:** Um die Tests einfacher zu machen, wurden kleine Anpassungen an der Ausgabe des Clients vorgenommen. So wird erwartet, dass Sie
bei den Verben *add*, *feed* und *bill* nur das Ergebnis (ID, übrige Leckerli oder zu zahlenden Betrag) als Ganzzahl auf die Konsole ausgeben.

Sie können die Testsuite wie gehabt als ausführbares Jar-Archiv ausführen, auch die Ausgaben sind identisch zum letzten Übungsblatt.

## Tipps zur Bearbeitung

Machen Sie sich zunächst anhand eines einfachen Beispiels mit der Benutzung
von gRPC vertraut. Es gibt unzählige gRPC Tutorials im Netz. Mit [1](https://grpc.io/docs/languages/java/basics/) sei
hier nur eines davon genannt. Erproben Sie das dort gezeigte Beispiel, um die
Benutzung der Tools kennenzulernen.

## Hinweise C#

Für .NET existieren im Augenblick zwei verschiedene Implementierungen von gRPC, einmal
eine verwaltete Implementierung auf Basis von ASP.NET Core und eine nicht-verwaltete Implementierung
auf Basis eines in C implementierten Kernfunktionalität. Das Problem hierbei:
Erstere funktioniert im Moment nicht unter MacOS, zweitere wird nicht aktiv weiterentwickelt.
Wenn Sie MacOS verwenden und die Aufgabe in C# erledigen wollen, würden wir Ihnen dringend
dazu raten, diese Aufgabe in einer virtuellen Maschine entweder auf Basis von Linux oder
Windows zu erledigen.

Ansonsten können Sie das Tooling für gRPC in .NET bequem über NuGet-Pakete installieren.
Verwenden Sie hierbei für den Server das Paket Grpc.AspNetCore und für den Client
die Pakete Grpc.Tools, Grpc.Net.Client und Google.ProtoBuf. Damit können Sie auf die
Proto-Dateien in der Projektdatei verweisen, bspw. mit

```
<Protobuf Include="..\Proto\hamster.proto" GrpcServices="Server"
Link="Protos\hamster.proto" />
```

Die Vorlage enthält bereits eine Projektmappe mit Projekten, die diese Pakete bereits enthalten.
Die Integration von gRPC in .NET geht soweit, dass mit diesen Paketen (insb. Grpc.Tools)
automatisch beim Speichern einer Proto-Datei Code erzeugt wird. Das bedeutet auch, dass
wenn Sie die Datei mit einem Syntaxfehler speichern, der bisherige Code gelöscht wird und Sie
jede Menge Compilerfehler bekommen.

Das Vorgehen mit verlinkten Proto-Dateien hat hier den Vorzug, dass Sie keine gemeinsam
verwendeten Bibliotheken zwischen Server und Client haben müssen, was insbesondere sinnvoll
ist, wenn Server und Client andere Zielarchitekturen verwenden. Leider ist die Interaktion mit
der Proto-Generierung nicht so gut, Visual Studio generiert im Hintergrund den Code nur für
das Projekt, in dem Sie die Proto-Datei gerade geöffnet haben.