# TODO
## NEXT
- Command, das eine HTTP Verbindung aufbaut zu einer URL und den Status 200 als True nimmt, und alle anderen abweichenden Codes als "Fehler, Response als Fehlermeldung rausgeben"
    - HtmlGetter-Test schreiben
    - HtmlGetter-Implementierung in DI registrieren
    - Command mit HtmlGetter umsetzen
    - Test f�r Command schreiben
    - Command in Config ber�cksichtigen
    - Config-Test anpassen
    - CommandBuilder um neues Command erweitern
    - CommandBuilder-Test anpassen

## BUGS

## FEATURES (unsortiert)

- E-Mail-Empf�nger auch bei Server: "additionalMessageReceivers[]"
  -> Achtung, Parser und Test auch erweitern!

- E-Mail Versand via Command (Success und Failed)


## DONE
- R�ckgaben von Commands umstellen auf neues CommandResult: bool Succesful, Error[] Errors; das dann auch immer mit durchgeben in der Kette
- Command: CleanupOldUnpackedFiles
- Command: RunInstallerCommand
   -> f�hrt installerCommand mit installerCommandArguments aus; STDOUT wird ans Logging weitergeleitet
- Auswertung Config-Parameter (pro Server): DownloadOnly und SkipDatabaseUpdate
- Command: UpdateInterWattDatabase #1: Datenbankskripte *.DDL aus dem angegebenen Ordner raussuchen und nach Version aufsteigend sortiert zur�ckgeben
- UpdateInterWattDatabase r�ckbauen, stattdessen externes DBUpdater Command einbinden (analog Installer-Programm)
