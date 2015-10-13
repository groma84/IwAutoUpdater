# TODO
## NEXT
- DBUpdater testen und so anpassen, dass das Übergeben eines ConnectionStrings auch funktioniert...

## BUGS
###v0.1.2


## FEATURES (unsortiert)
###v0.1.2

###später
- Fehlernachrichten versenden, wenn in einem Command ein Fehler auftritt
- E-Mail-Empfänger auch bei Server: "additionalMessageReceivers[]"
    - Achtung, Parser und Test auch erweitern!
    - Es sollen am Ende immer die globalen Empfänger benachrichtigt werden, und pro Server zusätzlich die beim Server hinterlegten
- Unhandled Exceptions sollen zu einem "internen Restart" der Applikation führen, nicht zu einem totalen Crash
- Umstellen der DI Mappings Ausführung auf Reflection

## DONE
###v0.1.2
- CleanupFiles funktioniert nicht

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