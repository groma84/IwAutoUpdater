# TODO
## NEXT

## BUGS
###v0.2.2


## FEATURES (unsortiert)
###v0.2.2

###später
- E-Mail-Empfänger auch bei Server: "additionalMessageReceivers[]"
    - Achtung, Parser und Test auch erweitern!
    - Es sollen am Ende immer die globalen Empfänger benachrichtigt werden, und pro Server zusätzlich die beim Server hinterlegten
- Hinterlegbarkeit einer "Versionsinfo-Datei" -> diese wird am Ende für die Notification ausgelesen und 1:1 in den Body kopiert
- Protokollierung, welche Dateien wann erfolgreich installiert wurden -> "resume"-Funktion (ev. mit Textdatei oder SQLite DB?)

## DONE
###v0.2.2
- Protokolldatei, statt Logging nur in Console
- Umstellen der DI Mappings Ausführung auf Reflection


###v0.2.1
- [bug] UpdateDatabase schickt das falsche Command in der Error-Notification
    - Copy() für alle Commands implementieren
    - finalCommand.Copy() an den entsprechenden Stellen einsetzen, damit wir nicht via Pointer immer nur das letzte Command ansteuern
- man müsste jedem Command, bevor es in die Queue gelegt wird, alle bisherigen Nachrichten mitgeben - so könnte man dann doch wieder "kreuz und quer" Commands von versch. Servern ausführen
- Queues so umstellen, dass erst alle heruntergeladen werden, danach aber jeder Server komplett durchgezogen wird
- Unhandled Exceptions sollen zu einem "internen Restart" der Applikation führen, nicht zu einem totalen Crash


###v0.2.0
- [bug] warum wird am Ende der Queue-Bearbeitung der CommandName statt Server-URL ausgegeben?
- CleanupFiles funktioniert nicht
- Fehlernachrichten versenden, wenn in einem Command ein Fehler auftritt

###v0.1.1
- Zahlreiche Bugfix an SMB-Verbindung und Datei-Download
- DI-Mappings für einige Bibliotheken fehlen
- UncPath-Zugriff benötigt Benutzername und Passwort
- E-Mail-Server braucht Option "UseDefaultCredentials"
- Unc-Connection sollte bei "Datei nicht gefunden" nicht mit einer Exception crashen
- config-example.json aktualisieren

###v0.1.0
- Rückgaben von Commands umstellen auf neues CommandResult: bool Succesful, Error[] Errors; das dann auch immer mit durchgeben in der Kette
- Command: CleanupOldUnpackedFiles
- Command: RunInstallerCommand
   -> führt installerCommand mit installerCommandArguments aus; STDOUT wird ans Logging weitergeleitet
- Auswertung Config-Parameter (pro Server): DownloadOnly und SkipDatabaseUpdate
- Command: UpdateInterWattDatabase #1: Datenbankskripte *.DDL aus dem angegebenen Ordner raussuchen und nach Version aufsteigend sortiert zurückgeben
- UpdateInterWattDatabase rückbauen, stattdessen externes DBUpdater Command einbinden (analog Installer-Programm)
- Command, das eine HTTP Verbindung aufbaut zu einer URL und den Status 200 als True nimmt, und alle anderen abweichenden Codes als "Fehler, Response als Fehlermeldung rausgeben"
- ProxySettings und E-Mail-Server-Settings zusammenführen in AddressUsernamePassword-Klasse
- E-Mail Versand via Command (Success und Failed)
    - Command in Config berücksichtigen
    - Config-Test anpassen
    - DAL-Schnittstelle erstellen    
        - SMTP-Server-Connection
        - MailPickupDirectory
    - DAL-Schnittstelle im DI registrieren
    - Command mit Schnittstelle implementieren
    - CommandBuilder um neues Command erweitern
    - CommandBuilder-Test anpassen
    - Test für Command schreiben
- v1 
    - taggen
    - deployen
    - testen