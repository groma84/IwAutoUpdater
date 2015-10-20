using IwAutoUpdater.CrossCutting.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocks
{
    public class CommandMock : Command
    {
        public CommandResult DoResult = new CommandResult();
        public int DoCalled = 0;
        public override CommandResult Do()
        {
            ++DoCalled;
            return DoResult;
        }

        public override Command Copy()
        {
            return this;
        }

        public string PackageNameResult = "";
        public int PackageNameCalled = 0;
        public override string PackageName
        {
            get
            {
                ++PackageNameCalled;
                return PackageNameResult;
            }
        }
    }
}
