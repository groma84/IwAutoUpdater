# TODO
## NEXT
- UpdateInterWattDatabase rückbauen, stattdessen externes DBUpdater Command einbinden (analog Installer-Programm)


## BUGS

## FEATURES (unsortiert)

- E-Mail-Empfänger auch bei Server: "additionalMessageReceivers[]"
  -> Achtung, Parser und Test auch erweitern!

- E-Mail Versand via Command (Success und Failed)

## DONE
- Rückgaben von Commands umstellen auf neues CommandResult: bool Succesful, Error[] Errors; das dann auch immer mit durchgeben in der Kette
- Command: CleanupOldUnpackedFiles
- Command: RunInstallerCommand
   -> führt installerCommand mit installerCommandArguments aus; STDOUT wird ans Logging weitergeleitet
- Auswertung Config-Parameter (pro Server): DownloadOnly und SkipDatabaseUpdate
- Command: UpdateInterWattDatabase #1: Datenbankskripte *.DDL aus dem angegebenen Ordner raussuchen und nach Version aufsteigend sortiert zurückgeben
