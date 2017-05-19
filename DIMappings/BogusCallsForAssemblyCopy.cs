
// Datei generiert von BogusCallsForAssemblyCopy.tt
using System;
using System.Diagnostics;

namespace IwAutoUpdater.DIMappings
{
    // MSBuild schmeisst DLLs raus, die nirgends im Code aufgerufen werden, auch wenn sie auf CopyLocal stehen und
    // explizit als Projekt-Referenz eingetragen sind. Daher faken wir hier Aufrufe (die nie ausgeführt werden) *seufz*.
    public static class BogusCallsForAssemblyCopy
    {
        public static void DoBogusCalls()
        {

        }
    }
}