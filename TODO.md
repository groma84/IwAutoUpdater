# TODO
## NEXT
- E-Mail Versand via Command (Success und Failed)
    - Test f�r Command schreiben
    - CommandBuilder-Test anpassen
- v1 
    - taggen
    - deployen
    - testen

## BUGS

## FEATURES (unsortiert)
###v2
- E-Mail-Empf�nger auch bei Server: "additionalMessageReceivers[]"
    - Achtung, Parser und Test auch erweitern!
    - Es sollen am Ende immer die globalen Empf�nger benachrichtigt werden, und pro Server zus�tzlich die beim Server hinterlegten



## DONE
###v1
- R�ckgaben von Commands umstellen auf neues CommandResult: bool Succesful, Error[] Errors; das dann auch immer mit durchgeben in der Kette
- Command: CleanupOldUnpackedFiles
- Command: RunInstallerCommand
   -> f�hrt installerCommand mit installerCommandArguments aus; STDOUT wird ans Logging weitergeleitet
- Auswertung Config-Parameter (pro Server): DownloadOnly und SkipDatabaseUpdate
- Command: UpdateInterWattDatabase #1: Datenbankskripte *.DDL aus dem angegebenen Ordner raussuchen und nach Version aufsteigend sortiert zur�ckgeben
- UpdateInterWattDatabase r�ckbauen, stattdessen externes DBUpdater Command einbinden (analog Installer-Programm)
- Command, das eine HTTP Verbindung aufbaut zu einer URL und den Status 200 als True nimmt, und alle anderen abweichenden Codes als "Fehler, Response als Fehlermeldung rausgeben"
- ProxySettings und E-Mail-Server-Settings zusammenf�hren in AddressUsernamePassword-Klasse
- E-Mail Versand via Command (Success und Failed)
    - Command in Config ber�cksichtigen
    - Config-Test anpassen
    - DAL-Schnittstelle erstellen    
        - SMTP-Server-Connection
        - MailPickupDirectory
    - DAL-Schnittstelle im DI registrieren
    - Command mit Schnittstelle implementieren
    - CommandBuilder um neues Command erweitern
